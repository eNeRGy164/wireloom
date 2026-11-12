import assert from "node:assert/strict";
import test from "node:test";
import { collectLeadingDocumentation, findDocumentationSymbol, formatDocumentation } from "./documentation.ts";

test("template-specialized struct declarations retain their hover documentation", () => {
  // Arrange
  const lines = [
    "/// @brief Provides the DDS topic type name for this generated value type.",
    "/** @see [RTI Modern C++ topic traits](https://example.test/traits) */",
    "/// @code",
    "/// auto name = dds::topic::topic_type_name<::Showcase::Reading>::value();",
    "/// @endcode",
    "template<>",
    "struct topic_type_name<::Showcase::Reading>",
    "{",
  ];

  // Act
  const symbol = findDocumentationSymbol(lines[6]);
  const docs = collectLeadingDocumentation((lineNumber) => lines[lineNumber - 1] ?? "", 6);
  const sections = formatDocumentation(docs, "cpp");

  // Assert
  assert.equal(symbol, "topic_type_name");
  assert.deepEqual(sections, [
    "Provides the DDS topic type name for this generated value type.",
    "**See also**\n\n[RTI Modern C++ topic traits](https://example.test/traits)",
    "```cpp\nauto name = dds::topic::topic_type_name<::Showcase::Reading>::value();\n```",
  ]);
});

test("declaration lookup recognizes common declaration forms and rejects unrelated lines", () => {
  // Arrange
  const lines = [
    "public int Value { get; }",
    "void Read()",
    "enum Mode",
    "namespace Example",
    "public int Count;",
    "}",
  ];

  // Act
  const symbols = lines.map(findDocumentationSymbol);

  // Assert
  assert.deepEqual(symbols, ["Value", "Read", "Mode", "Example", "Count", undefined]);
});

test("leading documentation crosses blank lines and stops at unrelated comments or code", () => {
  // Arrange
  const source = ["int unrelated;", "// ordinary note", "", "/// attached summary"];
  const getLine = (lineNumber) => source[lineNumber - 1] ?? "";

  // Act
  const docs = collectLeadingDocumentation(getLine, source.length);
  const ordinaryComment = collectLeadingDocumentation(getLine, 2);

  // Assert
  assert.deepEqual(docs, ["attached summary"]);
  assert.deepEqual(ordinaryComment, ["ordinary note"]);
});

test("Doxygen details, linked references, continuation lines, and code retain their order", () => {
  // Arrange
  const docs = [
    "@brief IDL member location.",
    "@details String metadata bound is 64 bytes, excluding the trailing",
    "null. The generated C++ field does not enforce this limit when assigned.",
    "@see https://example.test/strings",
    "RTI string bounds (7.7.0)",
    "@code",
    "auto sample = ::Hello::Reading{};",
    "sample.location.assign(64, 'a');",
    "@endcode",
  ];

  // Act
  const markdown = formatDocumentation(docs, "cpp");

  // Assert
  assert.deepEqual(markdown, [
    "IDL member location.",
    "String metadata bound is 64 bytes, excluding the trailing null. The generated C++ field does not enforce this limit when assigned.",
    "**See also**\n\n[RTI string bounds (7.7.0)](https://example.test/strings)",
    "```cpp\nauto sample = ::Hello::Reading{};\nsample.location.assign(64, 'a');\n```",
  ]);
});

test("Doxygen see links use descriptions as labels for Markdown and RTI legacy syntax", () => {
  // Arrange
  const docs = [
    "@see [RTI Modern C++ topic traits](https://example.test/topic-traits)",
    "/** @see https://example.test/type-support (RTI type support) */",
  ];

  // Act
  const sections = formatDocumentation(docs, "cpp");

  // Assert
  assert.deepEqual(sections, [
    "**See also**\n\n[RTI Modern C++ topic traits](https://example.test/topic-traits)",
    "**See also**\n\n[RTI type support](https://example.test/type-support)",
  ]);
});

test("C# XML code examples render as language-tagged code blocks", () => {
  // Arrange
  const docs = [
    "<summary>Initializes a copy of the specified instance.</summary>",
    '<param name="other">The instance to copy, or <see langword="null"/>.</param>',
    "<code>",
    "var original = new Reading();",
    "var copy = new Reading(original);",
    "</code>",
  ];

  // Act
  const markdown = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(markdown, [
    "Initializes a copy of the specified instance.",
    "**other:** The instance to copy, or `null`.",
    "```csharp\nvar original = new Reading();\nvar copy = new Reading(original);\n```",
  ]);
});

test("C# langword references remain visible as inline code", () => {
  // Arrange
  const docs = ['<summary>Returns <see langword="null"/> when no value is set.</summary>'];

  // Act
  const markdown = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(markdown, ["Returns `null` when no value is set."]);
});

test("C# hover sections render seealso links and remarks separately", () => {
  // Arrange
  const docs = [
    "<summary>Gets or sets the <c>location</c> member. Its maximum length is <c>64</c> bytes.</summary>",
    '<seealso href="https://example.test/strings">RTI string bounds</seealso>',
    "<remarks>The bound excludes the trailing <c>null</c> byte.</remarks>",
  ];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, [
    "Gets or sets the `location` member. Its maximum length is `64` bytes.",
    "**See also**\n\n[RTI string bounds](https://example.test/strings)",
    "**Remarks**\n\nThe bound excludes the trailing `null` byte.",
  ]);
});

