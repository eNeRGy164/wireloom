import * as monaco from "monaco-editor/editor/editor.api.js";
import "monaco-editor/editor/contrib/folding/browser/folding.js";
import "monaco-editor/languages/definitions/csharp/register.js";
import EditorWorker from "monaco-editor/editor/editor.worker.js?worker";
import type { Diagnostic } from "./protocol";
import type { SourceFile } from "./workspace";
import { onThemeChange } from "./theme";
import { xmlCommentEnd } from "./xml-comment";

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
  wordPattern: /[A-Za-z_][\w]*/g,
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

monaco.languages.register({ id: "xml" });
monaco.languages.setMonarchTokensProvider("xml", {
  tokenizer: {
    root: [
      [/<!--/, "comment", "@comment"],
      [/<\?/, "metatag", "@processing"],
      [/<\/?[A-Za-z_][\w:.-]*/, "tag"],
      [/\b[A-Za-z_][\w:.-]*(?=\s*=)/, "attribute.name"],
      [/\/?\s*>/, "delimiter"],
      [/=/, "delimiter"],
      [/["'][^"']*["']/, "string"],
    ],
    comment: [
      [xmlCommentEnd, "comment", "@pop"],
      [/./, "comment"],
    ],
    processing: [
      [/\?>/, "metatag", "@pop"],
      [/./, "metatag"],
    ],
  },
});

monaco.languages.registerFoldingRangeProvider("idl", {
  provideFoldingRanges(model) {
    const ranges: monaco.languages.FoldingRange[] = [];
    const openLines: number[] = [];
    let blockComment = false;
    let quote = "";

    for (let lineNumber = 1; lineNumber <= model.getLineCount(); lineNumber++) {
      const line = model.getLineContent(lineNumber);
      for (let index = 0; index < line.length; index++) {
        const character = line[index];
        const next = line[index + 1];
        if (blockComment) {
          if (character === "*" && next === "/") {
            blockComment = false;
            index++;
          }
          continue;
        }
        if (quote) {
          if (character === "\\") index++;
          else if (character === quote) quote = "";
          continue;
        }
        if (character === "/" && next === "/") break;
        if (character === "/" && next === "*") {
          blockComment = true;
          index++;
        } else if (character === '"' || character === "'") quote = character;
        else if (character === "{") openLines.push(lineNumber);
        else if (character === "}" && openLines.length) {
          const start = openLines.pop()!;
          if (lineNumber > start) ranges.push({ start, end: lineNumber });
        }
      }
      if (quote) quote = "";
    }

    return ranges.sort((left, right) => left.start - right.start || right.end - left.end);
  },
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
  glyphMargin: true,
  folding: true,
  showFoldingControls: "always",
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
    input.addEventListener(
      "keydown",
      (event) => {
        const isMac = /Mac|iPhone|iPad/.test(navigator.platform);
        if (event.key !== "ArrowLeft" && event.key !== "ArrowRight") return;
        if (isMac ? !event.altKey || event.ctrlKey : !event.ctrlKey || event.altKey) return;
        event.preventDefault();
        event.stopPropagation();
        this.moveByWord(event.key === "ArrowLeft" ? -1 : 1, event.shiftKey);
      },
      true,
    );
    onThemeChange((theme) => monaco.editor.setTheme(theme === "dark" ? "vs-dark" : "vs"));
  }

  private moveByWord(direction: -1 | 1, extend: boolean) {
    const model = this.input.getModel();
    const selection = this.input.getSelection();
    if (!model || !selection) return;
    const text = model.getValue();
    const activeOffset = model.getOffsetAt(selection.getPosition());
    let offset = activeOffset;
    if (!extend && !selection.isEmpty()) {
      offset = model.getOffsetAt(direction < 0 ? selection.getStartPosition() : selection.getEndPosition());
    }
    const isWord = (character: string | undefined) => character !== undefined && /[A-Za-z0-9_]/.test(character);
    if (direction < 0) {
      if (offset > 0) offset--;
      while (offset > 0 && !isWord(text[offset])) offset--;
      while (offset > 0 && isWord(text[offset - 1])) offset--;
    } else {
      while (offset < text.length && isWord(text[offset])) offset++;
      while (offset < text.length && !isWord(text[offset])) offset++;
    }
    const destination = model.getPositionAt(offset);
    if (!extend) this.input.setPosition(destination);
    else {
      const anchor = selection.isEmpty()
        ? selection.getPosition()
        : selection.getDirection() === monaco.SelectionDirection.LTR
          ? selection.getStartPosition()
          : selection.getEndPosition();
      this.input.setSelection(
        new monaco.Selection(anchor.lineNumber, anchor.column, destination.lineNumber, destination.column),
      );
    }
    this.input.revealPositionInCenter(destination);
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
          doc.name.toLowerCase().endsWith(".csproj") ? "xml" : "csharp",
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

  updateOutput(name: string, source: string) {
    const model = this.outputs.get(name);
    if (model && model.getValue(monaco.editor.EndOfLinePreference.TextDefined) !== source) model.setValue(source);
  }
}
