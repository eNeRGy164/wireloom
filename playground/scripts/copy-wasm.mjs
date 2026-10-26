import { cp, mkdir, rm } from "node:fs/promises";
import { isAbsolute, relative, resolve } from "node:path";

const repoRoot = resolve(import.meta.dirname, "../..");
const publishedAssets = resolve(repoRoot, "src/Wireloom.Playground.Browser/bin/Release/net10.0/publish/wwwroot");
const destination = resolve(repoRoot, "playground/public/dotnet");
const expectedDestination = resolve(repoRoot, "playground/public/dotnet");
const destinationFromRoot = relative(repoRoot, destination);

if (destination !== expectedDestination || isAbsolute(destinationFromRoot) || destinationFromRoot.startsWith("..")) {
  throw new Error(`Refusing to replace unexpected generated asset path: ${destination}`);
}

await rm(destination, { recursive: true, force: true });
await mkdir(destination, { recursive: true });
await cp(publishedAssets, destination, { recursive: true, force: true });
await cp(resolve(repoRoot, "playground/src/worker.mjs"), resolve(destination, "worker.mjs"));
await cp(resolve(repoRoot, "playground/src/md5.mjs"), resolve(destination, "md5.mjs"));
