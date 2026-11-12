import type { Document } from "../protocol";
import type { Editors } from "../editors";
import type { OutputTabStrip } from "./output-tab-strip";

interface GeneratedFilesViewOptions {
  editors: Editors;
  tabStrip: OutputTabStrip;
  icon(name: string): HTMLElement;
  projectDocument(): Document;
  exportStateChanged(): void;
}

interface ResultSummary {
  target: string;
  profile: string;
  revision: number;
}

const element = <T extends HTMLElement = HTMLElement>(id: string) => document.getElementById(id) as T;
const fileNameOrder = new Intl.Collator(undefined, { numeric: true, sensitivity: "base" });

/** Owns generated-file selection, tabs, and their context menus. */
export class GeneratedFilesView {
  private latestDocuments: Document[] = [];
  private activeName: string | undefined;
  private openNames: string[] = [];
  private readonly closedNames = new Set<string>();
  private exportable = false;
  private fileMenu: HTMLElement | undefined;

  constructor(private readonly options: GeneratedFilesViewOptions) {}

  get documents() {
    return this.latestDocuments;
  }

  get activeDocumentName() {
    return this.activeName;
  }

  get canExport() {
    return this.exportable;
  }

  invalidate() {
    this.exportable = false;
    this.options.exportStateChanged();
  }

  clear(message: string) {
    this.closeFileMenu();
    element("generated-panel").removeAttribute("aria-labelledby");
    this.latestDocuments = [];
    this.openNames = [];
    this.closedNames.clear();
    this.activeName = undefined;
    this.exportable = false;
    this.options.editors.setOutputs([]);
    element("file-list").replaceChildren();
    element("explorer-count").textContent = "0";
    element("explorer-empty").hidden = false;
    element("output-tabs").replaceChildren();
    this.options.tabStrip.update();
    element("document-count").textContent = "0";
    element("empty-output").hidden = false;
    element("result-summary").textContent = message;
    element("generated").classList.remove("outdated");
    this.options.exportStateChanged();
  }

  select(name: string) {
    this.closedNames.delete(name);
    if (!this.openNames.includes(name)) this.openNames.push(name);
    this.activeName = name;
    this.options.editors.select(name);

    for (const button of element("file-list").querySelectorAll("button")) {
      if (button.dataset.name === name) button.setAttribute("aria-current", "true");
      else button.removeAttribute("aria-current");
    }

    this.renderTabs();
    element("empty-output").hidden = this.openNames.length > 0;
    element("output-tabs")
      .querySelector('[aria-selected="true"]')
      ?.scrollIntoView({ block: "nearest", inline: "nearest" });
    this.options.exportStateChanged();
  }

  applyDocuments(documents: Document[], anchor: ReturnType<Editors["captureOutputAnchor"]>, summary: ResultSummary) {
    const previousActive = this.activeName;
    const preview = this.options.projectDocument();
    this.latestDocuments = documents;
    this.options.editors.setOutputs([...documents, preview]);
    const previewWasOpen = this.openNames.includes(preview.name) && !this.closedNames.has(preview.name);
    this.openNames = [
      ...documents.filter((doc) => !this.closedNames.has(doc.name)).map((doc) => doc.name),
      ...(previewWasOpen ? [preview.name] : []),
    ];

    const explorerDocuments = [...documents].sort((left, right) => fileNameOrder.compare(left.name, right.name));
    this.closeFileMenu();
    element("file-list").replaceChildren(
      ...[...explorerDocuments, preview].map((doc) => {
        const item = document.createElement("li");
        const button = document.createElement("button");
        button.textContent = doc.name;
        button.prepend(this.options.icon(doc.name));
        button.title = doc.name;
        button.dataset.name = doc.name;
        button.setAttribute("aria-haspopup", "menu");
        button.addEventListener("click", () => this.select(doc.name));
        button.addEventListener("contextmenu", (event) => this.showFileMenu(event, doc.name));
        item.append(button);
        return item;
      }),
    );

    element("explorer-count").textContent = String(documents.length + 1);
    element("explorer-empty").hidden = true;

    if (this.openNames.length) {
      const match = anchor ? this.options.editors.locateOutputAnchor(anchor, [...documents, preview]) : undefined;
      const selected =
        match?.name ?? (this.openNames.includes(previousActive ?? "") ? previousActive! : this.openNames[0]);
      this.select(this.openNames.includes(selected) ? selected : this.openNames[0]);
      if (match) this.options.editors.restoreOutputAnchor(match, anchor?.scrollTop);
    } else {
      this.activeName = undefined;
      element("output-tabs").replaceChildren();
      this.options.tabStrip.update();
    }

    element("document-count").textContent = String(documents.length);
    element("empty-output").hidden = this.openNames.length > 0;
    element("result-state").textContent = "Current";
    element("result-state").dataset.state = "success";
    element("result-summary").textContent = documents.length
      ? `${documents.length} files · ${summary.target} · RTI ${summary.profile} · revision ${summary.revision}`
      : "Generation succeeded · no documents";
    element("generated").classList.remove("outdated");
    this.exportable = true;
    this.options.exportStateChanged();
  }

