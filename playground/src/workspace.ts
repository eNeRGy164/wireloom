import type { CompileRequest, FileOptions, WorkspaceFile, WorkspaceOptions } from "./protocol";

export const workspaceRoot = "/workspace";
export const maxSourceBytes = 256 * 1024;
export const maxFiles = 512;
export const maxFolders = 512;
export const maxPathLength = 1024;
export const maxPathDepth = 32;
export type Profile = CompileRequest["profile"];

/** Stable identities keep editor models and undo history through virtual moves. */
export interface SourceFile extends WorkspaceFile {
  id: string;
  options: FileOptions;
}

export interface Workspace {
  version: 4;
  files: SourceFile[];
  folders: string[];
  profile: Profile;
  options: WorkspaceOptions;
  activeFileId?: string;
}

export function defaultOptions(): WorkspaceOptions {
  return {
    sequenceArrayMapping: "RtiCSharp",
    sequenceSize: 100,
    stringSize: 255,
    includeDirectories: [],
    enableEscapeChar: false,
    unboundedSupport: false,
  };
}

export function createFile(path: string, source = ""): SourceFile {
  return {
    id: crypto.randomUUID(),
    path,
    source,
    isGenerationRoot: true,
    options: { defines: [], undefines: [], strict: false },
  };
}

export function createWorkspace(source = "", profile: Profile = "7.7.0"): Workspace {
  return {
    version: 4,
    files: [createFile(`${workspaceRoot}/main.idl`, source)],
    folders: [],
    profile,
    options: defaultOptions(),
  };
}

export function createEmptyWorkspace(profile: Profile = "7.7.0"): Workspace {
  return {
    version: 4,
    files: [],
    folders: [],
    profile,
    options: defaultOptions(),
  };
}

export function sourceFits(source: string): boolean {
  return source.length <= maxSourceBytes && new TextEncoder().encode(source).byteLength <= maxSourceBytes;
}

export function sourceBytes(workspace: Workspace): number {
  return workspace.files.reduce((total, file) => total + new TextEncoder().encode(file.source).byteLength, 0);
}

export function parentPath(path: string): string {
  return path.slice(0, path.lastIndexOf("/"));
}

export function isWithin(path: string, folder: string): boolean {
  return path === folder || path.startsWith(`${folder}/`);
}

export function validatePath(value: unknown, allowRoot = false): asserts value is string {
  if (allowRoot && value === workspaceRoot) return;
  if (
    typeof value !== "string" ||
    !value.startsWith(`${workspaceRoot}/`) ||
    value.length > maxPathLength ||
    /[\\\x00-\x1f\x7f-\x9f]/.test(value)
  )
    throw new Error("Use a virtual path under /workspace/ (at most 1,024 characters).");
  const parts = value.slice(workspaceRoot.length + 1).split("/");
  if (
    parts.length > maxPathDepth ||
    parts.some((part) => !part || part === "." || part === ".." || part.trim() !== part)
  )
    throw new Error("Paths must be normalized, with at most 32 levels and no empty, dot, or padded names.");
}

export function validateOptions(value: unknown): WorkspaceOptions {
  const item = value as WorkspaceOptions | null;
  if (
    !item ||
    typeof item.enableEscapeChar !== "boolean" ||
    typeof item.unboundedSupport !== "boolean" ||
    !["RtiCSharp", "Idl"].includes(item.sequenceArrayMapping) ||
    ![item.sequenceSize, item.stringSize].every((size) => Number.isInteger(size) && size > 0 && size <= 2147483647)
  )
    throw new Error("Workspace settings are invalid.");
  for (const list of [item.includeDirectories]) {
    if (
      !Array.isArray(list) ||
      list.length > 128 ||
      list.some(
        (entry) => typeof entry !== "string" || !entry || entry.length > 1024 || /[\x00-\x1f\x7f-\x9f]/.test(entry),
      )
    )
      throw new Error("Each settings list supports at most 128 non-empty entries of 1,024 characters.");
  }
  for (const folder of item.includeDirectories) validatePath(folder, true);
  return {
    ...item,
    includeDirectories: [...new Set(item.includeDirectories)].sort(),
  };
}

function validateFileOptions(value: unknown): asserts value is FileOptions {
  const item = value as FileOptions | null;
  if (!item || typeof item.strict !== "boolean") throw new Error("File settings are invalid.");
  for (const list of [item.defines, item.undefines]) {
    if (
      !Array.isArray(list) ||
      list.length > 128 ||
      list.some(
        (entry) => typeof entry !== "string" || !entry || entry.length > 1024 || /[\x00-\x1f\x7f-\x9f]/.test(entry),
      )
    )
      throw new Error("Each settings list supports at most 128 non-empty entries of 1,024 characters.");
  }
  if (
    (item.sequenceArrayMapping !== undefined && !["RtiCSharp", "Idl"].includes(item.sequenceArrayMapping)) ||
    (item.unboundedSupport !== undefined && typeof item.unboundedSupport !== "boolean") ||
    [item.sequenceSize, item.stringSize].some(
      (size) => size !== undefined && (!Number.isInteger(size) || size <= 0 || size > 2147483647),
    )
  )
    throw new Error("File settings require a known mapping and positive 32-bit sizes.");
}