test("C# parameter references, inline code, and return values remain explicit", () => {
  // Arrange
  const docs = [
    '<summary>Compares this instance with <paramref name="other"/>.</summary>',
    '<param name="other">The instance to compare. Pass <see langword="null"/> to compare against no value.</param>',
    '<returns><see langword="true"/> when values are equal; otherwise <see langword="false"/>.</returns>',
  ];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, [
    "Compares this instance with *other*.",
    "**other:** The instance to compare. Pass `null` to compare against no value.",
    "**Return value:** `true` when values are equal; otherwise `false`.",
  ]);
});

test("C# XML links, paragraphs, entities, values, and reference labels are retained", () => {
  // Arrange
  const docs = [
    '<summary>Use <see href="https://example.test/a (b)">the guide</see> &amp; <c>string.Empty</c>.<para>Second paragraph.</para></summary>',
    "<value>A &lt; B &gt; C &quot;quoted&quot; &apos;text&apos;.</value>",
    '<seealso cref="T:System.String">String type</seealso>',
    '<seealso cref="M:Example.Reader.Read">Read method</seealso>',
    "<seealso>Related API</seealso>",
    '<seealso href="https://example.test/unlabeled"></seealso>',
    '<seealso cref="T:System.Uri"></seealso>',
    "<example>Example source.</example>",
  ];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, [
    "Use [the guide](https://example.test/a%20%28b%29) & `string.Empty`.\n\nSecond paragraph.",
    "A &lt; B &gt; C \"quoted\" 'text'.",
    "**See also**\n\n`String type`",
    "**See also**\n\n`Read method`",
    "**See also**\n\nRelated API",
    "**See also**\n\n[https://example.test/unlabeled](https://example.test/unlabeled)",
    "**See also**\n\n`Uri`",
    "Example source.",
  ]);
});

test("escaped HTML in XML documentation stays text in hover Markdown", () => {
  // Arrange
  const docs = ["<summary>&lt;script&gt;alert(1)&lt;/script&gt;</summary>"];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, ["&lt;script&gt;alert(1)&lt;/script&gt;"]);
});

test("nested HTML-like text remains escaped instead of being stripped", () => {
  // Arrange
  const docs = ["<summary>&lt;scr&lt;b&gt;ipt&gt;alert(1)&lt;/script&gt;</summary>"];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, ["&lt;scr&lt;b&gt;ipt&gt;alert(1)&lt;/script&gt;"]);
});

test("C# see references without text retain their code or hyperlink target", () => {
  // Arrange
  const docs = [
    '<summary>See <see cref="T:System.String"/> and <see cref="T:System.String">String values</see>, <see cref="T:System.Boolean"></see>, <see href="https://example.test/unlabeled"></see>, and <see href="https://example.test/api"/>.</summary>',
    "<param>Unnamed parameter documentation.</param>",
  ];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, [
    "See `String` and `String values`, `Boolean`, [https://example.test/unlabeled](https://example.test/unlabeled), and [https://example.test/api](https://example.test/api).",
    "Unnamed parameter documentation.",
  ]);
});

test("C# examples preserve prose and render XML code as a language-tagged code block", () => {
  // Arrange
  const docs = [
    "<example>",
    "Create a sample and call <c>ToString</c>.",
    "<code>",
    "var sample = new Reading();",
    "if (sample.Value < 0 && sample.Name != null)",
    "    throw new InvalidOperationException();",
    "</code>",
    "Then inspect the generated value.",
    "<code></code>",
    "This is the end of the example.",
    "</example>",
  ];

  // Act
  const sections = formatDocumentation(docs, "csharp");

  // Assert
  assert.deepEqual(sections, [
    [
      "Create a sample and call `ToString`.",
      "```csharp\nvar sample = new Reading();\nif (sample.Value < 0 && sample.Name != null)\n    throw new InvalidOperationException();\n```",
      "Then inspect the generated value.",
      "This is the end of the example.",
    ].join("\n\n"),
  ]);
});

test("Doxygen parameters, exceptions, returns, and empty blocks use labeled sections", () => {
  // Arrange
  const docs = [
    "@param value Input value.",
    "@return The resulting value.",
    "@throws Error When the input is invalid.",
    "<summary></summary>",
    "@code",
    "@endcode",
    "",
  ];

  // Act
  const sections = formatDocumentation(docs, "cpp");

  // Assert
  assert.deepEqual(sections, [
    "**param:** value Input value.",
    "**Return value:** The resulting value.",
    "**throws:** Error When the input is invalid.",
  ]);
});

test("Doxygen code examples retain their language tag when the closing marker is missing", () => {
  // Arrange
  const docs = ["@code", "int answer = 42;"];

  // Act
  const sections = formatDocumentation(docs, "cpp");

  // Assert
  assert.deepEqual(sections, ["```cpp\nint answer = 42;\n```"]);
});
