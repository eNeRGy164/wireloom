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

interface Callbacks {
  current(): { workspace: Workspace; activeId?: string; revision: number };
  change(workspace: Workspace, activeId?: string): void;
  select(id: string): void;
  notice(message: string, kind?: "info" | "error"): void;
}

/** Presents the virtual source tree; the compiler receives only immutable snapshots. */
export class SourceWorkbench {
  private selected = workspaceRoot;
  private expanded = new Set([workspaceRoot]);
  private knownFolders = new Set<string>();
  private menu?: HTMLElement;
  private draggedPath?: string;
  private importTicket = 0;

  constructor(
    private tree: HTMLElement,
    private tabs: HTMLElement,
    private callbacks: Callbacks,
  ) {
    document.getElementById("new-file")!.addEventListener("click", () => void this.create(false));
    document.getElementById("new-folder")!.addEventListener("click", () => void this.create(true));
    document.getElementById("collapse-folders")!.addEventListener("click", () => {
      this.expanded = new Set([workspaceRoot]);
      this.selected = workspaceRoot;
      this.render();
    });
    document
      .getElementById("source-actions")!
      .addEventListener("click", (event) => this.contextMenu(event as MouseEvent));
    document.getElementById("workspace-settings")!.addEventListener("click", () => void this.settings());
    document
      .getElementById("file-properties")!
      .addEventListener("click", () => void this.settings(this.callbacks.current().activeId));
    this.tree.addEventListener("keydown", (event) => this.navigate(event));
    document.addEventListener("pointerdown", (event) => {
      if (this.menu && !this.menu.contains(event.target as Node)) this.closeMenu();
    });
  }

  private destination() {
    const { workspace } = this.callbacks.current();
    return workspace.files.some((file) => file.path === this.selected) ? parentPath(this.selected) : this.selected;
  }

