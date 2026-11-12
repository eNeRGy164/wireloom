import type { Workspace } from "./workspace";

function cmakeQuote(value: string) {
  const escaped = value.replaceAll("\\", "\\\\").replaceAll('"', '\\"').replaceAll("$", "\\$").replaceAll(";", "\\;");
  return `"${escaped}"`;
}

/** Creates the CMake project preview for the current virtual workspace. */
export function cppProjectPreview(workspace: Workspace): string {
  const relative = (path: string) => (path === "/workspace" ? "." : path.slice("/workspace/".length));
  const roots = workspace.files.filter((file) => file.isGenerationRoot);
  const lines = [
    "# Preview based on the current Playground workspace.",
    "cmake_minimum_required(VERSION 3.20)",
    "project(WireloomPlayground LANGUAGES CXX)",
    `set(WIRELOOM_CPP_RTI_VERSION ${cmakeQuote(workspace.profile)} CACHE STRING "RTI Connext version")`,
    "find_package(WireloomCpp CONFIG REQUIRED)",
    "",
  ];
  const generatedOutputLists: string[] = [];
  roots.forEach((file, index) => {
    const name = `wireloom_idl_${index + 1}`;
    const outputsName = `wireloom_outputs_${index + 1}`;
    lines.push(`add_library(${name} OBJECT EXCLUDE_FROM_ALL)`);
    const args = [`IDL ${cmakeQuote(relative(file.path))}`];
    const unboundedSupport = file.options.unboundedSupport ?? workspace.options.unboundedSupport;
    if (unboundedSupport) args.push("UNBOUNDED_SUPPORT");
    else if (workspace.options.unboundedSupport) args.push("FINITE_SUPPORT");
    if (workspace.options.enableEscapeChar) args.push("ENABLE_ESCAPE_CHAR");
    if (file.options.strict) args.push("STRICT");
    if (workspace.options.sequenceSize !== 100) args.push(`SEQUENCE_SIZE ${workspace.options.sequenceSize}`);
    if (workspace.options.stringSize !== 255) args.push(`STRING_SIZE ${workspace.options.stringSize}`);
    if (file.options.defines.length) args.push(`DEFINES ${file.options.defines.map(cmakeQuote).join(" ")}`);
    if (file.options.undefines.length) args.push(`UNDEFINES ${file.options.undefines.map(cmakeQuote).join(" ")}`);
    if (workspace.options.includeDirectories.length)
      args.push(
        `INCLUDE_DIRECTORIES ${workspace.options.includeDirectories.map((path) => cmakeQuote(relative(path))).join(" ")}`,
      );
    const fileSequenceSizes = workspace.files.flatMap((item) =>
      item.options.sequenceSize === undefined
        ? []
        : [cmakeQuote(relative(item.path)), String(item.options.sequenceSize)],
    );
    const fileStringSizes = workspace.files.flatMap((item) =>
      item.options.stringSize === undefined ? [] : [cmakeQuote(relative(item.path)), String(item.options.stringSize)],
    );
    const fileUnboundedSettings = workspace.files.flatMap((item) =>
      item.options.unboundedSupport === undefined
        ? []
        : [cmakeQuote(relative(item.path)), String(item.options.unboundedSupport)],
    );
    if (fileSequenceSizes.length) args.push(`FILE_SEQUENCE_SIZES ${fileSequenceSizes.join(" ")}`);
    if (fileStringSizes.length) args.push(`FILE_STRING_SIZES ${fileStringSizes.join(" ")}`);
    if (fileUnboundedSettings.length) args.push(`FILE_UNBOUNDED_SUPPORT ${fileUnboundedSettings.join(" ")}`);
    lines.push(`wireloom_add_idl(${name}`, ...args.map((arg) => `  ${arg}`), ")", "");
    lines.push(`get_target_property(${outputsName} ${name} WIRELOOM_GENERATED_OUTPUTS)`);
    generatedOutputLists.push(outputsName);
  });
  if (roots.length) {
    const generatedOutputReferences = generatedOutputLists.map((name) => `\${${name}}`);
    lines.push(
      "set(wireloom_generated_outputs)",
      `list(APPEND wireloom_generated_outputs ${generatedOutputReferences.join(" ")})`,
      "add_custom_target(wireloom_generate DEPENDS ${wireloom_generated_outputs})",
      "",
    );
  }
  return `${lines.join("\n")}\n`;
}