export function validateWorkspace(value: unknown): Workspace {
  const raw = value as
    | (Omit<Workspace, "version"> & { version: number; options: WorkspaceOptions & { unboundedSupport?: boolean } })
    | null;
  const item = (
    raw?.version === 3 ? { ...raw, version: 4 as const, options: { ...raw.options, unboundedSupport: false } } : raw
  ) as Workspace | null;
  if (
    !item ||
    item.version !== 4 ||
    !["7.3.1", "7.7.0"].includes(item.profile) ||
    !Array.isArray(item.files) ||
    item.files.length > maxFiles ||
    !Array.isArray(item.folders) ||
    item.folders.length > maxFolders
  )
    throw new Error("Saved workspace has an unsupported format or exceeds the 512 file/folder limits.");
  const paths = new Set<string>();
  const ids = new Set<string>();
  for (const path of item.folders) {
    validatePath(path);
    if (paths.has(path)) throw new Error("Workspace paths must be unique.");
    paths.add(path);
  }
  for (const file of item.files) {
    if (
      !file ||
      typeof file.source !== "string" ||
      typeof file.isGenerationRoot !== "boolean" ||
      typeof file.id !== "string" ||
      !file.id ||
      file.id.length > 128 ||
      ids.has(file.id)
    )
      throw new Error("Workspace file identity or source is invalid.");
    validatePath(file.path);
    if (paths.has(file.path)) throw new Error("Workspace paths must be unique.");
    paths.add(file.path);
    ids.add(file.id);
    validateFileOptions(file.options);
  }
  if (item.activeFileId !== undefined && !ids.has(item.activeFileId))
    throw new Error("The active file must exist in the workspace.");
  const folders = new Set(item.folders);
  for (const path of paths) {
    if (parentPath(path) !== workspaceRoot && !folders.has(parentPath(path)))
      throw new Error("Every parent folder must exist.");
  }
  if (sourceBytes(item) > maxSourceBytes) throw new Error("Workspace input exceeds the 256 KiB UTF-8 limit.");
  const options = validateOptions(item.options);
  if (options.includeDirectories.some((path) => path !== workspaceRoot && !folders.has(path)))
    throw new Error("Select existing virtual include folders.");
  return structuredClone({ ...item, options });
}

/** Apply a complete edit only after all paths, options, and resource bounds validate. */
function edit(workspace: Workspace, action: (draft: Workspace) => void): Workspace {
  const draft = structuredClone(workspace);
  action(draft);
  return validateWorkspace(draft);
}

export function hasPath(workspace: Workspace, path: string): boolean {
  return (
    path === workspaceRoot || workspace.folders.includes(path) || workspace.files.some((file) => file.path === path)
  );
}

export function addFolder(workspace: Workspace, path: string): Workspace {
  return edit(workspace, (draft) => {
    if (hasPath(draft, path)) throw new Error("The destination already exists.");
    draft.folders.push(path);
  });
}

export function putFile(workspace: Workspace, path: string, source: string, replace = false): Workspace {
  return edit(workspace, (draft) => {
    const existing = draft.files.find((file) => file.path === path);
    if (existing && replace) existing.source = source;
    else {
      if (hasPath(draft, path)) throw new Error("The destination already exists.");
      draft.files.push(createFile(path, source));
    }
  });
}

function remove(draft: Workspace, path: string) {
  draft.files = draft.files.filter((file) => !isWithin(file.path, path));
  draft.folders = draft.folders.filter((folder) => !isWithin(folder, path));
  draft.options.includeDirectories = draft.options.includeDirectories.filter((folder) => !isWithin(folder, path));
  if (!draft.files.some((file) => file.id === draft.activeFileId)) delete draft.activeFileId;
}

export function deleteEntry(workspace: Workspace, path: string): Workspace {
  validatePath(path);
  return edit(workspace, (draft) => remove(draft, path));
}

export function moveEntry(workspace: Workspace, path: string, destination: string, replace = false): Workspace {
  validatePath(path);
  validatePath(destination);
  if (path === destination) return structuredClone(workspace);
  if (!hasPath(workspace, path) || isWithin(destination, path) || isWithin(path, destination))
    throw new Error("Choose a destination outside the moved item and its parents.");
  const parent = parentPath(destination);
  if (parent !== workspaceRoot && !workspace.folders.includes(parent))
    throw new Error("Destination folder does not exist.");
  return edit(workspace, (draft) => {
    if (hasPath(draft, destination)) {
      if (!replace) throw new Error("The destination already exists.");
      remove(draft, destination);
    }
    const moved = (value: string) => (isWithin(value, path) ? destination + value.slice(path.length) : value);
    draft.files.forEach((file) => {
      file.path = moved(file.path);
    });
    draft.folders = draft.folders.map(moved);
    draft.options.includeDirectories = draft.options.includeDirectories.map(moved);
  });
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
    if (value === undefined) return undefined;
    return validateWorkspace(value);
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
