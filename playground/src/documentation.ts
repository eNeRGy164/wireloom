/** Converts generated XML and Doxygen documentation into independently rendered hover sections. */
export function formatDocumentation(docs: readonly string[], language: string): string[] {
  const source = docs.join("\n");
  const xmlSections = formatXmlDocumentation(source, language);
  return xmlSections.length ? xmlSections : formatDoxygenDocumentation(docs, language);
}

/** Finds the name whose declaration is on a source line. */
export function findDocumentationSymbol(line: string): string | undefined {
  return (
    /\b(?:class|struct)\s+(?:(?:NDDSUSERDllExport|final|sealed)\s+)*([A-Za-z_]\w*)/.exec(line)?.[1] ??
    /\b([A-Za-z_]\w*)\s*\{\s*(?:get|set|init)\b/.exec(line)?.[1] ??
    /\b([A-Za-z_]\w*)\s*\(/.exec(line)?.[1] ??
    /\b(?:enum\s+(?:class\s+)?|namespace\s+|interface\s+)([A-Za-z_]\w*)/.exec(line)?.[1] ??
    /\b([A-Za-z_]\w*)\s*(?:\[[^\]]*\]\s*)?(?:=|;)/.exec(line)?.[1]
  );
}

/** Collects documentation attached to a declaration, crossing its template header. */
export function collectLeadingDocumentation(
  getLineContent: (lineNumber: number) => string,
  precedingLineNumber: number,
): string[] {
  const docs: string[] = [];
  for (let number = precedingLineNumber; number > 0; number--) {
    const previous = getLineContent(number).trim();
    if (!previous) {
      if (docs.length) break;
      continue;
    }
    if (/^template\s*</.test(previous) || /^\[[\w.(), ="'<>]+\]$/.test(previous)) continue;
    if (previous.startsWith("/**")) {
      docs.unshift(
        previous
          .replace(/^\/\*\*\s?/, "")
          .replace(/\*\/$/, "")
          .trim(),
      );
      continue;
    }
    if (!previous.startsWith("///") && !previous.startsWith("//")) break;
    docs.unshift(previous.replace(/^\/\/\/?\s?/, ""));
  }

  return docs;
}

function formatXmlDocumentation(source: string, language: string): string[] {
  const sections: string[] = [];
  const blocks = /<(summary|remarks|returns|param|seealso|value|example|code)\b([^>]*)>([\s\S]*?)<\/\1\s*>/gi;
  let match: RegExpExecArray | null;

  while ((match = blocks.exec(source))) {
    const [, tag, attributes, rawContent] = match;
    const content = formatInlineXml(rawContent).trim();
    if (!content && tag.toLowerCase() !== "seealso") continue;

    switch (tag.toLowerCase()) {
      case "summary":
      case "value":
        sections.push(content);
        break;
      case "param": {
        const name = /\bname\s*=\s*"([^"]+)"/i.exec(attributes)?.[1];
        sections.push(name ? `**${formatInlineXml(name)}:** ${content}` : content);
        break;
      }
      case "returns":
        sections.push(`**Return value:** ${content}`);
        break;
      case "remarks":
        sections.push(`**Remarks**\n\n${content}`);
        break;
      case "seealso":
        sections.push(`**See also**\n\n${formatSeeAlso(attributes, rawContent)}`);
        break;
      case "example":
        sections.push(formatExample(rawContent, language));
        break;
      case "code":
        sections.push(`\`\`\`${language}\n${decodeXml(rawContent).trim()}\n\`\`\``);
        break;
    }
  }

  return sections;
}

function formatExample(value: string, language: string): string {
  const sections: string[] = [];
  const codeBlocks = /<code\b[^>]*>([\s\S]*?)<\/code\s*>/gi;
  let cursor = 0;
  let match: RegExpExecArray | null;

  while ((match = codeBlocks.exec(value))) {
    const prose = formatInlineXml(value.slice(cursor, match.index)).trim();
    if (prose) sections.push(prose);
    const code = decodeXml(match[1]).trim();
    if (code) sections.push(`\`\`\`${language}\n${code}\n\`\`\``);
    cursor = codeBlocks.lastIndex;
  }

  const trailingProse = formatInlineXml(value.slice(cursor)).trim();
  if (trailingProse) sections.push(trailingProse);
  return sections.join("\n\n");
}

function formatSeeAlso(attributes: string, rawContent: string): string {
  const label = formatInlineXml(rawContent).trim();
  const href = /\bhref\s*=\s*"([^"]+)"/i.exec(attributes)?.[1];
  if (href) return `[${label || decodeXml(href)}](${encodeMarkdownUrl(decodeXml(href))})`;

  const cref = /\bcref\s*=\s*"([^"]+)"/i.exec(attributes)?.[1];
  if (!cref) return label;
  const reference = decodeXml(cref).replace(/^[A-Z]:/, "");
  return `\`${label || reference.split(/[.#]/).at(-1)}\``;
}

