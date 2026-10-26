export interface RecoverySaveCallbacks<T> {
  persist(value: T): Promise<void>;
  status(message: string): void;
  unavailable(): void;
}

/** Keeps asynchronous recovery results aligned with the latest workspace state. */
export class RecoverySaver<T> {
  private generation = 0;
  private callbacks: RecoverySaveCallbacks<T>;

  constructor(callbacks: RecoverySaveCallbacks<T>) {
    this.callbacks = callbacks;
  }

  save(value: T, available: boolean, fits: boolean) {
    const generation = ++this.generation;
    if (!available) return;

    if (!fits) {
      this.callbacks.status("Too large to save locally");
      return;
    }

    this.callbacks.status("Saving…");
    let persistence: Promise<void>;
    try {
      persistence = this.callbacks.persist(value);
    } catch (error) {
      persistence = Promise.reject(error);
    }

    void persistence
      .then(() => {
        if (generation === this.generation) this.callbacks.status("Saved locally");
      })
      .catch(() => {
        if (generation !== this.generation) return;
        this.callbacks.unavailable();
        this.callbacks.status("Recovery unavailable");
      });
  }

  invalidate() {
    this.generation++;
  }
}
