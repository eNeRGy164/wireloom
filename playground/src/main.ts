import { zipSync, strToU8 } from "fflate";
import type { CompileResponse, Diagnostic } from "./protocol";
import { Editors } from "./editors";
import { CompilerClient } from "./compiler-client";
import { examples } from "./examples";
import {
  RecoveryStore,
  createEmptyWorkspace,
  createWorkspace,
  sourceBytes,
  validateWorkspace,
  maxSourceBytes,
  type Workspace,
  type Profile,
  type Target,
} from "./workspace";
import { SourceWorkbench } from "./workspace-ui";
import { sourceArchive } from "./source-archive";
import { RecoverySaver } from "./recovery-saver";
import {
  projectFileName as getProjectFileName,
  projectPreview as createProjectPreview,
} from "./project/project-preview";
import { QuickOpen } from "./ui/quick-open";
import { NoticeCenter } from "./ui/notice-center";
import { OutputTabStrip } from "./ui/output-tab-strip";
import { PaneLayout } from "./ui/pane-layout";
import { GeneratedFilesView } from "./ui/generated-files-view";
import { generatedFileIconKind, type FileIconKind } from "./file-icon-kind";

const element = <T extends HTMLElement = HTMLElement>(id: string) => document.getElementById(id) as T;

function fileIcon(language: "idl" | FileIconKind) {
  const icon = document.createElement("span");
  icon.className = `file-icon file-icon-${language}`;
  icon.setAttribute("aria-hidden", "true");
  return icon;
}

function outputFileIcon(name: string) {
  return fileIcon(generatedFileIconKind(name, workspace.target, projectFileName()));
}

const profile = element<HTMLDivElement>("profile");
const target = element<HTMLDivElement>("target");
const examplePicker = element<HTMLSelectElement>("example");
const recovery = new RecoveryStore();

let workspace = createEmptyWorkspace();
let activeSource: string | undefined = workspace.files[0]?.id;
const editors = new Editors(element("source"), element("generated"));

let revision = 0;
let debounce = 0;
let pendingOutputAnchor: ReturnType<Editors["captureOutputAnchor"]>;
let replacing = false;
let recoveryAvailable = false;
let edited = false;
let fontSize = 12;

const recoverySaver = new RecoverySaver<Workspace>({
  persist: async (value) => {
    await recovery.save(value);
  },
  status: (message) => {
    element("source-state").textContent = message;
  },
  unavailable: () => {
    recoveryAvailable = false;
    notice("Local recovery is unavailable. You can keep editing; save your IDL before leaving.", "error");
  },
});

try {
  const savedFont = Number(localStorage.getItem("wireloom-editor-font-size"));
  if (Number.isInteger(savedFont) && savedFont >= 10 && savedFont <= 20) fontSize = savedFont;
} catch {
  /* Font controls remain usable when browser preferences cannot be stored. */
}

function setFontSize(size: number) {
  fontSize = Math.max(10, Math.min(20, size));
  editors.setFontSize(fontSize);
  element("font-size").textContent = `${fontSize} px`;
  element<HTMLButtonElement>("font-decrease").disabled = fontSize === 10;
  element<HTMLButtonElement>("font-increase").disabled = fontSize === 20;
  try {
    localStorage.setItem("wireloom-editor-font-size", String(fontSize));
  } catch {
    /* Optional preference. */
  }
}

setFontSize(fontSize);

element("font-decrease").addEventListener("click", () => setFontSize(fontSize - 1));
element("font-increase").addEventListener("click", () => setFontSize(fontSize + 1));
element("font-reset").addEventListener("click", () => setFontSize(12));
element("explorer-toggle").addEventListener("click", () => {
  const toggle = element("explorer-toggle");
  const expanded = toggle.getAttribute("aria-expanded") !== "true";
  toggle.setAttribute("aria-expanded", String(expanded));
  toggle.title = expanded ? "Hide file explorer" : "Show file explorer";
  element("explorer-content").hidden = !expanded;
  element("workbench").classList.toggle("explorer-collapsed", !expanded);
  element("explorer-splitter").tabIndex = expanded ? 0 : -1;
});

for (const group of ["source", "generated"]) {
  const toggle = element(`${group}-group-toggle`);
  const content = element(`${group}-group-content`);
  toggle.addEventListener("click", () => {
    const expanded = toggle.getAttribute("aria-expanded") !== "true";
    toggle.setAttribute("aria-expanded", String(expanded));
    content.hidden = !expanded;
  });
}

