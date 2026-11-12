import type { Target } from "./workspace";

export type FileIconKind = "idl" | "csharp" | "csproj" | "cpp" | "cpp-header" | "cmake" | "cmake-script";

/** Selects a file icon from the generated artifact name and active target. */
export function generatedFileIconKind(name: string, target: Target, projectName: string): FileIconKind {
  if (name === projectName) return target === "cpp" ? "cmake" : "csproj";
  if (target !== "cpp") return "csharp";
  if (/\.cmake$/i.test(name)) return "cmake-script";
  return /\.(?:hpp|hxx|h\+\+|hh)$/i.test(name) ? "cpp-header" : "cpp";
}
