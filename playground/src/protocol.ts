export interface WorkspaceFile {
  path: string;
  source: string;
  isGenerationRoot: boolean;
  options?: FileOptions;
}

export interface FileOptions {
  sequenceArrayMapping: "RtiCSharp" | "Idl";
  sequenceSize: number;
  stringSize: number;
}

export interface WorkspaceOptions {
  defines: string[];
  undefines: string[];
  includeDirectories: string[];
  strict: boolean;
  enableEscapeChar: boolean;
}

export interface CompileRequest {
  protocolVersion: 2;
  workerGeneration: number;
  requestId: number;
  workspaceRevision: number;
  profile: "7.3.1" | "7.7.0";
  files: WorkspaceFile[];
  options: WorkspaceOptions;
}

export interface Diagnostic {
  id: string;
  severity: string;
  message: string;
  filePath: string | null;
  startOffset: number;
  length: number;
}

export interface Document {
  name: string;
  source: string;
}

export interface CompileResponse {
  protocolVersion: number;
  workerGeneration: number;
  requestId: number;
  workspaceRevision: number;
  profile: string;
  kind: "compiler" | "limit";
  success: boolean;
  documents: Document[];
  diagnostics: Diagnostic[];
  error: string | null;
}

export type WorkerMessage =
  | { type: "ready" }
  | { type: "startup-error"; message: string }
  | { type: "result"; response: CompileResponse }
  | { type: "error"; requestId: number; message: string };
