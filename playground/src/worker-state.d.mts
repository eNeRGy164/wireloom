import type { CompileRequest, CompileResponse } from "./protocol";

export class WorkerRequestState {
  readonly generation: number;
  readonly ready: boolean;
  replaceWorker(): number;
  markReady(generation: number): boolean;
  markFailed(generation: number): boolean;
  enqueue(request: CompileRequest): void;
  shouldDrain(running: boolean): boolean;
  takePending(): CompileRequest | null;
  isCurrent(request: CompileRequest, workspaceRevision: number, profile: string): boolean;
  isCurrentWorkspaceRequest(request: CompileRequest, workspaceRevision: number, profile: string): boolean;
  matchesResponse(request: CompileRequest, response: CompileResponse, workspaceRevision: number, profile: string): boolean;
}