  private renderTabs() {
    element("output-tabs").replaceChildren(
      ...this.openNames.map((name, index) => {
        const tab = document.createElement("button");
        const wrapper = document.createElement("div");
        wrapper.className = "editor-tab";
        wrapper.setAttribute("role", "presentation");
        tab.id = `generated-tab-${index}`;
        tab.className = "editor-tab-label";
        tab.textContent = name;
        tab.title = name;
        tab.prepend(this.options.icon(name));
        tab.setAttribute("role", "tab");
        tab.setAttribute("aria-selected", String(name === this.activeName));
        tab.setAttribute("aria-controls", "generated-panel");
        if (name === this.activeName) element("generated-panel").setAttribute("aria-labelledby", tab.id);
        tab.tabIndex = name === this.activeName ? 0 : -1;
        tab.addEventListener("click", () => {
          this.select(name);
          element("output-tabs").querySelector<HTMLButtonElement>('[aria-selected="true"]')!.focus();
        });
        tab.addEventListener("contextmenu", (event) => {
          event.preventDefault();
          event.stopPropagation();
          this.showTabMenu(event, name);
        });
        tab.addEventListener("auxclick", (event) => {
          if (event.button !== 1) return;
          event.preventDefault();
          event.stopPropagation();
          this.close(name);
        });
        tab.addEventListener("keydown", (event) => {
          const direction = event.key === "ArrowRight" ? 1 : event.key === "ArrowLeft" ? -1 : 0;
          if (!direction && event.key !== "Home" && event.key !== "End") return;
          event.preventDefault();
          const next =
            event.key === "Home"
              ? 0
              : event.key === "End"
                ? this.openNames.length - 1
                : (index + direction + this.openNames.length) % this.openNames.length;
          this.select(this.openNames[next]);
          element("output-tabs").querySelector<HTMLButtonElement>('[aria-selected="true"]')!.focus();
        });
        const close = document.createElement("button");
        close.type = "button";
        close.className = "editor-tab-close";
        close.textContent = "×";
        close.title = `Close ${name}`;
        close.setAttribute("aria-label", `Close ${name}`);
        close.addEventListener("click", (event) => {
          event.stopPropagation();
          this.close(name);
        });
        wrapper.append(tab, close);
        return wrapper;
      }),
    );
    this.options.tabStrip.update();
  }

  private close(name: string) {
    const index = this.openNames.indexOf(name);
    if (index < 0) return;
    this.openNames = this.openNames.filter((item) => item !== name);
    this.closedNames.add(name);
    if (this.activeName === name) {
      const next = this.openNames[index] ?? this.openNames[index - 1];
      if (next) {
        this.select(next);
        element("output-tabs").querySelector<HTMLButtonElement>('[aria-selected="true"]')?.focus();
      } else {
        this.activeName = undefined;
        this.options.editors.select("");
        element("generated-panel").removeAttribute("aria-labelledby");
        for (const button of element("file-list").querySelectorAll("button")) button.removeAttribute("aria-current");
        this.renderTabs();
      }
    } else this.renderTabs();
    element("empty-output").hidden = this.openNames.length > 0;
    this.options.exportStateChanged();
  }

