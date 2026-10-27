async (page) => {
  const input = __WIRELOOM_INPUT__;
  const browser = page.context().browser();
  const report = {
    phase: "desktop",
    engine: browser.browserType().name(),
    version: browser.version(),
    checks: [],
    passed: true,
  };
  const assert = (condition, message) => {
    if (!condition) throw new Error(message);
  };
  const check = async (name, action) => {
    const context = await browser.newContext({ viewport: { width: 1440, height: 1080 }, acceptDownloads: true });
    const app = await context.newPage();
    app.on("dialog", (dialog) => dialog.accept());
    try {
      await app.goto(input.root);
      await app.waitForFunction(
        () => ["Current", "Empty"].includes(document.querySelector("#result-state")?.textContent),
        null,
        { timeout: 40_000 },
      );
      if ((await app.locator("#result-state").textContent()) === "Empty") {
        await app.locator("#empty-load-example").click();
        await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current");
      }
      report.checks.push({ name, passed: true, details: await action(app, context) });
    } catch (error) {
      report.passed = false;
      report.checks.push({ name, passed: false, error: String(error) });
    } finally {
      await context.close();
    }
  };
  const wait = (app) =>
    app.waitForFunction(
      () => ["Current", "Failed"].includes(document.querySelector("#result-state")?.textContent),
      null,
      { timeout: 15_000 },
    );
  const importSource = async (app, source) => {
    const name = await app.locator("#source-tabs [aria-selected=true]").textContent();
    await app.locator("#file").setInputFiles({ name, mimeType: "text/plain", buffer: Buffer.from(source, "utf8") });
    await app.getByRole("dialog").getByRole("button", { name: "Replace", exact: true }).click();
    await app.waitForFunction(
      (name) => document.querySelector("#source-tabs [aria-selected=true]").textContent === name,
      name,
    );
    await wait(app);
  };
  await check("all example/profile combinations match native generated names and text", async (app) => {
    const results = [];
    await app.evaluate(() => {
      window.__blobs = [];
      const original = URL.createObjectURL;
      URL.createObjectURL = (blob) => {
        window.__blobs.push(blob);
        return original(blob);
      };
      // Inspect every export without triggering hundreds of browser saves and
      // download throttling. The separate UI suite exercises actual downloads.
      HTMLAnchorElement.prototype.click = function () {
        window.__downloadName = this.download;
      };
    });
    for (const example of input.examples) {
      for (const profile of ["7.3.1", "7.7.0"]) {
        await app.locator("#profile").selectOption(profile);
        await wait(app);
        await app.locator("#example").selectOption(example.id);
        await app.locator("#load-example").click();
        await wait(app);
        const expected = input.exampleExpected.find(
          (value) => value.caseId === `example-${example.id}` && value.profile === profile,
        );
        assert(expected, `Missing native expectation: ${example.id}/${profile}`);
        const success = (await app.locator("#result-state").textContent()) === "Current";
        assert(success === expected.success, `Success differs: ${example.id}/${profile}`);
        const names = await app.locator("#output-tabs [role=tab]").allTextContents();
        assert(
          JSON.stringify([...names].sort()) === JSON.stringify(expected.documents.map((doc) => doc.name).sort()),
          "Document names differ",
        );
        for (const doc of expected.documents) {
          await app.getByRole("button", { name: doc.name, exact: true }).click();
          await app.locator("#download").click();
          assert((await app.evaluate(() => window.__downloadName)) === doc.name, "Download filename differs");
          assert(
            (await app.evaluate(() => window.__blobs.at(-1).text())) === doc.source,
            `Generated text differs: ${doc.name}`,
          );
        }
        results.push({ example: example.id, profile, success, documents: names.length });
      }
    }
    return results;
  });
  await check("keyboard tabs, independently resizable panes, theme and font preferences", async (app) => {
    const first = app.locator('#output-tabs [role=tab][aria-selected="true"]');
    await first.focus();
    await app.keyboard.press("End");
    const last = await app.locator('#output-tabs [role=tab][aria-selected="true"]').getAttribute("id");
    assert(
      (await app.locator("#generated-panel").getAttribute("aria-labelledby")) === last,
      "Output panel label does not follow selection",
    );
    await app.keyboard.press("Home");
    assert(
      (await app.locator('#output-tabs [role=tab][aria-selected="true"]').getAttribute("id")) === "generated-tab-0",
      "Home did not select the first tab",
    );
    await app.locator("#explorer-splitter").focus();
    await app.keyboard.press("ArrowRight");
    assert(
      (await app.locator("#explorer-splitter").getAttribute("aria-valuenow")) === "220",
      "Explorer keyboard resize failed",
    );
    await app.locator("#splitter").focus();
    await app.keyboard.press("ArrowRight");
    assert((await app.locator("#splitter").getAttribute("aria-valuenow")) === "47", "Editor keyboard resize failed");
    assert(
      (await app.locator("#explorer-splitter").getAttribute("aria-valuenow")) === "220",
      "Editor divider changed the explorer",
    );
    await app.locator("#font-decrease").click();
    await app.locator(".theme-toggle label").filter({ hasText: "Dark" }).click();
    await app.reload();
    await wait(app);
    assert((await app.locator("#font-size").textContent()) === "11 px", "Font preference was lost");
    assert((await app.locator("html").getAttribute("data-theme")) === "dark", "Theme preference was lost");
    for (const width of [1440, 1000, 800]) {
      await app.setViewportSize({ width, height: 1080 });
      assert(
        await app.evaluate(() => document.documentElement.scrollWidth <= innerWidth),
        `Page overflow at ${width}px`,
      );
    }
    return {
      keyboardTabs: true,
      namedPanel: true,
      independentDividers: true,
      savedPreferences: true,
      widths: [1440, 1000, 800],
    };
  });
  await check("UTF-8 import, recovery, failure clearing, diagnostic focus and reset", async (app) => {
    await importSource(app, "// café\r\nstruct Imported { long value; };\r\n");
    assert((await app.locator("#file-list").textContent()).includes("Imported.g.cs"), "Import did not compile");
    await app.locator("#profile").selectOption("7.3.1");
    await wait(app);
    await app.waitForTimeout(600);
    await app.reload();
    await wait(app);
    assert((await app.locator("#profile").inputValue()) === "7.3.1", "Restored profile differs");
    assert((await app.locator("#file-list").textContent()).includes("Imported.g.cs"), "Restored source differs");
    await importSource(app, "// café\r\nstruct Broken { long value;\r\n");
    assert((await app.locator("#result-state").textContent()) === "Failed", "Invalid source succeeded");
    assert(
      (await app.locator("#output-tabs [role=tab]").count()) === 0 && (await app.locator("#download").isDisabled()),
      "Failure retained exportable output",
    );
    await app.locator("#diagnostics button").first().click();
    assert(await app.evaluate(() => !!document.activeElement?.closest("#source")), "Diagnostic did not focus source");
    await app.locator("#reset").click();
    await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Empty");
    assert((await app.locator("#result-state").textContent()) === "Empty", "Reset did not clear the workspace");
    return {
      import: true,
      restoredSourceAndProfile: true,
      currentFailureCleared: true,
      diagnosticFocus: true,
      reset: true,
    };
  });
  return report;
};
