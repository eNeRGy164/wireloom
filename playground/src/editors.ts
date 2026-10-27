import * as monaco from "monaco-editor/editor/editor.api.js";
import "monaco-editor/languages/definitions/csharp/register.js";
import EditorWorker from "monaco-editor/editor/editor.worker.js?worker";
import type { Diagnostic } from "./protocol";
import type { SourceFile } from "./workspace";
import { onThemeChange } from "./theme";

self.MonacoEnvironment = { getWorker: () => new EditorWorker() };
monaco.languages.register({ id: "idl" });
monaco.languages.setMonarchTokensProvider("idl", {
  keywords: [
    "module",
    "struct",
    "union",
    "switch",
    "case",
    "default",
    "enum",
    "typedef",
    "const",
    "interface",
    "exception",
    "bitmask",
    "bitset",
    "valuetype",
    "public",
    "private",
  ],
  typeKeywords: [
    "long",
    "short",
    "unsigned",
    "float",
    "double",
    "boolean",
    "char",
    "wchar",
    "octet",
    "string",
    "wstring",
    "sequence",
    "map",
    "int8",
    "uint8",
    "int16",
    "uint16",
    "int32",
    "uint32",
    "int64",
    "uint64",
  ],
  tokenizer: {
    root: [
      [/\/\*/, "comment", "@comment"],
      [/\/\/.*$/, "comment"],
      [/^\s*#.*$/, "keyword"],
      [/@[\w:]+/, "annotation"],
      [/"([^"\\]|\\.)*"/, "string"],
      [/'([^'\\]|\\.)*'/, "string"],
      [/\b\d+(\.\d+)?([eE][+-]?\d+)?\b/, "number"],
      [/[a-zA-Z_]\w*/, { cases: { "@keywords": "keyword", "@typeKeywords": "type", "@default": "identifier" } }],
      [/[{}()[\]<>]/, "@brackets"],
      [/[;,:]/, "delimiter"],
    ],
    comment: [
      [/[^/*]+/, "comment"],
      [/\*\//, "comment", "@pop"],
      [/[/*]/, "comment"],
    ],
  },
});
monaco.languages.setLanguageConfiguration("idl", {
  comments: { lineComment: "//", blockComment: ["/*", "*/"] },
  brackets: [
    ["{", "}"],
    ["[", "]"],
    ["(", ")"],
  ],
  autoClosingPairs: [
    { open: "{", close: "}" },
    { open: "(", close: ")" },
    { open: '"', close: '"' },
  ],
});

const common: monaco.editor.IStandaloneEditorConstructionOptions = {
  automaticLayout: true,
  fontSize: 12,
  lineHeight: 20,
  fontFamily: "'CaskaydiaCove Nerd Font', 'Cascadia Code', 'SFMono-Regular', Consolas, monospace",
  minimap: { enabled: false },
  scrollBeyondLastLine: false,
  padding: { top: 20 },
  renderLineHighlight: "gutter",
  tabSize: 4,
  wordWrap: "off",
  fixedOverflowWidgets: true,
};

export class Editors {
  readonly input: monaco.editor.IStandaloneCodeEditor;
  readonly output: monaco.editor.IStandaloneCodeEditor;
  private inputs = new Map<string, { path: string; model: monaco.editor.ITextModel }>();
  private inputViews = new Map<string, monaco.editor.ICodeEditorViewState | null>();
  private activeInput?: string;
  private outputs = new Map<string, monaco.editor.ITextModel>();
  private views = new Map<string, monaco.editor.ICodeEditorViewState | null>();
  private active?: string;

  constructor(input: HTMLElement, output: HTMLElement) {
    this.input = monaco.editor.create(input, { ...common, model: null, ariaLabel: "IDL source editor" });
    this.output = monaco.editor.create(output, {
      ...common,
      model: null,
      readOnly: true,
      domReadOnly: true,
      ariaLabel: "Generated C# editor",
    });
    onThemeChange((theme) => monaco.editor.setTheme(theme === "dark" ? "vs-dark" : "vs"));
  }

  get source() {
    return this.input.getModel()?.getValue(monaco.editor.EndOfLinePreference.TextDefined) ?? "";
  }

  setFontSize(fontSize: number) {
    const options = { fontSize, lineHeight: Math.round(fontSize * 1.65) };
    this.input.updateOptions(options);
    this.output.updateOptions(options);
  }

  syncSources(files: SourceFile[], activeId?: string) {
    if (this.activeInput) this.inputViews.set(this.activeInput, this.input.saveViewState());
    this.input.setModel(null);
    for (const [id, entry] of this.inputs) {
      if (!files.some((file) => file.id === id)) {
        entry.model.dispose();
        this.inputs.delete(id);
        this.inputViews.delete(id);
      }
    }
    for (const file of files) {
      let entry = this.inputs.get(file.id);
      if (!entry) {
        entry = {
          path: file.path,
          model: monaco.editor.createModel(
            file.source,
            "idl",
            monaco.Uri.parse(`inmemory://source/${encodeURIComponent(file.id)}`),
          ),
        };
        this.inputs.set(file.id, entry);
      } else if (entry.model.getValue(monaco.editor.EndOfLinePreference.TextDefined) !== file.source) {
        entry.model.setValue(file.source);
      }
      entry.path = file.path;
    }
    this.activeInput = activeId;
    this.input.setModel(activeId ? (this.inputs.get(activeId)?.model ?? null) : null);
    this.input.restoreViewState(activeId ? (this.inputViews.get(activeId) ?? null) : null);
  }

  positionAt(offset: number, path?: string) {
    const model = path ? [...this.inputs.values()].find((entry) => entry.path === path)?.model : this.input.getModel();
    return model?.getPositionAt(offset) ?? { lineNumber: 1, column: 1 };
  }

  clearMarkers() {
    for (const { model } of this.inputs.values()) monaco.editor.setModelMarkers(model, "wireloom", []);
  }

  markers(diagnostics: Diagnostic[]) {
    for (const { path, model } of this.inputs.values()) {
      monaco.editor.setModelMarkers(
        model,
        "wireloom",
        diagnostics
          .filter((d) => d.filePath === path && d.startOffset >= 0)
          .map((d) => {
            const start = model.getPositionAt(d.startOffset);
            const end = model.getPositionAt(d.startOffset + Math.max(1, d.length));
            return {
              startLineNumber: start.lineNumber,
              startColumn: start.column,
              endLineNumber: end.lineNumber,
              endColumn: end.column,
              message: `${d.id}: ${d.message}`,
              severity:
                d.severity.toLowerCase() === "error"
                  ? monaco.MarkerSeverity.Error
                  : d.severity.toLowerCase() === "warning"
                    ? monaco.MarkerSeverity.Warning
                    : monaco.MarkerSeverity.Info,
            };
          }),
      );
    }
  }

  navigate(diagnostic: Diagnostic) {
    const model = this.input.getModel();
    if (!model) return;

    const start = model.getPositionAt(diagnostic.startOffset);
    const end = model.getPositionAt(diagnostic.startOffset + Math.max(1, diagnostic.length));

    this.input.setSelection(new monaco.Range(start.lineNumber, start.column, end.lineNumber, end.column));
    this.input.revealPositionInCenter(start);
    this.input.focus();
  }

  setOutputs(documents: { name: string; source: string }[]) {
    this.output.setModel(null);
    for (const model of this.outputs.values()) model.dispose();
    this.outputs.clear();
    this.views.clear();
    this.active = undefined;
    for (const doc of documents)
      this.outputs.set(
        doc.name,
        monaco.editor.createModel(
          doc.source,
          "csharp",
          monaco.Uri.parse(`inmemory://generated/${encodeURIComponent(doc.name)}`),
        ),
      );
  }

  select(name: string) {
    if (this.active) this.views.set(this.active, this.output.saveViewState());
    this.active = name;
    this.output.setModel(this.outputs.get(name) ?? null);
    this.output.restoreViewState(this.views.get(name) ?? null);
  }
}
