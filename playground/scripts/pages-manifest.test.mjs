import assert from "node:assert/strict";
import { mkdtemp, rm, symlink, writeFile } from "node:fs/promises";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { test } from "node:test";
import { inventory, validateBase, verifyManifest } from "./pages-manifest.mjs";

test("Pages accepts root and project paths and rejects paths that can escape the site", () => {
  assert.equal(validateBase("/"), "/");
  assert.equal(validateBase("/wireloom/"), "/wireloom/");
  for (const base of ["//example.com/", "/../", "/%2e%2e/", "wireloom/", "/wireloom", "/a?b/", "/a\\b/"]) {
    assert.throws(() => validateBase(base));
  }
});

test("Pages detects changed, added, missing, or substituted artifact files and revisions", async () => {
  const directory = await mkdtemp(join(tmpdir(), "wireloom-pages-"));
  const revision = "a".repeat(40);
  try {
    await writeFile(join(directory, "index.html"), "<html>Preview</html>");
    await writeFile(join(directory, "runtime.wasm"), new Uint8Array([0, 97, 115, 109]));
    const manifest = {
      schemaVersion: 1,
      sourceRevision: revision,
      displayRevision: revision.slice(0, 8),
      version: "0.5.0",
      base: "/wireloom/",
      files: await inventory(directory),
    };
    const save = () => writeFile(join(directory, "deployment.json"), JSON.stringify(manifest));
    await save();
    await verifyManifest(directory, revision);
    await assert.rejects(verifyManifest(directory, "b".repeat(40)), /Unexpected source revision/);
    await writeFile(join(directory, "index.html"), "Replaced");
    await assert.rejects(verifyManifest(directory), /differ/);
    await writeFile(join(directory, "index.html"), "<html>Preview</html>");
    await writeFile(join(directory, "extra.js"), "Unexpected");
    await assert.rejects(verifyManifest(directory), /differ/);
    await rm(join(directory, "extra.js"));
    await rm(join(directory, "runtime.wasm"));
    await assert.rejects(verifyManifest(directory), /differ/);
    await writeFile(join(directory, "runtime.wasm"), new Uint8Array([0, 97, 115, 109]));
    manifest.files[0].path = "../index.html";
    await save();
    await assert.rejects(verifyManifest(directory), /differ/);
  } finally {
    await rm(directory, { recursive: true, force: true });
  }
});

test("Pages refuses symbolic links instead of following files outside the artifact", async (context) => {
  const directory = await mkdtemp(join(tmpdir(), "wireloom-pages-link-"));
  try {
    await writeFile(join(directory, "original"), "Contents");
    try {
      await symlink(join(directory, "original"), join(directory, "linked"));
    } catch (error) {
      if (error.code !== "EPERM") throw error;
      context.skip("Windows requires symbolic-link privileges");
      return;
    }
    await assert.rejects(inventory(directory), /symbolic link/);
  } finally {
    await rm(directory, { recursive: true, force: true });
  }
});