const outputTabStrip = new OutputTabStrip(
  element("output-tabs"),
  element("tab-scrollbar"),
  element("tab-scroll-thumb"),
  element("output-tabs-strip"),
);

element("version").textContent = `Wireloom ${__WIRELOOM_VERSION__} · ${__SOURCE_REVISION__}`;

for (const example of examples) examplePicker.add(new Option(example.title, example.id));

function describeExample() {
  const description = examples.find((e) => e.id === examplePicker.value)!.description;
  const targetName = workspace.target === "cpp" ? "C++" : "C#";
  const output = element("example-description");
  output.textContent = description.replace("Generate types", `Generate ${targetName} types`);
  output.classList.remove("is-changing");
  void output.offsetWidth;
  output.classList.add("is-changing");
}

examplePicker.addEventListener("change", describeExample);

const noticeCenter = new NoticeCenter(
  element("notice"),
  element("notice-message"),
  element("notice-kind"),
  element("notice-action"),
  element("notice-dismiss"),
);

function notice(message: string, kind: "info" | "error" = "info", action?: { label: string; run: () => void }) {
  noticeCenter.show(message, kind, action);
}

function exportControls() {
  const activeAvailable = generatedFiles.canExport && Boolean(generatedFiles.activeDocumentName);
  element<HTMLButtonElement>("copy").disabled = !activeAvailable;
  element<HTMLButtonElement>("download").disabled = !activeAvailable;
  element<HTMLButtonElement>("download-all").disabled = !generatedFiles.canExport || !generatedFiles.documents.length;
  element<HTMLButtonElement>("download-source").disabled = workspace.files.length === 0;
}

const generatedFiles = new GeneratedFilesView({
  editors,
  tabStrip: outputTabStrip,
  icon: outputFileIcon,
  projectDocument: () => ({ name: projectFileName(), source: projectPreview() }),
  exportStateChanged: exportControls,
});

function clearDiagnostics() {
  editors.clearMarkers();
  element("diagnostics").replaceChildren();
  element("diagnostic-count").textContent = "0";
  element("diagnostics-empty").hidden = false;
  element("diagnostics-empty").textContent = "Waiting for current diagnostics…";
}

function invalidate() {
  generatedFiles.invalidate();
  clearDiagnostics();
  element("result-state").textContent = generatedFiles.documents.length ? "Outdated" : "Pending";
  element("result-state").dataset.state = "pending";
  element("result-summary").textContent = generatedFiles.documents.length
    ? "Previous output · waiting for current source"
    : "Generating…";
  element("generated").classList.toggle("outdated", generatedFiles.documents.length > 0);
}

function clearDocuments(message: string) {
  generatedFiles.clear(message);
}

function failure(message: string) {
  clearDocuments("No output · generation failed");
  element("result-state").textContent = "Failed";
  element("result-state").dataset.state = "error";
  clearDiagnostics();
  element("diagnostics-empty").textContent = message;
}

function showDiagnostic(diagnostic: Diagnostic) {
  const row = document.createElement("li");
  const file = workspace.files.find((file) => file.path === diagnostic.filePath);
  const canNavigate = file !== undefined && diagnostic.startOffset >= 0;
  const control = document.createElement(canNavigate ? "button" : "span");
  const severity = diagnostic.severity.toLowerCase();

  control.className = `diagnostic ${severity}`;
  control.textContent = `${diagnostic.severity} · ${diagnostic.id} — ${diagnostic.message}`;

  if (canNavigate) {
    const position = editors.positionAt(diagnostic.startOffset, diagnostic.filePath ?? undefined);
    control.textContent += ` (${file!.path.slice("/workspace/".length)}:${position.lineNumber}:${position.column})`;
    (control as HTMLButtonElement).type = "button";
    control.title = "Go to source location";
    control.addEventListener("click", () => {
      sourceWorkbench.selectFile(file!.id);
      editors.navigate(diagnostic);
    });
  } else if (diagnostic.filePath) {
    control.textContent += ` (${diagnostic.filePath})`;
  }

  row.append(control);

  element("diagnostics").append(row);
}

function projectFileName() {
  return getProjectFileName(workspace.target);
}

function projectPreview() {
  return createProjectPreview(workspace);
}

