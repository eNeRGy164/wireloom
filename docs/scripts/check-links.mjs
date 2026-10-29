import { execFileSync } from "node:child_process";
import { readFile, stat } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const root = fileURLToPath(new URL("../../", import.meta.url));
const files = [
  ...new Set(
    execFileSync("git", ["ls-files", "-z", "--cached", "--others", "--exclude-standard", "--", "*.md"], {
      cwd: root,
      encoding: "utf8",
    })
      .split("\0")
      .filter(Boolean),
  ),
];
const documents = new Map();
for (const file of files) {
  try {
    documents.set(file, await readFile(path.join(root, file), "utf8"));
  } catch (error) {
    if (error.code !== "ENOENT") throw error;
  }
}

function slug(text) {
  // Compute an anchor lookup key only; the result is never rendered as HTML.
  let headingText = text.toLowerCase();
  let previous;
  do {
    previous = headingText;
    headingText = headingText.replace(/<[^>]*>/g, "");
  } while (headingText !== previous);

  return headingText
    .replace(/\\([\\`*_{}\[\]()#+.!-])/g, "$1")
    .replace(/[^\p{L}\p{N}_\-\s]/gu, "")
    .replace(/\s/g, "-");
}

function anchors(text) {
  const result = new Set();
  const counts = new Map();
  let fenced = false;
  for (const line of text.split(/\r?\n/)) {
    if (/^\s*(\x60{3,}|~{3,})/.test(line)) {
      fenced = !fenced;
      continue;
    }
    if (fenced) continue;
    const heading = line.match(/^#{1,6}\s+(.+?)\s*#*$/);
    if (heading) {
      const base = slug(heading[1]);
      const count = counts.get(base) ?? 0;
      counts.set(base, count + 1);
      result.add(base + (count ? "-" + count : ""));
    }
    for (const match of line.matchAll(/<(?:a|h[1-6])\b[^>]*\b(?:id|name)=["']([^"']+)["']/g)) {
      result.add(match[1]);
    }
  }
  return result;
}

const problems = [];
let checked = 0;
async function check(file, line, original, rawCapture = false) {
  let target = original.replace(/^<|>$/g, "");
  const repositoryUrl = target.match(/^https:\/\/github\.com\/eNeRGy164\/wireloom\/(?:blob|tree)\/main\/(.*)$/i);
  if (repositoryUrl) target = "/" + repositoryUrl[1];
  else if (/^[a-z][a-z\d+.-]*:|^\/\//i.test(target)) return;

  const [pathname, fragment] = target.split("#");
  const relative = decodeURIComponent(pathname.split("?")[0]);
  const absolute = relative.startsWith("/")
    ? path.join(root, relative.slice(1))
    : path.resolve(path.dirname(path.join(root, file)), relative || path.basename(file));
  checked++;
  try {
    const info = await stat(absolute);
    if (!rawCapture && fragment && info.isFile() && /\.md$/i.test(absolute)) {
      const text = await readFile(absolute, "utf8");
      if (!anchors(text).has(decodeURIComponent(fragment))) {
        problems.push(file + ":" + line + ": missing anchor: " + original);
      }
    }
  } catch (error) {
    if (error.code !== "ENOENT") throw error;
    problems.push(file + ":" + line + ": missing path: " + original);
  }
}

for (const [file, text] of documents) {
  const definitions = new Map(
    [...text.matchAll(/^\s*\[([^\]]+)\]:\s*(\S+)/gm)].map((match) => [match[1].toLowerCase(), match[2]]),
  );
  let fenced = false;
  for (const [index, line] of text.split(/\r?\n/).entries()) {
    if (/^\s*(\x60{3,}|~{3,})/.test(line)) {
      fenced = !fenced;
      continue;
    }
    if (fenced) continue;
    const targets = [...line.matchAll(/!?\[[^\]]*\]\((<[^>]+>|[^\s)]+)(?:\s+["'][^)]*)?\)/g)].map((match) => match[1]);
    for (const match of line.matchAll(/\[[^\]]+\]\[([^\]]+)\]/g)) {
      const target = definitions.get(match[1].toLowerCase());
      if (target) targets.push(target);
      else problems.push(file + ":" + (index + 1) + ": missing reference: " + match[1]);
    }
    if (/^\s*\[[^\]]+\]:/.test(line)) {
      targets.push(line.replace(/^\s*\[[^\]]+\]:\s*/, "").split(/\s/)[0]);
    }
    for (const match of line.matchAll(/<(?:img|a)\b[^>]*\b(?:src|href)=["']([^"']+)["']/g)) {
      targets.push(match[1]);
    }
    for (const target of targets) await check(file, index + 1, target);
    const capture = line.match(/Raw capture: \x60([^\x60]+)\x60/);
    if (capture) await check(file, index + 1, capture[1], true);
  }
}

if (problems.length) {
  for (const problem of problems) console.error(problem);
  process.exitCode = 1;
} else {
  console.log("Checked " + checked + " local targets across " + documents.size + " Markdown files.");
}
