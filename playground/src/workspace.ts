import type { CompileRequest, WorkspaceFile } from "./protocol";

export const maxSourceBytes = 256 * 1024;
export type Profile = CompileRequest["profile"];
export interface Workspace {
  version: 1;
  files: WorkspaceFile[];
  profile: Profile;
}

export function sourceFits(source: string): boolean {
  return source.length <= maxSourceBytes && new TextEncoder().encode(source).byteLength <= maxSourceBytes;
}

export function validateWorkspace(value: unknown): Workspace {
  const item = value as Partial<Workspace> | null;
  if (
    !item ||
    item.version !== 1 ||
    !["7.3.1", "7.7.0"].includes(item.profile ?? "") ||
    !Array.isArray(item.files) ||
    item.files.length !== 1
  )
    throw new Error("Saved workspace has an unsupported format.");

  const file = item.files[0];

  if (
    !file ||
    typeof file.path !== "string" ||
    !/^\/workspace\/[^/\\\x00-\x1f]+\.idl$/i.test(file.path) ||
    typeof file.source !== "string" ||
    file.isGenerationRoot !== true ||
    !sourceFits(file.source)
  ) {
    throw new Error("Saved workspace contains invalid or oversized IDL.");
  }

  return { version: 1, profile: item.profile!, files: [{ ...file }] };
}

/** Serial operations prevent an older save from overwriting reset or newer edits. */
export class RecoveryStore {
  private database?: IDBDatabase;
  private operations: Promise<unknown> = Promise.resolve();

  async open(): Promise<Workspace | undefined> {
    this.database = await new Promise<IDBDatabase>((resolve, reject) => {
      const request = indexedDB.open("wireloom-playground", 1);
      request.onupgradeneeded = () => request.result.createObjectStore("workspace");
      request.onsuccess = () => resolve(request.result);
      request.onerror = () => reject(request.error);
      request.onblocked = () => reject(new Error("Local recovery is blocked by another tab."));
    });
    this.database.onversionchange = () => this.database?.close();
    const value = await new Promise<unknown>((resolve, reject) => {
      const request = this.database!.transaction("workspace").objectStore("workspace").get("current");
      request.onsuccess = () => resolve(request.result);
      request.onerror = () => reject(request.error);
    });
    return value === undefined ? undefined : validateWorkspace(value);
  }

  save(workspace: Workspace): Promise<unknown> {
    return this.write(structuredClone(workspace));
  }
  clear(): Promise<unknown> {
    return this.write(undefined);
  }

  private write(value: Workspace | undefined): Promise<unknown> {
    const operation = this.operations
      .catch(() => undefined)
      .then(
        () =>
          new Promise<void>((resolve, reject) => {
            if (!this.database) return reject(new Error("Local recovery is unavailable."));
            const transaction = this.database.transaction("workspace", "readwrite");
            const store = transaction.objectStore("workspace");
            if (value) store.put(value, "current");
            else store.delete("current");
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
            transaction.onabort = () => reject(transaction.error ?? new Error("Local recovery was interrupted."));
          }),
      );
    this.operations = operation;
    return operation;
  }
}
