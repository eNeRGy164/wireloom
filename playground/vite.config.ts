import { defineConfig } from "vite";
import { readFileSync } from "node:fs";
import { execFileSync } from "node:child_process";

const props = readFileSync(new URL("../Directory.Build.props", import.meta.url), "utf8");
const version = props.match(/<WireloomVersion>([^<]+)<\/WireloomVersion>/)?.[1];
const head = execFileSync("git", ["rev-parse", "--short", "HEAD"], { encoding: "utf8" }).trim();
const dirty = execFileSync("git", ["status", "--porcelain"], { encoding: "utf8" }).trim().length > 0;
const revision = `${head}${dirty ? "-dirty" : ""}`;

export default defineConfig({
  base: process.env.VITE_BASE_PATH ?? "/",
  define: { __WIRELOOM_VERSION__: JSON.stringify(version), __SOURCE_REVISION__: JSON.stringify(revision) },
  plugins: [
    {
      name: "third-party-notices",
      generateBundle() {
        const licenses = [
          "monaco-editor/LICENSE",
          "monaco-editor/ThirdPartyNotices.txt",
          "fflate/LICENSE",
          "marked/LICENSE.md",
          "dompurify/LICENSE",
        ];
        const source = licenses
          .map(
            (path) =>
              `${path}\n${"=".repeat(72)}\n${readFileSync(new URL(`node_modules/${path}`, import.meta.url), "utf8")}`,
          )
          .join("\n\n");
        this.emitFile({ type: "asset", fileName: "third-party-notices.txt", source });
      },
    },
  ],
});
