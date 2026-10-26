import { readFile, writeFile, mkdir } from "node:fs/promises";
import { resolve, dirname } from "node:path";
import { examples } from "../../src/examples.ts";

const repoRoot = resolve(import.meta.dirname, "../../..");
const phase = process.argv[2] ?? "parity";
if (!["parity", "corpus", "corpus-ui", "faults", "limits", "ui", "recovery", "ui-faults", "desktop"].includes(phase)) {
  throw new Error("Choose parity, corpus, corpus-ui, faults, limits, ui, recovery, ui-faults, or desktop.");
}
const payload = {
  phase,
  examples,
  exampleExpected: ["ui", "desktop"].includes(phase)
    ? JSON.parse(await readFile(resolve(repoRoot, "output/playwright/example-native-baseline.json"), "utf8"))
    : [],
  cases: JSON.parse(await readFile(resolve(import.meta.dirname, "cases.json"), "utf8")),
  expected: JSON.parse(await readFile(resolve(repoRoot, "output/playwright/native-baseline.json"), "utf8")),
  root: process.env.WIRELOOM_VERIFY_ROOT ?? "http://127.0.0.1:4173/",
  nested: process.env.WIRELOOM_VERIFY_NESTED ?? "http://127.0.0.1:4174/playground/",
};
if (["corpus", "corpus-ui"].includes(phase)) {
  payload.cases = JSON.parse(await readFile(resolve(repoRoot, "output/playwright/corpus-cases.json"), "utf8"));
  payload.expected = JSON.parse(
    await readFile(resolve(repoRoot, "output/playwright/corpus-native-baseline.json"), "utf8"),
  );
}
const templateName =
  phase === "corpus-ui"
    ? "verify-corpus-ui.cli.mjs"
    : phase === "desktop"
      ? "verify-desktop.cli.mjs"
      : ["ui", "recovery", "ui-faults"].includes(phase)
        ? "verify-ui.cli.mjs"
        : "verify-browser.cli.mjs";
const template = await readFile(resolve(import.meta.dirname, templateName), "utf8");
const output = resolve(repoRoot, `output/playwright/run-${phase}.mjs`);
await mkdir(dirname(output), { recursive: true });
await writeFile(output, template.replace("__WIRELOOM_INPUT__", JSON.stringify(payload)));
console.log(output);
