import { strToU8, zipSync } from "fflate";
import type { Workspace } from "./workspace";

/** Save exact source text, workspace settings, and its generated project preview. */
export function sourceArchive(workspace: Workspace, projectFile: string) {
  const entries: Record<string, Uint8Array> = Object.create(null);
  entries["workspace.json"] = strToU8(JSON.stringify(workspace, null, 2) + "\n");
  entries["workspace/"] = new Uint8Array();
  entries["workspace/Wireloom.csproj"] = strToU8(projectFile);
  for (const folder of workspace.folders) entries[`${folder.slice(1)}/`] = new Uint8Array();
  for (const file of workspace.files) entries[file.path.slice(1)] = strToU8(file.source);
  return zipSync(entries, { level: 0 });
}
