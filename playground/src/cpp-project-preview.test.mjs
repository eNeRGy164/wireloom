import assert from "node:assert/strict";
import test from "node:test";
import { addFolder, createWorkspace, putFile } from "./workspace.ts";
import { cppProjectPreview } from "./cpp-project-preview.ts";

test("C++ project preview creates a CMake generation target for each root", () => {
  let workspace = createWorkspace("struct Main { long value; };");
  workspace.target = "cpp";
  workspace = addFolder(workspace, "/workspace/nested");
  workspace = addFolder(workspace, "/workspace/includes");
  workspace = putFile(workspace, "/workspace/nested/other.idl", "struct Other { long value; };");
  workspace.options.includeDirectories = ["/workspace/includes"];
  workspace.profile = "7.3.1";
  workspace.options.unboundedSupport = true;
  workspace.options.enableEscapeChar = true;
  workspace.options.sequenceSize = 12;
  workspace.options.stringSize = 42;
  workspace.files[0].options.strict = true;
  workspace.files[0].options.defines = ["FEATURE=1"];
  workspace.files[0].options.undefines = ["DISABLED"];
  workspace.files[0].options.sequenceSize = 12;
  workspace.files[0].options.stringSize = 42;
  workspace.files[1].options.sequenceSize = 24;
  workspace.files[1].options.unboundedSupport = false;

  const project = cppProjectPreview(workspace);

  assert.match(project, /project\(WireloomPlayground LANGUAGES CXX\)/);
  assert.match(project, /find_package\(WireloomCpp CONFIG REQUIRED\)/);
  assert.match(project, /WIRELOOM_CPP_RTI_VERSION "7\.3\.1"/);
  assert.match(project, /wireloom_add_idl\(wireloom_idl_1/);
  assert.match(project, /wireloom_add_idl\(wireloom_idl_2/);
  assert.match(project, /UNBOUNDED_SUPPORT/);
  assert.match(project, /ENABLE_ESCAPE_CHAR/);
  assert.match(project, /FILE_SEQUENCE_SIZES "main\.idl" 12 "nested\/other\.idl" 24/);
  assert.match(project, /FILE_STRING_SIZES "main\.idl" 42/);
  assert.match(project, /FILE_UNBOUNDED_SUPPORT "nested\/other\.idl" false/);
  assert.match(project, /DEFINES "FEATURE=1"/);
  assert.match(project, /UNDEFINES "DISABLED"/);
  assert.match(project, /INCLUDE_DIRECTORIES "includes"/);
  assert.match(project, /WIRELOOM_GENERATED_OUTPUTS/);
  assert.match(project, /wireloom_outputs_1/);
  assert.match(project, /wireloom_outputs_2/);
  assert.match(project, /add_custom_target\(wireloom_generate DEPENDS/);
});

test("C++ project preview carries finite-support overrides and safely quotes paths", () => {
  let workspace = createWorkspace("struct Sample { long value; };");
  workspace.target = "cpp";
  workspace.options.unboundedSupport = true;
  workspace.files[0].options.unboundedSupport = false;
  workspace.files[0].path = '/workspace/quoted"name.idl';

  const project = cppProjectPreview(workspace);

  assert.match(project, /FINITE_SUPPORT/);
  assert.match(project, /quoted\\"name\.idl/);
  assert.doesNotMatch(project, /\n  UNBOUNDED_SUPPORT\n/);
});

test("C++ project preview omits generation targets when no files are roots", () => {
  const workspace = createWorkspace("struct Sample { long value; };");
  workspace.target = "cpp";
  workspace.files[0].isGenerationRoot = false;
  workspace.options.sequenceSize = 12;
  workspace.options.stringSize = 42;

  const project = cppProjectPreview(workspace);

  assert.doesNotMatch(project, /wireloom_add_idl/);
  assert.doesNotMatch(project, /wireloom_generate/);
});