function formatInlineXml(value: string): string {
  const formatted = value
    .replace(/<c\b[^>]*>([\s\S]*?)<\/c>/gi, (_match, code: string) => `\`${code.trim()}\``)
    .replace(/<paramref\b[^>]*\bname\s*=\s*"([^"]+)"[^>]*\s*\/?\s*>/gi, (_match, name: string) => `*${name}*`)
    .replace(/<see\b[^>]*\blangword\s*=\s*"([^"]+)"[^>]*\s*\/?\s*>/gi, (_match, word: string) => `\`${word}\``)
    .replace(
      /<see\b(?![^>]*\/\s*>)[^>]*\bhref\s*=\s*"([^"]+)"[^>]*>([\s\S]*?)<\/see>/gi,
      (_match, href: string, text: string) => {
        const label = formatInlineXml(text).trim() || href;
        return `[${label}](${encodeMarkdownUrl(decodeXml(href))})`;
      },
    )
    .replace(
      /<see\b(?![^>]*\/\s*>)[^>]*\bcref\s*=\s*"([^"]+)"[^>]*>([\s\S]*?)<\/see>/gi,
      (_match, cref: string, text: string) => {
        const label =
          formatInlineXml(text).trim() ||
          cref
            .replace(/^[A-Z]:/, "")
            .split(/[.#]/)
            .at(-1);
        return `\`${label}\``;
      },
    )
    .replace(/<see\b[^>]*\bcref\s*=\s*"([^"]+)"[^>]*\s*\/?\s*>/gi, (_match, cref: string) => {
      const label = cref
        .replace(/^[A-Z]:/, "")
        .split(/[.#]/)
        .at(-1);
      return `\`${label}\``;
    })
    .replace(
      /<see\b[^>]*\bhref\s*=\s*"([^"]+)"[^>]*\s*\/?\s*>/gi,
      (_match, href: string) => `[${decodeXml(href)}](${encodeMarkdownUrl(decodeXml(href))})`,
    )
    .replace(/<br\s*\/?\s*>/gi, "  \n")
    .replace(/<\/?para\b[^>]*>/gi, "\n\n")
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/&quot;/g, '"')
    .replace(/&apos;/g, "'")
    .replace(/&amp;/g, "&");

  return formatted.replace(/[<>]/g, (character) => (character === "<" ? "&lt;" : "&gt;"));
}

function encodeMarkdownUrl(value: string): string {
  return value.replace(/\(/g, "%28").replace(/\)/g, "%29").replace(/\s/g, "%20");
}

function decodeXml(value: string): string {
  return value
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/&quot;/g, '"')
    .replace(/&apos;/g, "'")
    .replace(/&amp;/g, "&");
}

function formatDoxygenDocumentation(docs: readonly string[], language: string): string[] {
  const sections: string[] = [];
  const code: string[] = [];
  let activeParagraph = -1;
  let inCode = false;

  const appendCode = () => {
    if (code.length) sections.push(`\`\`\`${language}\n${code.join("\n")}\n\`\`\``);
    code.length = 0;
    inCode = false;
    activeParagraph = -1;
  };

  for (let docIndex = 0; docIndex < docs.length; docIndex++) {
    const doc = docs[docIndex];
    if (/^\s*<(summary|remarks|returns|param|seealso|value|example|code)\b[^>]*>\s*<\/\1\s*>\s*$/i.test(doc)) continue;
    const openingCode = /(?:@code\b|<code\b[^>]*>)/i.exec(doc);
    const closingCode = /(?:@endcode\b|<\/code>)/i.exec(doc);
    if (inCode || openingCode) {
      const start = openingCode ? openingCode.index + openingCode[0].length : 0;
      const end = closingCode && closingCode.index >= start ? closingCode.index : doc.length;
      const codeLine = decodeXml(doc.slice(start, end));
      if (codeLine.trim()) code.push(codeLine);
      if (closingCode) appendCode();
      else inCode = true;
      continue;
    }

    const clean = formatInlineXml(doc).trim();
    if (/^@brief\b/.test(clean)) {
      sections.push(clean.replace(/^@brief\s*/, ""));
      activeParagraph = sections.length - 1;
    } else if (/^@details\b/.test(clean)) {
      sections.push(clean.replace(/^@details\s*/, ""));
      activeParagraph = sections.length - 1;
    } else if (/^(?:\/\*\*?\s*)?\**\s*@see\b/.test(clean)) {
      const target = clean
        .replace(/^(?:\/\*\*?\s*)?\**\s*@see\s*/, "")
        .replace(/\s*\*\/$/, "")
        .trim();
      const markdownLink = /^\[([^\]]+)\]\((.+)\)$/.exec(target);
      const legacyLink = /^(https?:\/\/\S+?)(?:\s+\((.*)\))?$/.exec(target);
      let link: string;
      if (markdownLink) {
        link = `[${markdownLink[1]}](${encodeMarkdownUrl(markdownLink[2])})`;
      } else if (legacyLink) {
        let label = legacyLink[2];
        if (!label && docs[docIndex + 1] && !/^\s*(?:@|\/\*|\*\/)/.test(docs[docIndex + 1].trim())) {
          label = docs[++docIndex].trim();
        }
        link = `[${label || legacyLink[1]}](${encodeMarkdownUrl(legacyLink[1])})`;
      } else {
        link = `\`${target}\``;
      }
      sections.push(`**See also**\n\n${link}`);
      activeParagraph = -1;
    } else if (/^@(?:param|return|returns|throws)\b/.test(clean)) {
      const formatted = clean.replace(/^@(param|return|returns|throws)\s*/, (_match, kind: string) =>
        kind === "return" || kind === "returns" ? "**Return value:** " : `**${kind}:** `,
      );
      sections.push(formatted);
      activeParagraph = sections.length - 1;
    } else if (clean) {
      if (clean.startsWith("**") || activeParagraph < 0) {
        sections.push(clean);
        activeParagraph = sections.length - 1;
      } else sections[activeParagraph] += ` ${clean}`;
    }
  }

  if (inCode) appendCode();
  return sections;
}
