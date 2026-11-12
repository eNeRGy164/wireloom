import {
  addFolder,
  createFile,
  deleteEntry,
  hasPath,
  isWithin,
  maxSourceBytes,
  moveEntry,
  parentPath,
  putFile,
  validatePath,
  validateWorkspace,
  workspaceRoot,
  type Workspace,
} from "./workspace";
import { askPath, choose, settingsDialog } from "./workspace-dialogs";

interface WorkspaceFileActionsOptions {
  current(): { workspace: Workspace; revision: number };
  change(workspace: Workspace, activeId?: string): void;
  notice(message: string, kind?: "info" | "error"): void;
  destination(): string;
  selectedPath(): string | undefined;
  selectPath(path: string): void;
  expand(path: string): void;
  revealSourceGroup(): void;
}

/** Coordinates virtual-file operations and their conflict/confirmation dialogs. */
export class WorkspaceFileActions {
  private importTicket = 0;

  constructor(private readonly options: WorkspaceFileActionsOptions) {}

  async create(folder: boolean) {
    const destination = this.options.destination();
    const name = await askPath(folder ? "New Folder" : "New File", "Name", folder ? "folder" : "untitled.idl");
    if (name === undefined) return;
    try {
      if (name.includes("/")) throw new Error("Enter one name. Select its parent folder in the Explorer.");
      const path = `${destination}/${name}`;
      const { workspace } = this.options.current();
      const next = folder ? addFolder(workspace, path) : putFile(workspace, path, "");
      this.options.selectPath(path);
      this.options.expand(destination);
      this.options.revealSourceGroup();
      this.options.change(next, next.files.find((file) => file.path === path)?.id);
    } catch (error) {
      this.options.notice(String(error), "error");
    }
  }

  async rename() {
    const path = this.options.selectedPath();
    if (!path || path === workspaceRoot) return;
    const name = await askPath("Rename", "New name", path.split("/").pop()!);
    if (name === undefined) return;
    if (name.includes("/")) {
      this.options.notice("Use Move to… to change the parent folder.", "error");
      return;
    }
    await this.move(path, `${parentPath(path)}/${name}`);
  }

  async move(path: string, destination?: string) {
    const { workspace, revision } = this.options.current();
    try {
      destination ??= await askPath(
        "Move to",
        "Destination folder (relative to workspace; leave blank for root)",
        this.relativePath(this.options.destination()),
        false,
      ).then((folder) =>
        folder === undefined ? undefined : `${this.resolveRelativePath(folder)}/${path.split("/").pop()}`,
      );
      if (destination === undefined) return;
      let replace = false;
      while (hasPath(workspace, destination) && destination !== path) {
        const choice = await choose(
          "Path conflict",
          `${this.relativePath(destination)} already exists. Replacing a folder removes its contents.`,
          ["Replace", "Rename"],
        );
        if (!choice) return;
        if (choice === "Replace") {
          replace = true;
          break;
        }
        const renamed = await askPath(
          "Rename destination",
          "Full destination path relative to workspace",
          this.relativePath(destination),
        );
        if (renamed === undefined) return;
        destination = this.resolveRelativePath(renamed);
      }
      if (revision !== this.options.current().revision)
        throw new Error("The workspace changed while the dialog was open. Try the move again.");
      const next = moveEntry(workspace, path, destination, replace);
      this.options.selectPath(destination);
      this.options.expand(parentPath(destination));
      this.options.change(next);
    } catch (error) {
      this.options.notice(String(error), "error");
    }
  }

  async delete() {
    const path = this.options.selectedPath();
    if (!path || path === workspaceRoot) return;
    const { workspace, revision } = this.options.current();
    const nonempty =
      workspace.files.some((file) => file.path !== path && isWithin(file.path, path)) ||
      workspace.folders.some((folder) => folder !== path && isWithin(folder, path));
    const displayPath = this.relativePath(path);
    const choice = await choose(
      "Delete",
      nonempty ? `Delete ${displayPath} and all its contents?` : `Delete ${displayPath}?`,
      ["Delete"],
    );
    if (!choice) return;
    try {
      if (revision !== this.options.current().revision) throw new Error("The workspace changed. Try deleting again.");
      this.options.change(deleteEntry(workspace, path));
    } catch (error) {
      this.options.notice(String(error), "error");
    }
  }