  render() {
    const { workspace, activeId } = this.callbacks.current();
    const folders = new Set([workspaceRoot, ...workspace.folders]);
    for (const folder of folders) if (!this.knownFolders.has(folder)) this.expanded.add(folder);
    for (const folder of this.expanded) if (!folders.has(folder)) this.expanded.delete(folder);
    this.knownFolders = folders;
    const focused = this.tree.contains(document.activeElement);
    if (!hasPath(workspace, this.selected)) this.selected = workspaceRoot;
    const paths = [workspaceRoot, ...workspace.folders, ...workspace.files.map((file) => file.path)];
    const nodes: HTMLElement[] = [];
    const visit = (path: string, level: number) => {
      const file = workspace.files.find((file) => file.path === path);
      const row = document.createElement("div");
      row.className = "source-tree-item";
      row.dataset.path = path;
      row.setAttribute("role", "treeitem");
      row.setAttribute("aria-level", String(level));
      row.setAttribute("aria-selected", String(path === this.selected));
      if (!file) row.setAttribute("aria-expanded", String(this.expanded.has(path)));
      row.tabIndex = path === this.selected ? 0 : -1;
      row.addEventListener("focus", () => {
        this.selected = path;
        for (const item of this.tree.querySelectorAll<HTMLElement>('[role="treeitem"]')) {
          item.tabIndex = item === row ? 0 : -1;
          item.setAttribute("aria-selected", String(item === row));
        }
      });
      row.style.paddingLeft = `${(level - 1) * 14 + 8}px`;
      row.textContent = path.split("/").pop()!;
      if (!file) {
        const chevron = document.createElement("span");
        chevron.className = "explorer-chevron";
        chevron.setAttribute("aria-hidden", "true");
        row.prepend(chevron);
      }
      row.title = file ? `${path} · ${file.isGenerationRoot ? "Root" : "Include-only"}` : path;
      if (file) {
        const icon = document.createElement("span");
        icon.className = "file-icon file-icon-idl";
        icon.setAttribute("aria-hidden", "true");
        row.prepend(icon);
        const badge = document.createElement("span");
        badge.className = "source-role";
        badge.textContent = file.isGenerationRoot ? "Root" : "Include-only";
        row.append(badge);
      }
      row.addEventListener("click", () => {
        this.selected = path;
        if (file) this.callbacks.select(file.id);
        else {
          if (this.expanded.has(path)) this.expanded.delete(path);
          else this.expanded.add(path);
          this.render();
        }
        this.focusSelected();
      });
      row.addEventListener("contextmenu", (event) => {
        event.preventDefault();
        this.selected = path;
        this.render();
        this.contextMenu(event);
      });
      row.draggable = path !== workspaceRoot;
      row.addEventListener("dragstart", (event) => {
        this.draggedPath = path;
        if (event.dataTransfer) {
          event.dataTransfer.effectAllowed = "move";
          event.dataTransfer.setData("text/plain", path);
          event.dataTransfer.setData("application/x-wireloom-path", path);
        }
      });
      row.addEventListener("dragend", () => {
        this.draggedPath = undefined;
      });
      row.addEventListener("dragover", (event) => {
        // WebKit may hide custom MIME types during native dragover events.
        if (!file && this.draggedPath) {
          event.preventDefault();
          if (event.dataTransfer) event.dataTransfer.dropEffect = "move";
        }
      });
      row.addEventListener("drop", (event) => {
        event.preventDefault();
        const source = this.draggedPath;
        this.draggedPath = undefined;
        if (source && !file) void this.move(source, `${path}/${source.split("/").pop()}`);
      });
      nodes.push(row);
      if (!file && this.expanded.has(path)) {
        const children = paths.filter((child) => child !== workspaceRoot && parentPath(child) === path);
        children.sort(
          (a, b) =>
            Number(workspace.folders.includes(b)) - Number(workspace.folders.includes(a)) ||
            (a < b ? -1 : a > b ? 1 : 0),
        );
        children.forEach((child) => visit(child, level + 1));
      }
    };
    visit(workspaceRoot, 1);
    this.tree.replaceChildren(...nodes);
    if (focused) this.focusSelected();
    this.tabs.replaceChildren(
      ...workspace.files.map((file, index) => {
        const tab = document.createElement("button");
        tab.id = `source-tab-${index}`;
        tab.textContent = file.path.split("/").pop()!;
        tab.title = file.path;
        const icon = document.createElement("span");
        icon.className = "file-icon file-icon-idl";
        icon.setAttribute("aria-hidden", "true");
        tab.prepend(icon);
        tab.setAttribute("role", "tab");
        tab.setAttribute("aria-selected", String(file.id === activeId));
        tab.setAttribute("aria-controls", "source-panel");
        tab.tabIndex = file.id === activeId ? 0 : -1;
        if (file.id === activeId) document.getElementById("source-panel")!.setAttribute("aria-labelledby", tab.id);
        tab.addEventListener("click", () => this.selectFile(file.id));
        tab.addEventListener("keydown", (event) => {
          const direction = event.key === "ArrowRight" ? 1 : event.key === "ArrowLeft" ? -1 : 0;
          if (!direction && event.key !== "Home" && event.key !== "End") return;
          event.preventDefault();
          const next =
            event.key === "Home"
              ? 0
              : event.key === "End"
                ? workspace.files.length - 1
                : (index + direction + workspace.files.length) % workspace.files.length;
          this.selectFile(workspace.files[next].id);
          this.tabs.querySelector<HTMLButtonElement>('[aria-selected="true"]')?.focus();
        });
        return tab;
      }),
    );
    for (const id of ["file-properties", "copy-source", "download-source-file"])
      (document.getElementById(id) as HTMLButtonElement).disabled = !activeId;
    document.getElementById("empty-source")!.hidden = workspace.files.length > 0;
    if (!workspace.files.length) document.getElementById("source-panel")!.removeAttribute("aria-labelledby");
    document.getElementById("source-count")!.textContent =
      `${workspace.files.length} files · ${workspace.files.filter((file) => file.isGenerationRoot).length} Roots`;
  }

  selectFile(id: string) {
    const file = this.callbacks.current().workspace.files.find((file) => file.id === id);
    if (!file) return;
    this.revealSourceGroup();
    this.selected = file.path;
    let path = parentPath(file.path);
    while (path.startsWith(workspaceRoot)) {
      this.expanded.add(path);
      path = parentPath(path);
    }
    this.callbacks.select(id);
    this.tabs.querySelector('[aria-selected="true"]')?.scrollIntoView({ block: "nearest", inline: "nearest" });
  }

  private focusSelected() {
    [...this.tree.querySelectorAll<HTMLElement>('[role="treeitem"]')]
      .find((row) => row.dataset.path === this.selected)
      ?.focus();
  }

