import assert from "node:assert/strict";
import test from "node:test";
import { strFromU8, unzipSync } from "fflate";
import { sourceArchive } from "./source-archive.ts";
import { addFolder, createWorkspace, putFile } from "./workspace.ts";

test("source backups preserve exact UTF-8 text, virtual folders, roles, and settings", () => {
  // Arrange
  let workspace = addFolder(createWorkspace("// café 😀\r\nstruct Main { long x; };\r\n"), "/workspace/shared");
  workspace = addFolder(workspace, "/workspace/empty");
  workspace = putFile(workspace, "/workspace/shared/types.idl", "// include-only\n");
  workspace = putFile(workspace, "/workspace/workspace.json", "source named like the metadata");
  workspace.files[1].isGenerationRoot = false;
  workspace.files[1].options.stringSize = 42;
  workspace.options.includeDirectories = ["/workspace/shared"];
  workspace.options.defines = ["FEATURE=1"];
  workspace.profile = "7.3.1";
  const before = structuredClone(workspace);

  // Act
  const entries = unzipSync(sourceArchive(workspace));

  // Assert
  for (const file of workspace.files) assert.equal(strFromU8(entries[file.path.slice(1)]), file.source);
  assert.deepEqual(entries["workspace/empty/"], new Uint8Array());
  assert.deepEqual(entries["workspace/shared/"], new Uint8Array());
  assert.deepEqual(JSON.parse(strFromU8(entries["workspace.json"])), workspace);
  assert.deepEqual(workspace, before);
  assert.deepEqual(
    Object.keys(entries).sort(),
    [
      "workspace.json",
      "workspace/",
      "workspace/empty/",
      "workspace/main.idl",
      "workspace/shared/",
      "workspace/shared/types.idl",
      "workspace/workspace.json",
    ].sort(),
  );
});

test("source backups remain available for empty, rootless, and oversized unfinished workspaces", () => {
  const workspace = createWorkspace("broken source");
  workspace.files[0].isGenerationRoot = false;
  assert.equal(strFromU8(unzipSync(sourceArchive(workspace))["workspace/main.idl"]), "broken source");
  workspace.files[0].source = "é".repeat(131073);
  assert.equal(strFromU8(unzipSync(sourceArchive(workspace))["workspace/main.idl"]), workspace.files[0].source);
  workspace.files = [];
  const empty = unzipSync(sourceArchive(workspace));
  assert.deepEqual(Object.keys(empty).sort(), ["workspace.json", "workspace/"]);
  assert.deepEqual(JSON.parse(strFromU8(empty["workspace.json"])).files, []);
});
