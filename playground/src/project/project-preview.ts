import { cppProjectPreview } from "../cpp-project-preview";
import { escapeMsbuild } from "../msbuild-escape";
import type { Target, Workspace } from "../workspace";

function escapeXml(value: string) {
  return value.replaceAll("&", "&amp;").replaceAll("<", "&lt;").replaceAll(">", "&gt;").replaceAll('"', "&quot;");
}

export function projectFileName(target: Target) {
  return target === "cpp" ? "CMakeLists.txt" : "Wireloom.csproj";
}

export function projectPreview(workspace: Workspace) {
  if (workspace.target === "cpp") return cppProjectPreview(workspace);

  const relative = (path: string) => (path === "/workspace" ? "." : path.slice("/workspace/".length));
  const projectSettings = new Map<string, string>();
  const { files } = workspace;

  if (workspace.options.sequenceArrayMapping !== "RtiCSharp") {
    projectSettings.set("DdsIdlSequenceArrayMapping", workspace.options.sequenceArrayMapping);
  }
  if (workspace.options.sequenceSize !== 100) {
    projectSettings.set("DdsIdlSequenceSize", String(workspace.options.sequenceSize));
  }
  if (workspace.options.stringSize !== 255) {
    projectSettings.set("DdsIdlStringSize", String(workspace.options.stringSize));
  }
  if (workspace.options.unboundedSupport) {
    projectSettings.set("DdsIdlUnboundedSupport", "true");
  }
  if (workspace.options.enableEscapeChar) {
    projectSettings.set("DdsIdlEnableEscapeChar", "true");
  }
  if (workspace.options.includeDirectories.length) {
    projectSettings.set(
      "DdsIdlIncludeDirectories",
      workspace.options.includeDirectories.map((path) => escapeMsbuild(relative(path))).join(";"),
    );
  }

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
    if (file.options.unboundedSupport !== undefined)
      settings.push(["UnboundedSupport", String(file.options.unboundedSupport)]);
    return settings.map(([key, value]) => `      <${key}>${escapeXml(value)}</${key}>`).join("\n");
  };

  const item = (tag: "DdsIdl" | "AdditionalFiles", file: Workspace["files"][number]) => {
    const inner = metadata(file);
    const opening = `    <${tag} Include="${escapeXml(escapeMsbuild(relative(file.path)))}"`;
    return inner ? `${opening}>\n${inner}\n    </${tag}>` : `${opening} />`;
  };

  const roots = files.filter((file) => file.isGenerationRoot).map((file) => item("DdsIdl", file));
  // Include-only files are discovered under the project root unless they carry per-file metadata.
  const additionalFiles = files
    .filter((file) => !file.isGenerationRoot && metadata(file))
    .map((file) => item("AdditionalFiles", file));
  const projectProperties = [...projectSettings].map(([key, value]) => `    <${key}>${escapeXml(value)}</${key}>`);
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