  private navigate(event: KeyboardEvent) {
    if (event.key === "ContextMenu" || (event.shiftKey && event.key === "F10")) {
      event.preventDefault();
      this.contextMenu();
      return;
    }
    const rows = [...this.tree.querySelectorAll<HTMLElement>('[role="treeitem"]')];
    const index = rows.findIndex((row) => row.dataset.path === this.selected);
    let target = index;
    if (event.key === "ArrowDown") target = Math.min(rows.length - 1, index + 1);
    else if (event.key === "ArrowUp") target = Math.max(0, index - 1);
    else if (event.key === "Home") target = 0;
    else if (event.key === "End") target = rows.length - 1;
    else if (event.key === "ArrowRight") {
      if (rows[index]?.getAttribute("aria-expanded") === "false") {
        this.expanded.add(this.selected);
        this.render();
      } else if (rows[index]?.hasAttribute("aria-expanded")) target = Math.min(rows.length - 1, index + 1);
    } else if (event.key === "ArrowLeft") {
      if (this.expanded.has(this.selected)) {
        this.expanded.delete(this.selected);
        this.render();
      } else target = rows.findIndex((row) => row.dataset.path === parentPath(this.selected));
    } else if (event.key === "Enter" || event.key === " ") {
      event.preventDefault();
      rows[index]?.click();
      return;
    } else return;
    event.preventDefault();
    if (target >= 0) this.selected = rows[target].dataset.path!;
    this.render();
    this.focusSelected();
  }

  private closeMenu() {
    this.menu?.remove();
    this.menu = undefined;
  }

  private revealSourceGroup() {
    const toggle = document.getElementById("source-group-toggle")!;
    if (toggle.getAttribute("aria-expanded") === "false") toggle.click();
  }

  private toggleIncludeFolder(folder: string) {
    const next = structuredClone(this.callbacks.current().workspace);
    if (next.options.includeDirectories.includes(folder))
      next.options.includeDirectories = next.options.includeDirectories.filter((path) => path !== folder);
    else next.options.includeDirectories.push(folder);
    try {
      this.callbacks.change(validateWorkspace(next));
    } catch (error) {
      this.callbacks.notice(String(error), "error");
    }
  }

  private toggleRoot(id: string) {
    const next = structuredClone(this.callbacks.current().workspace);
    const file = next.files.find((file) => file.id === id);
    if (!file) return;
    file.isGenerationRoot = !file.isGenerationRoot;
    this.callbacks.change(next);
  }

  private contextMenu(event?: MouseEvent) {
    this.closeMenu();
    const menu = document.createElement("div");
    menu.className = "source-context-menu";
    menu.setAttribute("role", "menu");
    menu.setAttribute("aria-label", `Actions for ${this.selected}`);
    const { workspace } = this.callbacks.current();
    const file = workspace.files.find((file) => file.path === this.selected);
    const actions: [string, () => void][] = [];
    if (!file)
      actions.push(
        ["New File…", () => void this.create(false)],
        ["New Folder…", () => void this.create(true)],
        ["Import Files…", () => document.getElementById("import")!.click()],
      );
    if (this.selected !== workspaceRoot)
      actions.push(
        ["Rename…", () => void this.rename()],
        ["Move to…", () => void this.move(this.selected)],
        ["Delete", () => void this.delete()],
      );
    if (!file) {
      const folder = this.selected;
      const included = workspace.options.includeDirectories.includes(folder);
      actions.push([
        `${included ? "Remove Folder from" : "Add Folder to"} Include Folders`,
        () => this.toggleIncludeFolder(folder),
      ]);
    }
    if (file)
      actions.push(
        [file.isGenerationRoot ? "Mark as Include-only" : "Mark as Root", () => this.toggleRoot(file.id)],
        ["Properties…", () => void this.settings(file.id)],
      );
    for (const [label, action] of actions) {
      const button = document.createElement("button");
      button.type = "button";
      button.textContent = label;
      button.setAttribute("role", "menuitem");
      button.addEventListener("click", () => {
        this.closeMenu();
        this.focusSelected();
        action();
      });
      menu.append(button);
    }
    const bounds = this.tree.getBoundingClientRect();
    menu.addEventListener("keydown", (event) => {
      const buttons = [...menu.querySelectorAll("button")];
      const index = buttons.indexOf(document.activeElement as HTMLButtonElement);
      if (event.key === "Escape") {
        this.closeMenu();
        this.focusSelected();
      } else if (["ArrowDown", "ArrowUp", "Home", "End"].includes(event.key)) {
        event.preventDefault();
        buttons[
          event.key === "Home"
            ? 0
            : event.key === "End"
              ? buttons.length - 1
              : (index + (event.key === "ArrowDown" ? 1 : -1) + buttons.length) % buttons.length
        ].focus();
      } else if (event.key === "Tab") this.closeMenu();
    });
    document.body.append(menu);
    menu.style.left = `${Math.max(8, Math.min(event?.clientX || bounds.left + 12, window.innerWidth - menu.offsetWidth - 8))}px`;
    menu.style.top = `${Math.max(8, Math.min(event?.clientY || bounds.top, window.innerHeight - menu.offsetHeight - 8))}px`;
    this.menu = menu;
    menu.querySelector("button")?.focus();
  }

