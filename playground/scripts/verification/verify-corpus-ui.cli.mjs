async (page) => {
  const input = __WIRELOOM_INPUT__;
  const browser = page.context().browser();
  const report = { phase: input.phase, browser: browser.version(), checks: [], passed: true };
  const context = await browser.newContext({ acceptDownloads: true });
  const app = await context.newPage();

  app.on("dialog", (dialog) => dialog.accept());

  const assert = (condition, message) => {
    if (!condition) throw new Error(message);
  };

  try {
    await app.goto(input.root);
    await app.waitForFunction(
      () => ["Current", "Empty"].includes(document.querySelector("#result-state")?.textContent),
      null,
      { timeout: 40_000 },
    );
    await app.evaluate(() => {
      window.__exports = [];
      const original = URL.createObjectURL;
      URL.createObjectURL = (blob) => {
        window.__exports.push(blob);
        return original(blob);
      };
      HTMLAnchorElement.prototype.click = function () {
        window.__exportName = this.download;
      };
    });

    for (const expected of input.expected) {
      const fixture = input.cases.find((value) => value.id === expected.caseId);
      if (fixture.files.length !== 1) continue; // Worker-only include closure.
      const source = fixture.files[0];
      const label = `${fixture.id}/${expected.profile}`;

      try {
        await app.locator("#reset").click();
        await app.waitForFunction(() => document.querySelector("#result-state").textContent === "Empty");
        await app.locator("#profile").selectOption(expected.profile);
        await app.waitForFunction(() =>
          ["Current", "Failed", "Empty"].includes(document.querySelector("#result-state")?.textContent),
        );

        if (source.options?.unboundedSupport) {
          await app.getByRole("button", { name: "Workspace Settings" }).click();
          await app.getByLabel("Unbounded sequences and strings", { exact: true }).check();
          await app.getByRole("button", { name: "Apply", exact: true }).click();
        }

        const name = source.path.split("/").pop();

        await app
          .locator("#file")
          .setInputFiles({ name, mimeType: "text/plain", buffer: Buffer.from(source.source, "utf8") });
        await app.waitForFunction(
          (name) => document.querySelector("#source-tabs [aria-selected=true]")?.textContent === name,
          name,
        );
        await app.waitForFunction(
          () => ["Current", "Failed", "Empty"].includes(document.querySelector("#result-state")?.textContent),
          null,
          {
            timeout: 15_000,
          },
        );

        const state = await app.locator("#result-state").textContent();
        assert(state === (expected.success ? "Current" : "Failed"), `Wrong result state: ${state}`);

        const names = await app.locator("#output-tabs [role=tab]").allTextContents();
        assert(
          JSON.stringify([...names].sort()) === JSON.stringify(expected.documents.map((doc) => doc.name).sort()),
          "Document names differ",
        );

        const diagnostics = await app.locator("#diagnostics .diagnostic").allTextContents();
        assert(diagnostics.length === expected.diagnostics.length, "Diagnostic count differs");

        for (const diagnostic of expected.diagnostics) {
          const prefix = `${diagnostic.severity} · ${diagnostic.id} — ${diagnostic.message}`;
          let text = prefix;

          if (diagnostic.filePath === source.path && diagnostic.startOffset >= 0) {
            const before = source.source.slice(0, diagnostic.startOffset);
            const lines = before.split(/\r\n|\r|\n/);
            text += ` (${name}:${lines.length}:${lines.at(-1).length + 1})`;
          } else if (diagnostic.filePath) {
            text += ` (${diagnostic.filePath})`;
          }

          const index = diagnostics.indexOf(text);

          assert(index >= 0, `Diagnostic text/location differs: ${text}`);

          diagnostics.splice(index, 1);
        }

        if (!expected.success || expected.documents.length === 0) {
          for (const selector of ["#copy", "#download", "#download-all"]) {
            assert(await app.locator(selector).isDisabled(), `Invalid/empty output remained exportable: ${selector}`);
          }
        }

        for (const document of expected.documents) {
          await app.getByRole("button", { name: document.name, exact: true }).click();
          await app.locator("#download").click();
          assert((await app.evaluate(() => window.__exportName)) === document.name, "Export filename differs");
          assert(
            (await app.evaluate(() => window.__exports.at(-1).text())) === document.source,
            `Export content differs: ${document.name}`,
          );
        }

        report.checks.push({
          name: label,
          passed: true,
          documents: names.length,
          diagnostics: expected.diagnostics.length,
          success: expected.success,
        });
      } catch (error) {
        report.passed = false;
        report.checks.push({ name: label, passed: false, error: String(error) });
      }
    }

    assert(report.checks.length > 0, "No single-file corpus cases were exercised");
  } finally {
    await context.close();
  }

  return report;
};
