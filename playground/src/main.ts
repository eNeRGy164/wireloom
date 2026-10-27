import { zipSync, strToU8 } from "fflate";
import type { CompileResponse, Diagnostic, Document } from "./protocol";
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
} from "./workspace";
import { SourceWorkbench } from "./workspace-ui";
import { sourceArchive } from "./source-archive";
import { RecoverySaver } from "./recovery-saver";
import { escapeMsbuild } from "./msbuild-escape";
import "./style.css";

const element = <T extends HTMLElement = HTMLElement>(id: string) => document.getElementById(id) as T;
const fileNameOrder = new Intl.Collator(undefined, { numeric: true, sensitivity: "base" });

function fileIcon(language: "idl" | "csharp" | "csproj") {
  const icon = document.createElement("span");
  icon.className = `file-icon file-icon-${language}`;
  icon.setAttribute("aria-hidden", "true");
  return icon;
}

const profile = element<HTMLSelectElement>("profile");
const examplePicker = element<HTMLSelectElement>("example");
const recovery = new RecoveryStore();

let workspace = createEmptyWorkspace();
let activeSource: string | undefined = workspace.files[0]?.id;
const editors = new Editors(element("source"), element("generated"));

let revision = 0;
let debounce = 0;
let latestDocuments: Document[] = [];
let activeDocument: string | undefined;
let openDocuments: string[] = [];
const closedDocuments = new Set<string>();
let exportable = false;
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

const tabs = element("output-tabs");
const tabThumb = element("tab-scroll-thumb");

let draggingTabs = false;
let tabDragX = 0;
let tabDragScroll = 0;

function updateTabScrollbar() {
  const maximum = tabs.scrollWidth - tabs.clientWidth;
  element("tab-scrollbar").hidden = maximum <= 0;
  if (maximum <= 0) {
    tabThumb.setAttribute("aria-valuemax", "0");
    tabThumb.setAttribute("aria-valuenow", "0");
    return;
  }
  const width = Math.min(tabs.clientWidth, Math.max(24, (tabs.clientWidth * tabs.clientWidth) / tabs.scrollWidth));
  tabThumb.style.width = `${width}px`;
  tabThumb.style.transform = `translateX(${(tabs.scrollLeft / maximum) * (tabs.clientWidth - width)}px)`;
  tabThumb.setAttribute("aria-valuemax", String(Math.round(maximum)));
  tabThumb.setAttribute("aria-valuenow", String(Math.round(tabs.scrollLeft)));
}

tabs.addEventListener("scroll", updateTabScrollbar);

new ResizeObserver(updateTabScrollbar).observe(tabs);

tabThumb.addEventListener("pointerdown", (event) => {
  event.preventDefault();
  draggingTabs = true;
  tabDragX = event.clientX;
  tabDragScroll = tabs.scrollLeft;
  element("output-tabs-strip").classList.add("dragging");
  tabThumb.setPointerCapture(event.pointerId);
});

tabThumb.addEventListener("pointermove", (event) => {
  const travel = tabs.clientWidth - tabThumb.getBoundingClientRect().width;
  if (draggingTabs && travel > 0)
    tabs.scrollLeft = tabDragScroll + ((event.clientX - tabDragX) * (tabs.scrollWidth - tabs.clientWidth)) / travel;
});

function stopTabDrag() {
  draggingTabs = false;
  element("output-tabs-strip").classList.remove("dragging");
}

tabThumb.addEventListener("pointerup", stopTabDrag);
tabThumb.addEventListener("pointercancel", stopTabDrag);
tabThumb.addEventListener("lostpointercapture", stopTabDrag);
tabThumb.addEventListener("keydown", (event) => {
  if (!["ArrowLeft", "ArrowRight", "Home", "End", "PageUp", "PageDown"].includes(event.key)) return;
  event.preventDefault();
  if (event.key === "Home") tabs.scrollLeft = 0;
  else if (event.key === "End") tabs.scrollLeft = tabs.scrollWidth;
  else
    tabs.scrollLeft +=
      event.key === "PageUp"
        ? -tabs.clientWidth
        : event.key === "PageDown"
          ? tabs.clientWidth
          : event.key === "ArrowLeft"
            ? -40
            : 40;
});

