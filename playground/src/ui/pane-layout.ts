/** Owns the editor/output and explorer splitters and their responsive bounds. */
export class PaneLayout {
  private explorerWidth = 210;
  private draggingExplorer = false;
  private draggingWorkspace = false;

  constructor(
    private readonly workspace: HTMLElement,
    private readonly workbench: HTMLElement,
    private readonly splitter: HTMLElement,
    private readonly explorerSplitter: HTMLElement,
  ) {
    this.bindExplorerSplitter();
    this.bindWorkspaceSplitter();
    window.addEventListener("resize", () => this.resizeExplorer(this.explorerWidth));
    this.resizeExplorer(this.explorerWidth);
  }

  private resizeExplorer(width: number) {
    const maximum = Math.min(420, Math.max(160, this.workbench.clientWidth - 489));
    this.explorerWidth = Math.min(maximum, Math.max(160, width));
    this.workbench.style.setProperty("--explorer-width", `${this.explorerWidth}px`);
    this.explorerSplitter.setAttribute("aria-valuemax", String(maximum));
    this.explorerSplitter.setAttribute("aria-valuenow", String(Math.round(this.explorerWidth)));
    this.explorerSplitter.setAttribute("aria-valuetext", `${Math.round(this.explorerWidth)} pixels`);
  }

  private bindExplorerSplitter() {
    this.explorerSplitter.addEventListener("pointerdown", (event) => {
      this.draggingExplorer = true;
      this.explorerSplitter.setPointerCapture(event.pointerId);
    });
    this.explorerSplitter.addEventListener("pointermove", (event) => {
      if (this.draggingExplorer) this.resizeExplorer(event.clientX - this.workbench.getBoundingClientRect().left);
    });
    const stop = () => (this.draggingExplorer = false);
    this.explorerSplitter.addEventListener("pointerup", stop);
    this.explorerSplitter.addEventListener("pointercancel", stop);
    this.explorerSplitter.addEventListener("lostpointercapture", stop);
    this.explorerSplitter.addEventListener("keydown", (event) => {
      if (!["ArrowLeft", "ArrowRight", "Home", "End"].includes(event.key)) return;
      event.preventDefault();
      this.resizeExplorer(
        event.key === "Home"
          ? 160
          : event.key === "End"
            ? 420
            : this.explorerWidth + (event.key === "ArrowRight" ? 10 : -10),
      );
    });
  }

  private resizeWorkspace(percent: number) {
    const value = Math.min(75, Math.max(25, percent));
    this.workspace.style.setProperty("--input-width", `${value}%`);
    this.splitter.setAttribute("aria-valuenow", String(Math.round(value)));
  }

  private bindWorkspaceSplitter() {
    this.splitter.addEventListener("pointerdown", (event) => {
      this.draggingWorkspace = true;
      this.splitter.setPointerCapture(event.pointerId);
    });
    this.splitter.addEventListener("pointermove", (event) => {
      if (!this.draggingWorkspace) return;
      const bounds = this.workspace.getBoundingClientRect();
      this.resizeWorkspace(((event.clientX - bounds.left) / bounds.width) * 100);
    });
    const stop = () => (this.draggingWorkspace = false);
    this.splitter.addEventListener("pointerup", stop);
    this.splitter.addEventListener("pointercancel", stop);
    this.splitter.addEventListener("lostpointercapture", stop);
    this.splitter.addEventListener("keydown", (event) => {
      if (!["ArrowLeft", "ArrowRight", "Home", "End"].includes(event.key)) return;
      event.preventDefault();
      const value = Number(this.splitter.getAttribute("aria-valuenow"));
      this.resizeWorkspace(
        event.key === "Home" ? 25 : event.key === "End" ? 75 : value + (event.key === "ArrowRight" ? 2 : -2),
      );
    });
  }
}
