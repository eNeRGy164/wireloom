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

  const check = async (name, action, startEmpty = false) => {
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
      await settle(app);
      if (!startEmpty && (await app.locator("#result-state").textContent()) === "Empty") {
        await app.locator("#example").selectOption("hello");
        await app.locator("#load-example").click();
        await current(app);
      }
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
  const clickExplorerBlank = async (app, button = "left") => {
    const bounds = await app.locator("#explorer-content").boundingBox();
    assert(bounds, "Explorer content has no visible bounds");
    await app.mouse.click(bounds.x + bounds.width - 12, bounds.y + bounds.height - 12, { button });
  };
  const properties = async (app) => {
    await app.locator('#source-tabs [role="tab"][aria-selected="true"]').click({ button: "right" });
    await app.getByRole("menuitem", { name: "Properties…", exact: true }).click();
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
    await properties(app);
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
    assert((await row(app, "/workspace").count()) === 0, "The implicit workspace root appeared in the Explorer");
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

  await check("The hidden workspace root leaves a visible Explorer keyboard tab stop", async (app) => {
    const tabStops = app.locator('#source-tree [role="treeitem"][tabindex="0"]');
    assert((await tabStops.count()) === 1, "Explorer does not have exactly one keyboard tab stop");
    assert((await tabStops.getAttribute("data-path")) !== "/workspace", "The hidden workspace root received focus");
    await tabStops.focus();
    assert(
      await tabStops.evaluate((item) => item === document.activeElement),
      "The visible tree item could not receive focus",
    );
  });

  await check(
    "Explorer blank space clears tree selection and targets root actions",
    async (app) => {
      await addFolder(app, "first");
      await menu(app, "/workspace/first", "New File…");
      await pathDialog(app, "nested.idl");
      await row(app, "/workspace/first/nested.idl").waitFor();
      await settle(app);
      assert(
        (await saved(app)).files.every((file) => file.path.startsWith("/workspace/first/")),
        "Test workspace unexpectedly has a root-level file",
      );

      const activeTab = app.locator('#source-tabs [role="tab"][aria-selected="true"]');
      const activePath = await activeTab.getAttribute("title");
      await clickExplorerBlank(app);
      assert(
        (await app.locator('#source-tree [role="treeitem"][aria-selected="true"]').count()) === 0,
        "Clicking Explorer blank space did not clear tree selection",
      );
      assert(
        (await app.locator('#source-tabs [role="tab"][aria-selected="true"]').getAttribute("title")) === activePath,
        "Clearing Explorer selection changed the active source tab",
      );

      await clickExplorerBlank(app, "right");
      for (const name of ["New File…", "New Folder…", "Import Files…"])
        assert(
          (await app.getByRole("menuitem", { name, exact: true }).count()) === 1,
          "Blank-space menu is missing " + name,
        );
      await app.getByRole("menuitem", { name: "New Folder…", exact: true }).click();
      await pathDialog(app, "second");
      await row(app, "/workspace/second").waitFor();
      await settle(app);

      await clickExplorerBlank(app, "right");
      await app.getByRole("menuitem", { name: "New File…", exact: true }).click();
      await pathDialog(app, "root.idl");
      await row(app, "/workspace/root.idl").waitFor();
      assert(
        (await saved(app)).files.some((file) => file.path === "/workspace/root.idl"),
        "New File from the blank-space menu did not target the workspace root",
      );
    },
    true,
  );

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
    assert(
      !(await app.getByRole("dialog").textContent()).includes("/workspace"),
      "Move dialog exposed the internal workspace prefix",
    );
    assert(
      !(await app.getByRole("dialog").textContent()).includes("/workspace"),
      "Move dialog exposed the internal workspace prefix",
    );
    await pathDialog(app, "missing");
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
      center: node.getBoundingClientRect().left + node.getBoundingClientRect().width / 2,
      viewportCenter: window.innerWidth / 2,
    }));
    assert(
      Math.abs(light.center - light.viewportCenter) < 2,
      "Notification is not horizontally centered in the viewport",
    );
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
    assert(
      entries["workspace/Wireloom.csproj"]?.includes('Include="main.idl"') &&
        !entries["workspace/Wireloom.csproj"].includes("/workspace"),
      `Source ZIP project preview missing expected relative path: ${entries["workspace/Wireloom.csproj"]}`,
    );
    for (const file of workspace.files)
      assert(entries[file.path.slice(1)] === file.source, `Source ZIP changed ${file.path}`);
  });

  await check("CSProj escapes special file and include-directory paths for MSBuild", async (app) => {
    const { execFileSync } = await import("node:child_process");
    const { mkdtempSync, mkdirSync, rmSync, writeFileSync } = await import("node:fs");
    const { tmpdir } = await import("node:os");
    const { join } = await import("node:path");
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "sensor;v2.idl");
    await settle(app);
    await addFolder(app, "headers;v2");
    await menu(app, "/workspace/headers;v2", "Add Folder to Include Folders");
    await settle(app);

    await app.locator('#file-list button[data-name="Wireloom.csproj"]').click();
    await app.locator("#generated-panel").click();
    await app.keyboard.press("ControlOrMeta+a");
    await app.keyboard.press("ControlOrMeta+c");
    const projectSource = await app.evaluate(() => navigator.clipboard.readText());
    const directory = mkdtempSync(join(tmpdir(), "wireloom-msbuild-paths-"));
    try {
      writeFileSync(join(directory, "Wireloom.csproj"), projectSource);
      writeFileSync(join(directory, "main.idl"), "struct Main { long value; };\n");
      writeFileSync(join(directory, "sensor;v2.idl"), "struct Sensor { long value; };\n");
      mkdirSync(join(directory, "headers;v2"));
      writeFileSync(
        join(directory, "Directory.Build.targets"),
        '<Project><ItemGroup><_DdsIdlProjectIncludeDirectory Include="$(DdsIdlIncludeDirectories)" /></ItemGroup></Project>',
      );

      const roots = JSON.parse(
        execFileSync("dotnet", ["msbuild", "Wireloom.csproj", "-getItem:DdsIdl", "-nologo"], {
          cwd: directory,
          encoding: "utf8",
        }),
      ).Items.DdsIdl.map((item) => item.Identity.replaceAll("\\", "/"));
      const includeDirectories = JSON.parse(
        execFileSync("dotnet", ["msbuild", "Wireloom.csproj", "-getItem:_DdsIdlProjectIncludeDirectory", "-nologo"], {
          cwd: directory,
          encoding: "utf8",
        }),
      ).Items._DdsIdlProjectIncludeDirectory.map((item) => item.Identity.replaceAll("\\", "/"));
      assert(roots.includes("sensor;v2.idl"), `MSBuild split the IDL path into items: ${JSON.stringify(roots)}`);
      assert(
        includeDirectories.some((path) => path.endsWith("headers;v2")),
        `MSBuild split the include-directory path into items: ${JSON.stringify(includeDirectories)}`,
      );
    } finally {
      rmSync(directory, { recursive: true, force: true });
    }
  });

  await check("Escape cancels every dialog and Properties exposes keyboard-operable toggles", async (app) => {
    const before = await saved(app);
    await app.locator("#workspace-settings").click();
    await app.getByRole("dialog").getByLabel("Enable escape character", { exact: true }).check();
    await app.keyboard.press("Escape");
    await app.getByRole("dialog").waitFor({ state: "hidden" });
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Escape applied Workspace Settings");
    await properties(app);
    const propertiesDialog = app.getByRole("dialog");
    assert(
      !(await propertiesDialog.textContent()).includes("/workspace"),
      "File properties title exposed the internal workspace prefix",
    );
    await propertiesDialog.getByRole("radio", { name: "Root", exact: true }).focus();
    await app.keyboard.press("ArrowRight");
    assert(
      await propertiesDialog.getByRole("radio", { name: "Include-only", exact: true }).isChecked(),
      "Role toggle did not respond to arrow keys",
    );
    await app.screenshot({ path: "output/playwright/workspace-properties-toggles.png", fullPage: true });
    await app.keyboard.press("Escape");
    await propertiesDialog.waitFor({ state: "hidden" });
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Escape applied File Properties");

    await app.locator("#workspace-settings").click();
    const workspaceSettings = app.getByRole("dialog");
    await workspaceSettings.getByRole("radio", { name: "RTI C#", exact: true }).focus();
    await app.keyboard.press("ArrowRight");
    assert(
      await workspaceSettings.getByRole("radio", { name: "IDL", exact: true }).isChecked(),
      "Mapping toggle did not respond to arrow keys",
    );
    await app.keyboard.press("Escape");
    await workspaceSettings.waitFor({ state: "hidden" });
    assert(JSON.stringify(await saved(app)) === JSON.stringify(before), "Escape applied Workspace Settings");
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
        const expected = input.exampleExpected.find(
          (value) => value.caseId === `example-${id}` && value.profile === profile,
        );
        const expectedNames = expected.documents.map((doc) => doc.name).sort();
        const expectedSource = input.examples.find((example) => example.id === id).source;
        await app.waitForFunction(async (source) => {
          const database = await new Promise((resolve, reject) => {
            const request = indexedDB.open("wireloom-playground", 1);
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
          });
          try {
            const workspace = await new Promise((resolve, reject) => {
              const request = database.transaction("workspace").objectStore("workspace").get("current");
              request.onsuccess = () => resolve(request.result);
              request.onerror = () => reject(request.error);
            });
            return workspace?.files.some((file) => file.source === source);
          } finally {
            database.close();
          }
        }, expectedSource);
        await current(app);
        await app.waitForFunction((names) => {
          const actual = [...document.querySelectorAll('#file-list button:not([data-name$=".csproj"])')]
            .map((button) => button.dataset.name)
            .sort();
          return JSON.stringify(actual) === JSON.stringify(names);
        }, expectedNames);
        await current(app);
        const names = (await app.locator("#file-list button").allTextContents()).filter(
          (name) => !name.endsWith(".csproj"),
        );
        assert(
          JSON.stringify([...names].sort()) === JSON.stringify(expected.documents.map((doc) => doc.name).sort()),
          `Multi-file generated document names differ: actual=${JSON.stringify(names)} expected=${JSON.stringify(expected.documents.map((doc) => doc.name))}`,
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
    await row(app, "/workspace/renamed").focus();
    await app.keyboard.press("Home");
    assert(
      (await row(app, "/workspace/renamed").getAttribute("aria-selected")) === "true",
      "Home did not select the first visible tree entry",
    );
    await app.keyboard.press("ArrowDown");
    assert(
      (await row(app, "/workspace/main.idl").getAttribute("aria-selected")) === "true",
      "Tree navigation did not move selection",
    );
    await row(app, "/workspace/renamed").click();
    await addFolder(app, "nested", "/workspace/renamed/nested");
    await menu(app, "/workspace/renamed/nested", "Move to…");
    await pathDialog(app, "");
    await row(app, "/workspace/nested").waitFor();
    await row(app, "/workspace/nested").dragTo(row(app, "/workspace/renamed"));
    await row(app, "/workspace/renamed/nested").waitFor();
    await app.locator("#source-explorer-panel").hover();
    assert((await row(app, "/workspace").count()) === 0, "The implicit workspace root appeared in the Explorer");
    await row(app, "/workspace/renamed").click();
    assert((await row(app, "/workspace/renamed/nested").count()) === 0, "Collapsing a folder left its child visible");
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
      assert(workspace.files[1].options.strict === false, "Include-only file inherited root properties");
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
    assert(
      !(await app.getByRole("dialog").textContent()).includes("/workspace") &&
        (await app.getByRole("dialog").textContent()).includes("main.idl"),
      "Delete confirmation exposed the internal workspace prefix",
    );
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
    assert(await app.locator("#download-source").isDisabled(), "Empty workspace left the IDL ZIP enabled");
    assert(
      (await app.locator("#example-description").textContent()) === "",
      "Empty workspace retained the example description",
    );
    assert(await app.locator("#empty-create-file").isVisible(), "Empty workspace hid the blank-file action");
    assert(await app.locator("#empty-load-example").isVisible(), "Empty workspace hid the example action");
    assert(
      await app.locator("#empty-source .empty-source-actions").evaluate((node) => {
        const style = getComputedStyle(node);
        return style.display === "flex" && style.flexDirection === "row" && style.gap === "8px";
      }),
      "Empty workspace actions are not horizontally spaced",
    );
    for (const id of ["copy-source", "download-source-file"])
      assert(await app.locator(`#${id}`).isDisabled(), "Empty workspace left single-file controls enabled");
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "first.idl");
    await current(app);
    assert((await saved(app)).files[0].isGenerationRoot, "New file is not a Root by default");
    assert((await app.locator("#source-count").textContent()).includes("1 Root"), "Single root count is pluralized");
  });

  await check("source and generated tabs close, reopen, and expose Close All", async (app) => {
    const sourceTab = app.getByRole("tab", { name: "main.idl", exact: true });
    await sourceTab.click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close", exact: true }).click();
    await app.locator("#empty-source").waitFor();
    await row(app, "/workspace/main.idl").click();
    await app.locator("#empty-source").waitFor({ state: "hidden" });
    await app.locator("#source").click();
    await app.keyboard.press("ControlOrMeta+a");
    await app.keyboard.press("ControlOrMeta+c");
    assert(
      (await app.evaluate(() => navigator.clipboard.readText())).includes("Hello"),
      "Reopened source body is missing",
    );
    await app.locator("#source-tabs .editor-tab-close").click();
    await app.locator("#empty-source").waitFor();
    await row(app, "/workspace/main.idl").click();
    await app.locator('#source-tabs [role="tab"][aria-selected="true"]').click({ button: "middle" });
    await app.locator("#empty-source").waitFor();

    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "second.idl");
    await current(app);
    await app.locator('#source-tabs [role="tab"][aria-selected="true"]').click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close All", exact: true }).click();
    await app.locator("#empty-source").waitFor();
    assert((await saved(app)).files.length === 2, "Close All deleted source files");
    await app.locator("#source-tabs").click({ button: "right" });
    assert(
      await app.getByRole("menuitem", { name: "Close All", exact: true }).isDisabled(),
      "Idle source tab strip omitted its disabled Close All action",
    );
    await app.keyboard.press("Escape");

    await row(app, "/workspace/main.idl").click();
    await app.locator("#source-explorer-panel").hover();
    await app.locator("#new-file").click();
    await pathDialog(app, "third.idl");
    await current(app);
    await row(app, "/workspace/second.idl").click();
    await app.locator("#source-tabs .editor-tab").filter({ hasText: "main.idl" }).locator(".editor-tab-close").click();
    assert(
      (await app.locator('#source-tabs [role="tab"][aria-selected="true"]').textContent()).includes("second.idl"),
      "Closing an inactive source tab changed the active tab",
    );
    await app.locator('#source-tabs [role="tab"][aria-selected="true"]').focus();
    await app.keyboard.press("ArrowRight");
    const arrowTabs = await app.locator('#source-tabs [role="tab"]').allTextContents();
    const arrowActive = await app.locator('#source-tabs [role="tab"][aria-selected="true"]').textContent();
    assert(
      arrowActive.includes("third.idl"),
      `ArrowRight did not advance among the remaining open source tabs: active=${arrowActive}; tabs=${arrowTabs.join(",")}`,
    );
    await app.keyboard.press("Home");
    assert(
      (await app.locator('#source-tabs [role="tab"][aria-selected="true"]').textContent()).includes("second.idl") &&
        !(await app.locator('#source-tabs [role="tab"]').allTextContents()).some((name) => name.includes("main.idl")),
      "Home selected a closed source file instead of the first visible tab",
    );
    await app.locator("#source-tabs").click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close All", exact: true }).click();
    await app.locator("#empty-source").waitFor();

    const generatedFiles = app.locator('#file-list button:not([data-name$=".csproj"])');
    const generatedCount = await generatedFiles.count();
    for (let index = 0; index < generatedCount; index++) await generatedFiles.nth(index).click();
    const inactiveTab = app
      .locator("#output-tabs .editor-tab")
      .filter({ hasNot: app.locator('[aria-selected="true"]') })
      .first();
    if (generatedCount > 1) {
      const close = inactiveTab.locator(".editor-tab-close");
      assert(
        (await close.evaluate((node) => getComputedStyle(node).opacity)) === "0" &&
          (await close.evaluate((node) => node.getBoundingClientRect().width)) > 0,
        "Inactive tab did not reserve a hidden close-button slot",
      );
      await inactiveTab.locator(".editor-tab-label").hover();
      const accentSoft = await app.evaluate(() => {
        const probe = document.createElement("span");
        probe.style.backgroundColor = "var(--accent-soft)";
        document.body.append(probe);
        const color = getComputedStyle(probe).backgroundColor;
        probe.remove();
        return color;
      });
      assert(
        (await close.evaluate((node) => getComputedStyle(node).opacity)) === "1" &&
          (await inactiveTab.evaluate((node) => getComputedStyle(node).backgroundColor)) === accentSoft,
        "Hovered tab did not reveal its close button across the full tab background",
      );
    }
    await app.locator('#output-tabs [role="tab"][aria-selected="true"]').click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close All", exact: true }).click();
    await app.locator("#empty-output").waitFor();
    await generatedFiles.first().click();
    const generatedName = await generatedFiles.first().getAttribute("data-name");
    const generatedFileEntry = app.locator(`#file-list button[data-name="${generatedName}"]`);
    await generatedFileEntry.click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close", exact: true }).click();
    await app.locator("#empty-output").waitFor();
    await generatedFileEntry.click({ button: "right" });
    await app.getByRole("menuitem", { name: "Open", exact: true }).click();
    await app.locator("#empty-output").waitFor({ state: "hidden" });
    await generatedFileEntry.click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close", exact: true }).click();
    await app.locator("#empty-output").waitFor();
    await generatedFileEntry.click();
    await app.locator("#empty-output").waitFor({ state: "hidden" });
    assert(!(await app.locator("#copy").isDisabled()), "Copy stayed disabled after reopening generated output");
    assert(!(await app.locator("#download").isDisabled()), "Download stayed disabled after reopening generated output");
    await app.locator('#output-tabs [role="tab"][aria-selected="true"]').click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close", exact: true }).click();
    await app.locator("#empty-output").waitFor();
    await app.locator(`#file-list button[data-name="${generatedName}"]`).click();
    await app.locator("#empty-output").waitFor({ state: "hidden" });
    await app.locator("#generated-panel").click();
    await app.keyboard.press("ControlOrMeta+a");
    await app.keyboard.press("ControlOrMeta+c");
    assert((await app.evaluate(() => navigator.clipboard.readText())).length > 0, "Reopened generated body is missing");
    await app.locator("#output-tabs .editor-tab-close").click();
    await app.locator("#empty-output").waitFor();
    await generatedFileEntry.click();
    await app.locator('#output-tabs [role="tab"][aria-selected="true"]').click({ button: "middle" });
    await app.locator("#empty-output").waitFor();

    await generatedFiles.first().click();
    await app.locator('#output-tabs [role="tab"][aria-selected="true"]').click({ button: "right" });
    await app.getByRole("menuitem", { name: "Close All", exact: true }).click();
    await app.locator("#empty-output").waitFor();
    assert((await app.locator("#output-tabs [role=tab]").count()) === 0, "Generated Close All left tabs open");
    assert(
      (await app.locator('#file-list button[data-name="Wireloom.csproj"]').count()) === 1,
      "CSProj preview is missing",
    );
    assert(
      (await app.getByRole("tab", { name: "Wireloom.csproj", exact: true }).count()) === 0,
      "CSProj preview opened automatically",
    );
    assert(
      (await app.locator('#file-list button[data-name="Wireloom.csproj"] .file-icon-csproj').count()) === 1,
      "CSProj file icon is missing",
    );
  });

  await check(
    "fresh workspace offers blank IDL and Hello, Wireloom actions",
    async (app) => {
      assert((await saved(app)).files.length === 0, "First-use workspace was not empty");
      assert((await app.locator("#result-state").textContent()) === "Empty", "First-use state was not Empty");
      assert(
        (await app.locator("#example-description").textContent()) === "",
        "First use showed an example description",
      );
      assert(await app.locator("#download-source").isDisabled(), "Empty workspace enabled source ZIP download");
      await app.locator("#empty-create-file").click();
      await pathDialog(app, "untitled.idl");
      await current(app);
      assert((await saved(app)).files[0].path === "/workspace/untitled.idl", "Blank action did not create an IDL file");
      app.removeAllListeners("dialog");
      app.on("dialog", (dialog) => dialog.accept());
      await app.locator("#reset").click();
      await settle(app);
      await app.waitForFunction(() => document.querySelector("#source-state").textContent === "Saved locally");
      assert((await saved(app)).files.length === 0, "Reset created or retained an IDL file");
      assert(
        (await app.locator("#example-description").textContent()) === "",
        "Reset left example description visible",
      );
      await app.locator("#empty-load-example").click();
      await current(app);
      assert(
        (await saved(app)).files.some((file) => file.path === "/workspace/main.idl"),
        "Hello example did not load",
      );
    },
    true,
  );

  await check("Workspace and root Properties apply their compiler scopes", async (app) => {
    await app.locator("#example").selectOption("workspace-preprocessor");
    await app.locator("#load-example").click();
    await current(app);
    await app.locator("#workspace-settings").click();
    const settings = app.getByRole("dialog");
    await settings.getByLabel("Enable escape character", { exact: true }).check();
    await settings.getByRole("radio", { name: "IDL", exact: true }).locator("..").click();
    await settings.getByLabel("Unbounded sequence size", { exact: true }).fill("12");
    await settings.getByLabel("Unbounded string size", { exact: true }).fill("42");
    await settings.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    await app.getByRole("tab", { name: "main.idl", exact: true }).click();
    await properties(app);
    const rootProperties = app.getByRole("dialog");
    await rootProperties.getByLabel("Undefines (one symbol per line; applied after defines)", { exact: true }).fill("");
    await rootProperties.getByLabel("Strict validation", { exact: true }).check();
    await rootProperties.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    assert(
      !(await app.locator("#file-list").textContent()).includes("Enabled.g.cs"),
      "Removing an undefine did not affect preprocessing",
    );
    const configured = await saved(app);
    assert(
      configured.files[0].options.strict &&
        configured.options.enableEscapeChar &&
        configured.files[0].options.undefines.length === 0,
      "Workspace settings were not saved",
    );
    const workspace = await saved(app);
    assert(
      workspace.options.sequenceArrayMapping === "Idl" &&
        workspace.options.sequenceSize === 12 &&
        workspace.options.stringSize === 42,
      "Workspace sequence settings were lost",
    );
    await app.getByRole("tab", { name: "types.idl", exact: true }).click();
    await properties(app);
    const fileProperties = app.getByRole("dialog");
    const fileSequence = fileProperties.getByLabel("Unbounded sequence size", { exact: true });
    assert((await fileSequence.inputValue()) === "12", "File properties did not inherit the project sequence size");
    await fileSequence.fill("22");
    await fileProperties.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    await app.locator("#workspace-settings").click();
    const changedProjectSettings = app.getByRole("dialog");
    await changedProjectSettings.getByLabel("Unbounded sequence size", { exact: true }).fill("88");
    await changedProjectSettings.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    const withFileOverride = await saved(app);
    assert(
      withFileOverride.options.sequenceSize === 88 && withFileOverride.files[1].options.sequenceSize === 22,
      "File sequence preference did not remain after a project setting change",
    );
    await app.getByRole("tab", { name: "types.idl", exact: true }).click();
    await properties(app);
    const overriddenProperties = app.getByRole("dialog");
    await overriddenProperties
      .getByRole("button", { name: "Reset sequence size to project default", exact: true })
      .click();
    await overriddenProperties.getByRole("button", { name: "Apply", exact: true }).click();
    await current(app);
    const afterFileReset = await saved(app);
    assert(
      afterFileReset.files[1].options.sequenceSize === undefined,
      "Reset did not restore the project sequence setting",
    );
    await app.locator("#workspace-settings").click();
    await app.getByRole("dialog").getByLabel("Unbounded sequence size", { exact: true }).fill("99");
    await app.getByRole("dialog").getByRole("button", { name: "Cancel", exact: true }).click();
    assert(JSON.stringify(await saved(app)) === JSON.stringify(afterFileReset), "Cancel applied Workspace Settings");
  });

  await check(
    "Explicit file size overrides survive unrelated property changes when equal to project defaults",
    async (app) => {
      await app.locator("#workspace-settings").click();
      const projectSettings = app.getByRole("dialog");
      await projectSettings.getByLabel("Unbounded sequence size", { exact: true }).fill("22");
      await projectSettings.getByLabel("Unbounded string size", { exact: true }).fill("33");
      await projectSettings.getByRole("button", { name: "Apply", exact: true }).click();
      await settle(app);

      await app.getByRole("tab", { name: "main.idl", exact: true }).click({ button: "right" });
      await app.getByRole("menuitem", { name: "Properties…", exact: true }).click();
      const fileSettings = app.getByRole("dialog");
      await fileSettings.getByLabel("Unbounded sequence size", { exact: true }).fill("22");
      await fileSettings.getByLabel("Unbounded string size", { exact: true }).fill("33");
      await fileSettings.getByRole("button", { name: "Apply", exact: true }).click();
      await settle(app);

      await app.getByRole("tab", { name: "main.idl", exact: true }).click({ button: "right" });
      await app.getByRole("menuitem", { name: "Properties…", exact: true }).click();
      const unrelatedEdit = app.getByRole("dialog");
      assert(
        await unrelatedEdit
          .getByRole("button", { name: "Reset sequence size to project default", exact: true })
          .isVisible(),
        "An explicit sequence override equal to the project default did not offer Reset",
      );
      assert(
        await unrelatedEdit
          .getByRole("button", { name: "Reset string size to project default", exact: true })
          .isVisible(),
        "An explicit string override equal to the project default did not offer Reset",
      );
      await unrelatedEdit.getByLabel("Strict validation", { exact: true }).check();
      await unrelatedEdit.getByRole("button", { name: "Apply", exact: true }).click();
      await settle(app);

      await app.locator("#workspace-settings").click();
      const changedProject = app.getByRole("dialog");
      await changedProject.getByLabel("Unbounded sequence size", { exact: true }).fill("88");
      await changedProject.getByLabel("Unbounded string size", { exact: true }).fill("99");
      await changedProject.getByRole("button", { name: "Apply", exact: true }).click();
      await settle(app);
      const state = await saved(app);
      assert(
        state.files[0].options.sequenceSize === 22 && state.files[0].options.stringSize === 33,
        "Applying unrelated file properties removed an explicit override equal to the old project default",
      );
    },
  );

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
    await pathDialog(app, "third.idl");
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

  await check("IDL word navigation and IDL/C# folding work in the editors", async (app) => {
    await replaceText(app, "// alpha beta gamma");
    await app.locator("#source").click();
    await app.keyboard.press("ControlOrMeta+End");
    await app.keyboard.press("ControlOrMeta+ArrowLeft");
    await app.keyboard.insertText("delta ");
    await settle(app);
    assert(
      (await saved(app)).files[0].source.includes("alpha beta delta gamma"),
      "Ctrl+Left did not move by a word in the IDL editor",
    );
    await replaceText(app, "// alpha beta gamma");
    await app.locator("#source").click();
    await app.keyboard.press("ControlOrMeta+End");
    await app.keyboard.press("ControlOrMeta+Shift+ArrowLeft");
    await app.keyboard.press("ControlOrMeta+c");
    assert(
      (await app.evaluate(() => navigator.clipboard.readText())) === "gamma",
      "Ctrl+Shift+Left did not select the previous IDL word",
    );

    const idlSource = "module Folding {\nstruct Sample {\nlong value;\n};\n};";
    await app.evaluate((source) => navigator.clipboard.writeText(source), idlSource);
    await app.locator("#source").click();
    await app.keyboard.press("ControlOrMeta+a");
    await app.keyboard.press("ControlOrMeta+v");
    await app.locator('#file-list button:not([data-name$=".csproj"])').first().waitFor();
    await current(app);
    await app.locator("#source").hover({ position: { x: 38, y: 60 } });
    const idlFold = app.locator("#source .codicon-folding-expanded").first();
    await idlFold.waitFor().catch(async () => {
      throw new Error(`IDL folding control missing: ${await app.locator("#source .margin-view-overlays").innerHTML()}`);
    });
    await idlFold.click();
    await app.locator("#source .codicon-folding-collapsed").first().waitFor();

    const csharpFold = app.locator("#generated-panel .codicon-folding-expanded").first();
    await csharpFold.waitFor();
    await csharpFold.click();
    await app.locator("#generated-panel .codicon-folding-collapsed").first().waitFor();
  });

  return report;
};
