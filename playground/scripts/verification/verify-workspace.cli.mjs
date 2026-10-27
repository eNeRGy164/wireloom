async (page, hooks = {}) => {
  const input = __WIRELOOM_INPUT__;
  const browser = page.context().browser();
  const report = { phase: input.phase, browser: browser.version(), checks: [], passed: true };
  const assert = (condition, message) => {
    if (!condition) throw new Error(message);
  };

  const saved = (app) =>
    app.evaluate(
      () =>
        new Promise((resolve, reject) => {
          const opening = indexedDB.open("wireloom-playground", 1);
          opening.onsuccess = () => {
            const database = opening.result;
            const request = database.transaction("workspace").objectStore("workspace").get("current");
            request.onsuccess = () => {
              database.close();
              resolve(request.result);
            };
            request.onerror = () => reject(request.error);
          };
          opening.onerror = () => reject(opening.error);
        }),
    );

  const settle = async (app) => {
    await app.waitForFunction(
      () => ["Current", "Failed", "Empty"].includes(document.querySelector("#result-state")?.textContent),
      null,
      { timeout: 20000 },
    );
    await app.waitForFunction(() => document.querySelector("#source-state")?.textContent === "Saved locally");
  };

  const current = async (app) => {
    await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current", null, {
      timeout: 20000,
    });
    await settle(app);
  };

  const check = async (name, action) => {
    const context = await browser.newContext({ viewport: { width: 1440, height: 1050 } });
    if (browser.browserType().name() === "chromium")
      await context.grantPermissions(["clipboard-read", "clipboard-write"]);
    const app = await context.newPage();
    app.setDefaultTimeout(7000);
    const errors = [];
    app.on("dialog", (dialog) => dialog.accept());
    app.on("pageerror", (error) => errors.push(String(error)));
    await hooks.start?.(app);
    try {
      await app.goto(input.root);
      await current(app);
      await action(app);
      assert(!errors.length, `Page errors: ${errors.join("; ")}`);
      report.checks.push({ name, passed: true });
      console.log(`workspace PASS: ${name}`);
    } catch (error) {
      report.checks.push({
        name,
        passed: false,
        error: String(error.stack ?? error),
        state: await app.locator("#result-state").textContent(),
        diagnostics: await app.locator("#diagnostics").textContent(),
        sources: (await saved(app))?.files.map((file) => ({ path: file.path, source: file.source.slice(0, 500) })),
      });
      report.passed = false;
    } finally {
      await hooks.finish?.(app);
      await context.close();
    }
  };

  const row = (app, path) => app.locator(`#source-tree [data-path="${path}"]`);
  const menu = async (app, path, action) => {
    await row(app, path).click({ button: "right" });
    await app.getByRole("menuitem", { name: action, exact: true }).click();
  };
  const pathDialog = async (app, value) => {
    await app.getByRole("dialog").locator("input").fill(value);
    await app.getByRole("button", { name: "Continue", exact: true }).click();
  };
  const addFolder = async (app, name, path = `/workspace/${name}`) => {
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-folder").click();
    await pathDialog(app, name);
    await row(app, path).waitFor();
    await settle(app);
  };

  const filePayload = (name, source) => ({ name, mimeType: "text/plain", buffer: Buffer.from(source, "utf8") });
  const replaceText = async (app, source) => {
    await app.locator("#source").click({ position: { x: 160, y: 40 } });
    const modifier = await app.evaluate(() => (navigator.userAgent.includes("Macintosh") ? "Meta" : "Control"));
    await app.keyboard.press(`${modifier}+a`);
    await app.keyboard.insertText(source);
    await settle(app);
  };

  const role = async (app, value) => {
    await app.locator("#file-properties").click();
    await app
      .getByRole("dialog")
      .getByRole("radio", { name: value === "root" ? "Root" : "Include-only", exact: true })
      .locator("..")
      .click();
    await app.getByRole("button", { name: "Apply", exact: true }).click();
    await settle(app);
  };

  await check("Explorer groups collapse independently and actions appear on hover or keyboard focus", async (app) => {
    await app.locator("#profile").focus();
    await app.locator(".intro").hover();
    if (!(await app.evaluate(() => matchMedia("(hover: none)").matches)))
      assert(
        (await app.locator(".source-tools").evaluate((node) => getComputedStyle(node).opacity)) === "0",
        "Explorer actions remained visible outside the pane",
      );
    await app.locator("#generated-group-heading").hover();
    if (!(await app.evaluate(() => matchMedia("(hover: none)").matches)))
      assert(
        (await app.locator(".source-tools").evaluate((node) => getComputedStyle(node).opacity)) === "0",
        "Generated hover revealed Source actions",
      );
    const gap = await app.evaluate(() => {
      const groups = document.querySelectorAll(".explorer-group");
      return groups[1].getBoundingClientRect().top - groups[0].getBoundingClientRect().bottom;
    });
    assert(gap >= 20, "Generated is missing its blank-line spacing");
    await app.locator("#source-explorer-panel").hover();
    assert(
      (await app.locator(".source-tools").evaluate((node) => getComputedStyle(node).opacity)) === "1",
      "Hover did not reveal Explorer actions",
    );
    await app.locator(".intro").hover();
    await app.locator("#new-file").focus();
    assert(
      (await app.locator(".source-tools").evaluate((node) => getComputedStyle(node).opacity)) === "1",
      "Keyboard focus did not reveal Explorer actions",
    );
    assert(
      (await row(app, "/workspace")
        .locator(".explorer-chevron")
        .evaluate((node) => node.getBoundingClientRect().width)) >= 18,
      "Folder chevron is too small",
    );
    await app.locator("#source-group-toggle").click();
    assert(await app.locator("#source-group-content").isHidden(), "Source group did not collapse");
    assert(await app.locator("#generated-group-content").isVisible(), "Source collapse affected Generated");
    await app.locator("#generated-group-toggle").click();
    assert(await app.locator("#generated-group-content").isHidden(), "Generated group did not collapse");
    assert((await app.locator("#result-state").textContent()) === "Current", "Group collapse invalidated compilation");
    await app.locator("#source-group-toggle").focus();
    await app.keyboard.press("Enter");
    assert(await app.locator("#source-group-content").isVisible(), "Keyboard did not expand Source");
    await app.locator("#generated-group-toggle").click();
    await app.locator(".theme-toggle label").filter({ hasText: "Dark" }).click();
    await app.locator("#source-explorer-panel").hover();
    await app.locator(".explorer").screenshot({ path: "output/playwright/workspace-explorer-groups-dark.png" });
  });

  await check("file and folder context menus expose only their own actions", async (app) => {
    await row(app, "/workspace/main.idl").click({ button: "right" });
    for (const name of [
      "New File…",
      "New Folder…",
      "Import Files…",
      "Add Containing Folder to Include Folders",
      "Add Folder to Include Folders",
    ])
      assert(
        (await app.getByRole("menuitem", { name, exact: true }).count()) === 0,
        `File menu exposed folder action ${name}`,
      );
    await app.getByRole("menuitem", { name: "Mark as Include-only", exact: true }).click();
    await settle(app);
    assert(!(await saved(app)).files[0].isGenerationRoot, "Context action did not remove Root role");
    await menu(app, "/workspace/main.idl", "Mark as Root");
    await current(app);
    await addFolder(app, "headers");
    await row(app, "/workspace/headers").click({ button: "right" });
    assert(
      (await app.getByRole("menuitem", { name: "New Folder…", exact: true }).count()) === 1,
      "Folder creation action is missing",
    );
    assert(
      (await app.getByRole("menuitem", { name: /Mark as/ }).count()) === 0,
      "Folder menu exposed a file role action",
    );
    await app.getByRole("menuitem", { name: "Add Folder to Include Folders", exact: true }).click();
    await current(app);
    assert(
      JSON.stringify((await saved(app)).options.includeDirectories) === JSON.stringify(["/workspace/headers"]),
      "Folder context did not select that folder",
    );
    await menu(app, "/workspace/headers", "Remove Folder from Include Folders");
    await current(app);
    await app.reload();
    await current(app);
    assert(
      (await saved(app)).files[0].isGenerationRoot && (await saved(app)).options.includeDirectories.length === 0,
      "Context changes were not recovered",
    );
  });

  await check("folders open by default while manual collapse survives workspace edits", async (app) => {
    await app.locator("#example").selectOption("workspace-includes");
    await app.locator("#load-example").click();
    await current(app);
    assert(
      (await row(app, "/workspace/shared").getAttribute("aria-expanded")) === "true",
      "Loaded folder did not open by default",
    );
    await row(app, "/workspace/shared/types.idl").waitFor();
    await row(app, "/workspace/shared").click();
    await menu(app, "/workspace/main.idl", "Mark as Include-only");
    await settle(app);
    assert(
      (await row(app, "/workspace/shared").getAttribute("aria-expanded")) === "false",
      "Workspace edit reopened a manually collapsed folder",
    );
    await menu(app, "/workspace/main.idl", "Mark as Root");
    await current(app);
    await app.reload();
    await current(app);
    await row(app, "/workspace/shared/types.idl").waitFor();
    assert(
      (await row(app, "/workspace/shared").getAttribute("aria-expanded")) === "true",
      "Recovery did not expand folders by default",
    );
  });

  await check("IDL Copy and Download use the active file and preserve exact source", async (app) => {
    const source = "// café\r\nstruct Imported { long value; };\r\n";
    await app.locator("#file").setInputFiles(filePayload("copy-me.idl", source));
    await app.getByRole("tab", { name: "copy-me.idl", exact: true }).waitFor();
    await current(app);
    await app.evaluate(() => {
      Object.defineProperty(navigator, "clipboard", {
        configurable: true,
        value: {
          writeText: async (value) => {
            window.__idlCopied = value;
          },
        },
      });
      const original = URL.createObjectURL;
      URL.createObjectURL = (blob) => {
        window.__idlDownload = blob;
        return original(blob);
      };
      HTMLAnchorElement.prototype.click = function () {
        window.__idlDownloadName = this.download;
      };
    });
    await app.locator("#copy-source").click();
    assert(
      (await app.evaluate(() => window.__idlCopied)) === source,
      "IDL Copy changed source or selected the wrong file",
    );
    await app.locator("#download-source-file").click();
    assert(
      (await app.evaluate(() => window.__idlDownloadName)) === "copy-me.idl",
      "IDL download used the wrong filename",
    );
    assert((await app.evaluate(() => window.__idlDownload.text())) === source, "IDL download changed UTF-8/CRLF text");
    await app.getByRole("tab", { name: "main.idl", exact: true }).click();
    await replaceText(app, "struct Broken;");
    await app.locator("#copy-source").click();
    assert(
      (await app.evaluate(() => window.__idlCopied)) === "struct Broken;",
      "Invalid active IDL could not be copied",
    );
    await app.locator("#download-source-file").click();
    assert(
      (await app.evaluate(() => window.__idlDownloadName)) === "main.idl",
      "Source download did not follow tab selection",
    );
    assert(
      (await app.evaluate(() => window.__idlDownload.text())) === "struct Broken;",
      "Source download used previous successful source",
    );
  });

  await check("operation errors use explicit alert styling in both themes", async (app) => {
    const before = await saved(app);
    await menu(app, "/workspace/main.idl", "Move to…");
    await pathDialog(app, "/workspace/missing");
    await app.waitForFunction(() => document.querySelector("#notice").dataset.kind === "error");
    assert(
      (await app.locator("#notice-message").textContent()).includes("Destination folder does not exist"),
      "Move error did not explain the missing destination",
    );
    assert(
      (await app.locator("#notice-kind").isVisible()) &&
        (await app.locator("#notice-message").getAttribute("role")) === "alert",
      "Error notice lacks its label or alert semantics",
    );
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Failed move mutated the workspace");
    const light = await app.locator("#notice").evaluate((node) => ({
      background: getComputedStyle(node).backgroundColor,
      color: getComputedStyle(node).color,
    }));
    await app.locator("#notice").hover();
    await app.screenshot({ path: "output/playwright/workspace-error-light.png", fullPage: true });
    await app.locator(".theme-toggle label").filter({ hasText: "Dark" }).click();
    await app.locator("#notice").hover();
    const dark = await app.locator("#notice").evaluate((node) => ({
      background: getComputedStyle(node).backgroundColor,
      color: getComputedStyle(node).color,
    }));
    assert(
      light.background !== dark.background && light.color !== dark.color,
      "Error notice did not adapt to the theme",
    );
    for (const theme of [light, dark]) {
      const channels = theme.background.match(/\d+/g).map(Number);
      assert(channels[0] > channels[1], "Error notice background does not use the red error palette");
    }
    await app.screenshot({ path: "output/playwright/workspace-error-dark.png", fullPage: true });
    await app.locator("#notice-dismiss").click();
    await app.evaluate(() =>
      Object.defineProperty(navigator, "clipboard", {
        configurable: true,
        value: {
          writeText: async () => {
            throw new Error("denied");
          },
        },
      }),
    );
    await app.locator("#copy-source").click();
    await app.waitForFunction(() =>
      document.querySelector("#notice-message").textContent.includes("Clipboard access is unavailable"),
    );
    assert(
      (await app.locator("#notice").getAttribute("data-kind")) === "error",
      "Clipboard failure used an informational notice",
    );
  });

  await check("source ZIP saves the current failed workspace with folders and settings", async (app) => {
    await app.locator("#example").selectOption("workspace-preprocessor");
    await app.locator("#load-example").click();
    await current(app);
    await addFolder(app, "empty");
    await replaceText(app, "// café\nstruct Broken { Missing member; };");
    assert((await app.locator("#result-state").textContent()) === "Failed", "Backup probe did not fail compilation");
    assert(
      (await app.locator("#download-all").isDisabled()) && !(await app.locator("#download-source").isDisabled()),
      "Source ZIP incorrectly depends on generated output",
    );
    await app.evaluate(() => {
      const original = URL.createObjectURL;
      URL.createObjectURL = (blob) => {
        window.__sourceBackup = blob;
        return original(blob);
      };
      HTMLAnchorElement.prototype.click = function () {
        window.__sourceBackupName = this.download;
      };
    });
    await app.locator("#download-source").click();
    assert(
      (await app.evaluate(() => window.__sourceBackupName)) === "wireloom-sources.zip",
      "Wrong source ZIP filename",
    );
    const entries = await app.evaluate(async () => {
      const bytes = new Uint8Array(await window.__sourceBackup.arrayBuffer());
      const view = new DataView(bytes.buffer);
      const decoder = new TextDecoder();
      const entries = {};
      let offset = 0;
      while (view.getUint32(offset, true) === 0x04034b50) {
        if (view.getUint16(offset + 8, true) !== 0) throw new Error("Expected stored ZIP entries");
        const size = view.getUint32(offset + 18, true);
        const nameSize = view.getUint16(offset + 26, true);
        const extraSize = view.getUint16(offset + 28, true);
        const name = decoder.decode(bytes.subarray(offset + 30, offset + 30 + nameSize));
        const start = offset + 30 + nameSize + extraSize;
        entries[name] = decoder.decode(bytes.subarray(start, start + size));
        offset = start + size;
      }
      return entries;
    });
    const workspace = await saved(app);
    assert(
      JSON.stringify(JSON.parse(entries["workspace.json"])) === JSON.stringify(workspace),
      "Source ZIP lost workspace settings or current source",
    );
    assert(entries["workspace/empty/"] === "", "Source ZIP lost an empty folder");
    for (const file of workspace.files)
      assert(entries[file.path.slice(1)] === file.source, `Source ZIP changed ${file.path}`);
  });

  await check("Escape cancels every dialog and Properties exposes keyboard-operable toggles", async (app) => {
    const before = await saved(app);
    await app.locator("#workspace-settings").click();
    await app
      .getByRole("dialog")
      .getByLabel("Defines (one NAME or NAME=value per line; all Roots)", { exact: true })
      .fill("CANCELED=1");
    await app.keyboard.press("Escape");
    await app.getByRole("dialog").waitFor({ state: "hidden" });
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Escape applied Workspace Settings");
    await app.locator("#file-properties").click();
    const properties = app.getByRole("dialog");
    await properties.getByRole("radio", { name: "Root", exact: true }).focus();
    await app.keyboard.press("ArrowRight");
    assert(
      await properties.getByRole("radio", { name: "Include-only", exact: true }).isChecked(),
      "Role toggle did not respond to arrow keys",
    );
    await properties.getByRole("radio", { name: "RTI C#", exact: true }).focus();
    await app.keyboard.press("ArrowRight");
    assert(
      await properties.getByRole("radio", { name: "IDL", exact: true }).isChecked(),
      "Mapping toggle did not respond to arrow keys",
    );
    await app.screenshot({ path: "output/playwright/workspace-properties-toggles.png", fullPage: true });
    await app.keyboard.press("Escape");
    await properties.waitFor({ state: "hidden" });
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Escape applied File Properties");
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await app.getByRole("dialog").locator("input").fill("canceled.idl");
    await app.keyboard.press("Escape");
    await app.getByRole("dialog").waitFor({ state: "hidden" });
    assert((await saved(app)).files.length === 1, "Escape created a file");
    await app.locator("#file").setInputFiles(filePayload("main.idl", "struct Canceled { long x; };"));
    await app.getByRole("dialog", { name: "Import conflict", exact: true }).waitFor();
    await app.keyboard.press("Escape");
    await app.getByRole("dialog").waitFor({ state: "hidden" });
    assert(
      JSON.stringify(await saved(app)) === JSON.stringify(before),
      "Escape partially applied a conflicting import",
    );
  });

  await check("multi-file examples match complete native output on both profiles", async (app) => {
    await app.evaluate(() => {
      window.__exports = [];
      const original = URL.createObjectURL;
      URL.createObjectURL = (blob) => {
        window.__exports.push(blob);
        return original(blob);
      };
      HTMLAnchorElement.prototype.click = function () {};
    });
    for (const id of ["workspace-includes", "workspace-preprocessor"]) {
      for (const profile of ["7.3.1", "7.7.0"]) {
        await app.locator("#profile").selectOption(profile);
        await settle(app);
        await app.locator("#example").selectOption(id);
        await app.locator("#load-example").click();
        await current(app);
        const expected = input.exampleExpected.find(
          (value) => value.caseId === `example-${id}` && value.profile === profile,
        );
        const names = await app.locator("#file-list button").allTextContents();
        assert(
          JSON.stringify([...names].sort()) === JSON.stringify(expected.documents.map((doc) => doc.name).sort()),
          "Multi-file generated document names differ",
        );
        for (const doc of expected.documents) {
          await app.locator("#file-list button").filter({ hasText: doc.name }).click();
          await app.locator("#download").click();
          assert(
            (await app.evaluate(() => window.__exports.at(-1).text())) === doc.source,
            `Native output differs for ${doc.name}`,
          );
        }
        const workspace = await saved(app);
        assert(
          workspace.files.length === 2 && !workspace.files[1].isGenerationRoot,
          "Example lost its Include-only file",
        );
      }
    }
    await app.screenshot({ path: "output/playwright/workspace-settings-example.png", fullPage: true });
    await app.locator(".explorer").screenshot({ path: "output/playwright/workspace-explorer.png" });
  });

  await check("imports use the selected folder, support every conflict choice, and cancel atomically", async (app) => {
    await addFolder(app, "imports");
    await app
      .locator("#file")
      .setInputFiles([
        filePayload("one.idl", "struct One { long x; };"),
        filePayload("two.idl", "struct Two { long y; };"),
      ]);
    await app.getByRole("tab", { name: "two.idl", exact: true }).waitFor();
    await current(app);
    assert(
      (await saved(app)).files.some((file) => file.path === "/workspace/imports/one.idl"),
      "Import ignored its selected folder",
    );
    const before = await saved(app);
    await app
      .locator("#file")
      .setInputFiles([filePayload("fresh.idl", "struct Fresh { long x; };"), filePayload("one.idl", "changed")]);
    await app.getByRole("dialog", { name: "Import conflict", exact: true }).waitFor();
    await app.getByRole("button", { name: "Cancel", exact: true }).click();
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Cancel partially applied the import");
    await app.locator("#file").setInputFiles(filePayload("one.idl", "struct Replaced { long x; };"));
    await app.getByRole("button", { name: "Replace", exact: true }).click();
    await current(app);
    await app.locator("#file").setInputFiles(filePayload("one.idl", "bad"));
    await app.getByRole("button", { name: "Skip", exact: true }).click();
    await current(app);
    assert(
      (await saved(app)).files.find((file) => file.path.endsWith("one.idl")).source.includes("Replaced"),
      "Skip replaced an existing file",
    );
    await app.locator("#file").setInputFiles(filePayload("one.idl", "struct Renamed { long x; };"));
    await app.getByRole("button", { name: "Rename", exact: true }).click();
    await pathDialog(app, "renamed.idl");
    await app.getByRole("tab", { name: "renamed.idl", exact: true }).waitFor();
    await current(app);
    assert((await saved(app)).files.length === 4, "Rename import did not add one file");
    await app.locator("#file").setInputFiles(filePayload("large.idl", "x".repeat(262145)));
    await app.waitForFunction(() =>
      document.querySelector("#notice-message").textContent.includes("Could not import files"),
    );
    assert((await saved(app)).files.length === 4, "Rejected import mutated the workspace");
  });

  await check("empty folders persist, keyboard tree actions work, and folders move by menu and drag", async (app) => {
    await addFolder(app, "empty");
    await app.reload();
    await current(app);
    await row(app, "/workspace/empty").waitFor();
    await row(app, "/workspace/empty").focus();
    await app.keyboard.press("Shift+F10");
    await app.getByRole("menuitem", { name: "Rename…", exact: true }).click();
    await pathDialog(app, "renamed");
    await row(app, "/workspace/renamed").waitFor();
    await row(app, "/workspace").focus();
    await app.keyboard.press("Home");
    await app.keyboard.press("ArrowDown");
    assert(
      (await row(app, "/workspace/renamed").getAttribute("aria-selected")) === "true",
      "Tree navigation did not move selection",
    );
    await addFolder(app, "nested", "/workspace/renamed/nested");
    await menu(app, "/workspace/renamed/nested", "Move to…");
    await pathDialog(app, "/workspace");
    await row(app, "/workspace/nested").waitFor();
    await row(app, "/workspace/nested").dragTo(row(app, "/workspace/renamed"));
    await row(app, "/workspace/renamed/nested").waitFor();
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#collapse-folders").click();
    assert(
      (await row(app, "/workspace/renamed/nested").count()) === 0,
      "Collapse Folders left a nested folder visible",
    );
    await row(app, "/workspace/renamed").click();
    await row(app, "/workspace/renamed/nested").waitFor();
    await menu(app, "/workspace/renamed", "Delete");
    assert(
      (await app.getByRole("dialog").textContent()).includes("all its contents"),
      "Non-empty folder deletion was not confirmed",
    );
    await app.getByRole("button", { name: "Cancel", exact: true }).click();
    await row(app, "/workspace/renamed/nested").waitFor();
  });

  await check("source tabs retain undo across switches and file renames", async (app) => {
    const original = (await saved(app)).files[0].source;
    await replaceText(app, "struct Edited { long x; };");
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "second.idl");
    await app.getByRole("tab", { name: "main.idl", exact: true }).click();
    await menu(app, "/workspace/main.idl", "Rename…");
    await pathDialog(app, "renamed.idl");
    await app.locator("#source").click({ position: { x: 160, y: 40 } });
    const undoKey = await app.evaluate(() => (navigator.userAgent.includes("Macintosh") ? "Meta+z" : "Control+z"));
    for (let undo = 0; undo < 20; undo++) {
      await app.keyboard.press(undoKey);
      await settle(app);
      if ((await saved(app)).files.find((file) => file.path === "/workspace/renamed.idl").source === original) break;
    }
    await current(app);
    assert(
      (await saved(app)).files.find((file) => file.path === "/workspace/renamed.idl").source === original,
      "Tab switch or rename lost undo history",
    );
  });

  await check(
    "folder settings follow renames while source directives remain unchanged and diagnostics navigate",
    async (app) => {
      await app.locator("#example").selectOption("workspace-preprocessor");
      await app.locator("#load-example").click();
      await current(app);
      const original = (await saved(app)).files[0].source;
      await menu(app, "/workspace/config", "Rename…");
      await pathDialog(app, "settings");
      await current(app);
      let workspace = await saved(app);
      assert(
        workspace.options.includeDirectories[0] === "/workspace/settings",
        "Include folder setting did not follow rename",
      );
      assert(workspace.files[0].source === original, "Folder rename rewrote IDL text");
      assert(
        (await row(app, "/workspace/settings").getAttribute("aria-expanded")) === "true",
        "Renamed folder did not expand by default",
      );
      await row(app, "/workspace/settings/types.idl").click();
      await replaceText(app, "module Config { struct Value { Missing member; }; };");
      await app.waitForFunction(() => document.querySelector("#result-state").textContent === "Failed");
      await app.getByRole("tab", { name: "main.idl", exact: true }).click();
      await app.locator("#diagnostics button").first().click();
      assert(
        (await app.getByRole("tab", { name: "types.idl", exact: true }).getAttribute("aria-selected")) === "true",
        "Diagnostic did not select the included file",
      );
      assert(
        await app.evaluate(() => !!document.activeElement.closest("#source")),
        "Diagnostic did not focus the source editor",
      );
      workspace = await saved(app);
      assert(workspace.files[1].options.sequenceSize === 100, "Include-only file lost default properties");
    },
  );

  await check("no-Root and empty workspaces clear output and survive recovery", async (app) => {
    await role(app, "include");
    assert(
      (await app.locator("#diagnostics-empty").textContent()) === "Mark at least one file as Root",
      "No-Root workspace error differs",
    );
    assert((await app.locator("#diagnostics button").count()) === 0, "Workspace error has a fake source location");
    assert(
      (await app.locator("#file-list button").count()) === 0 && (await app.locator("#download-all").isDisabled()),
      "No-Root workspace retained exportable output",
    );
    await app.reload();
    await settle(app);
    await menu(app, "/workspace/main.idl", "Delete");
    await app.getByRole("button", { name: "Delete", exact: true }).click();
    await app.locator("#empty-source").waitFor();
    await settle(app);
    assert(
      (await app.locator("#result-state").textContent()) === "Empty",
      "Final file deletion did not produce an empty workspace",
    );
    await app.reload();
    await settle(app);
    assert((await saved(app)).files.length === 0, "Empty workspace was replaced on reload");
    for (const id of ["copy-source", "download-source-file"])
      assert(await app.locator(`#${id}`).isDisabled(), "Empty workspace left single-file controls enabled");
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "first.idl");
    await current(app);
    assert((await saved(app)).files[0].isGenerationRoot, "New file is not a Root by default");
  });

  await check("Workspace Settings and file Properties apply their own compiler scopes", async (app) => {
    await app.locator("#example").selectOption("workspace-preprocessor");
    await app.locator("#load-example").click();
    await current(app);
    await app.locator("#workspace-settings").click();
    const settings = app.getByRole("dialog");
    await settings.getByLabel("Undefines (one symbol per line; applied after defines)", { exact: true }).fill("");
    await settings.getByLabel("Strict validation (whole batch)", { exact: true }).check();
    await settings.getByLabel("Enable escape character (whole batch)", { exact: true }).check();
    await settings.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    assert(
      !(await app.locator("#file-list").textContent()).includes("Enabled.g.cs"),
      "Removing an undefine did not affect preprocessing",
    );
    const configured = await saved(app);
    assert(
      configured.options.strict && configured.options.enableEscapeChar && configured.options.undefines.length === 0,
      "Workspace settings were not saved",
    );
    await app.getByRole("tab", { name: "types.idl", exact: true }).click();
    await app.locator("#file-properties").click();
    const properties = app.getByRole("dialog");
    await properties.getByRole("radio", { name: "IDL", exact: true }).locator("..").click();
    await properties.getByLabel("Unbounded sequence size (this declaring file)", { exact: true }).fill("12");
    await properties.getByLabel("Unbounded string size (this declaring file)", { exact: true }).fill("42");
    await properties.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    const workspace = await saved(app);
    assert(
      workspace.files[1].options.sequenceArrayMapping === "Idl" &&
        workspace.files[1].options.sequenceSize === 12 &&
        workspace.files[1].options.stringSize === 42,
      "Declaring-file settings were lost",
    );
    assert(workspace.files[0].options.sequenceSize === 100, "File properties leaked to another file");
    await app.locator("#workspace-settings").click();
    await app
      .getByRole("dialog")
      .getByLabel("Defines (one NAME or NAME=value per line; all Roots)", { exact: true })
      .fill("CHANGED=1");
    await app.getByRole("dialog").getByRole("button", { name: "Cancel", exact: true }).click();
    assert(JSON.stringify(await saved(app)) === JSON.stringify(workspace), "Cancel applied Workspace Settings");
  });

  await check("rename conflicts require explicit Cancel, Rename, or Replace", async (app) => {
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "second.idl");
    await current(app);
    const original = await saved(app);
    await menu(app, "/workspace/main.idl", "Rename…");
    await pathDialog(app, "second.idl");
    await app.getByRole("dialog").getByRole("button", { name: "Cancel", exact: true }).click();
    assert(JSON.stringify(await saved(app)) === JSON.stringify(original), "Cancel overwrote a conflicting path");
    await menu(app, "/workspace/main.idl", "Rename…");
    await pathDialog(app, "second.idl");
    await app.getByRole("dialog").getByRole("button", { name: "Rename", exact: true }).click();
    await pathDialog(app, "/workspace/third.idl");
    await current(app);
    assert((await saved(app)).files[0].id === original.files[0].id, "Rename lost file identity");
    await menu(app, "/workspace/third.idl", "Rename…");
    await pathDialog(app, "second.idl");
    await app.getByRole("dialog").getByRole("button", { name: "Replace", exact: true }).click();
    await current(app);
    const replaced = await saved(app);
    assert(
      replaced.files.length === 1 &&
        replaced.files[0].path === "/workspace/second.idl" &&
        replaced.files[0].source === original.files[0].source,
      "Replace did not preserve the moved source",
    );
  });

  await check("cyclic includes retain an actionable original source location", async (app) => {
    await app.locator("#example").selectOption("workspace-includes");
    await app.locator("#load-example").click();
    await current(app);
    await app.getByRole("tab", { name: "types.idl", exact: true }).click();
    await replaceText(app, '#include "../main.idl"');
    assert(
      (await app.locator("#diagnostics").textContent()).includes("Cyclic include detected"),
      "Include cycle was not visible",
    );
    await app.getByRole("tab", { name: "main.idl", exact: true }).click();
    await app.locator("#diagnostics button").first().click();
    assert(
      (await app.getByRole("tab", { name: "types.idl", exact: true }).getAttribute("aria-selected")) === "true",
      "Include cycle diagnostic lost its including file",
    );
    assert(await app.locator("#download-all").isDisabled(), "Cycle retained exportable output");
  });

  await check("v1 recovery migrates source and profile without loss", async (app) => {
    const source = "// café\r\nstruct Legacy { long x; };\r\n";
    await app.evaluate(
      (source) =>
        new Promise((resolve, reject) => {
          const opening = indexedDB.open("wireloom-playground", 1);
          opening.onsuccess = () => {
            const database = opening.result;
            const transaction = database.transaction("workspace", "readwrite");
            transaction.objectStore("workspace").put(
              {
                version: 1,
                profile: "7.3.1",
                files: [{ path: "/workspace/legacy.idl", source, isGenerationRoot: true }],
              },
              "current",
            );
            transaction.oncomplete = () => {
              database.close();
              resolve();
            };
            transaction.onerror = () => reject(transaction.error);
          };
        }),
      source,
    );
    await app.reload();
    await current(app);
    const workspace = await saved(app);
    assert(
      workspace.version === 2 &&
        workspace.profile === "7.3.1" &&
        workspace.files[0].source === source &&
        workspace.files[0].isGenerationRoot,
      "Legacy migration changed source, role, or profile",
    );
    assert(workspace.options.includeDirectories.length === 0, "Migration invented include folders");
  });
  return report;
};
