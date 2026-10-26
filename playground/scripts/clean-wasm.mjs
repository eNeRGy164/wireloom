import { rm } from "node:fs/promises";
import { isAbsolute, relative, resolve } from "node:path";

const repoRoot = resolve(import.meta.dirname, "../..");
const projectRoot = resolve(repoRoot, "src/Wireloom.Playground.Browser");
const generatedPaths = [
  resolve(projectRoot, "bin/Release/net10.0/publish"),
  resolve(projectRoot, "obj/Release/net10.0"),
];
const allowedPaths = new Set([
  resolve(repoRoot, "src/Wireloom.Playground.Browser/bin/Release/net10.0/publish"),
  resolve(repoRoot, "src/Wireloom.Playground.Browser/obj/Release/net10.0"),
]);

for (const path of generatedPaths) {
  const relativePath = relative(projectRoot, path);
  if (!allowedPaths.has(path) || isAbsolute(relativePath) || relativePath.startsWith("..")) {
    throw new Error(`Refusing to clean unexpected generated path: ${path}`);
  }
  await rm(path, { recursive: true, force: true });
}
