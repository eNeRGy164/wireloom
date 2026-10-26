import assert from "node:assert/strict";
import testContext from "node:test";
import test from "node:test";
import "fake-indexeddb/auto";
import { RecoveryStore, sourceFits, validateWorkspace } from "./workspace.ts";
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
  assert.deepEqual(restored, original);
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
