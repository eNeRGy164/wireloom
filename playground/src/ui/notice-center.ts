export type NoticeKind = "info" | "error";

export interface NoticeAction {
  label: string;
  run: () => void;
}

/** Owns the transient status message and its pause, resume, and dismiss behavior. */
export class NoticeCenter {
  private runAction: (() => void) | undefined;
  private timer: number | undefined;
  private remainingMs = 5000;
  private deadline = 0;

  constructor(
    private readonly box: HTMLElement,
    private readonly messageElement: HTMLElement,
    private readonly kindElement: HTMLElement,
    private readonly actionButton: HTMLButtonElement,
    dismissButton: HTMLButtonElement,
  ) {
    actionButton.addEventListener("click", () => this.runAction?.());
    box.addEventListener("pointerenter", () => this.pause());
    box.addEventListener("pointerleave", () => this.resume());
    box.addEventListener("focusin", () => this.pause());
    box.addEventListener("focusout", () => queueMicrotask(() => this.resume()));
    dismissButton.addEventListener("click", () => this.show(""));
  }

  show(message: string, kind: NoticeKind = "info", action?: NoticeAction) {
    clearTimeout(this.timer);
    this.timer = undefined;
    this.box.dataset.kind = kind;
    this.kindElement.hidden = kind !== "error";
    this.messageElement.setAttribute("role", kind === "error" ? "alert" : "status");
    this.messageElement.textContent = kind === "error" ? message.replace(/^Error:\s*/, "") : message;
    this.actionButton.hidden = !action;
    this.actionButton.textContent = action?.label ?? "";
    this.runAction = action?.run;
    this.box.hidden = !message;
    if (message) {
      this.remainingMs = 5000;
      this.resume();
    }
  }

  private pause() {
    if (this.timer === undefined) return;
    clearTimeout(this.timer);
    this.timer = undefined;
    this.remainingMs = Math.max(0, this.deadline - performance.now());
  }

  private resume() {
    if (this.box.hidden || this.box.matches(":hover, :focus-within")) return;
    clearTimeout(this.timer);
    this.deadline = performance.now() + this.remainingMs;
    this.timer = window.setTimeout(() => this.show(""), this.remainingMs);
  }
}
