async (page) => {
  const input = __WIRELOOM_INPUT__;
  const browser = page.context().browser();
  const report = { phase: input.phase, browser: browser.version(), checks: [], passed: true };
  const assert = (value, message) => {
    if (!value) throw new Error(message);
  };
  const check = async (name, action) => {
    try {
      report.checks.push({ name, passed: true, details: await action() });
    } catch (error) {
      report.passed = false;
      report.checks.push({ name, passed: false, error: String(error) });
    }
  };
  const open = async (url = input.root, setup, loadExampleForEmptyWorkspace = true) => {
    const context = await browser.newContext({ viewport: { width: 1440, height: 1080 }, acceptDownloads: true });
    await context.grantPermissions(["clipboard-read", "clipboard-write"]);
    const app = await context.newPage();
    app.on("dialog", (dialog) => dialog.accept());
    if (setup) await setup(context, app);
    await app.goto(url);
    await app.waitForFunction(
      () => ["Current", "Failed", "Empty"].includes(document.querySelector("#result-state")?.textContent),
      null,
      { timeout: 40_000 },
    );
    if (loadExampleForEmptyWorkspace && (await app.locator("#result-state").textContent()) === "Empty") {
      await app.locator("#example").selectOption("hello");
      await app.locator("#load-example").click();
      await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current", null, {
        timeout: 40_000,
      });
    }
    return { app, context };
  };
  const wait = (app) =>
    app.waitForFunction(
      () => ["Current", "Failed", "Empty"].includes(document.querySelector("#result-state")?.textContent),
      null,
      { timeout: 15_000 },
    );
  const fillSource = async (app, source) => {
    await app.locator("#source").click({ position: { x: 150, y: 45 } });
    await app.keyboard.press("ControlOrMeta+a");
    await app.keyboard.press("Backspace");
    if (source) {
      await app.evaluate((source) => navigator.clipboard.writeText(source), source);
      await app.keyboard.press("ControlOrMeta+v");
    }
  };
  const edit = async (app, source) => {
    await fillSource(app, source);
    await wait(app);
  };
  const saved = (app) =>
    app.evaluate(async () => {
      const db = await new Promise((resolve, reject) => {
        const request = indexedDB.open("wireloom-playground", 1);
        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error);
      });
      try {
        return await new Promise((resolve, reject) => {
          const request = db.transaction("workspace").objectStore("workspace").get("current");
          request.onsuccess = () => resolve(request.result);
          request.onerror = () => reject(request.error);
        });
      } finally {
        db.close();
      }
    });
  const direct = (app, source, profile, path = "/workspace/main.idl") =>
    app.evaluate(
      async ({ source, profile, path }) => {
        const base = new URL(".", location.href);
        const worker = new Worker(new URL("dotnet/worker.mjs", base), { type: "module" });
        try {
          await new Promise((resolve, reject) => {
            worker.onmessage = ({ data }) => (data.type === "ready" ? resolve() : reject(new Error(data.message)));
            worker.onerror = (event) => reject(new Error(event.message));
          });
          return await new Promise((resolve, reject) => {
            worker.onmessage = ({ data }) =>
              data.type === "result" ? resolve(data.response) : reject(new Error(data.message));
            worker.postMessage({
              protocolVersion: 3,
              workerGeneration: 1,
              requestId: 1,
              workspaceRevision: 1,
              profile,
              files: [{ path, source, isGenerationRoot: true, options: { defines: [], undefines: [], strict: false } }],
              options: {
                sequenceArrayMapping: "RtiCSharp",
                sequenceSize: 100,
                stringSize: 255,
                includeDirectories: [],
                enableEscapeChar: false,
              },
            });
          });
        } finally {
          worker.terminate();
        }
      },
      { source, profile, path },
    );

  if (input.phase === "ui") {
    await check("generation is automatic with no Generate toolbar button", async () => {
      const { app, context } = await open();
      try {
        assert(
          (await app.getByRole("button", { name: "Generate", exact: true }).count()) === 0,
          "Generate remained in the toolbar",
        );
        await edit(app, "struct AutomaticGeneration { long value; };");
        assert(
          (await app.locator("#file-list").textContent()).includes("AutomaticGeneration.g.cs"),
          "Editing did not generate output automatically",
        );
        return { automatic: true, manualButton: false };
      } finally {
        await context.close();
      }
    });
    await check("floating notices preserve layout, close after five seconds, and pause while hovered", async () => {
      const { app, context } = await open();
      try {
        await app.clock.install();
        const top = await app.locator("#workbench").evaluate((node) => node.getBoundingClientRect().top);
        await app.locator("#copy").click();
        await app.locator("#notice").waitFor({ state: "visible" });
        assert(
          (await app.locator("#notice").evaluate((node) => getComputedStyle(node).position)) === "fixed",
          "Notice was not floating",
        );
        assert(
          (await app.locator("#workbench").evaluate((node) => node.getBoundingClientRect().top)) === top,
          "Notice shifted the workspace",
        );
        await app.clock.fastForward(5100);
        assert(await app.locator("#notice").isHidden(), "Notice did not auto-close");
        await app.locator("#copy").click();
        await app.locator("#notice").waitFor({ state: "visible" });
        await app.locator("#notice").hover();
        await app.clock.fastForward(6000);
        assert(await app.locator("#notice").isVisible(), "Hovered notice closed while being read");
        await app.locator(".toolbar").hover();
        await app.clock.fastForward(5100);
        assert(await app.locator("#notice").isHidden(), "Notice did not close after hover ended");
        assert(
          (await app.locator(".site-header").evaluate((node) => node.getBoundingClientRect().height)) === 65,
          "Header height was not 65 px",
        );
        assert(
          (await app.locator(".eyebrow").evaluate((node) => getComputedStyle(node).margin)) === "0px",
          "Eyebrow margin was not cleared",
        );
        assert(
          (await app.locator(".intro p:last-child").evaluate((node) => getComputedStyle(node).marginTop)) === "5px",
          "Intro spacing was not reduced",
        );
        return { floating: true, autoCloseMs: 5000, hoverPause: true, stableLayout: true, compactHeader: true };
      } finally {
        await context.close();
      }
    });
    await check(
      "theme resolves the system appearance immediately and remembers light/dark choices despite system changes",
      async () => {
        const { app, context } = await open(input.root, async (_context, app) =>
          app.emulateMedia({ colorScheme: "light" }),
        );
        try {
          const waitTheme = (theme) =>
            app.waitForFunction(
              (theme) =>
                document.documentElement.dataset.theme === theme &&
                document
                  .querySelector("#source .monaco-editor")
                  .classList.contains(theme === "dark" ? "vs-dark" : "vs"),
              theme,
            );
          const choose = (value) => app.locator(".theme-toggle label").filter({ hasText: value }).click();
          assert(
            (await app.locator('input[name="theme"][value="system"]').count()) === 0,
            "System remained a user-facing choice",
          );
          assert(
            await app.locator('input[name="theme"][value="light"]').isChecked(),
            "System light appearance did not resolve to Light",
          );
          await app.emulateMedia({ colorScheme: "dark" });
          await waitTheme("light");
          await choose("Light");
          await waitTheme("light");
          await app.emulateMedia({ colorScheme: "light" });
          await app.emulateMedia({ colorScheme: "dark" });
          await waitTheme("light");
          await app.reload();
          await wait(app);
          await waitTheme("light");
          assert(await app.locator('input[name="theme"][value="light"]').isChecked(), "Light choice was not restored");
          await choose("Dark");
          await app.emulateMedia({ colorScheme: "light" });
          await waitTheme("dark");
          await app.reload();
          await wait(app);
          await waitTheme("dark");
          assert(await app.locator('input[name="theme"][value="dark"]').isChecked(), "Dark choice was not restored");
          const firstDark = await open(input.root, async (_context, app) => app.emulateMedia({ colorScheme: "dark" }));
          try {
            assert(
              await firstDark.app.locator('input[name="theme"][value="dark"]').isChecked(),
              "First-visit system dark appearance did not resolve to Dark",
            );
            assert(
              (await firstDark.app.evaluate(() => localStorage.getItem("wireloom-theme"))) === "dark",
              "Initial resolved choice was not saved",
            );
          } finally {
            await firstDark.context.close();
          }
          return {
            systemResolvedImmediately: true,
            firstVisitModes: ["light", "dark"],
            explicitChoiceStable: true,
            persistedChoices: ["light", "dark"],
            editorsSynchronized: true,
          };
        } finally {
          await context.close();
        }
      },
    );
    await check(
      "all files open immediately; explorer collapses; both editors zoom and remember font size",
      async () => {
        const { app, context } = await open();
        try {
          const files = (await app.locator("#file-list button").allTextContents()).filter(
            (name) => !name.endsWith(".csproj"),
          );
          assert(
            files.length === 4 && (await app.locator("#output-tabs [role=tab]").count()) === 4,
            "Initial output hid unopened files",
          );
          await app.getByRole("button", { name: files[3], exact: true }).click();
          assert(
            (await app.locator('#output-tabs [aria-selected="true"]').textContent()) === files[3],
            "Explorer selection did not open the file",
          );
          const width = await app.locator("#workspace").evaluate((node) => node.getBoundingClientRect().width);
          await app.locator("#explorer-toggle").click();
          assert(await app.locator("#explorer-content").isHidden(), "Explorer did not collapse");
          assert(
            (await app.locator("#workspace").evaluate((node) => node.getBoundingClientRect().width)) > width,
            "Collapsing explorer did not reclaim space",
          );
          await app.locator("#explorer-toggle").click();
          const fonts = () =>
            app.evaluate(() =>
              ["source", "generated"].map(
                (id) => getComputedStyle(document.querySelector(`#${id} .view-lines`)).fontSize,
              ),
            );
          assert(
            (await fonts()).every((size) => size === "12px"),
            "Default font is not 12 px",
          );
          await app.locator("#font-decrease").click();
          assert(
            (await fonts()).every((size) => size === "11px"),
            "Zoom out did not update both editors",
          );
          await app.locator("#font-increase").click();
          await app.locator("#font-increase").click();
          await app.reload();
          await wait(app);
          assert(
            (await fonts()).every((size) => size === "13px"),
            "Font preference was not restored",
          );
          await app.locator("#font-reset").click();
          assert(
            (await fonts()).every((size) => size === "12px"),
            "Font reset failed",
          );
          await app.locator(".toolbar").hover();
          await app.waitForFunction(() => getComputedStyle(document.querySelector("#tab-scrollbar")).opacity === "0");
          const tabHeight = await app
            .locator("#output-tabs button")
            .first()
            .evaluate((node) => node.getBoundingClientRect().height);
          await app.locator("#output-tabs").evaluate((node) => {
            node.scrollLeft = 0;
          });
          await app.locator("#output-tabs").hover();
          await app.waitForFunction(() => getComputedStyle(document.querySelector("#tab-scrollbar")).opacity === "1");
          assert(
            (await app.locator("#tab-scrollbar").evaluate((node) => getComputedStyle(node).backgroundColor)) ===
              "rgba(0, 0, 0, 0)",
            "Scrollbar track was not transparent",
          );
          assert(
            Math.abs(
              (await app
                .locator("#output-tabs button")
                .first()
                .evaluate((node) => node.getBoundingClientRect().height)) - tabHeight,
            ) < 0.5,
            "Scrollbar changed tab height",
          );
          const pageY = await app.evaluate(() => scrollY);
          await app.mouse.wheel(0, 180);
          await app.waitForFunction(() => document.querySelector("#output-tabs").scrollLeft > 0);
          assert((await app.evaluate(() => scrollY)) === pageY, "Tab wheel scrolled the page instead of the tabs");
          await app.locator("#tab-scroll-thumb").focus();
          await app.keyboard.press("Home");
          await app.waitForFunction(() => document.querySelector("#output-tabs").scrollLeft === 0);
          const thumb = await app.locator("#tab-scroll-thumb").boundingBox();
          await app.mouse.move(thumb.x + 10, thumb.y + 3);
          await app.mouse.down();
          await app.mouse.move(thumb.x + 45, thumb.y + 3);
          await app.mouse.up();
          await app.waitForFunction(() => document.querySelector("#output-tabs").scrollLeft > 0);
          await app.locator("#tab-scroll-thumb").focus();
          await app.keyboard.press("End");
          await app.waitForFunction(() => {
            const tabs = document.querySelector("#output-tabs");
            return Math.abs(tabs.scrollLeft - (tabs.scrollWidth - tabs.clientWidth)) < 2;
          });
          assert(
            await app.evaluate(
              () =>
                !!document.querySelector(".toolbar .font-controls") &&
                !!(
                  document.querySelector(".font-controls").compareDocumentPosition(document.querySelector("#profile")) &
                  Node.DOCUMENT_POSITION_FOLLOWING
                ),
            ),
            "Font controls did not precede the profile in the toolbar",
          );
          await app.locator("#notice-dismiss").click();
          assert(await app.locator("#notice").isHidden(), "Notice could not be dismissed");
          assert(
            !(await app.locator("#example-description").textContent()).includes("Restored"),
            "Permanent restore description remained",
          );
          const bounds = async () => ({
            explorer: await app.locator(".explorer").boundingBox(),
            workspace: await app.locator("#workspace").boundingBox(),
            source: await app.locator(".input-pane").boundingBox(),
          });
          const before = await bounds();
          const explorerHandle = await app.locator("#explorer-splitter").boundingBox();
          await app.mouse.move(explorerHandle.x + 4, explorerHandle.y + 80);
          await app.mouse.down();
          await app.mouse.move(explorerHandle.x + 84, explorerHandle.y + 80);
          await app.mouse.up();
          const resized = await bounds();
          assert(
            resized.explorer.width > before.explorer.width + 70 &&
              resized.workspace.width < before.workspace.width - 70,
            "Explorer resize did not reclaim editor space",
          );
          assert(
            (await app.locator("#splitter").getAttribute("aria-valuenow")) === "45",
            "Explorer resize changed the editor split ratio",
          );
          const editorHandle = await app.locator("#splitter").boundingBox();
          await app.mouse.move(editorHandle.x + 4, editorHandle.y + 80);
          await app.mouse.down();
          await app.mouse.move(editorHandle.x + 54, editorHandle.y + 80);
          await app.mouse.up();
          const split = await bounds();
          assert(
            Math.abs(split.explorer.width - resized.explorer.width) < 1 &&
              Math.abs(split.workspace.width - resized.workspace.width) < 1 &&
              split.source.width > resized.source.width + 40,
            "Editor divider changed the explorer instead of only the editor split",
          );
          await app.locator("#explorer-splitter").focus();
          await app.keyboard.press("ArrowRight");
          assert(
            Number(await app.locator("#explorer-splitter").getAttribute("aria-valuenow")) > resized.explorer.width,
            "Explorer keyboard resize failed",
          );
          return {
            initiallyOpen: files.length,
            explorerSelection: true,
            collapse: true,
            independentResizers: true,
            noticeDismissal: true,
            toolbarFontControls: true,
            zoomBothEditors: true,
            persistedFont: true,
            tabWheelScroll: true,
            transparentTrackHoverThumb: true,
          };
        } finally {
          await context.close();
        }
      },
    );
    await check(
      "all seven examples on both profiles match native generation, including the type showcase",
      async () => {
        const { app, context } = await open();
        const results = [];
        try {
          for (const profile of ["7.7.0", "7.3.1"]) {
            await app.locator("#profile").selectOption(profile);
            for (const example of input.examples) {
              await app.locator("#example").selectOption(example.id);
              await app.locator("#load-example").click();
              await wait(app);
              const baseline = input.exampleExpected.find(
                (value) => value.caseId === `example-${example.id}` && value.profile === profile,
              );
              assert(baseline, `Missing native example baseline: ${example.id}/${profile}`);
              const expected = baseline;
              const documents = [...expected.documents].sort((a, b) =>
                a.name < b.name ? -1 : a.name > b.name ? 1 : 0,
              );
              assert(
                JSON.stringify({ success: expected.success, documents, diagnostics: expected.diagnostics }) ===
                  JSON.stringify({
                    success: baseline.success,
                    documents: baseline.documents,
                    diagnostics: baseline.diagnostics,
                  }),
                `Native/browser mismatch for ${example.id}/${profile}`,
              );
              if (example.id === "types" && profile === "7.7.0")
                assert(
                  documents.some((doc) => doc.source.includes('unit: "°C"')),
                  "Temperature unit annotation was not generated",
                );
              const actual = await app.locator("#result-state").textContent();
              assert(
                actual === (expected.success ? "Current" : "Failed"),
                `${example.id}/${profile}: unexpected state ${actual}`,
              );
              assert(
                Number(await app.locator("#document-count").textContent()) === expected.documents.length,
                "Missing generated files",
              );
              const diagnostics = await app.locator("#diagnostics").textContent();
              for (const diagnostic of expected.diagnostics)
                assert(diagnostics.includes(diagnostic.id), "Missing diagnostic");
              results.push({
                example: example.id,
                profile,
                success: expected.success,
                documents: expected.documents.length,
                diagnostics: expected.diagnostics.map((d) => d.id),
              });
            }
          }
          return results;
        } finally {
          await context.close();
        }
      },
    );
    await check("downloads and clipboard preserve exact current names/text; ZIP contains each file once", async () => {
      const { app, context } = await open();
      try {
        await context.grantPermissions(["clipboard-read", "clipboard-write"]);
        const source = input.cases.find((c) => c.id === "member-hash-unicode").files[0].source;
        await edit(app, source);
        const baseline = input.expected.find((c) => c.caseId === "member-hash-unicode" && c.profile === "7.7.0");
        const expected = baseline?.documents ?? (await direct(app, source, "7.7.0")).documents;
        assert(
          (await app.locator("#output-tabs [role=tab]").count()) === expected.length,
          "Files were not all opened immediately",
        );
        assert(
          (await app.locator('#file-list button:not([data-name$=".csproj"])').count()) === expected.length,
          "Explorer omitted generated files",
        );
        await app.evaluate(() => {
          window.__blobs = [];
          const original = URL.createObjectURL;
          URL.createObjectURL = (blob) => {
            window.__blobs.push(blob);
            return original(blob);
          };
          const write = navigator.clipboard.writeText.bind(navigator.clipboard);
          navigator.clipboard.writeText = (text) => {
            window.__copiedText = text;
            return write(text);
          };
        });
        for (const doc of expected) {
          await app.getByRole("button", { name: doc.name, exact: true }).click();
          await app.locator("#copy").click();
          await app.waitForFunction(
            (name) => document.querySelector("#notice-message").textContent === `Copied ${name}.`,
            doc.name,
          );
          const copied = await app.evaluate(() => navigator.clipboard.readText());
          assert(
            (await app.evaluate(() => window.__copiedText)) === doc.source,
            "Clipboard API received altered source",
          );
          // Windows clipboard reads normalize LF to CRLF; downloads still compare byte-for-byte.
          assert(
            copied.replace(/\r\n/g, "\n") === doc.source.replace(/\r\n/g, "\n"),
            "Clipboard content differs beyond platform newline conversion",
          );
          const downloading = app.waitForEvent("download");
          await app.locator("#download").click();
          assert((await downloading).suggestedFilename() === doc.name, "Wrong individual filename");
          assert((await app.evaluate(() => window.__blobs.at(-1).text())) === doc.source, "Individual text differs");
        }
        assert((await app.locator("#output-tabs [role=tab]").count()) === expected.length, "Tabs omitted a file");
        const downloading = app.waitForEvent("download");
        await app.locator("#download-all").click();
        assert((await downloading).suggestedFilename() === "wireloom-generated.zip", "Wrong ZIP filename");
        const entries = await app.evaluate(async () => {
          const bytes = new Uint8Array(await window.__blobs.at(-1).arrayBuffer());
          const view = new DataView(bytes.buffer);
          const decoder = new TextDecoder();
          const entries = [];
          let offset = 0;
          while (view.getUint32(offset, true) === 0x04034b50) {
            if (view.getUint16(offset + 8, true) !== 0) throw new Error("Expected stored ZIP entries");
            const size = view.getUint32(offset + 18, true),
              nameSize = view.getUint16(offset + 26, true),
              extraSize = view.getUint16(offset + 28, true);
            const name = decoder.decode(bytes.subarray(offset + 30, offset + 30 + nameSize));
            const start = offset + 30 + nameSize + extraSize;
            entries.push({ name, source: decoder.decode(bytes.subarray(start, start + size)) });
            offset = start + size;
          }
          return entries;
        });
        const byName = (a, b) => a.name.localeCompare(b.name);
        assert(
          JSON.stringify(entries.sort(byName)) ===
            JSON.stringify(expected.map((d) => ({ name: d.name, source: d.source })).sort(byName)),
          "ZIP contents differ",
        );
        return {
          files: expected.length,
          exactClipboardApiInput: true,
          clipboardReadPlatformNewlines: true,
          exactDownloads: true,
        };
      } finally {
        await context.close();
      }
    });
    await check(
      "stale exports/diagnostics clear immediately; errors recover; empty input succeeds with zero files",
      async () => {
        const { app, context } = await open();
        try {
          await edit(app, "struct Broken { long value;\n");
          assert((await app.locator("#result-state").textContent()) === "Failed", "Syntax error accepted");
          assert(
            (await app.locator("#output-tabs button").count()) === 0 &&
              (await app.locator("#download-all").isDisabled()),
            "Failure retained export",
          );
          await app.locator("#source").click({ position: { x: 150, y: 45 } });
          await app.keyboard.insertText(" ");
          assert((await app.locator("#diagnostics button").count()) === 0, "Stale diagnostic navigation remained");
          await edit(app, "struct Recovered { long value; };");
          assert(!(await app.locator("#download-all").isDisabled()), "Success did not recover exports");
          await edit(app, "");
          assert(
            (await app.locator("#result-state").textContent()) === "Current" &&
              (await app.locator("#document-count").textContent()) === "0",
            "Empty input treated as failure",
          );
          assert(await app.locator("#download-all").isDisabled(), "Empty result exports old files");
          return { failureCleared: true, recovered: true, zeroDocumentSuccess: true };
        } finally {
          await context.close();
        }
      },
    );
    await check("warnings remain visible alongside exportable generated code", async () => {
      const { app, context } = await open();
      try {
        const fixture = input.cases.find((c) => c.id === "unknown-annotation-warning");
        await edit(app, fixture.files[0].source);
        assert((await app.locator("#diagnostics").textContent()).includes("DDSG0101"), "Warning was hidden");
        assert((await app.locator("#diagnostics .warning").count()) === 1, "Warning severity was lost");
        assert(!(await app.locator("#download-all").isDisabled()), "Warning disabled valid output");
        return { warning: "DDSG0101", exportable: true };
      } finally {
        await context.close();
      }
    });
    await check(
      "diagnostics navigate after CRLF/LF Unicode input; tabs, resize, dark theme, and nested assets work",
      async () => {
        const { app, context } = await open(input.nested);
        try {
          for (const eol of ["\n", "\r\n"]) {
            await app.locator("#file").setInputFiles({
              name: "main.idl",
              mimeType: "text/plain",
              buffer: Buffer.from(`// é 😀${eol}struct Broken { long value;${eol}`),
            });
            await app.getByRole("dialog").getByRole("button", { name: "Replace", exact: true }).click();
            await wait(app);
            const diagnostic = app.locator("#diagnostics button").first();
            assert((await diagnostic.count()) > 0, "Source diagnostic was not navigable");
            await diagnostic.click();
            const focused = await app.evaluate(() => document.activeElement?.closest("#source") !== null);
            assert(focused, "Diagnostic did not focus the source");
          }
          await app.locator("#splitter").focus();
          await app.keyboard.press("ArrowRight");
          assert((await app.locator("#splitter").getAttribute("aria-valuenow")) === "47", "Keyboard resize failed");
          await app.locator(".theme-toggle label").filter({ hasText: "Dark" }).click();
          await app.waitForFunction(() => document.documentElement.dataset.theme === "dark");
          assert(
            (await app.evaluate(() => getComputedStyle(document.documentElement).backgroundColor)) ===
              "rgb(19, 27, 32)",
            "Dark presentation failed",
          );
          await edit(app, input.examples[0].source);
          await app.screenshot({ path: "output/playwright/stage1-dark.png", fullPage: true });
          await app.locator(".theme-toggle label").filter({ hasText: "Light" }).click();
          await app.screenshot({ path: "output/playwright/stage1-light.png", fullPage: true });
          const asset = await app.evaluate(async () => {
            const logo = document.querySelector(".brand img");
            await logo.decode();
            return {
              logo: logo.currentSrc,
              icon: document.querySelector('link[rel="icon"]').href,
              width: logo.naturalWidth,
            };
          });
          assert(
            asset.logo === asset.icon && asset.width === 128 && asset.logo.includes("/playground/assets/"),
            "Nested logo/favicon failed",
          );
          return { unicodeNavigation: 2, keyboardResize: true, themes: 2, nestedAssets: true };
        } finally {
          await context.close();
        }
      },
    );
  }

  if (input.phase === "recovery") {
    await check(
      "UTF-8 import and reload preserve exact source/profile; replacement can be canceled; reset wins pending saves",
      async () => {
        const { app, context } = await open();
        try {
          const source = "// é 😀\r\nstruct Imported { long value; };\r\n";
          await app
            .locator("#file")
            .setInputFiles({ name: "imported.idl", mimeType: "text/plain", buffer: Buffer.from(source) });
          await wait(app);
          await app.locator("#profile").selectOption("7.3.1");
          await wait(app);
          await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
          const stored = await saved(app);
          assert(
            stored.profile === "7.3.1" &&
              stored.files.some((file) => file.source === source && file.path === "/workspace/imported.idl"),
            "Recovery record differs",
          );
          await app.reload();
          await wait(app);
          assert(
            (await app.locator("#profile").inputValue()) === "7.3.1" &&
              (await app.locator("#source-tabs [aria-selected=true]").textContent()) === "imported.idl",
            "Reload did not restore",
          );
          assert(
            (await app.locator("#example-description").textContent()) === "",
            "Custom restored source kept a permanent restore description",
          );
          await app.locator("#notice-dismiss").click();
          assert(await app.locator("#notice").isHidden(), "Restore notice was not dismissible");
          assert(
            await app.evaluate(
              () =>
                document.querySelector("#workbench").getBoundingClientRect().top -
                  document.querySelector(".toolbar").getBoundingClientRect().bottom >=
                12,
            ),
            "Dismissing restore notice removed all workspace spacing",
          );
          app.removeAllListeners("dialog");
          app.on("dialog", (dialog) => dialog.dismiss());
          await app.locator("#load-example").click();
          assert(
            (await app.locator("#source-tabs [aria-selected=true]").textContent()) === "imported.idl",
            "Canceled example replaced source",
          );
          await app.locator("#file").setInputFiles({
            name: "imported.idl",
            mimeType: "text/plain",
            buffer: Buffer.from("struct Cancelled { long x; };"),
          });
          await app.getByRole("dialog").getByRole("button", { name: "Cancel", exact: true }).click();
          assert(
            (await app.locator("#source-tabs [aria-selected=true]").textContent()) === "imported.idl",
            "Canceled import replaced source",
          );
          app.removeAllListeners("dialog");
          app.on("dialog", (dialog) => dialog.accept());
          await app.locator("#reset").click();
          await wait(app);
          await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
          const reset = await saved(app);
          assert(reset.profile === "7.7.0" && reset.files.length === 0, "Reset retained previous workspace");
          assert(
            (await app.locator("#result-state").textContent()) === "Empty" &&
              (await app.locator("#example-description").textContent()) === "" &&
              (await app.locator("#download-source").isDisabled()) &&
              (await app.locator("#empty-source").isVisible()),
            "Reset did not restore the empty workspace view",
          );
          await app.reload();
          await wait(app);
          assert((await app.locator("#profile").inputValue()) === "7.7.0", "Reset profile returned after reload");
          return { restored: true, cancellations: 2, reset: true };
        } finally {
          await context.close();
        }
      },
    );
    await check(
      "invalid UTF-8, invalid virtual names, and oversized imports leave successful workspace untouched",
      async () => {
        const { app, context } = await open();
        try {
          const names = await app.locator("#file-list button").allTextContents();
          for (const file of [
            { name: "invalid.idl", mimeType: "text/plain", buffer: Buffer.from([0xc3, 0x28]) },
            { name: "invalid\u0001.idl", mimeType: "text/plain", buffer: Buffer.from("wrong") },
            { name: "large.idl", mimeType: "text/plain", buffer: Buffer.alloc(262145, 97) },
          ]) {
            await app.locator("#file").setInputFiles(file);
            await app.waitForFunction(() =>
              document.querySelector("#notice").textContent.includes("Could not import files"),
            );
            assert(
              JSON.stringify(await app.locator("#file-list button").allTextContents()) === JSON.stringify(names),
              "Rejected import destroyed output",
            );
            assert(!(await app.locator("#download-all").isDisabled()), "Rejected import invalidated success");
          }
          return { rejectedImports: 3, preservedOutput: true };
        } finally {
          await context.close();
        }
      },
    );

    await check("unavailable, failed-write, and invalid/older recovery data leave editing usable", async () => {
      const outcomes = [];
      for (const mode of ["unavailable", "quota", "older"]) {
        const { app, context } = await open(
          input.root,
          async (context) => {
            await context.addInitScript((mode) => {
              if (mode === "unavailable")
                Object.defineProperty(window, "indexedDB", {
                  get() {
                    throw new Error("Storage disabled for verification");
                  },
                });
              if (mode === "quota")
                IDBObjectStore.prototype.put = function () {
                  throw new DOMException("Verification quota", "QuotaExceededError");
                };
            }, mode);
          },
          false,
        );

        try {
          if (mode === "older") {
            await app.evaluate(async () => {
              const db = await new Promise((resolve) => {
                const request = indexedDB.open("wireloom-playground", 1);
                request.onsuccess = () => resolve(request.result);
              });
              await new Promise((resolve) => {
                const transaction = db.transaction("workspace", "readwrite");
                transaction.objectStore("workspace").put({ version: 2 }, "current");
                transaction.oncomplete = resolve;
              });
              db.close();
            });
            await app.reload();
            await wait(app);
          }

          try {
            await app.waitForFunction(() => {
              const notice = document.querySelector("#notice-message")?.textContent.toLowerCase() ?? "";
              return notice.includes("recovery is unavailable") || notice.includes("unsupported format");
            });
          } catch (error) {
            throw new Error(`Recovery mode ${mode} did not report an unavailable store: ${error}`);
          }

          if (mode === "older") {
            const clearSavedData = app.getByRole("button", { name: "Clear saved data", exact: true });
            assert(await clearSavedData.isVisible(), "Unsupported saved data had no in-app clear action");
            await clearSavedData.click();
            await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
          }

          await app.locator("#empty-load-example").click();
          await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current");
          if (mode === "older") {
            await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
            assert(
              (await saved(app)).files.some((file) => file.source.includes("module Hello")),
              "Loading an example after clearing old data did not persist the new workspace",
            );
          }

          await edit(app, "struct StorageStillWorks { long value; };");
          assert(
            await app
              .locator("#file-list")
              .textContent()
              .then((text) => text.includes("StorageStillWorks")),
            "Storage failure prevented generation",
          );

          if (mode === "older") {
            await app.locator("#reset").click();
            await wait(app);
            await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
          }

          outcomes.push(mode);
        } finally {
          await context.close();
        }
      }
      return { modes: outcomes };
    });
  }

  if (input.phase === "ui-faults") {
    await check("missing editor chunk offers reload without losing recovery", async () => {
      const { app, context } = await open();

      try {
        await app.route("**/assets/main-*.js", (route) => route.abort());
        await app.reload();
        await app.getByRole("button", { name: "Reload playground" }).waitFor({ state: "visible" });
        await app.unroute("**/assets/main-*.js");
        await app.getByRole("button", { name: "Reload playground" }).click();
        await wait(app);
        assert((await app.locator("#document-count").textContent()) === "4", "Reload did not recover editor");
        return { reload: true };
      } finally {
        await context.close();
      }
    });
    await check("missing runtime asset is retryable without losing editor source", async () => {
      const { app, context } = await open();

      try {
        await app.route("**/dotnet.native*.js", (route) => route.abort());
        await app.reload();
        await app.locator("#retry").waitFor({ state: "visible", timeout: 35_000 });
        await app.unroute("**/dotnet.native*.js");
        await app.locator("#retry").click();
        await wait(app);
        assert((await app.locator("#document-count").textContent()) === "4", "Retry did not recover");
        return { retry: true };
      } finally {
        await context.close();
      }
    });

    await check("busy compiler keeps newest edit/profile, clears stale exports, and replaces on watchdog", async () => {
      const { app, context } = await open();
      try {
        const worker = app.workers().find((w) => w.url().endsWith("/dotnet/worker.mjs"));
        await worker.evaluate(() =>
          setTimeout(() => {
            const until = performance.now() + 20_000;
            while (performance.now() < until) {}
          }, 0),
        );
        await app.waitForTimeout(50);
        await fillSource(app, "struct OldRequest { long value; };");
        await app.waitForTimeout(350);
        await fillSource(app, "struct NewestRequest { long value; };");
        assert(await app.locator("#download-all").isDisabled(), "Pending output remained exportable");
        await app.locator("#profile").selectOption("7.3.1");
        await app.waitForFunction(
          () => document.querySelector("#file-list").textContent.includes("NewestRequest"),
          null,
          { timeout: 25_000 },
        );
        assert(!(await app.locator("#file-list").textContent()).includes("OldRequest"), "Stale result applied");
        assert((await app.locator("#result-summary").textContent()).includes("7.3.1"), "Stale profile applied");
        return { latestOnly: true, profile: "7.3.1", watchdogReplacement: true };
      } finally {
        await context.close();
      }
    });
  }

  return report;
};
