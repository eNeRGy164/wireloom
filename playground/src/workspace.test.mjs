import assert from "node:assert/strict";

import testContext from "node:test";

import test from "node:test";

import "fake-indexeddb/auto";

import {
  RecoveryStore,
  sourceFits,
  validateWorkspace,
  createWorkspace,
  createFile,
  defaultOptions,
  validateOptions,
  validatePath,
  sourceBytes,
  putFile,
  addFolder,
  deleteEntry,
  moveEntry,
  hasPath,
  parentPath,
  isWithin,
} from "./workspace.ts";

import { indexedDB } from "fake-indexeddb";

const saved = (source) => ({
  version: 1,

  profile: "7.3.1",

  files: [{ path: "/workspace/imported.idl", source, isGenerationRoot: true }],
});

test("workspace limits count UTF-8 bytes and accept the exact boundary", () => {
  assert.equal(sourceFits("é".repeat(131072)), true);

  assert.equal(sourceFits("é".repeat(131073)), false);

  assert.equal(sourceFits("a".repeat(262144)), true);

  assert.equal(sourceFits("a".repeat(262145)), false);
});

test("recovery preserves the selected profile, file identity, and exact source", () => {
  const source = "// é\r\nstruct Imported { long value; };\r\n";

  const original = saved(source);

  const restored = validateWorkspace(original);

  assert.equal(restored.version, 2);

  assert.equal(restored.profile, original.profile);

  assert.equal(restored.files[0].path, original.files[0].path);

  assert.deepEqual(restored.options, defaultOptions());

  assert.deepEqual(restored.folders, []);

  original.files[0].source = "changed";

  assert.equal(restored.files[0].source, source);
});

test("recovery rejects older schemas, unsupported profiles, multiple files, and oversized data", () => {
  for (const value of [
    null,

    {},

    { ...saved(""), version: 0 },

    { ...saved(""), profile: "unknown" },

    { ...saved(""), files: [] },

    { ...saved(""), files: [saved("").files[0], saved("").files[0]] },

    saved("é".repeat(131073)),
  ]) {
    assert.throws(() => validateWorkspace(value));
  }
});

test("recovery rejects host paths, traversal, malformed source, and non-root inputs", () => {
  for (const path of [
    "C:\\input.idl",

    "/workspace/../escape.idl",

    "/workspace/sub/file.idl",

    "/workspace/bad\0.idl",

    "/other/main.idl",

    "/workspace/main.txt",
  ]) {
    assert.throws(() => validateWorkspace({ ...saved(""), files: [{ ...saved("").files[0], path }] }));
  }

  assert.throws(() =>
    validateWorkspace({ ...saved(""), files: [{ path: "/workspace/main.idl", source: 5, isGenerationRoot: true }] }),
  );

  assert.throws(() => validateWorkspace({ ...saved(""), files: [{ ...saved("").files[0], isGenerationRoot: false }] }));
});

test("IndexedDB creates an empty store, persists cloned source and profile, and serializes reset", async () => {
  // Arrange

  const store = new RecoveryStore();

  const source = saved("// café\r\nstruct Sample { long value; };");

  // Act

  assert.equal(await store.open(), undefined);

  const saving = store.save(source);

  source.files[0].source = "changed after save";

  await saving;

  const restored = new RecoveryStore();

  const value = await restored.open();

  const clearing = restored.clear();

  const savedAgain = restored.save(saved("latest"));

  await Promise.all([clearing, savedAgain]);

  const finalStore = new RecoveryStore();

  const final = await finalStore.open();

  // Assert

  assert.equal(value.files[0].source, "// café\r\nstruct Sample { long value; };");

  assert.equal(value.profile, "7.3.1");

  assert.equal(final.files[0].source, "latest");
});

test("IndexedDB rejects writes before open and after a version change closes the database", async () => {
  // Arrange

  const unopened = new RecoveryStore();

  const store = new RecoveryStore();

  await store.open();

  // Act and Assert

  await assert.rejects(unopened.save(saved("")), /unavailable/);

  store["database"].onversionchange();

  await assert.rejects(store.clear());
});

test("IndexedDB restores invalid stored data as a storage error", async () => {
  // Arrange

  const database = await new Promise((resolve, reject) => {
    const request = indexedDB.open("wireloom-playground-corrupt", 1);

    request.onupgradeneeded = () => request.result.createObjectStore("workspace");

    request.onsuccess = () => resolve(request.result);

    request.onerror = () => reject(request.error);
  });

  const transaction = database.transaction("workspace", "readwrite");

  transaction.objectStore("workspace").put({ version: 0 }, "current");

  await new Promise((resolve) => (transaction.oncomplete = resolve));

  database.close();

  const original = indexedDB.open.bind(indexedDB);

  indexedDB.open = (name, version) =>
    original(name === "wireloom-playground" ? "wireloom-playground-corrupt" : name, version);

  // Act and Assert

  try {
    await assert.rejects(new RecoveryStore().open(), /unsupported format/);
  } finally {
    indexedDB.open = original;
  }
});

