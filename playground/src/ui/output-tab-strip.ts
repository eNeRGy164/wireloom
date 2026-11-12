/** Adds an accessible draggable scrollbar and wheel navigation to generated file tabs. */
export class OutputTabStrip {
  private dragging = false;
  private dragX = 0;
  private dragScroll = 0;

  constructor(
    private readonly tabs: HTMLElement,
    private readonly scrollbar: HTMLElement,
    private readonly thumb: HTMLElement,
    private readonly strip: HTMLElement,
  ) {
    tabs.addEventListener("scroll", () => this.update());
    new ResizeObserver(() => this.update()).observe(tabs);
    thumb.addEventListener("pointerdown", (event) => this.startDrag(event));
    thumb.addEventListener("pointermove", (event) => this.moveDrag(event));
    thumb.addEventListener("pointerup", () => this.stopDrag());
    thumb.addEventListener("pointercancel", () => this.stopDrag());
    thumb.addEventListener("lostpointercapture", () => this.stopDrag());
    thumb.addEventListener("keydown", (event) => this.onKeyDown(event));
    strip.addEventListener("wheel", (event) => this.onWheel(event), { passive: false });
  }

  update() {
    const maximum = this.tabs.scrollWidth - this.tabs.clientWidth;
    this.scrollbar.hidden = maximum <= 0;
    if (maximum <= 0) {
      this.thumb.setAttribute("aria-valuemax", "0");
      this.thumb.setAttribute("aria-valuenow", "0");
      return;
    }
    const width = Math.min(
      this.tabs.clientWidth,
      Math.max(24, (this.tabs.clientWidth * this.tabs.clientWidth) / this.tabs.scrollWidth),
    );
    this.thumb.style.width = `${width}px`;
    this.thumb.style.transform = `translateX(${(this.tabs.scrollLeft / maximum) * (this.tabs.clientWidth - width)}px)`;
    this.thumb.setAttribute("aria-valuemax", String(Math.round(maximum)));
    this.thumb.setAttribute("aria-valuenow", String(Math.round(this.tabs.scrollLeft)));
  }

  private startDrag(event: PointerEvent) {
    event.preventDefault();
    this.dragging = true;
    this.dragX = event.clientX;
    this.dragScroll = this.tabs.scrollLeft;
    this.strip.classList.add("dragging");
    this.thumb.setPointerCapture(event.pointerId);
  }

  private moveDrag(event: PointerEvent) {
    const travel = this.tabs.clientWidth - this.thumb.getBoundingClientRect().width;
    if (this.dragging && travel > 0)
      this.tabs.scrollLeft =
        this.dragScroll + ((event.clientX - this.dragX) * (this.tabs.scrollWidth - this.tabs.clientWidth)) / travel;
  }

  private stopDrag() {
    this.dragging = false;
    this.strip.classList.remove("dragging");
  }

  private onKeyDown(event: KeyboardEvent) {
    if (!["ArrowLeft", "ArrowRight", "Home", "End", "PageUp", "PageDown"].includes(event.key)) return;
    event.preventDefault();
    if (event.key === "Home") this.tabs.scrollLeft = 0;
    else if (event.key === "End") this.tabs.scrollLeft = this.tabs.scrollWidth;
    else
      this.tabs.scrollLeft +=
        event.key === "PageUp"
          ? -this.tabs.clientWidth
          : event.key === "PageDown"
            ? this.tabs.clientWidth
            : event.key === "ArrowLeft"
              ? -40
              : 40;
  }

  private onWheel(event: WheelEvent) {
    if (event.ctrlKey || event.metaKey || this.tabs.scrollWidth <= this.tabs.clientWidth) return;
    const delta = Math.abs(event.deltaX) > Math.abs(event.deltaY) ? event.deltaX : event.deltaY;
    if (!delta) return;
    const scale =
      event.deltaMode === WheelEvent.DOM_DELTA_LINE
        ? 20
        : event.deltaMode === WheelEvent.DOM_DELTA_PAGE
          ? this.tabs.clientWidth
          : 1;
    this.tabs.scrollLeft += delta * scale;
    event.preventDefault();
  }
}
