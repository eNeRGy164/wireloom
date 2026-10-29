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
  onChange?: () => void,
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
    input.addEventListener("change", () => onChange?.());
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

export function askPath(title: string, label: string, value: string, required = true): Promise<string | undefined> {
  return dialog(title, (form, submit) => {
    const input = textInput(value);
    input.required = required;
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
  const title = file ? `Properties — ${file.path.slice(workspaceRoot.length + 1)}` : "Workspace Settings";
  return dialog(title, (form, submit) => {
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
      let defines: HTMLTextAreaElement | undefined;
      let undefines: HTMLTextAreaElement | undefined;
      let strict: HTMLInputElement | undefined;
      const unboundedMode = toggleChoice(
        form,
        "Unbounded support",
        [
          ["project", "Use project setting"],
          ["enabled", "Enabled"],
          ["disabled", "Disabled"],
        ],
        file.options.unboundedSupport === undefined
          ? "project"
          : file.options.unboundedSupport
            ? "enabled"
            : "disabled",
        () => updateFileSizeControls(),
      );
      const sequence = textInput(String(file.options.sequenceSize ?? draft.options.sequenceSize)) as HTMLInputElement;
      const string = textInput(String(file.options.stringSize ?? draft.options.stringSize)) as HTMLInputElement;
      let sequenceEdited = false;
      let stringEdited = false;
      let resetSequenceToProject = false;
      let resetStringToProject = false;
      for (const input of [sequence, string]) {
        input.type = "number";
        input.min = "1";
        input.max = "2147483647";
        input.required = true;
      }
      const updateFileSizeControls = () => {
        const enabled = unboundedMode() === "project" ? draft.options.unboundedSupport : unboundedMode() === "enabled";
        sequence.disabled = enabled;
        string.disabled = enabled;
      };
      const resetSequence = document.createElement("button");
      resetSequence.type = "button";
      resetSequence.className = "property-reset-link";
      resetSequence.textContent = "Reset to project default";
      resetSequence.setAttribute("aria-label", "Reset sequence size to project default");
      resetSequence.hidden = file.options.sequenceSize === undefined;
      resetSequence.addEventListener("click", () => {
        sequence.value = String(draft.options.sequenceSize);
        sequenceEdited = false;
        resetSequenceToProject = true;
        resetSequence.hidden = true;
      });
      const sequenceGroup = document.createElement("div");
      sequenceGroup.className = "property-size-group";
      field(sequenceGroup, "Unbounded sequence size", sequence);
      sequenceGroup.append(resetSequence);
      form.append(sequenceGroup);
      const resetString = document.createElement("button");
      resetString.type = "button";
      resetString.className = "property-reset-link";
      resetString.textContent = "Reset to project default";
      resetString.setAttribute("aria-label", "Reset string size to project default");
      resetString.hidden = file.options.stringSize === undefined;
      resetString.addEventListener("click", () => {
        string.value = String(draft.options.stringSize);
        stringEdited = false;
        resetStringToProject = true;
        resetString.hidden = true;
      });
      sequence.addEventListener("input", () => {
        sequenceEdited = true;
        resetSequenceToProject = false;
        resetSequence.hidden = false;
      });
      string.addEventListener("input", () => {
        stringEdited = true;
        resetStringToProject = false;
        resetString.hidden = false;
      });
      const stringGroup = document.createElement("div");
      stringGroup.className = "property-size-group";
      field(stringGroup, "Unbounded string size", string);
      stringGroup.append(resetString);
      form.append(stringGroup);
      updateFileSizeControls();
      if (file.isGenerationRoot) {
        defines = textInput(file.options.defines.join("\n"), true) as HTMLTextAreaElement;
        undefines = textInput(file.options.undefines.join("\n"), true) as HTMLTextAreaElement;
        field(form, "Defines (one NAME or NAME=value per line)", defines);
        field(form, "Undefines (one symbol per line; applied after defines)", undefines);
        strict = document.createElement("input");
        strict.type = "checkbox";
        strict.checked = file.options.strict;
        field(form, "Strict validation", strict);
      }
      apply = () => {
        file.isGenerationRoot = role() === "root";
        file.options = {
          ...file.options,
          sequenceSize: resetSequenceToProject
            ? undefined
            : sequenceEdited
              ? Number(sequence.value)
              : file.options.sequenceSize,
          stringSize: resetStringToProject ? undefined : stringEdited ? Number(string.value) : file.options.stringSize,
          unboundedSupport: unboundedMode() === "project" ? undefined : unboundedMode() === "enabled",
          defines:
            file.isGenerationRoot && defines
              ? defines.value
                  .split(/\r?\n/)
                  .map((line) => line.trim())
                  .filter(Boolean)
              : file.options.defines,
          undefines:
            file.isGenerationRoot && undefines
              ? undefines.value
                  .split(/\r?\n/)
                  .map((line) => line.trim())
                  .filter(Boolean)
              : file.options.undefines,
          strict: file.isGenerationRoot && strict ? strict.checked : file.options.strict,
        };
      };
    } else {
      const mapping = toggleChoice(
        form,
        "Sequence-array mapping",
        [
          ["RtiCSharp", "RTI C#"],
          ["Idl", "IDL"],
        ],
        draft.options.sequenceArrayMapping,
      );
      const unbounded = document.createElement("input");
      unbounded.type = "checkbox";
      unbounded.checked = draft.options.unboundedSupport;
      const sequence = textInput(String(draft.options.sequenceSize)) as HTMLInputElement;
      const string = textInput(String(draft.options.stringSize)) as HTMLInputElement;
      for (const input of [sequence, string]) {
        input.type = "number";
        input.min = "1";
        input.max = "2147483647";
        input.required = true;
      }
      const updateProjectSizeControls = () => {
        sequence.disabled = unbounded.checked;
        string.disabled = unbounded.checked;
      };
      unbounded.addEventListener("change", updateProjectSizeControls);
      field(form, "RTI-style unbounded sequences and strings", unbounded);
      updateProjectSizeControls();
      field(form, "Unbounded sequence size", sequence);
      field(form, "Unbounded string size", string);
      const escape = document.createElement("input");
      escape.type = "checkbox";
      escape.checked = draft.options.enableEscapeChar;
      field(form, "Enable escape character", escape);
      const folders = document.createElement("fieldset");
      const legend = document.createElement("legend");
      legend.textContent = "Include folders — searched alphabetically";
      folders.append(legend);
      const includeFolders = [workspaceRoot, ...draft.folders].sort().map((path) => {
        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.checked = draft.options.includeDirectories.includes(path);
        field(folders, path === workspaceRoot ? "Workspace root" : path, checkbox);
        if (path === workspaceRoot) checkbox.parentElement!.title = workspaceRoot;
        return { path, checkbox };
      });
      form.append(folders);
      apply = () => {
        draft.options = validateOptions({
          sequenceArrayMapping: mapping(),
          sequenceSize: Number(sequence.value),
          stringSize: Number(string.value),
          enableEscapeChar: escape.checked,
          unboundedSupport: unbounded.checked,
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
