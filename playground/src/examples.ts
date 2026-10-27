import { createFile, createWorkspace } from "./workspace.ts";

export const examples = [
  {
    id: "hello",
    title: "Hello, Wireloom",
    description: "A keyed temperature reading. Generate C# types and DDS support from a few lines of IDL.",
    source: `module Hello {
    struct Reading {
        @key long sensor_id;
        double temperature;
        string<64> location;
    };
};
`,
  },
  {
    id: "collections",
    title: "Aggregates & collections",
    description: "Enums, nested types, bounded sequences, strings, and arrays.",
    source: `module Geometry {
    enum Color { Red, Green, Blue };
    struct Point { long x; long y; };
    @appendable struct Palette {
        Color color;
        Point origin;
        sequence<string<16>, 4> names;
        long samples[3];
    };
};
`,
  },
  {
    id: "types",
    title: "Type showcase & XML docs",
    description:
      "Explore every supported primitive spelling, bounded/unbounded text and collections, defaults, ranges, aliases, unions, inheritance, and value types. Start with Showcase.PrimitiveTypes.g.cs, TextTypes.g.cs, CollectionTypes.g.cs, and RangesAndDefaults.g.cs.",
    source: `// Open the generated public C# types to explore their XML documentation.
// Numeric members describe their representable ranges. Explicit annotations
// add narrower ranges and defaults; constructors show default initialization.
// Unbounded IDL uses the Playground defaults: 100 sequence elements and
// 255 UTF-8 bytes (string) / UTF-16 code units (wstring). These are effective
// generation limits, not bounds written into the original IDL declaration.
module Showcase {
    const long TEXT_BOUND = 32;

    // Enums, scalar/text/collection aliases, and fixed-array aliases.
    enum Mode { Idle, @default_literal Ready, Busy };
    typedef long SensorId;
    @default(7) typedef long Seven;
    typedef string<TEXT_BOUND> Label;
    typedef wstring<TEXT_BOUND> WideLabel;
    typedef sequence<octet, 16> BoundedBytes;
    typedef sequence<long, 3> BoundedLongs;
    typedef sequence<long> UnboundedNumbers;
    typedef long Grid[2][3];
    typedef Mode ModeArray[3];

    struct PrimitiveTypes {
        short signed16;
        long signed32;
        long long signed64;
        unsigned short unsigned16;
        unsigned long unsigned32;
        unsigned long long unsigned64;
        // Fixed-width spellings map to the corresponding C# numeric types.
        int8 fixedInt8;
        int16 fixedInt16;
        int32 fixedInt32;
        int64 fixedInt64;
        uint8 fixedUint8;
        uint16 fixedUint16;
        uint32 fixedUint32;
        uint64 fixedUint64;
        octet rawByte;
        boolean enabled;
        char narrowCharacter;
        wchar wideCharacter;
        float singlePrecision;
        double doublePrecision;
        long double extendedPrecision;
    };

    struct TextTypes {
        string unboundedText;
        string<TEXT_BOUND> boundedText;
        wstring unboundedWideText;
        wstring<TEXT_BOUND> boundedWideText;
        @default("hello") string<TEXT_BOUND> greeting;
        @default(L"世界") wstring<TEXT_BOUND> wideGreeting;
        Label aliasedText;
        WideLabel aliasedWideText;
    };

    struct CollectionTypes {
        sequence<long, 4> boundedNumbers;
        sequence<long> unboundedNumbers;
        sequence<string<16>, 4> boundedTextItems;
        sequence<string> unboundedTextItems;
        sequence<wstring<16>, 4> boundedWideItems;
        sequence<wstring> unboundedWideItems;
        sequence<BoundedLongs, 2> nestedNumbers;
        octet fixedBytes[8];
        long matrix[2][3];
        string<16> textArray[3];
        wstring<16> wideTextArray[3];
        Grid aliasedMatrix;
        ModeArray enumArray;
        BoundedBytes aliasedBytes;
        UnboundedNumbers aliasedNumbers;
    };

    struct RangesAndDefaults {
        @unit("°C") @range(min = -40, max = 125) @default(20) long temperature;
        @min(0) @max(100) @default(50) long percentage;
        @default(true) boolean enabled;
        @default('A') char narrowCharacter;
        @default(L'Ω') wchar wideCharacter;
        @default(1.25) float singlePrecision;
        @default(2.5) double doublePrecision;
        Mode enumDefault;
        @default(Busy) Mode overriddenEnumDefault;
        Seven inheritedAliasDefault;
        @default(9) Seven overriddenAliasDefault;
    };

    struct Point { double x; double y; };

    @appendable struct Reading {
        @key SensorId sensorId;
        Point position;
        @optional string<TEXT_BOUND> note;
        @optional Point optionalPosition;
        sequence<Point, 4> boundedPoints;
        sequence<Point> unboundedPoints;
        Point fixedPoints[2];
        Mode currentMode;
    };
    struct NamedReading : Reading { Label label; };

    // An enum-discriminated union with primitive, alias, and struct branches.
    union Payload switch(Mode) {
        case Idle: long number;
        case Ready: Label text;
        default: Point position;
    };

    valuetype BaseValue { public long baseValue; };
    valuetype NamedValue : BaseValue { public Label label; };

    struct Envelope {
        PrimitiveTypes primitives;
        TextTypes text;
        CollectionTypes collections;
        RangesAndDefaults defaults;
        NamedReading reading;
        Payload payload;
        NamedValue value;
    };
};
`,
  },
  {
    id: "preprocessor",
    title: "Inline preprocessing",
    description: "Define a constant in the same file and use it as an array bound.",
    source: `#define SAMPLE_COUNT 3
struct Samples { long values[SAMPLE_COUNT]; };
`,
  },
  {
    id: "hash",
    title: "Hash member IDs",
    description: "Automatic member hashing and an explicit UTF-8 hash annotation.",
    source: `@autoid(HASH)
struct HashSample {
    long color;
    long shapesize;
    @hashid("é") long unicode;
};
`,
  },
  {
    id: "profiles",
    title: "Compare RTI profiles",
    description: "@must_understand succeeds with 7.7.0. Switch to 7.3.1 to see the profile diagnostic.",
    source: `struct MustUnderstand {
    @must_understand long value;
};
`,
  },
  {
    id: "unsupported",
    title: "Explore a diagnostic",
    description: "Recursive sequence typedefs are unsupported. Follow the diagnostic back to the IDL.",
    source: `struct Node;
typedef sequence<Node> Children;
struct Node { long value; Children children; };
`,
  },
  {
    id: "workspace-includes",
    title: "Workspace: Root and includes",
    description:
      "A Root includes a shared type using a relative virtual path. Inspect each file's generation role in Properties.",
    source: '#include "shared/types.idl"\nmodule Example { struct Reading { Shared::Point position; }; };\n',
    workspace: () => {
      const value = createWorkspace(
        '#include "shared/types.idl"\nmodule Example { struct Reading { Shared::Point position; }; };\n',
      );
      value.folders = ["/workspace/shared"];
      const shared = createFile(
        "/workspace/shared/types.idl",
        "#pragma once\nmodule Shared { struct Point { long x; long y; }; };\n",
      );
      shared.isGenerationRoot = false;
      value.files.push(shared);
      return value;
    },
  },
  {
    id: "workspace-preprocessor",
    title: "Workspace: preprocessor settings",
    description:
      "Workspace Settings defines FEATURE and removes DISABLED. An angle include searches the selected virtual include folder.",
    source:
      "#include <types.idl>\n#if defined(FEATURE) && !defined(DISABLED)\nstruct Enabled { Config::Value value; };\n#endif\n",
    workspace: () => {
      const value = createWorkspace(
        "#include <types.idl>\n#if defined(FEATURE) && !defined(DISABLED)\nstruct Enabled { Config::Value value; };\n#endif\n",
      );
      value.folders = ["/workspace/config"];
      const shared = createFile(
        "/workspace/config/types.idl",
        "#pragma once\nmodule Config { struct Value { long count; }; };\n",
      );
      shared.isGenerationRoot = false;
      value.files.push(shared);
      value.options.defines = ["FEATURE=1", "DISABLED=1"];
      value.options.undefines = ["DISABLED"];
      value.options.includeDirectories = ["/workspace/config"];
      return value;
    },
  },
];
