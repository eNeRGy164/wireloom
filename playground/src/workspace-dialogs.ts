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
    const outside = (event: PointerEvent | MouseEvent) => {
      const bounds = modal.getBoundingClientRect();
      return (
        event.target === modal &&
        (event.clientX < bounds.left ||
          event.clientX > bounds.right ||
          event.clientY < bounds.top ||
          event.clientY > bounds.bottom)
      );
    };
    let pressedOutside = false;
    modal.addEventListener("pointerdown", (event) => {
      pressedOutside = outside(event);
    });
    modal.addEventListener("click", (event) => {
      if (pressedOutside && outside(event)) finish();
      pressedOutside = false;
    });
    document.body.append(modal);
    modal.showModal();
  });
}

export function field(form: HTMLElement, label: string, control: HTMLElement, description?: string) {
  control.setAttribute("aria-label", label);
  const wrapper = document.createElement("label");
  wrapper.textContent = label;
  if (control instanceof HTMLInputElement && control.type === "checkbox") wrapper.className = "checkbox-field";
  wrapper.append(control);
  if (description) describe(wrapper, control, description);
  form.append(wrapper);
}

function describe(container: HTMLElement, control: HTMLElement, text: string) {
  const hint = document.createElement("span");
  hint.id = crypto.randomUUID();
  hint.className = "setting-hint";
  hint.textContent = text;
  control.setAttribute("aria-describedby", hint.id);
  container.append(hint);
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
  description?: string,
) {
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
  if (description) describe(group, group, description);
  form.append(group);
  return {
    value: () => inputs.find((input) => input.checked)!.value as T,
    select: (value: T) => {
      for (const input of inputs) input.checked = input.value === value;
    },
    element: group,
  };
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
        undefined,
        "Root files start generation. Include-only files are available to #include directives without starting generation themselves.",
      );
      let defines: HTMLTextAreaElement | undefined;
      let undefines: HTMLTextAreaElement | undefined;
      let strict: HTMLInputElement | undefined;
      let unboundedOverride = file.options.unboundedSupport;
      const unboundedMode = toggleChoice(
        form,
        "Unbounded support",
        [
          ["enabled", "Enabled"],
          ["disabled", "Disabled"],
        ],
        (unboundedOverride ?? draft.options.unboundedSupport) ? "enabled" : "disabled",
        () => {
          unboundedOverride = unboundedMode.value() === "enabled";
          updateUnboundedDefault();
          updateFileSizeControls();
        },
        "Enabled preserves unbounded sequences and strings, including wide strings. Disabled uses the finite sizes below. Explicit IDL bounds always apply.",
      );
      const unboundedDefault = document.createElement("p");
      unboundedDefault.className = "setting-hint";
      unboundedDefault.setAttribute("aria-live", "polite");
      const resetUnbounded = document.createElement("button");
      resetUnbounded.type = "button";
      resetUnbounded.className = "property-reset-link";
      resetUnbounded.textContent = "Reset to project default";
      resetUnbounded.setAttribute("aria-label", "Reset unbounded support to project default");
      const updateUnboundedDefault = () => {
        resetUnbounded.hidden = unboundedOverride === undefined;
        unboundedDefault.textContent =
          unboundedOverride === undefined
            ? `Using project default (${draft.options.unboundedSupport ? "Enabled" : "Disabled"}).`
            : "Overridden for this file.";
      };
      resetUnbounded.addEventListener("click", () => {
        unboundedOverride = undefined;
        unboundedMode.select(draft.options.unboundedSupport ? "enabled" : "disabled");
        updateUnboundedDefault();
        updateFileSizeControls();
      });
      unboundedMode.element.append(unboundedDefault, resetUnbounded);
      updateUnboundedDefault();
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
        const enabled = unboundedOverride ?? draft.options.unboundedSupport;
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
        field(
          form,
          "Strict validation",
          strict,
          "Enable additional semantic checks for all roots in the generation batch.",
        );
      }
      apply = () => {
        file.isGenerationRoot = role.value() === "root";
        file.options = {
          ...file.options,
          sequenceSize: resetSequenceToProject
            ? undefined
            : sequenceEdited
              ? Number(sequence.value)
              : file.options.sequenceSize,
          stringSize: resetStringToProject ? undefined : stringEdited ? Number(string.value) : file.options.stringSize,
          unboundedSupport: unboundedOverride,
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
        undefined,
        "For arrays of sequences, RTI C# flattens the outer array to match RTI's C# mapping. IDL preserves the array and its separate sequences.",
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
      field(
        form,
        "Unbounded sequences and strings",
        unbounded,
        "Preserve unbounded sequences and strings, including wide strings, instead of applying finite fallback sizes. Explicit IDL bounds always apply.",
      );
      updateProjectSizeControls();
      field(
        form,
        "Unbounded sequence size",
        sequence,
        "Maximum element count for sequences without an IDL bound. Ignored while unbounded support is enabled.",
      );
      field(
        form,
        "Unbounded string size",
        string,
        "Maximum bytes for narrow strings or code units for wide strings without an IDL bound. Ignored while unbounded support is enabled.",
      );
      const escape = document.createElement("input");
      escape.type = "checkbox";
      escape.checked = draft.options.enableEscapeChar;
      field(
        form,
        "Enable escape character",
        escape,
        "Remove one leading underscore from IDL identifiers. Applies to all files and can change generated names and member IDs.",
      );
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
          sequenceArrayMapping: mapping.value(),
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
