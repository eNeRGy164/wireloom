import assert from "node:assert/strict";
import { createHash } from "node:crypto";
import { lstat, readFile, readdir } from "node:fs/promises";
import { resolve } from "node:path";

export const sha256 = (bytes) => createHash("sha256").update(bytes).digest("hex");

export function validateBase(base) {
  assert.equal(typeof base, "string", "A site base path is required");
  assert.match(base, /^\/(?:[a-zA-Z0-9_-]+\/)*$/, "Use / or a base path such as /wireloom/");
  return base;
}

export async function inventory(directory, prefix = "") {
  const files = [];
  for (const entry of await readdir(directory, { withFileTypes: true })) {
    const path = `${prefix}${entry.name}`;
    assert(!entry.isSymbolicLink(), `Deployment must not contain a symbolic link: ${path}`);
    if (entry.isDirectory()) {
      files.push(...(await inventory(resolve(directory, entry.name), `${path}/`)));
    } else {
      assert(entry.isFile(), `Deployment must contain only regular files: ${path}`);
      if (path === "deployment.json") continue;
      const bytes = await readFile(resolve(directory, entry.name));
      files.push({ path, bytes: bytes.length, sha256: sha256(bytes) });
    }
  }
  return files.sort((left, right) => left.path.localeCompare(right.path, "en"));
}

export async function verifyManifest(directory, expectedRevision) {
  assert((await lstat(resolve(directory, "deployment.json"))).isFile(), "Manifest must be a regular file");
  const manifest = JSON.parse(await readFile(resolve(directory, "deployment.json"), "utf8"));
  assert.equal(manifest.schemaVersion, 1, "Unsupported deployment manifest");
  assert.match(manifest.sourceRevision, /^[a-f0-9]{40}$/, "A full source revision is required");
  if (expectedRevision) assert.equal(manifest.sourceRevision, expectedRevision, "Unexpected source revision");
  validateBase(manifest.base);
  assert.equal(typeof manifest.displayRevision, "string");
  assert(manifest.displayRevision.startsWith(manifest.sourceRevision.slice(0, 7)), "Invalid display revision");
  assert.equal(typeof manifest.version, "string");
  assert(manifest.version.length > 0, "Wireloom version is required");
  assert(Array.isArray(manifest.files) && manifest.files.length > 0, "Deployment must contain files");
  assert.deepEqual(await inventory(directory), manifest.files, "Deployment files differ from the validated manifest");
  assert(
    manifest.files.some((file) => file.path.endsWith(".wasm")),
    "WebAssembly assets are required",
  );
  assert(
    manifest.files.some((file) => file.path === "index.html"),
    "Client entry point is required",
  );
  assert(manifest.files.reduce((total, file) => total + file.bytes, 0) < 1_000_000_000, "Pages site exceeds 1 GB");
  return manifest;
}
