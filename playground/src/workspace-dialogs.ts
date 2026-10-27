import type { Workspace } from "./workspace";
import { validateOptions, validateWorkspace, workspaceRoot } from "./workspace";

/** Native modal dialogs provide focus containment, Escape, labels, and return focus. */
export function dialog<T>(
  title: string,
  build: (form: HTMLFormElement, submit: (value: T) => void) => void,
): Promise<T | undefined> {
  return new Promise((resolve) => {
    const previous = document.activeElement as HTMLElement | null;
    const modal = document.createElement("dialog");
    modal.className = "workspace-dialog";
    const heading = document.createElement("h2");
    heading.id = "workspace-dialog-title";
    heading.textContent = title;
    modal.setAttribute("aria-labelledby", heading.id);
    const form = document.createElement("form");
    const cancel = document.createElement("button");
    cancel.type = "button";
    cancel.textContent = "Cancel";
    let finished = false;
    const finish = (value?: T) => {
      if (finished) return;
      finished = true;
      if (modal.open) modal.close();
      modal.remove();
      previous?.focus();
      resolve(value);
    };
    cancel.addEventListener("click", () => finish());
    build(form, (value) => finish(value));
    form.append(cancel);
    modal.append(heading, form);
    modal.addEventListener("cancel", (event) => {
      event.preventDefault();
      finish();
    });
    modal.addEventListener("keydown", (event) => {
      if (event.key !== "Escape") return;
      event.preventDefault();
      event.stopPropagation();
      finish();
    });
    modal.addEventListener("close", () => finish(), { once: true });
    document.body.append(modal);
    modal.showModal();
  });
}

export function field(form: HTMLElement, label: string, control: HTMLElement) {
  control.setAttribute("aria-label", label);
  const wrapper = document.createElement("label");
  wrapper.textContent = label;
  wrapper.append(control);
  form.append(wrapper);
}

export function textInput(value: string, multiline = false): HTMLInputElement | HTMLTextAreaElement {
  const control = document.createElement(multiline ? "textarea" : "input");
  control.value = value;
  if (control instanceof HTMLTextAreaElement) control.rows = 4;
  else control.type = "text";
  return control;
}

export function submitButton(form: HTMLElement, label: string) {
  const button = document.createElement("button");
  button.type = "submit";
  button.textContent = label;
  form.append(button);
}

function toggleChoice<T extends string>(
  form: HTMLElement,
  title: string,
  choices: [T, string][],
  selected: T,
): () => T {
  const group = document.createElement("fieldset");
  group.className = "property-toggle";
  group.setAttribute("role", "radiogroup");
  group.setAttribute("aria-label", title);
  const legend = document.createElement("legend");
  legend.textContent = title;
  const options = document.createElement("div");
  options.className = "property-toggle-options";
  const name = crypto.randomUUID();
  const inputs = choices.map(([value, text]) => {
    const label = document.createElement("label");
    const input = document.createElement("input");
    input.type = "radio";
    input.name = name;
    input.value = value;
    input.checked = value === selected;
    const caption = document.createElement("span");
    caption.textContent = text;
    label.append(input, caption);
    options.append(label);
    return input;
  });
  group.append(legend, options);
  form.append(group);
  return () => inputs.find((input) => input.checked)!.value as T;
}

export function choose(title: string, message: string, choices: string[]): Promise<string | undefined> {
  return dialog(title, (form, submit) => {
    const description = document.createElement("p");
    description.textContent = message;
    form.append(description);
    for (const choice of choices) {
      const button = document.createElement("button");
      button.type = "button";
      button.textContent = choice;
      button.addEventListener("click", () => submit(choice));
      form.append(button);
    }
  });
}

export function askPath(title: string, label: string, value: string): Promise<string | undefined> {
  return dialog(title, (form, submit) => {
    const input = textInput(value);
    input.required = true;
    input.maxLength = 1024;
    field(form, label, input);
    submitButton(form, "Continue");
    form.addEventListener("submit", (event) => {
      event.preventDefault();
      submit(input.value);
    });
    queueMicrotask(() => {
      input.focus();
      input.select();
    });
  });
}

export function settingsDialog(workspace: Workspace, fileId?: string): Promise<Workspace | undefined> {
  const draft = structuredClone(workspace);
  const file = draft.files.find((item) => item.id === fileId);
  return dialog(file ? `Properties — ${file.path}` : "Workspace Settings", (form, submit) => {
    const error = document.createElement("p");
    error.setAttribute("role", "alert");
    error.className = "dialog-error";
    let apply: () => void;
    if (file) {
      const role = toggleChoice(
        form,
        "Generation role",
        [
          ["root", "Root"],
          ["include", "Include-only"],
        ],
        file.isGenerationRoot ? "root" : "include",
      );
      const mapping = toggleChoice(
        form,
        "Sequence-array mapping (this declaring file)",
        [
          ["RtiCSharp", "RTI C#"],
          ["Idl", "IDL"],
        ],
        file.options.sequenceArrayMapping,
      );
      const sequence = textInput(String(file.options.sequenceSize)) as HTMLInputElement;
      const string = textInput(String(file.options.stringSize)) as HTMLInputElement;
      for (const input of [sequence, string]) {
        input.type = "number";
        input.min = "1";
        input.max = "2147483647";
        input.required = true;
      }
      field(form, "Unbounded sequence size (this declaring file)", sequence);
      field(form, "Unbounded string size (this declaring file)", string);
      apply = () => {
        file.isGenerationRoot = role() === "root";
        file.options = {
          sequenceArrayMapping: mapping(),
          sequenceSize: Number(sequence.value),
          stringSize: Number(string.value),
        };
      };
    } else {
      const defines = textInput(draft.options.defines.join("\n"), true);
      const undefines = textInput(draft.options.undefines.join("\n"), true);
      field(form, "Defines (one NAME or NAME=value per line; all Roots)", defines);
      field(form, "Undefines (one symbol per line; applied after defines)", undefines);
      const strict = document.createElement("input");
      strict.type = "checkbox";
      strict.checked = draft.options.strict;
      field(form, "Strict validation (whole batch)", strict);
      const escape = document.createElement("input");
      escape.type = "checkbox";
      escape.checked = draft.options.enableEscapeChar;
      field(form, "Enable escape character (whole batch)", escape);
      const folders = document.createElement("fieldset");
      const legend = document.createElement("legend");
      legend.textContent = "Include folders — searched alphabetically";
      folders.append(legend);
      const includeFolders = [workspaceRoot, ...draft.folders].sort().map((path) => {
        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.checked = draft.options.includeDirectories.includes(path);
        field(folders, path, checkbox);
        return { path, checkbox };
      });
      form.append(folders);
      apply = () => {
        const lines = (value: string) =>
          value
            .split(/\r?\n/)
            .map((line) => line.trim())
            .filter(Boolean);
        draft.options = validateOptions({
          defines: lines(defines.value),
          undefines: lines(undefines.value),
          strict: strict.checked,
          enableEscapeChar: escape.checked,
          includeDirectories: includeFolders.filter((item) => item.checkbox.checked).map((item) => item.path),
        });
      };
    }
    form.append(error);
    submitButton(form, "Apply");
    form.addEventListener("submit", (event) => {
      event.preventDefault();
      try {
        apply();
        submit(validateWorkspace(draft));
      } catch (reason) {
        error.textContent = String(reason);
      }
    });
  });
}
