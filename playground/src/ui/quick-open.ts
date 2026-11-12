import { fuzzyScore } from "../quick-open-search";

type QuickOpenItem =
  { kind: "source"; name: string; id: string; score: number } | { kind: "generated"; name: string; score: number };

interface QuickOpenOptions {
  dialog: HTMLDialogElement;
  input: HTMLInputElement;
  results: HTMLUListElement;
  button: HTMLButtonElement;
  sourceFiles: () => { id: string; path: string }[];
  generatedFiles: () => string[];
  icon: (kind: QuickOpenItem["kind"], name: string) => HTMLElement;
  selectSource: (id: string) => void;
  selectGenerated: (name: string) => void;
}

/** Search source and generated files, then delegate opening the selected item. */
export class QuickOpen {
  private items: QuickOpenItem[] = [];
  private activeIndex = 0;

  constructor(private readonly options: QuickOpenOptions) {
    options.button.addEventListener("click", () => this.show());
    options.dialog.addEventListener("click", (event) => {
      if (event.target === options.dialog) options.dialog.close();
    });
    options.input.addEventListener("input", () => {
      this.activeIndex = 0;
      this.render();
    });
    options.input.addEventListener("keydown", (event) => this.onInputKeyDown(event));
    document.addEventListener("keydown", (event) => this.onDocumentKeyDown(event));
  }

  private show() {
    if (!this.options.dialog.open) this.options.dialog.showModal();
    this.options.input.value = "";
    this.activeIndex = 0;
    this.render();
    this.options.input.focus();
  }

  private render() {
    const query = this.options.input.value.trim();
    const sources = this.options.sourceFiles().map((file) => ({
      kind: "source" as const,
      name: file.path.replace(/^\/?workspace\//i, ""),
      id: file.id,
    }));
    const generated = this.options.generatedFiles().map((name) => ({ kind: "generated" as const, name }));

    this.items = [...sources, ...generated]
      .map((item) => ({ ...item, score: fuzzyScore(query, item.name) }))
      .filter((item) => Number.isFinite(item.score))
      .sort(
        (left, right) =>
          left.score - right.score ||
          left.name.localeCompare(right.name, undefined, { numeric: true, sensitivity: "base" }),
      )
      .slice(0, 60);
    this.activeIndex = Math.min(this.activeIndex, Math.max(0, this.items.length - 1));
    this.options.results.replaceChildren(...this.items.map((item, index) => this.renderItem(item, index)));

    if (!this.items.length) {
      const empty = document.createElement("li");
      empty.className = "quick-open-empty";
      empty.textContent = "No matching files";
      this.options.results.append(empty);
    }
  }

  private renderItem(item: QuickOpenItem, index: number) {
    const row = document.createElement("li");
    const button = document.createElement("button");
    button.type = "button";
    button.setAttribute("role", "option");
    button.setAttribute("aria-selected", String(index === this.activeIndex));
    button.className = index === this.activeIndex ? "is-active" : "";
    button.append(this.options.icon(item.kind, item.name));

    const name = document.createElement("span");
    name.className = "quick-open-name";
    name.textContent = item.name;
    const kind = document.createElement("span");
    kind.className = "quick-open-kind";
    kind.textContent = item.kind === "source" ? "Source" : "Generated";
    button.append(name, kind);
    button.addEventListener("click", () => this.open(item));
    row.append(button);
    return row;
  }

  private open(item: QuickOpenItem) {
    this.options.dialog.close();
    if (item.kind === "source") this.options.selectSource(item.id);
    else this.options.selectGenerated(item.name);
  }

  private onInputKeyDown(event: KeyboardEvent) {
    if (event.key === "ArrowDown" || event.key === "ArrowUp") {
      event.preventDefault();
      if (this.items.length) {
        const direction = event.key === "ArrowDown" ? 1 : -1;
        this.activeIndex = (this.activeIndex + direction + this.items.length) % this.items.length;
      }
      this.render();
      this.options.results.querySelector<HTMLElement>("[aria-selected=true]")?.scrollIntoView({ block: "nearest" });
    } else if (event.key === "Enter") {
      event.preventDefault();
      const item = this.items[this.activeIndex];
      if (item) this.open(item);
    }
  }

  private onDocumentKeyDown(event: KeyboardEvent) {
    if ((event.ctrlKey || event.metaKey) && event.key.toLowerCase() === "p") {
      event.preventDefault();
      this.show();
    }
  }
}