element("output-tabs-strip").addEventListener(
  "wheel",
  (event) => {
    if (event.ctrlKey || event.metaKey || tabs.scrollWidth <= tabs.clientWidth) return;
    const delta = Math.abs(event.deltaX) > Math.abs(event.deltaY) ? event.deltaX : event.deltaY;
    if (!delta) return;
    const scale =
      event.deltaMode === WheelEvent.DOM_DELTA_LINE
        ? 20
        : event.deltaMode === WheelEvent.DOM_DELTA_PAGE
          ? tabs.clientWidth
          : 1;
    tabs.scrollLeft += delta * scale;
    event.preventDefault();
  },
  { passive: false },
);

element("version").textContent = `Wireloom ${__WIRELOOM_VERSION__} · ${__SOURCE_REVISION__}`;

for (const example of examples) examplePicker.add(new Option(example.title, example.id));

function describeExample() {
  element("example-description").textContent = examples.find((e) => e.id === examplePicker.value)!.description;
}

examplePicker.addEventListener("change", describeExample);

const noticeBox = element("notice");
const noticeAction = element<HTMLButtonElement>("notice-action");
let runNoticeAction: (() => void) | undefined;

let noticeTimer: number | undefined;
let noticeRemainingMs = 5000;
let noticeDeadline = 0;

function pauseNotice() {
  if (noticeTimer === undefined) return;
  clearTimeout(noticeTimer);
  noticeTimer = undefined;
  noticeRemainingMs = Math.max(0, noticeDeadline - performance.now());
}

function resumeNotice() {
  if (noticeBox.hidden || noticeBox.matches(":hover, :focus-within")) return;
  clearTimeout(noticeTimer);
  noticeDeadline = performance.now() + noticeRemainingMs;
  noticeTimer = window.setTimeout(() => notice(""), noticeRemainingMs);
}

function notice(message: string, kind: "info" | "error" = "info", action?: { label: string; run: () => void }) {
  clearTimeout(noticeTimer);
  noticeTimer = undefined;
  noticeBox.dataset.kind = kind;
  element("notice-kind").hidden = kind !== "error";
  element("notice-message").setAttribute("role", kind === "error" ? "alert" : "status");
  element("notice-message").textContent = kind === "error" ? message.replace(/^Error:\s*/, "") : message;
  noticeAction.hidden = !action;
  noticeAction.textContent = action?.label ?? "";
  runNoticeAction = action?.run;
  noticeBox.hidden = !message;
  if (message) {
    noticeRemainingMs = 5000;
    resumeNotice();
  }
}

noticeAction.addEventListener("click", () => runNoticeAction?.());

noticeBox.addEventListener("pointerenter", pauseNotice);
noticeBox.addEventListener("pointerleave", resumeNotice);
noticeBox.addEventListener("focusin", pauseNotice);
noticeBox.addEventListener("focusout", () => queueMicrotask(resumeNotice));

element("notice-dismiss").addEventListener("click", () => notice(""));

function exportControls() {
  const activeAvailable = exportable && Boolean(activeDocument);
  element<HTMLButtonElement>("copy").disabled = !activeAvailable;
  element<HTMLButtonElement>("download").disabled = !activeAvailable;
  element<HTMLButtonElement>("download-all").disabled = !exportable || !latestDocuments.length;
  element<HTMLButtonElement>("download-source").disabled = workspace.files.length === 0;
}

function clearDiagnostics() {
  editors.clearMarkers();
  element("diagnostics").replaceChildren();
  element("diagnostic-count").textContent = "0";
  element("diagnostics-empty").hidden = false;
  element("diagnostics-empty").textContent = "Waiting for current diagnostics…";
}

function invalidate() {
  exportable = false;
  exportControls();
  clearDiagnostics();
  element("result-state").textContent = latestDocuments.length ? "Outdated" : "Pending";
  element("result-state").dataset.state = "pending";
  element("result-summary").textContent = latestDocuments.length
    ? "Previous output · waiting for current source"
    : "Generating…";
  element("generated").classList.toggle("outdated", latestDocuments.length > 0);
}