  private async create(folder: boolean) {
    const destination = this.destination();
    const name = await askPath(folder ? "New Folder" : "New File", "Name", folder ? "folder" : "untitled.idl");
    if (name === undefined) return;
    try {
      if (name.includes("/")) throw new Error("Enter one name. Select its parent folder in the Explorer.");
      const path = `${destination}/${name}`;
      const { workspace } = this.callbacks.current();
      const next = folder ? addFolder(workspace, path) : putFile(workspace, path, "");
      this.selected = path;
      this.expanded.add(destination);
      this.revealSourceGroup();
      this.callbacks.change(next, next.files.find((file) => file.path === path)?.id);
    } catch (error) {
      this.callbacks.notice(String(error), "error");
    }
  }

  private async rename() {
    const path = this.selected;
    const name = await askPath("Rename", "New name", path.split("/").pop()!);
    if (name !== undefined) {
      if (name.includes("/")) {
        this.callbacks.notice("Use Move to… to change the parent folder.", "error");
        return;
      }
      await this.move(path, `${parentPath(path)}/${name}`);
    }
  }

  private async move(path: string, destination?: string) {
    const { workspace, revision } = this.callbacks.current();
    destination ??= await askPath("Move to", "Destination folder (virtual path)", this.destination()).then((folder) =>
      folder === undefined ? undefined : `${folder}/${path.split("/").pop()}`,
    );
    if (destination === undefined) return;
    try {
      let replace = false;
      while (hasPath(workspace, destination) && destination !== path) {
        const choice = await choose(
          "Path conflict",
          `${destination} already exists. Replacing a folder removes its contents.`,
          ["Replace", "Rename"],
        );
        if (!choice) return;
        if (choice === "Replace") {
          replace = true;
          break;
        }
        destination = await askPath("Rename destination", "Full destination path", destination);
        if (destination === undefined) return;
      }
      if (revision !== this.callbacks.current().revision)
        throw new Error("The workspace changed while the dialog was open. Try the move again.");
      const next = moveEntry(workspace, path, destination, replace);
      this.selected = destination;
      this.expanded.add(parentPath(destination));
      this.callbacks.change(next);
    } catch (error) {
      this.callbacks.notice(String(error), "error");
    }
  }

  private async delete() {
    const path = this.selected;
    const { workspace, revision } = this.callbacks.current();
    const nonempty =
      workspace.files.some((file) => file.path !== path && isWithin(file.path, path)) ||
      workspace.folders.some((folder) => folder !== path && isWithin(folder, path));
    const choice = await choose("Delete", nonempty ? `Delete ${path} and all its contents?` : `Delete ${path}?`, [
      "Delete",
    ]);
    if (!choice) return;
    try {
      if (revision !== this.callbacks.current().revision) throw new Error("The workspace changed. Try deleting again.");
      this.callbacks.change(deleteEntry(workspace, path));
    } catch (error) {
      this.callbacks.notice(String(error), "error");
    }
  }

  private async settings(fileId?: string) {
    const { workspace, revision } = this.callbacks.current();
    const next = await settingsDialog(workspace, fileId);
    if (!next) return;
    if (revision !== this.callbacks.current().revision) {
      this.callbacks.notice("The workspace changed while settings were open. Apply the settings again.", "error");
      return;
    }
    this.callbacks.change(next);
  }

  cancelImport() {
    this.importTicket++;
  }

  async import(files: File[]) {
    if (!files.length) return;
    const ticket = ++this.importTicket;
    const { workspace, revision } = this.callbacks.current();
    const destination = this.destination();
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
      if (revision !== this.callbacks.current().revision)
        throw new Error("The workspace changed during import. Import again to use the latest workspace.");
      this.expanded.add(destination);
      this.revealSourceGroup();
      if (!draft.files.some((file) => file.id === draft.activeFileId)) delete draft.activeFileId;
      this.callbacks.change(validateWorkspace(draft), activeId);
      this.callbacks.notice(`Imported files into ${destination}. Changes remain in this browser.`);
    } catch (error) {
      if (ticket === this.importTicket)
        this.callbacks.notice(`Could not import files. ${String(error)} Your workspace is unchanged.`, "error");
    }
  }
}