  private closeAll() {
    if (!this.openNames.length) return;
    for (const name of this.openNames) this.closedNames.add(name);
    this.openNames = [];
    this.activeName = undefined;
    this.options.editors.select("");
    element("generated-panel").removeAttribute("aria-labelledby");
    for (const button of element("file-list").querySelectorAll("button")) button.removeAttribute("aria-current");
    this.renderTabs();
    element("empty-output").hidden = false;
    this.options.exportStateChanged();
  }

  private closeFileMenu() {
    this.fileMenu?.remove();
    this.fileMenu = undefined;
    document.removeEventListener("pointerdown", this.dismissFileMenu);
    document.removeEventListener("keydown", this.dismissFileMenuOnEscape);
  }

  private readonly dismissFileMenu = (event: Event) => {
    if (this.fileMenu && !this.fileMenu.contains(event.target as Node)) this.closeFileMenu();
  };

  private readonly dismissFileMenuOnEscape = (event: KeyboardEvent) => {
    if (event.key === "Escape") this.closeFileMenu();
  };

  private showFileMenu(event: MouseEvent, name: string) {
    event.preventDefault();
    this.closeFileMenu();
    const menu = document.createElement("div");
    menu.className = "source-context-menu";
    menu.setAttribute("role", "menu");
    menu.setAttribute("aria-label", `Actions for ${name}`);
    const isOpen = this.openNames.includes(name);
    const action = document.createElement("button");
    action.type = "button";
    action.textContent = isOpen ? "Close" : "Open";
    action.setAttribute("role", "menuitem");
    action.addEventListener("click", () => {
      this.closeFileMenu();
      if (isOpen) this.close(name);
      else this.select(name);
    });
    menu.append(action);
    menu.addEventListener("keydown", (keyEvent) => {
      if (keyEvent.key === "Escape") {
        keyEvent.preventDefault();
        this.closeFileMenu();
      } else if (keyEvent.key === "Enter" || keyEvent.key === " ") action.click();
    });
    this.positionMenu(menu, event.clientX, event.clientY);
    this.fileMenu = menu;
    document.addEventListener("pointerdown", this.dismissFileMenu);
    document.addEventListener("keydown", this.dismissFileMenuOnEscape);
    action.focus();
  }

  private showTabMenu(event: MouseEvent, name: string) {
    this.closeFileMenu();
    const menu = document.createElement("div");
    menu.className = "source-context-menu";
    menu.setAttribute("role", "menu");
    menu.setAttribute("aria-label", `Actions for ${name}`);
    const actions: [string, () => void][] = [
      ["Close", () => this.close(name)],
      ["Close All", () => this.closeAll()],
    ];
    for (const [label, action] of actions) {
      const button = document.createElement("button");
      button.type = "button";
      button.textContent = label;
      button.setAttribute("role", "menuitem");
      button.addEventListener("click", () => {
        this.closeFileMenu();
        action();
      });
      menu.append(button);
    }
    menu.addEventListener("keydown", (keyEvent) => {
      if (keyEvent.key === "Escape") {
        keyEvent.preventDefault();
        this.closeFileMenu();
      }
    });
    this.positionMenu(menu, event.clientX, event.clientY);
    this.fileMenu = menu;
    document.addEventListener("pointerdown", this.dismissFileMenu);
    document.addEventListener("keydown", this.dismissFileMenuOnEscape);
    menu.querySelector<HTMLButtonElement>("button")?.focus();
  }

  private positionMenu(menu: HTMLElement, x: number, y: number) {
    document.body.append(menu);
    menu.style.left = `${Math.max(8, Math.min(x, window.innerWidth - menu.offsetWidth - 8))}px`;
    menu.style.top = `${Math.max(8, Math.min(y, window.innerHeight - menu.offsetHeight - 8))}px`;
  }
}