function applyResult(response: CompileResponse) {
  clearDiagnostics();

  if (response.kind !== "compiler") {
    failure(response.error ?? "The result exceeds the workspace limits.");
    return;
  }

  element("diagnostics-empty").hidden = response.diagnostics.length > 0;
  element("diagnostics-empty").textContent = response.success
    ? "No diagnostics. Ready to explore."
    : "Generation failed.";
  element("diagnostic-count").textContent = String(response.diagnostics.length);
  editors.markers(response.diagnostics);
  response.diagnostics.forEach(showDiagnostic);

  if (!response.success) {
    clearDocuments("No output · fix the diagnostics to continue");
    element("result-state").textContent = "Failed";
    element("result-state").dataset.state = "error";
    return;
  }

  generatedFiles.applyDocuments(response.documents, pendingOutputAnchor, {
    target: workspace.target === "cpp" ? "C++" : "C#",
    profile: workspace.profile,
    revision,
  });
  pendingOutputAnchor = undefined;
}
const compiler = new CompilerClient({
  current: () => ({ revision, target: workspace.target, profile: workspace.profile }),
  status: (message, retry) => {
    element("status").textContent = message;
    element("retry").hidden = !retry;
  },
  result: applyResult,
  failure,
});

const sourceWorkbench = new SourceWorkbench(element("source-tree"), element("source-tabs"), {
  current: () => ({ workspace, activeId: activeSource, revision }),
  change: (next, selected) => replaceWorkspace(next, selected, true),
  select: selectSource,
  notice,
});

new QuickOpen({
  dialog: element("quick-open"),
  input: element("quick-open-query"),
  results: element("quick-open-results"),
  button: element("quick-open-button"),
  sourceFiles: () => workspace.files.map(({ id, path }) => ({ id, path })),
  generatedFiles: () => [
    ...generatedFiles.documents.map((doc) => doc.name),
    ...(generatedFiles.documents.length ? [projectFileName()] : []),
  ],
  icon: (kind, name) =>
    kind === "source" ? fileIcon("idl") : fileIcon(generatedFileIconKind(name, workspace.target, projectFileName())),
  selectSource: (id) => sourceWorkbench.selectFile(id),
  selectGenerated: (name) => generatedFiles.select(name),
});

function syncSources() {
  replacing = true;
  editors.syncSources(workspace.files, activeSource);
  replacing = false;
  sourceWorkbench.render();
}

function selectSource(id?: string) {
  activeSource = id;
  workspace.activeFileId = id;
  syncSources();
  save();
}

function compile() {
  clearTimeout(debounce);
  if (sourceBytes(workspace) > maxSourceBytes) {
    failure("Workspace IDL exceeds the 256 KiB UTF-8 limit. Reduce the source to continue.");
    return;
  }
  if (!workspace.files.length) {
    clearDiagnostics();
    clearDocuments("Empty workspace · create or import a file");
    element("result-state").textContent = "Empty";
    element("result-state").dataset.state = "pending";
    element("diagnostics-empty").textContent = "Create or import a source file to begin.";
    return;
  }
  if (!workspace.files.some((file) => file.isGenerationRoot)) {
    failure("Mark at least one file as Root");
    return;
  }
  invalidate();
  compiler.enqueue({
    workspaceRevision: revision,
    target: workspace.target,
    profile: workspace.profile,
    files: workspace.files.map((file) => ({
      path: file.path,
      source: file.source,
      isGenerationRoot: file.isGenerationRoot,
      options: structuredClone(file.options),
    })),
    options: structuredClone(workspace.options),
  });
}

function save() {
  recoverySaver.save(workspace, recoveryAvailable, sourceBytes(workspace) <= maxSourceBytes);
}

async function clearSavedRecovery() {
  recoverySaver.invalidate();
  try {
    await recovery.clear();
    recoveryAvailable = true;
    save();
    notice("Saved data cleared. Local recovery is ready.");
  } catch (error) {
    recoveryAvailable = false;
    element("source-state").textContent = "Recovery unavailable";
    notice(`Saved data could not be cleared: ${String(error)}`, "error");
  }
}

function changed() {
  if (!pendingOutputAnchor && element("result-state").dataset.state === "success")
    pendingOutputAnchor = editors.captureOutputAnchor();
  revision++;
  edited = true;
  element("source-size").textContent = `${sourceBytes(workspace).toLocaleString()} bytes`;
  invalidate();
  editors.updateOutput(projectFileName(), projectPreview());
  save();
  clearTimeout(debounce);
  if (!workspace.files.length || !workspace.files.some((file) => file.isGenerationRoot)) compile();
  else debounce = window.setTimeout(compile, 750);
}