test("v2 recovery supports empty and rootless workspaces, nested files, and empty folders", () => {
  let workspace = createWorkspace("struct Main { long x; };");

  workspace = addFolder(workspace, "/workspace/includes");

  workspace = addFolder(workspace, "/workspace/empty");

  workspace = putFile(workspace, "/workspace/includes/types.idl", "// caf\u00e9");

  workspace.files.forEach((file) => {
    file.isGenerationRoot = false;
  });

  workspace.options.includeDirectories = ["/workspace/empty", "/workspace/includes"];

  const restored = validateWorkspace(workspace);

  assert.deepEqual(restored, workspace);

  workspace.files[0].source = "mutated";

  assert.notEqual(restored.files[0].source, workspace.files[0].source);

  assert.deepEqual(deleteEntry(deleteEntry(restored, "/workspace/main.idl"), "/workspace/includes").files, []);

  assert.deepEqual(validateWorkspace({ ...createWorkspace(), files: [] }).files, []);

  assert.equal(sourceBytes(createWorkspace("\u00e9")), 2);
});

test("virtual paths are normalized and bounded without interpreting source folders", () => {
  validatePath("/workspace", true);

  validatePath("/workspace/folder/any.txt");

  for (const path of [
    null,
    5,
    "",
    "/workspace",
    "/other/a",
    "/workspace/a\\b",
    "/workspace/../x",
    "/workspace/./x",
    "/workspace//x",
    "/workspace/x/",
    "/workspace/x\0",
    "/workspace/x\x7f",
    "/workspace/ padded",
    "/workspace/padded ",
    "/workspace/" + "x".repeat(1024),
    "/workspace/" + Array(33).fill("a").join("/"),
  ]) {
    assert.throws(() => validatePath(path));
  }

  assert.equal(parentPath("/workspace/a/file.idl"), "/workspace/a");

  assert.equal(isWithin("/workspace/ab/file", "/workspace/a"), false);
});

test("workspace validation rejects malformed identities, settings, collisions, and missing parents", () => {
  const valid = createWorkspace();
  assert.throws(() => validateWorkspace({ ...valid, activeFileId: "missing" }));

  for (const mutation of [
    (w) => {
      w.version = 3;
    },
    (w) => {
      w.profile = "unknown";
    },
    (w) => {
      w.files = null;
    },

    (w) => {
      w.folders = null;
    },
    (w) => {
      w.files = Array(513).fill(w.files[0]);
    },

    (w) => {
      w.folders = Array(513).fill("/workspace/dir");
    },

    (w) => {
      w.files = [null];
    },
    (w) => {
      w.files[0].source = null;
    },

    (w) => {
      w.files[0].isGenerationRoot = null;
    },
    (w) => {
      w.files[0].id = 1;
    },

    (w) => {
      w.files[0].id = "";
    },
    (w) => {
      w.files[0].id = "x".repeat(129);
    },

    (w) => {
      w.files.push({ ...w.files[0], path: "/workspace/other.idl" });
    },

    (w) => {
      w.files.push({ ...w.files[0], id: "unique" });
    },

    (w) => {
      w.folders = ["/workspace/main.idl"];
    },

    (w) => {
      w.folders = ["/workspace/a", "/workspace/a"];
    },

    (w) => {
      w.folders = ["/workspace/missing/nested"];
    },

    (w) => {
      w.files[0].path = "/workspace/missing/file.idl";
    },

    (w) => {
      w.files[0].options = null;
    },
    (w) => {
      w.files[0].options.sequenceArrayMapping = "bad";
    },

    (w) => {
      w.files[0].options.sequenceSize = 0;
    },
    (w) => {
      w.files[0].options.stringSize = 1.5;
    },

    (w) => {
      w.files[0].options.stringSize = 2147483648;
    },

    (w) => {
      w.options.includeDirectories = ["/workspace/missing"];
    },

    (w) => {
      w.files[0].source = "a".repeat(262145);
    },
  ]) {
    const workspace = structuredClone(valid);

    mutation(workspace);

    assert.throws(() => validateWorkspace(workspace));
  }
});

