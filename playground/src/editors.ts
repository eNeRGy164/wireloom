import * as monaco from "monaco-editor/editor/editor.api.js";
import "monaco-editor/languages/definitions/csharp/register.js";
import EditorWorker from "monaco-editor/editor/editor.worker.js?worker";
import type { Diagnostic } from "./protocol";
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
  private inputModel: monaco.editor.ITextModel;
  private outputs = new Map<string, monaco.editor.ITextModel>();
  private views = new Map<string, monaco.editor.ICodeEditorViewState | null>();
  private active?: string;

  constructor(input: HTMLElement, output: HTMLElement, path: string, source: string) {
    this.inputModel = monaco.editor.createModel(source, "idl", monaco.Uri.file(path));
    this.input = monaco.editor.create(input, { ...common, model: this.inputModel, ariaLabel: "IDL source editor" });
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
    return this.inputModel.getValue(monaco.editor.EndOfLinePreference.TextDefined);
  }

  setFontSize(fontSize: number) {
    const options = { fontSize, lineHeight: Math.round(fontSize * 1.65) };
    this.input.updateOptions(options);
    this.output.updateOptions(options);
  }

  setSource(path: string, source: string) {
    const old = this.inputModel;
    this.input.setModel(null);
    old.dispose();
    this.inputModel = monaco.editor.createModel(source, "idl", monaco.Uri.file(path));
    this.input.setModel(this.inputModel);
  }

  positionAt(offset: number) {
    return this.inputModel.getPositionAt(offset);
  }

  clearMarkers() {
    monaco.editor.setModelMarkers(this.inputModel, "wireloom", []);
  }

  markers(diagnostics: Diagnostic[], path: string) {
    monaco.editor.setModelMarkers(
      this.inputModel,
      "wireloom",
      diagnostics
        .filter((d) => d.filePath === path && d.startOffset >= 0)
        .map((d) => {
          const start = this.inputModel.getPositionAt(d.startOffset);
          const end = this.inputModel.getPositionAt(d.startOffset + Math.max(1, d.length));
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

  navigate(diagnostic: Diagnostic) {
    const start = this.inputModel.getPositionAt(diagnostic.startOffset);
    const end = this.inputModel.getPositionAt(diagnostic.startOffset + Math.max(1, diagnostic.length));
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