function clearDocuments(message: string) {
  closeGeneratedFileMenu();
  element("generated-panel").removeAttribute("aria-labelledby");
  latestDocuments = [];
  openDocuments = [];
  closedDocuments.clear();
  activeDocument = undefined;
  exportable = false;
  editors.setOutputs([]);
  element("file-list").replaceChildren();
  element("explorer-count").textContent = "0";
  element("explorer-empty").hidden = false;
  element("output-tabs").replaceChildren();
  updateTabScrollbar();
  element("document-count").textContent = "0";
  element("empty-output").hidden = false;
  element("result-summary").textContent = message;
  element("generated").classList.remove("outdated");
  exportControls();
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

function selectDocument(name: string) {
  closedDocuments.delete(name);
  if (!openDocuments.includes(name)) openDocuments.push(name);
  activeDocument = name;
  editors.select(name);

  for (const button of element("file-list").querySelectorAll("button")) {
    if (button.dataset.name === name) button.setAttribute("aria-current", "true");
    else button.removeAttribute("aria-current");
  }

  renderTabs();
  element("empty-output").hidden = openDocuments.length > 0;
  element("output-tabs")
    .querySelector('[aria-selected="true"]')
    ?.scrollIntoView({ block: "nearest", inline: "nearest" });
  exportControls();
}

function renderTabs() {
  element("output-tabs").replaceChildren(
    ...openDocuments.map((name, index) => {
      const tab = document.createElement("button");
      const wrapper = document.createElement("div");
      wrapper.className = "editor-tab";
      wrapper.setAttribute("role", "presentation");
      tab.id = `generated-tab-${index}`;
      tab.className = "editor-tab-label";
      tab.textContent = name;
      tab.title = name;
      tab.prepend(fileIcon(name === "Wireloom.csproj" ? "csproj" : "csharp"));
      tab.setAttribute("role", "tab");
      tab.setAttribute("aria-selected", String(name === activeDocument));
      tab.setAttribute("aria-controls", "generated-panel");
      if (name === activeDocument) element("generated-panel").setAttribute("aria-labelledby", tab.id);
      tab.tabIndex = name === activeDocument ? 0 : -1;
      tab.addEventListener("click", () => {
        selectDocument(name);
        element("output-tabs").querySelector<HTMLButtonElement>('[aria-selected="true"]')!.focus();
      });
      tab.addEventListener("contextmenu", (event) => {
        event.preventDefault();
        event.stopPropagation();
        showGeneratedTabMenu(event, name);
      });
      tab.addEventListener("auxclick", (event) => {
        if (event.button !== 1) return;
        event.preventDefault();
        event.stopPropagation();
        closeDocument(name);
      });
      tab.addEventListener("keydown", (event) => {
        const direction = event.key === "ArrowRight" ? 1 : event.key === "ArrowLeft" ? -1 : 0;
        if (!direction && event.key !== "Home" && event.key !== "End") return;
        event.preventDefault();
        const next =
          event.key === "Home"
            ? 0
            : event.key === "End"
              ? openDocuments.length - 1
              : (index + direction + openDocuments.length) % openDocuments.length;
        selectDocument(openDocuments[next]);
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
        closeDocument(name);
      });
      wrapper.append(tab, close);
      return wrapper;
    }),
  );

  updateTabScrollbar();
}

function closeDocument(name: string) {
  const index = openDocuments.indexOf(name);
  if (index < 0) return;
  openDocuments = openDocuments.filter((item) => item !== name);
  closedDocuments.add(name);
  if (activeDocument === name) {
    const next = openDocuments[index] ?? openDocuments[index - 1];
    if (next) {
      selectDocument(next);
      element("output-tabs").querySelector<HTMLButtonElement>('[aria-selected="true"]')?.focus();
    } else {
      activeDocument = undefined;
      editors.select("");
      element("generated-panel").removeAttribute("aria-labelledby");
      for (const button of element("file-list").querySelectorAll("button")) button.removeAttribute("aria-current");
      renderTabs();
    }
  } else renderTabs();
  element("empty-output").hidden = openDocuments.length > 0;
  exportControls();
}

function closeAllDocuments() {
  if (!openDocuments.length) return;
  for (const name of openDocuments) closedDocuments.add(name);
  openDocuments = [];
  activeDocument = undefined;
  editors.select("");
  element("generated-panel").removeAttribute("aria-labelledby");
  for (const button of element("file-list").querySelectorAll("button")) button.removeAttribute("aria-current");
  renderTabs();
  element("empty-output").hidden = false;
  exportControls();
}

let generatedFileMenu: HTMLElement | undefined;

function closeGeneratedFileMenu() {
  generatedFileMenu?.remove();
  generatedFileMenu = undefined;
  document.removeEventListener("pointerdown", dismissGeneratedFileMenu);
  document.removeEventListener("keydown", dismissGeneratedFileMenuOnEscape);
}

function dismissGeneratedFileMenu(event: Event) {
  if (generatedFileMenu && !generatedFileMenu.contains(event.target as Node)) closeGeneratedFileMenu();
}

function dismissGeneratedFileMenuOnEscape(event: KeyboardEvent) {
  if (event.key === "Escape") closeGeneratedFileMenu();
}

function showGeneratedFileMenu(event: MouseEvent, name: string) {
  event.preventDefault();
  closeGeneratedFileMenu();
  const menu = document.createElement("div");
  menu.className = "source-context-menu";
  menu.setAttribute("role", "menu");
  menu.setAttribute("aria-label", `Actions for ${name}`);
  const isOpen = openDocuments.includes(name);
  const action = document.createElement("button");
  action.type = "button";
  action.textContent = isOpen ? "Close" : "Open";
  action.setAttribute("role", "menuitem");
  action.addEventListener("click", () => {
    closeGeneratedFileMenu();
    if (isOpen) closeDocument(name);
    else selectDocument(name);
  });
  menu.append(action);
  menu.addEventListener("keydown", (keyEvent) => {
    if (keyEvent.key === "Escape") {
      keyEvent.preventDefault();
      closeGeneratedFileMenu();
    } else if (keyEvent.key === "Enter" || keyEvent.key === " ") action.click();
  });
  document.body.append(menu);
  menu.style.left = `${Math.max(8, Math.min(event.clientX, window.innerWidth - menu.offsetWidth - 8))}px`;
  menu.style.top = `${Math.max(8, Math.min(event.clientY, window.innerHeight - menu.offsetHeight - 8))}px`;
  generatedFileMenu = menu;
  document.addEventListener("pointerdown", dismissGeneratedFileMenu);
  document.addEventListener("keydown", dismissGeneratedFileMenuOnEscape);
  action.focus();
}

function showGeneratedTabMenu(event: MouseEvent, name: string) {
  closeGeneratedFileMenu();
  const menu = document.createElement("div");
  menu.className = "source-context-menu";
  menu.setAttribute("role", "menu");
  menu.setAttribute("aria-label", `Actions for ${name}`);
  const actions: [string, () => void][] = [
    ["Close", () => closeDocument(name)],
    ["Close All", closeAllDocuments],
  ];
  for (const [label, action] of actions) {
    const button = document.createElement("button");
    button.type = "button";
    button.textContent = label;
    button.setAttribute("role", "menuitem");
    button.addEventListener("click", () => {
      closeGeneratedFileMenu();
      action();
    });
    menu.append(button);
  }
  menu.addEventListener("keydown", (keyEvent) => {
    if (keyEvent.key === "Escape") {
      keyEvent.preventDefault();
      closeGeneratedFileMenu();
    }
  });
  document.body.append(menu);
  menu.style.left = `${Math.max(8, Math.min(event.clientX, window.innerWidth - menu.offsetWidth - 8))}px`;
  menu.style.top = `${Math.max(8, Math.min(event.clientY, window.innerHeight - menu.offsetHeight - 8))}px`;
  generatedFileMenu = menu;
  document.addEventListener("pointerdown", dismissGeneratedFileMenu);
  document.addEventListener("keydown", dismissGeneratedFileMenuOnEscape);
  menu.querySelector<HTMLButtonElement>("button")?.focus();
}

function xml(value: string) {
  return value.replaceAll("&", "&amp;").replaceAll("<", "&lt;").replaceAll(">", "&gt;").replaceAll('"', "&quot;");
}

function projectPreview() {
  const relative = (path: string) => (path === "/workspace" ? "." : path.slice("/workspace/".length));
  const projectSettings = new Map<string, string>();
  const files = workspace.files;
  if (workspace.options.sequenceArrayMapping !== "RtiCSharp")
    projectSettings.set("DdsIdlSequenceArrayMapping", workspace.options.sequenceArrayMapping);
  if (workspace.options.sequenceSize !== 100)
    projectSettings.set("DdsIdlSequenceSize", String(workspace.options.sequenceSize));
  if (workspace.options.stringSize !== 255)
    projectSettings.set("DdsIdlStringSize", String(workspace.options.stringSize));
  if (workspace.options.enableEscapeChar) projectSettings.set("DdsIdlEnableEscapeChar", "true");
  if (workspace.options.includeDirectories.length)
    projectSettings.set(
      "DdsIdlIncludeDirectories",
      workspace.options.includeDirectories.map((path) => escapeMsbuild(relative(path))).join(";"),
    );

  const metadata = (file: Workspace["files"][number]) => {
    const settings: [string, string][] = [];
    if (file.isGenerationRoot && file.options.strict) settings.push(["Strict", "true"]);
    if (file.isGenerationRoot && file.options.defines.length)
      settings.push(["Defines", file.options.defines.join(";")]);
    if (file.isGenerationRoot && file.options.undefines.length)
      settings.push(["Undefines", file.options.undefines.join(";")]);
    if (
      file.options.sequenceArrayMapping &&
      file.options.sequenceArrayMapping !== workspace.options.sequenceArrayMapping
    )
      settings.push(["SequenceArrayMapping", file.options.sequenceArrayMapping]);
    if (file.options.sequenceSize !== undefined && file.options.sequenceSize !== workspace.options.sequenceSize)
      settings.push(["SequenceSize", String(file.options.sequenceSize)]);
    if (file.options.stringSize !== undefined && file.options.stringSize !== workspace.options.stringSize)
      settings.push(["StringSize", String(file.options.stringSize)]);
    return settings.map(([key, value]) => `      <${key}>${xml(value)}</${key}>`).join("\n");
  };
  const item = (tag: "DdsIdl" | "AdditionalFiles", file: Workspace["files"][number]) => {
    const inner = metadata(file);
    const opening = `    <${tag} Include="${xml(escapeMsbuild(relative(file.path)))}"`;
    return inner ? `${opening}>\n${inner}\n    </${tag}>` : `${opening} />`;
  };
  const roots = files.filter((file) => file.isGenerationRoot).map((file) => item("DdsIdl", file));
  // The generator discovers ordinary include-only .idl files under the project root
  // and configured include directories. Explicit items are only needed for metadata.
  const additionalFiles = files
    .filter((file) => !file.isGenerationRoot && metadata(file))
    .map((file) => item("AdditionalFiles", file));
  const projectProperties = [...projectSettings].map(([key, value]) => `    <${key}>${xml(value)}</${key}>`);
  const rootGroup = roots.length
    ? `\n\n  <ItemGroup Label="Generation roots">\n${roots.join("\n")}\n  </ItemGroup>`
    : "";
  const additionalGroup = additionalFiles.length
    ? `\n\n  <ItemGroup Label="Additional files">\n${additionalFiles.join("\n")}\n  </ItemGroup>`
    : "";
  return `<!-- Preview based on the current Playground workspace. -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>${projectProperties.length ? `\n${projectProperties.join("\n")}` : ""}
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Rti.ConnextDds" Version="${workspace.profile}" />
    <PackageReference Include="Wireloom.Dds.Generator" Version="${__WIRELOOM_VERSION__}" PrivateAssets="all" />
  </ItemGroup>${rootGroup}${additionalGroup}
</Project>`;
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

  const previousActive = activeDocument;

  latestDocuments = response.documents;
  const preview = { name: "Wireloom.csproj", source: projectPreview() };
  editors.setOutputs([...latestDocuments, preview]);
  const previewWasOpen = openDocuments.includes(preview.name) && !closedDocuments.has(preview.name);
  openDocuments = [
    ...latestDocuments.filter((doc) => !closedDocuments.has(doc.name)).map((doc) => doc.name),
    ...(previewWasOpen ? [preview.name] : []),
  ];

  const explorerDocuments = [...latestDocuments].sort((left, right) => fileNameOrder.compare(left.name, right.name));

  closeGeneratedFileMenu();
  element("file-list").replaceChildren(
    ...[...explorerDocuments, preview].map((doc) => {
      const item = document.createElement("li");
      const button = document.createElement("button");
      button.textContent = doc.name;
      button.prepend(fileIcon(doc.name.endsWith(".csproj") ? "csproj" : "csharp"));
      button.title = doc.name;
      button.dataset.name = doc.name;
      button.setAttribute("aria-haspopup", "menu");
      button.addEventListener("click", () => selectDocument(doc.name));
      button.addEventListener("contextmenu", (event) => showGeneratedFileMenu(event, doc.name));
      item.append(button);
      return item;
    }),
  );

  element("explorer-count").textContent = String(latestDocuments.length + 1);
  element("explorer-empty").hidden = true;

  if (openDocuments.length)
    selectDocument(openDocuments.includes(previousActive ?? "") ? previousActive! : openDocuments[0]);
  else {
    activeDocument = undefined;
    element("output-tabs").replaceChildren();
    updateTabScrollbar();
  }

  element("document-count").textContent = String(latestDocuments.length);
  element("empty-output").hidden = openDocuments.length > 0;
  element("result-state").textContent = "Current";
  element("result-state").dataset.state = "success";
  element("result-summary").textContent = latestDocuments.length
    ? `${latestDocuments.length} files · RTI ${workspace.profile} · revision ${revision}`
    : "Generation succeeded · no documents";
  element("generated").classList.remove("outdated");

  exportable = true;

  exportControls();
}

const compiler = new CompilerClient({
  current: () => ({ revision, profile: workspace.profile }),
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
  revision++;
  edited = true;
  element("source-size").textContent = `${sourceBytes(workspace).toLocaleString()} bytes`;
  invalidate();
  editors.updateOutput("Wireloom.csproj", projectPreview());
  save();
  clearTimeout(debounce);
  if (!workspace.files.length || !workspace.files.some((file) => file.isGenerationRoot)) compile();
  else debounce = window.setTimeout(compile, 300);
}

editors.input.onDidChangeModelContent(() => {
  if (replacing) return;
  const file = workspace.files.find((file) => file.id === activeSource);
  if (file) {
    file.source = editors.source;
    changed();
  }
});

profile.addEventListener("change", () => {
  workspace.profile = profile.value as Profile;
  changed();
  compile();
});

element("retry").addEventListener("click", () => {
  invalidate();
  void compiler.start();
  compile();
});

function replaceWorkspace(next: Workspace, selected = activeSource, isEdited = false) {
  workspace = next;
  activeSource = workspace.files.some((file) => file.id === selected) ? selected : workspace.files[0]?.id;
  workspace.activeFileId = activeSource;
  profile.value = workspace.profile;
  syncSources();
  changed();
  edited = isEdited;
  compile();
}

element("load-example").addEventListener("click", () => {
  if (edited && !confirm("Replace the current workspace with this example? Your current sources will be replaced."))
    return;
  sourceWorkbench.cancelImport();
  const example = examples.find((e) => e.id === examplePicker.value)!;
  describeExample();
  const next = createWorkspace(example.source, workspace.profile);
  if ("workspace" in example && example.workspace) {
    const multi = example.workspace();
    multi.profile = workspace.profile;
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
  if (!confirm("Reset the workspace and remove its saved files, folders, profile, and settings?")) return;
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
  if (!exportable) return undefined;
  return activeDocument === "Wireloom.csproj"
    ? { name: "Wireloom.csproj", source: projectPreview() }
    : latestDocuments.find((doc) => doc.name === activeDocument);
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
  download("wireloom-sources.zip", sourceArchive(structuredClone(workspace), projectPreview()), "application/zip");
});

element("download-all").addEventListener("click", () => {
  if (!exportable || !latestDocuments.length) return;
  const entries: Record<string, Uint8Array> = Object.create(null);
  for (const doc of latestDocuments) entries[doc.name] = strToU8(doc.source);
  // Stored ZIP entries avoid main-thread compression work; preserve exact UTF-8 source.
  download("wireloom-generated.zip", zipSync(entries, { level: 0 }), "application/zip");
});

const splitter = element("splitter");
const explorerSplitter = element("explorer-splitter");

let explorerWidth = 210;
let draggingExplorer = false;

function resizeExplorer(width: number) {
  const maximum = Math.min(420, Math.max(160, element("workbench").clientWidth - 489));
  explorerWidth = Math.min(maximum, Math.max(160, width));
  element("workbench").style.setProperty("--explorer-width", `${explorerWidth}px`);
  explorerSplitter.setAttribute("aria-valuemax", String(maximum));
  explorerSplitter.setAttribute("aria-valuenow", String(Math.round(explorerWidth)));
  explorerSplitter.setAttribute("aria-valuetext", `${Math.round(explorerWidth)} pixels`);
}

explorerSplitter.addEventListener("pointerdown", (event) => {
  draggingExplorer = true;
  explorerSplitter.setPointerCapture(event.pointerId);
});
explorerSplitter.addEventListener("pointermove", (event) => {
  if (draggingExplorer) resizeExplorer(event.clientX - element("workbench").getBoundingClientRect().left);
});
explorerSplitter.addEventListener("pointerup", () => {
  draggingExplorer = false;
});
explorerSplitter.addEventListener("pointercancel", () => {
  draggingExplorer = false;
});
explorerSplitter.addEventListener("lostpointercapture", () => {
  draggingExplorer = false;
});
explorerSplitter.addEventListener("keydown", (event) => {
  if (!["ArrowLeft", "ArrowRight", "Home", "End"].includes(event.key)) return;
  event.preventDefault();
  resizeExplorer(
    event.key === "Home" ? 160 : event.key === "End" ? 420 : explorerWidth + (event.key === "ArrowRight" ? 10 : -10),
  );
});

window.addEventListener("resize", () => resizeExplorer(explorerWidth));

resizeExplorer(explorerWidth);

let dragging = false;

function resize(percent: number) {
  const value = Math.min(75, Math.max(25, percent));
  element("workspace").style.setProperty("--input-width", `${value}%`);
  splitter.setAttribute("aria-valuenow", String(Math.round(value)));
}

splitter.addEventListener("pointerdown", (event) => {
  dragging = true;
  splitter.setPointerCapture(event.pointerId);
});
splitter.addEventListener("pointermove", (event) => {
  if (!dragging) return;
  const bounds = element("workspace").getBoundingClientRect();
  resize(((event.clientX - bounds.left) / bounds.width) * 100);
});
splitter.addEventListener("pointerup", () => {
  dragging = false;
});
splitter.addEventListener("pointercancel", () => {
  dragging = false;
});
splitter.addEventListener("keydown", (event) => {
  if (!["ArrowLeft", "ArrowRight", "Home", "End"].includes(event.key)) return;
  event.preventDefault();
  const value = Number(splitter.getAttribute("aria-valuenow"));
  resize(event.key === "Home" ? 25 : event.key === "End" ? 75 : value + (event.key === "ArrowRight" ? 2 : -2));
});

async function initialize() {
  // Do not let a slow restore overwrite an edit made while storage is opening.
  const before = revision;
  try {
    const restored = await recovery.open();
    recoveryAvailable = true;
    if (restored && before === revision) {
      workspace = restored;
      profile.value = workspace.profile;
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

syncSources();
void initialize();
