import { writeFile, mkdir } from "node:fs/promises";
import { resolve, dirname } from "node:path";
import { examples } from "../../src/examples.ts";

// Keep native expectations tied to the actual visitor-facing examples.
const cases = examples.map((example) => ({
  id: `example-${example.id}`,
  expectedSuccess: {
    "7.3.1": !["profiles", "unsupported"].includes(example.id),
    "7.7.0": example.id !== "unsupported",
  },
  files: [{ path: "/workspace/main.idl", source: example.source, isGenerationRoot: true }],
}));
const output = resolve(import.meta.dirname, "../../../output/playwright/example-cases.json");
await mkdir(dirname(output), { recursive: true });
await writeFile(output, JSON.stringify(cases, null, 2));
console.log(output);