editors.input.onDidChangeModelContent(() => {
  if (replacing) return;
  const file = workspace.files.find((file) => file.id === activeSource);
  if (file) {
    file.source = editors.source;
    changed();
  }
});

profile.addEventListener("click", (event) => {
  const button = (event.target as HTMLElement).closest<HTMLButtonElement>("button[data-profile]");
  if (!button || button.getAttribute("aria-pressed") === "true") return;
  workspace.profile = button.dataset.profile as Profile;
  syncProfilePresentation();
  changed();
  compile();
});

target.addEventListener("click", (event) => {
  const button = (event.target as HTMLElement).closest<HTMLButtonElement>("button[data-target]");
  if (!button || button.getAttribute("aria-pressed") === "true") return;
  workspace.target = button.dataset.target as Target;
  syncTargetPresentation(true);
  changed();
  compile();
});

element<HTMLButtonElement>("language-eyebrow").addEventListener("click", () => {
  const nextTarget = workspace.target === "cpp" ? "csharp" : "cpp";
  target.querySelector<HTMLButtonElement>(`button[data-target="${nextTarget}"]`)!.click();
});

function syncTargetPresentation(animate = false) {
  for (const button of target.querySelectorAll<HTMLButtonElement>("button[data-target]"))
    button.setAttribute("aria-pressed", String(button.dataset.target === workspace.target));
  const label = workspace.target === "cpp" ? "C++" : "C#";
  const eyebrow = element<HTMLButtonElement>("language-eyebrow");
  element("eyebrow-target").textContent = label;
  updateExampleForTarget();
  eyebrow.setAttribute(
    "aria-label",
    `Switch generation target from ${label} to ${workspace.target === "cpp" ? "C#" : "C++"}`,
  );
  if (animate) {
    eyebrow.classList.remove("is-transitioning");
    void eyebrow.offsetWidth;
    eyebrow.classList.add("is-transitioning");
  }
  element("generated-language").textContent = `Read-only ${label}`;
  element("generated-language").title = "Hover a documented property or member name for its documentation.";
  element("generated-panel").setAttribute("aria-label", `Generated ${label}`);
}

function syncProfilePresentation() {
  for (const button of profile.querySelectorAll<HTMLButtonElement>("button[data-profile]"))
    button.setAttribute("aria-pressed", String(button.dataset.profile === workspace.profile));
}

element<HTMLSpanElement>("eyebrow-target").addEventListener("animationend", (event) => {
  if ((event as AnimationEvent).animationName === "eyebrow-target-change")
    element("language-eyebrow").classList.remove("is-transitioning");
});

element("retry").addEventListener("click", () => {
  invalidate();
  void compiler.start();
  compile();
});

function replaceWorkspace(next: Workspace, selected = activeSource, isEdited = false) {
  pendingOutputAnchor = undefined;
  workspace = next;
  activeSource = workspace.files.some((file) => file.id === selected) ? selected : workspace.files[0]?.id;
  workspace.activeFileId = activeSource;
  syncProfilePresentation();
  syncTargetPresentation();
  syncSources();
  changed();
  edited = isEdited;
  compile();
}

function updateExampleForTarget() {
  if (!workspace.files.length) {
    element("example-description").textContent = "";
    return;
  }
  if (examplePicker.value) describeExample();
}

element("load-example").addEventListener("click", () => {
  if (edited && !confirm("Replace the current workspace with this example? Your current sources will be replaced."))
    return;
  sourceWorkbench.cancelImport();
  const example = examples.find((e) => e.id === examplePicker.value)!;
  describeExample();
  const next = createWorkspace(example.source, workspace.profile, workspace.target);
  if ("workspace" in example && example.workspace) {
    const multi = example.workspace();
    multi.profile = workspace.profile;
    multi.target = workspace.target;
    replaceWorkspace(validateWorkspace(multi));
  } else replaceWorkspace(next);
  notice("");
});

element("import").addEventListener("click", () => element<HTMLInputElement>("file").click());
element<HTMLInputElement>("file").addEventListener("change", (event) => {
  const input = event.target as HTMLInputElement;
  const files = [...(input.files ?? [])];
  input.value = "";
  void sourceWorkbench.import(files);
});

