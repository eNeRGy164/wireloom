import type { CompileRequest, CompileResponse, WorkerMessage } from "./protocol";
import { WorkerRequestState } from "./worker-state.mjs";

interface Callbacks {
  current(): { revision: number; profile: string };
  status(message: string, retry: boolean): void;
  result(response: CompileResponse): void;
  failure(message: string): void;
}

type WorkerFactory = () => Worker;
type PendingWorkerMessage = Extract<WorkerMessage, { type: "result" | "error" | "startup-error" }>;

/** Keeps one running request and the newest pending request across worker replacement. */
export class CompilerClient {
  private state = new WorkerRequestState();
  private worker?: Worker;
  private running = false;
  private requestId = 0;
  private outstanding = new Map<number, (message: PendingWorkerMessage) => void>();
  private cancelStartup?: (message: string) => void;
  private callbacks: Callbacks;
  private createWorker: WorkerFactory;

  constructor(
    callbacks: Callbacks,
    createWorker: WorkerFactory = () => new Worker(`${import.meta.env.BASE_URL}dotnet/worker.mjs`, { type: "module" }),
  ) {
    this.callbacks = callbacks;
    this.createWorker = createWorker;
  }

  async start(): Promise<void> {
    const generation = this.state.replaceWorker();
    const previousWorker = this.worker;
    this.worker = undefined;
    this.cancelStartup?.("Compiler worker was restarted.");
    this.cancelStartup = undefined;
    this.rejectOutstanding("Compiler worker was restarted before the request completed.");
    previousWorker?.terminate();
    this.callbacks.status("Starting compiler…", false);
    let failureReported = false;
    try {
      const worker = this.createWorker();
      this.worker = worker;
      await new Promise<void>((resolve, reject) => {
        let settled = false;
        let timeout = 0;
        const cancel = (message: string) => {
          settled = true;
          clearTimeout(timeout);
          reject(new Error(message));
        };
        const fail = (message: string) => {
          if (generation !== this.state.generation || failureReported) return;
          failureReported = true;
          clearTimeout(timeout);
          if (this.cancelStartup === cancel) this.cancelStartup = undefined;
          this.state.markFailed(generation);
          worker.terminate();
          if (this.worker === worker) this.worker = undefined;
          this.callbacks.status(message, true);
          this.callbacks.failure(message);
          this.rejectOutstanding(message);
          if (!settled) {
            settled = true;
            reject(new Error(message));
          }
        };
        timeout = window.setTimeout(() => fail("Compiler startup timed out. Restart to try again."), 30_000);
        this.cancelStartup = cancel;
        worker.onerror = (event) => fail(event.message || "Compiler worker stopped. Restart to continue.");
        worker.onmessage = ({ data }: MessageEvent<WorkerMessage>) => {
          if (generation !== this.state.generation) return;
          if (data.type === "startup-error") {
            fail(data.message);
            return;
          }
          if (data.type === "ready") {
            if (settled) return;
            settled = true;
            clearTimeout(timeout);
            if (this.cancelStartup === cancel) this.cancelStartup = undefined;
            this.state.markReady(generation);
            this.callbacks.status("Compiler ready · runs on your device", false);
            resolve();
            if (this.state.shouldDrain(this.running)) void this.drain();
            return;
          }
          const id = data.type === "result" ? data.response.requestId : data.requestId;
          this.outstanding.get(id)?.(data);
          this.outstanding.delete(id);
        };
      });
    } catch (error) {
      if (generation !== this.state.generation) return;
      this.state.markFailed(generation);
      if (!failureReported) {
        this.callbacks.failure(String(error));
        this.callbacks.status(String(error), true);
      }
    }
  }

  enqueue(snapshot: Omit<CompileRequest, "protocolVersion" | "workerGeneration" | "requestId">) {
    this.state.enqueue({
      ...snapshot,
      protocolVersion: 1,
      workerGeneration: this.state.generation,
      requestId: ++this.requestId,
    });
    if (this.state.shouldDrain(this.running)) void this.drain();
  }

  private submit(request: CompileRequest): Promise<CompileResponse> {
    return new Promise((resolve, reject) => {
      const timeout = window.setTimeout(() => {
        if (!this.outstanding.has(request.requestId)) return;
        this.outstanding.delete(request.requestId);
        void this.start();
        reject(
          new Error(
            "Compilation timed out. A replacement compiler is starting; edit the source or restart to try again.",
          ),
        );
      }, 10_000);
      this.outstanding.set(request.requestId, (message) => {
        clearTimeout(timeout);
        if (message.type === "result") {
          resolve(message.response);
          return;
        }

        reject(new Error(message.message));
      });
      try {
        this.worker!.postMessage(request);
      } catch (error) {
        this.outstanding.delete(request.requestId);
        clearTimeout(timeout);
        reject(error);
      }
    });
  }

  private rejectOutstanding(message: string) {
    for (const finish of this.outstanding.values()) finish({ type: "startup-error", message });
    this.outstanding.clear();
  }

  private async drain() {
    this.running = true;
    try {
      while (this.state.ready) {
        const request = this.state.takePending();
        if (!request) break;
        try {
          const response = await this.submit(request);
          const current = this.callbacks.current();
          if (this.state.matchesResponse(request, response, current.revision, current.profile))
            this.callbacks.result(response);
        } catch (error) {
          const current = this.callbacks.current();
          if (this.state.isCurrentWorkspaceRequest(request, current.revision, current.profile)) {
            this.callbacks.failure(String(error));
            this.callbacks.status(
              "Compiler request failed. Edit the source or restart the compiler to try again.",
              true,
            );
          }
        }
      }
    } finally {
      this.running = false;
      if (this.state.shouldDrain(false)) void this.drain();
    }
  }
}