  async settings(fileId?: string) {
    const { workspace, revision } = this.options.current();
    const next = await settingsDialog(workspace, fileId);
    if (!next) return;
    if (revision !== this.options.current().revision) {
      this.options.notice("The workspace changed while settings were open. Apply the settings again.", "error");
      return;
    }
    this.options.change(next);
  }

  cancelImport() {
    this.importTicket++;
  }

  async import(files: File[]) {
    if (!files.length) return;
    const ticket = ++this.importTicket;
    const { workspace, revision } = this.options.current();
    const destination = this.options.destination();
    try {
      if (
        files.length > 512 ||
        files.some((file) => file.size > maxSourceBytes) ||
        files.reduce((total, file) => total + file.size, 0) > maxSourceBytes
      )
        throw new Error("Choose at most 512 files with at most 256 KiB of UTF-8 source in total.");
      const imported = await Promise.all(
        files.map(async (file) => ({
          name: file.name,
          source: new TextDecoder("utf-8", { fatal: true }).decode(await file.arrayBuffer()),
        })),
      );
      const draft = structuredClone(workspace);
      let activeId: string | undefined;
      for (const file of imported) {
        if (ticket !== this.importTicket) return;
        if (file.name.includes("/") || file.name.includes("\\")) throw new Error("Import uses file basenames only.");
        let path = `${destination}/${file.name}`;
        let skip = false;
        while (hasPath(draft, path)) {
          const choice = await choose(
            "Import conflict",
            `${path} already exists. Replacing a folder removes its contents.`,
            ["Replace", "Skip", "Rename"],
          );
          if (!choice) return;
          if (choice === "Skip") {
            skip = true;
            break;
          }
          if (choice === "Replace") break;
          const name = await askPath("Rename import", "New basename", path.split("/").pop()!);
          if (name === undefined) return;
          if (name.includes("/") || name.includes("\\")) throw new Error("Import uses file basenames only.");
          path = `${destination}/${name}`;
        }
        if (skip) continue;
        validatePath(path);
        const existing = draft.files.find((entry) => entry.path === path);
        if (existing) {
          existing.source = file.source;
          activeId = existing.id;
        } else {
          draft.files = draft.files.filter((entry) => !isWithin(entry.path, path));
          draft.folders = draft.folders.filter((folder) => !isWithin(folder, path));
          draft.options.includeDirectories = draft.options.includeDirectories.filter(
            (folder) => !isWithin(folder, path),
          );
          const added = createFile(path, file.source);
          draft.files.push(added);
          activeId = added.id;
        }
      }
      if (ticket !== this.importTicket) return;
      if (revision !== this.options.current().revision)
        throw new Error("The workspace changed during import. Import again to use the latest workspace.");
      this.options.expand(destination);
      this.options.revealSourceGroup();
      if (!draft.files.some((file) => file.id === draft.activeFileId)) delete draft.activeFileId;
      this.options.change(validateWorkspace(draft), activeId);
      this.options.notice(`Imported files into ${destination}. Changes remain in this browser.`);
    } catch (error) {
      if (ticket === this.importTicket)
        this.options.notice(`Could not import files. ${String(error)} Your workspace is unchanged.`, "error");
    }
  }

  private relativePath(path: string) {
    return path === workspaceRoot ? "" : path.slice(workspaceRoot.length + 1);
  }

  private resolveRelativePath(path: string) {
    const value = path.trim();
    if (!value || value === ".") return workspaceRoot;
    if (value.startsWith("/")) throw new Error("Enter a path relative to the workspace root.");
    const fullPath = `${workspaceRoot}/${value}`;
    validatePath(fullPath, true);
    return fullPath;
  }
}