element("reset").addEventListener("click", async () => {
  if (!confirm("Reset the workspace and remove its saved files, folders, target, profile, and settings?")) return;
  sourceWorkbench.cancelImport();
  revision++;
  recoverySaver.invalidate();
  clearTimeout(debounce);
  invalidate();
  const clearing = recovery.clear();
  examplePicker.value = "hello";
  element("example-description").textContent = "";
  replaceWorkspace(createEmptyWorkspace());
  try {
    await clearing;
    recoveryAvailable = true;
    save();
    notice("Workspace reset.");
  } catch {
    recoveryAvailable = false;
    element("source-state").textContent = "Recovery unavailable";
    notice("The saved workspace could not be cleared. Local recovery is unavailable.", "error");
  }
});

element("empty-load-example").addEventListener("click", () => {
  examplePicker.value = "hello";
  describeExample();
  element("load-example").click();
});

element("empty-create-file").addEventListener("click", () => element("new-file").click());

function active() {
  if (!generatedFiles.canExport) return undefined;
  return generatedFiles.activeDocumentName === projectFileName()
    ? { name: projectFileName(), source: projectPreview() }
    : generatedFiles.documents.find((doc) => doc.name === generatedFiles.activeDocumentName);
}

function download(name: string, data: BlobPart, type: string) {
  const url = URL.createObjectURL(new Blob([data], { type }));
  const link = document.createElement("a");
  link.href = url;
  link.download = name;
  link.click();
  window.setTimeout(() => URL.revokeObjectURL(url), 30_000);
}

element("copy").addEventListener("click", async () => {
  const doc = active();
  if (!doc) return;
  try {
    await navigator.clipboard.writeText(doc.source);
    notice(`Copied ${doc.name}.`);
  } catch {
    notice("Clipboard access is unavailable. Select code in the output editor to copy it.", "error");
  }
});

element("download").addEventListener("click", () => {
  const doc = active();
  if (doc) download(doc.name, doc.source, "text/plain;charset=utf-8");
});

element("copy-source").addEventListener("click", async () => {
  const file = workspace.files.find((file) => file.id === activeSource);
  if (!file) return;
  try {
    await navigator.clipboard.writeText(file.source);
    notice(`Copied ${file.path.split("/").pop()}.`);
  } catch {
    notice("Clipboard access is unavailable. Select code in the IDL editor to copy it.", "error");
  }
});

element("download-source-file").addEventListener("click", () => {
  const file = workspace.files.find((file) => file.id === activeSource);
  if (file) download(file.path.split("/").pop()!, file.source, "text/plain;charset=utf-8");
});

element("download-source").addEventListener("click", () => {
  download(
    "wireloom-sources.zip",
    sourceArchive(structuredClone(workspace), projectFileName(), projectPreview()),
    "application/zip",
  );
});

element("download-all").addEventListener("click", () => {
  if (!generatedFiles.canExport || !generatedFiles.documents.length) return;
  const entries: Record<string, Uint8Array> = Object.create(null);
  for (const doc of generatedFiles.documents) entries[doc.name] = strToU8(doc.source);
  // Stored ZIP entries avoid main-thread compression work; preserve exact UTF-8 source.
  download("wireloom-generated.zip", zipSync(entries, { level: 0 }), "application/zip");
});

new PaneLayout(element("workspace"), element("workbench"), element("splitter"), element("explorer-splitter"));

async function initialize() {
  // Do not let a slow restore overwrite an edit made while storage is opening.
  const before = revision;
  try {
    const restored = await recovery.open();
    recoveryAvailable = true;
    if (restored && before === revision) {
      workspace = restored;
      syncProfilePresentation();
      replaceWorkspace(workspace, workspace.activeFileId ?? workspace.files[0]?.id, true);
      const example =
        workspace.files.length === 1 ? examples.find((value) => value.source === workspace.files[0].source) : undefined;
      if (example) {
        examplePicker.value = example.id;
        describeExample();
      } else element("example-description").textContent = "";
      notice("Workspace restored from this browser.");
    }
  } catch (error) {
    element("source-state").textContent = "Recovery unavailable";
    const canClearSavedData =
      error instanceof Error && /unsupported format|exceeds the 512 file\/folder limits/.test(error.message);
    notice(
      `Local recovery is unavailable: ${String(error)} You can still edit and generate.`,
      "error",
      canClearSavedData ? { label: "Clear saved data", run: clearSavedRecovery } : undefined,
    );
  } finally {
    save();
    element("source-size").textContent = `${sourceBytes(workspace).toLocaleString()} bytes`;
    void compiler.start();
    compile();
  }
}

syncTargetPresentation();
syncSources();
void initialize();
