import assert from "node:assert/strict";
import { createHash } from "node:crypto";
import { readFile, writeFile, mkdir } from "node:fs/promises";
import { resolve } from "node:path";

const root = resolve(import.meta.dirname, "../../..");
const corpus = resolve(root, "docs/corpus");
const manifest = JSON.parse(await readFile(resolve(corpus, "manifest.json"), "utf8"));
// Expected Wireloom outcomes follow CorpusRepository.cs, not RTI outcomes or
// manifest buckets. The complete corpus-compliance suite remains authoritative.
const selection = [
  ["01-primitives", true],
  ["01-full-widths", true],
  ["03-enums-aliases", true],
  ["04-strings", true],
  ["04-unbounded-string-aliases", true],
  ["05-unbounded-sequence-aliases", true],
  ["05-shapes", true],
  ["06-aggregates", true],
  ["07-union-enum", true],
  ["09-autoid-hash", true],
  ["09-must-understand", true],
  ["10-annotations", true],
  ["11-preprocessor-advanced", true],
  ["12-unknown-annotation", true],
  ["12-interface", true],
  ["12-unsupported-map", false],
  ["12-fixed", false],
  ["12-error-directive", false],
  ["14-typed-defaults", true],
  ["14-autoid-scope", true],
  ["14-unbounded-support", true],
];
const cases = [];
const provenance = [];
for (const [id, accepted] of selection) {
  const entry = [...manifest.positiveCases, ...manifest.negativeCases].find((value) => value.id === id);
  assert(entry, `Missing retained corpus case: ${id}`);
  assert(!entry.defines?.length && !entry.strict && !entry.sequenceArrayMapping, `Nondefault options: ${id}`);
  assert(
    (entry.rtiddsgenOptions ?? []).every((option) => option === "-unboundedSupport"),
    `Unsupported native options: ${id}`,
  );
  const unboundedSupport = entry.rtiddsgenOptions?.includes("-unboundedSupport") ?? false;
  const paths = [entry.idl];
  if (id === "14-autoid-scope") paths.push("idl/includes/autoid-hash-include.idl");
  const files = [];
  for (const [index, path] of paths.entries()) {
    const bytes = await readFile(resolve(corpus, path));
    const virtualPath = index === 0 ? `/workspace/${id}.idl` : "/workspace/includes/autoid-hash-include.idl";
    files.push({
      path: virtualPath,
      source: bytes.toString("utf8"),
      isGenerationRoot: index === 0,
      options: { defines: [], undefines: [], strict: false, unboundedSupport },
    });
    provenance.push({
      caseId: id,
      path: `docs/corpus/${path}`,
      virtualPath,
      sha256: createHash("sha256").update(bytes).digest("hex"),
    });
  }
  cases.push({
    id: `corpus-${id}`,
    expectedSuccess: { "7.3.1": accepted && id !== "09-must-understand", "7.7.0": accepted },
    files,
  });
}
// This is the same retained input with its include deliberately unavailable,
// matching what the single-file UI can import today.
const includeCase = cases.find((value) => value.id === "corpus-14-autoid-scope");
cases.push({
  id: "corpus-missing-include",
  expectedSuccess: { "7.3.1": false, "7.7.0": false },
  files: [includeCase.files[0]],
});
const output = resolve(root, "output/playwright");
await mkdir(output, { recursive: true });
await writeFile(resolve(output, "corpus-cases.json"), JSON.stringify(cases, null, 2));
await writeFile(resolve(output, "corpus-inputs.json"), JSON.stringify(provenance, null, 2));
console.log(`Prepared ${cases.length} corpus cases from unchanged retained inputs.`);