test("workspace options use alphabetical include precedence and reject malformed or unbounded lists", () => {
  const input = {
    ...defaultOptions(),
    includeDirectories: ["/workspace/z", "/workspace", "/workspace/a", "/workspace/z"],
  };

  const options = validateOptions(input);

  assert.deepEqual(options.includeDirectories, ["/workspace", "/workspace/a", "/workspace/z"]);

  options.defines.push("CHANGED");

  assert.deepEqual(input.defines, []);

  for (const invalid of [null, {}, { ...defaultOptions(), strict: 1 }, { ...defaultOptions(), enableEscapeChar: 1 }])
    assert.throws(() => validateOptions(invalid));

  for (const key of ["defines", "undefines", "includeDirectories"]) {
    for (const value of [null, [1], [""], ["a\nb"], ["a".repeat(1025)], Array(129).fill("a")])
      assert.throws(() => validateOptions({ ...defaultOptions(), [key]: value }));
  }

  assert.throws(() => validateOptions({ ...defaultOptions(), includeDirectories: ["host"] }));
});

test("file and folder operations are atomic, preserve identities, and follow configured folder moves", () => {
  let workspace = addFolder(createWorkspace(), "/workspace/includes");

  workspace = addFolder(workspace, "/workspace/includes/nested");

  workspace = putFile(workspace, "/workspace/includes/nested/shared.idl", '#include "unchanged.idl"');

  const id = workspace.files[1].id;

  workspace.options.includeDirectories = ["/workspace/includes", "/workspace/includes/nested"];

  assert.equal(hasPath(workspace, "/workspace"), true);

  assert.equal(hasPath(workspace, "/workspace/includes"), true);

  assert.equal(hasPath(workspace, "/workspace/main.idl"), true);

  assert.equal(hasPath(workspace, "/workspace/missing"), false);

  const moved = moveEntry(workspace, "/workspace/includes", "/workspace/shared");

  assert.equal(moved.files[1].id, id);

  assert.equal(moved.files[1].path, "/workspace/shared/nested/shared.idl");

  assert.equal(moved.files[1].source, '#include "unchanged.idl"');

  assert.deepEqual(moved.options.includeDirectories, ["/workspace/shared", "/workspace/shared/nested"]);

  assert.equal(workspace.files[1].path, "/workspace/includes/nested/shared.idl");

  assert.deepEqual(moveEntry(workspace, "/workspace/main.idl", "/workspace/main.idl"), workspace);

  const replaced = putFile(workspace, "/workspace/main.idl", "new source", true);

  assert.equal(replaced.files[0].id, workspace.files[0].id);

  assert.equal(replaced.files[0].source, "new source");

  for (const operation of [
    () => addFolder(workspace, "/workspace/includes"),
    () => putFile(workspace, "/workspace/main.idl", "x"),

    () => putFile(workspace, "/workspace/includes", "x", true),

    () => moveEntry(workspace, "/workspace/includes", "/workspace/main.idl"),

    () => moveEntry(workspace, "/workspace/includes", "/workspace/includes/deep"),

    () => moveEntry(workspace, "/workspace/includes/nested", "/workspace/includes", true),

    () => moveEntry(workspace, "/workspace/missing", "/workspace/new"),

    () => putFile(workspace, "/workspace/missing/file.idl", "x"),

    () => deleteEntry(workspace, "/workspace"),
  ])
    assert.throws(operation);

  const replacement = moveEntry(workspace, "/workspace/main.idl", "/workspace/includes", true);

  assert.equal(replacement.files.length, 1);

  assert.equal(replacement.files[0].path, "/workspace/includes");

  assert.deepEqual(replacement.folders, []);

  assert.deepEqual(replacement.options.includeDirectories, []);

  workspace.activeFileId = id;
  const deleted = deleteEntry(workspace, "/workspace/includes");
  assert.equal(deleted.activeFileId, undefined);

  assert.equal(deleted.files.length, 1);

  assert.deepEqual(deleted.options.includeDirectories, []);
});

test("aggregate source limit applies across files without mutating an existing workspace", () => {
  const workspace = createWorkspace("\u00e9".repeat(65536));

  const full = putFile(workspace, "/workspace/second.idl", "\u00e9".repeat(65536));

  assert.equal(sourceBytes(full), 262144);

  assert.throws(() => putFile(full, "/workspace/third.idl", "x"), /256 KiB/);

  assert.equal(full.files.length, 2);

  assert.equal(workspace.files.length, 1);

  assert.equal(createFile("/workspace/new.idl").source, "");
});

test("moves require an existing destination folder and leave failed operations unchanged", () => {
  // Arrange
  const workspace = addFolder(createWorkspace("source"), "/workspace/target");
  const before = structuredClone(workspace);

  // Act and Assert
  assert.throws(
    () => moveEntry(workspace, "/workspace/main.idl", "/workspace/missing/main.idl"),
    /Destination folder does not exist/,
  );
  assert.deepEqual(workspace, before);
  const moved = moveEntry(workspace, "/workspace/main.idl", "/workspace/target/main.idl");
  assert.equal(moved.files[0].path, "/workspace/target/main.idl");
  assert.equal(moved.files[0].source, "source");
});
