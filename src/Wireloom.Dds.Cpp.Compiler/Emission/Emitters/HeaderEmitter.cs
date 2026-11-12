using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Coordinates the public header's declarations and RTI traits in source order.</summary>
internal static class HeaderEmitter
{
    /// <summary>Renders the public C++ header for a generation plan.</summary>
    internal static string Emit(GenerationPlan plan)
    {
        var source = new SourceWriter(plan.Profile.RtiVersion);
        WritePreamble(source, plan);
        EmitDeclarations(source, plan);
        EmitRtiTraits(source, plan);
        return source.ToString();
    }

    private static void WritePreamble(SourceWriter source, GenerationPlan plan)
    {
        EmitBanner(source, plan);
        source.WriteLine("#pragma once");
        source.BlankLine();
        source.WriteComment("Standard library");
        source.WriteInclude("cstdint");
        source.WriteInclude("iosfwd");
        source.WriteInclude("string");
        source.WriteInclude("vector");

        if (plan.Structs.SelectMany(declaration => declaration.Members).Any(member => member.IsExternal))
        {
            source.WriteInclude("dds/core/types.hpp");
            source.WriteInclude("dds/core/External.hpp");
        }

        if (plan.Structs.SelectMany(declaration => declaration.Members).Any(member => member.IsOptional))
        {
            source.WriteInclude("dds/core/Optional.hpp");
        }

        if (plan.Structs.Any(declaration => RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration)
            || declaration.HasSelfRecursiveSequence))
        {
            source.WriteInclude(plan.Profile.RtiVersion == "7.7.0"
                ? "ndds/xcdr/xcdr_typeCode.h"
                : "ndds/cdr/cdr_typeCode.h");
        }

        var hasSequences = plan.Structs.SelectMany(declaration => declaration.Members).Select(member => member.Type)
                .Concat(plan.Unions.SelectMany(declaration => declaration.Branches).Select(branch => branch.Type))
                .Concat(plan.Aliases.Select(alias => alias.ResolvedType))
            .Any(type => type.ContainsSequence);
        if (hasSequences && plan.Profile.RtiVersion == "6.1.2")
        {
            source.WriteInclude("rti/core/BoundedSequence.hpp");
        }
        else if (hasSequences)
        {
            source.WriteInclude("omg/types/sequence.hpp");
        }

        if (plan.Structs.SelectMany(declaration => declaration.Members).Select(member => member.Type)
                .Concat(plan.Unions.SelectMany(declaration => declaration.Branches).Select(branch => branch.Type))
            .Any(type => type.ContainsArray))
        {
            source.WriteInclude("dds/core/array.hpp");
        }

        if (hasSequences && plan.Profile.RtiVersion == "6.1.2")
        {
            source.BlankLine();
            source.WriteComment("RTI 6.1.2 collection compatibility aliases");
            source.WriteLines("""
                namespace omg { namespace types {
                template <typename T>
                using sequence = ::std::vector<T>;
                template <typename T, ::std::size_t N>
                using bounded_sequence = ::rti::core::bounded_sequence<T, N>;
                } }
                """);
        }

        source.WriteInclude("rti/topic/TopicTraits.hpp");
        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("rti/core/Exception.hpp");
            source.WriteInclude("rti/topic/flat/FlatData.hpp");
        }
        source.BlankLine();
        source.WriteComment("RTI Connext DDS Modern C++ value support");
        if (plan.Unions.Count > 0)
        {
            source.WriteInclude("dds/core/Exception.hpp");
        }

        source.WriteInclude("rti/core/LongDouble.hpp");
        source.WriteLine("#ifndef NDDS_STANDALONE_TYPE");
        source.WriteComment("RTI Connext DDS Modern C++ API");
        source.WriteInclude("dds/dds.hpp");
        source.WriteInclude("dds/core/xtypes/EnumType.hpp");

        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("rti/xcdr/Interpreter.hpp");
        }

        if (plan.Aliases.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/AliasType.hpp");
        }

        if (plan.Unions.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/UnionType.hpp");
        }

        source.WriteLine("#endif // NDDS_STANDALONE_TYPE");

        if (plan.Profile.RtiVersion != "6.1.2"
            && plan.Unions.SelectMany(declaration => declaration.Branches)
            .Any(branch => branch.Type is MemberTypePlan.String))
        {
            source.WriteInclude("omg/types/string_view.hpp");
        }
        source.BlankLine();
        if (plan.Structs.Any(declaration => declaration.HasSelfRecursiveSequence
            || RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration)))
        {
            source.WriteLine(plan.Profile.RtiVersion == "7.7.0"
                ? "struct RTIXCdrResourceNode;"
                : "struct RTICdrResourceNode;");
            source.BlankLine();
        }

        source.WriteLines("""
            #if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
            #undef NDDSUSERDllExport
            #define NDDSUSERDllExport __declspec(dllexport)
            #endif
            """);
        source.BlankLine();
        source.WriteLines("""
            #if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
            #undef NDDSUSERDllExport
            #define NDDSUSERDllExport __attribute__((visibility("default")))
            #endif
            """);
    }

    private static void EmitDeclarations(SourceWriter source, GenerationPlan plan)
    {
        IReadOnlyList<string> openNamespaces = Array.Empty<string>();

        void SwitchNamespaces(IReadOnlyList<string> requested)
        {
            var commonLength = 0;
            while (commonLength < openNamespaces.Count
                && commonLength < requested.Count
                && string.Equals(openNamespaces[commonLength], requested[commonLength], StringComparison.Ordinal))
            {
                commonLength++;
            }

            source.CloseNamespaces(openNamespaces.Skip(commonLength).ToArray());
            var next = requested.Skip(commonLength).ToArray();
            source.OpenNamespaces(next);
            openNamespaces = requested.ToArray();
        }

        foreach (var constants in plan.Constants.Where(constant => !constant.IsEnum)
                     .GroupBy(constant => constant.Namespace, StringComparer.Ordinal))
        {
            source.BlankLine();

            var first = constants.First();
            SwitchNamespaces(first.NamespaceSegments);
            source.WriteComment("IDL constants.");

            foreach (var constant in constants)
            {
                var storageType = CppTypeNameFormatter.MakeRelative(constant.StorageType, constant.NamespaceSegments);
                var valueExpression = CppTypeNameFormatter.MakeRelative(constant.ValueExpression, constant.NamespaceSegments);
                source.WriteDoxygenBrief($"IDL constant {constant.Name}.");
                source.WriteDoxygenDetails($"C++ type: {storageType}; value: {valueExpression}.");
                source.WriteLine($"constexpr {storageType} {constant.Name} = {valueExpression};");
            }

        }

        foreach (var declaration in plan.Enums)
        {
            source.BlankLine();
            SwitchNamespaces(declaration.NamespaceSegments);
            ValueDeclarationEmitter.EmitEnum(source, declaration);
        }

        foreach (var constants in plan.Constants.Where(constant => constant.IsEnum)
                     .GroupBy(constant => constant.Namespace, StringComparer.Ordinal))
        {
            source.BlankLine();
            var first = constants.First();
            SwitchNamespaces(first.NamespaceSegments);
            source.WriteComment("Enum constants.");

            foreach (var constant in constants)
            {
                var storageType = CppTypeNameFormatter.MakeRelative(constant.StorageType, constant.NamespaceSegments);
                var valueExpression = CppTypeNameFormatter.MakeRelative(constant.ValueExpression, constant.NamespaceSegments);
                source.WriteDoxygenBrief($"IDL constant {constant.Name}.");
                source.WriteDoxygenDetails($"C++ type: {storageType}; value: {valueExpression}.");
                source.WriteLine($"constexpr {storageType} {constant.Name} = {valueExpression};");
            }

        }

        if (plan.Aliases.Count > 0)
        {
            var forwardDeclarations = plan.Aliases
                .Where(alias => alias.StructTargetType is not null)
                .Select(alias => plan.Structs.Single(declaration => declaration.CppType == $"::{alias.StructTargetType}"))
                .DistinctBy(declaration => declaration.CppType, StringComparer.Ordinal);
            foreach (var declaration in forwardDeclarations)
            {
                source.BlankLine();
                SwitchNamespaces(declaration.NamespaceSegments);
                source.WriteLine($"struct {declaration.Name};");
            }

            foreach (var declaration in plan.Unions.Where(declaration =>
                         plan.Aliases.Any(alias => ReferencesUnion(alias.ResolvedType, declaration.QualifiedName))))
            {
                source.BlankLine();
                SwitchNamespaces(declaration.NamespaceSegments);
                source.WriteLine($"class {declaration.Name};");
            }

            foreach (var alias in plan.Aliases)
            {
                source.BlankLine();
                SwitchNamespaces(alias.NamespaceSegments);

                source.WriteDoxygenBrief($"IDL typedef {alias.QualifiedName}.");
                var details = $"C++ storage type is {alias.TargetType}. "
                    + "RTI DynamicType preserves this typedef identity with AliasType metadata.";

                if (FindStringType(alias.MetadataTargetType) is { } stringType)
                {
                    var stringKind = stringType.IsWide ? "wide string" : "string";
                    details += stringType.MetadataBound == 0
                        ? $" The effective {stringKind} metadata bound is RTIXCdrLong_MAX (unbounded)."
                        : $" The effective {stringKind} metadata bound is {stringType.MetadataBound}.";
                }

                source.WriteDoxygenWrapped("@details", details);
                RtiDocumentation.WriteApiSee(source,
                    "RTI Modern C++ AliasType",
                    "classdds_1_1core_1_1xtypes_1_1AliasType.html");
                var targetType = CppTypeNameFormatter.MakeRelative(alias.TargetType, alias.NamespaceSegments);
                source.WriteLine($"using {alias.Name} = {targetType};");
                source.WriteDoxygenBrief($"RTI DynamicType identity for IDL typedef {alias.QualifiedName}.");
                source.WriteLine($"struct {alias.Name}_AliasTag_t {{}};");
            }
        }

        foreach (var entry in plan.Initialization.TypeInitializationOrder)
        {
            if (entry.Struct is { } structure)
            {
                source.BlankLine();
                SwitchNamespaces(structure.NamespaceSegments);
                ValueDeclarationEmitter.EmitStruct(source, structure);
            }
            else if (entry.Union is { } union)
            {
                source.BlankLine();
                SwitchNamespaces(union.NamespaceSegments);
                ValueDeclarationEmitter.EmitUnion(source, union, plan.Profile.RtiVersion == "6.1.2");
            }
        }

        source.CloseNamespaces(openNamespaces);
    }

    private static void EmitRtiTraits(SourceWriter source, GenerationPlan plan)
    {
        source.BlankLine();
        TypeTraitsEmitter.EmitDefaultEnumeratorTraits(source, plan);
        source.WriteLine("#ifndef NDDS_STANDALONE_TYPE");
        source.WriteComment("Register the generated value types with RTI's Modern C++ API.");

        foreach (var declaration in plan.Structs)
        {
            if (declaration.IsTopic)
            {
                source.WriteLine($"REGISTER_TOPIC_TYPE({declaration.CppType})");

                if (declaration.IsFlatData)
                {
                    source.WriteLine($"REGISTER_TOPIC_TYPE({declaration.FlatDataPlainHelperType})");
                }
            }
        }

        foreach (var declaration in plan.Unions)
        {
            source.WriteLine($"REGISTER_TOPIC_TYPE({declaration.CppType})");
        }

        source.BlankLine();
        source.WriteComment("DDS topic traits");
        source.OpenNamespaces("dds");
        source.OpenNamespaces("topic");

        foreach (var declaration in plan.Structs)
        {
            if (declaration.IsFlatData)
            {
                FlatDataHeaderEmitter.EmitFlatDataTopicTraits(source, declaration);
            }
            else
            {
                TypeTraitsEmitter.EmitTopicTraits(source, declaration);
            }
        }

        foreach (var declaration in plan.Unions)
        {
            TypeTraitsEmitter.EmitTopicTraits(source, declaration);
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("dds");

        foreach (var declaration in plan.Structs.Where(declaration => declaration.IsFlatData))
        {
            FlatDataHeaderEmitter.EmitFlatDataOnlyTrait(source, declaration);
        }

        source.BlankLine();
        source.WriteComment("RTI DynamicType traits");
        source.OpenNamespaces("rti");
        source.OpenNamespaces("topic");

        foreach (var declaration in plan.Enums)
        {
            TypeTraitsEmitter.EmitDynamicTypeTraits(source, declaration);
        }

        foreach (var declaration in plan.Structs)
        {
            TypeTraitsEmitter.EmitDynamicTypeTraits(source, declaration);
        }

        foreach (var declaration in plan.Unions)
        {
            TypeTraitsEmitter.EmitDynamicTypeTraits(source, declaration);
        }

        foreach (var declaration in plan.Aliases)
        {
            TypeTraitsEmitter.EmitDynamicTypeTraits(source, declaration);
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("rti");

        foreach (var declaration in plan.Structs.Where(declaration => declaration.IsFlatData))
        {
            FlatDataHeaderEmitter.EmitFlatDataTraits(source, declaration);
            source.WriteLine("namespace rti { namespace xcdr {");
            source.WriteLine("template<> struct type_programs<" + declaration.CppType + ">");
            source.OpenBrace();
            source.WriteLine("static RTIXCdrInterpreterPrograms* get();");
            source.CloseBlock(";");
            source.WriteLine("} } // namespace rti::xcdr");
        }

        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("rti/flat/FlatSampleImpl.hpp");
        }

        source.WriteLine("#endif // NDDS_STANDALONE_TYPE");
        source.BlankLine();
        source.WriteLines(
            """
            #if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
            #undef NDDSUSERDllExport
            #define NDDSUSERDllExport
            #endif
            """);
    }

    private static MemberTypePlan.String? FindStringType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.String stringType => stringType,
        MemberTypePlan.Alias alias => FindStringType(alias.Target),
        MemberTypePlan.Sequence sequence => FindStringType(sequence.ElementType),
        MemberTypePlan.Array array => FindStringType(array.ElementType),
        _ => null,
    };

    private static void EmitBanner(SourceWriter source, GenerationPlan plan)
    {
        source.WriteComment("<auto-generated/>");
        source.WriteComment($"Generated by Wireloom from '{plan.FileStem}.idl'.");
        source.WriteComment("Do not edit this file directly. Update the IDL source and rebuild.");
        source.WriteComment("Requires the RTI Connext DDS Modern C++ runtime.");
    }

    private static bool ReferencesUnion(MemberTypePlan type, string qualifiedName) => type switch
    {
        MemberTypePlan.Union union => string.Equals(union.QualifiedName, qualifiedName, StringComparison.Ordinal),
        MemberTypePlan.Alias alias => ReferencesUnion(alias.Target, qualifiedName),
        _ => false,
    };
}
