using System.Globalization;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits public C++ enum, struct, and union declarations and their API documentation.</summary>
internal static class ValueDeclarationEmitter
{
    /// <summary>Writes a struct or FlatData declaration and its public value API.</summary>
    internal static void EmitStruct(SourceWriter source, StructPlan declaration)
    {
        if (declaration.IsFlatData)
        {
            FlatDataHeaderEmitter.EmitFlatDataStruct(source, declaration);
            return;
        }

        source.WriteDoxygenBrief($"IDL struct {declaration.QualifiedName}");
        source.WriteDoxygenWrapped("@details", $"Extensibility: {declaration.Extensibility}.");

        if (declaration.IsNested)
        {
            source.WriteDoxygenWrapped("@details",
                "This IDL value is marked nested and is not a DDS topic type. It provides value and DynamicType support for nested member use.");
            RtiDocumentation.WriteApiSee(source,
                "RTI Modern C++ topic traits and nested-type eligibility",
                "group__DDSTopicTraitsModule.html");
        }

        if (declaration.HasPlainLanguageBinding)
        {
            source.WriteDoxygenWrapped("@details", "IDL language_binding(PLAIN) selects RTI's C++11 value binding.");
        }

        if (declaration.HasInbandTransferMode)
        {
            source.WriteDoxygenWrapped("@details", "IDL transfer_mode(INBAND) selects RTI's default in-band transfer mode.");
        }
        if (declaration.IsXcdr2Only)
        {
            source.WriteDoxygenWrapped("@details", "The IDL type permits XCDR2 only. Its DynamicType records that mask; the CDR helper retains RTI's auto_id default and rejects explicit non-XCDR2 representations.");
        }
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits and data-type support",
            "classdds_1_1core_1_1xtypes_1_1StructType.html");

        if (declaration.IsXcdr2Only)
        {
            RtiDocumentation.WriteApiSee(source,
                $"RTI Connext DDS {source.RtiVersion} DataRepresentation policy",
                "classdds_1_1core_1_1policy_1_1DataRepresentation.html");
        }

        source.WriteDoxygenCode([$"auto sample = {declaration.CppType}{{}};"]);
        var signature = $"struct NDDSUSERDllExport {declaration.Name}";

        if (declaration.CppBaseType is { } baseType)
        {
            var relativeBaseType = CppTypeNameFormatter.MakeRelative(baseType, declaration.NamespaceSegments);
            signature += $" : public {relativeBaseType}";
        }

        source.WriteLine(signature);
        source.WriteLine("{");

        EmitStructMembers(source, declaration);
        EmitConstructors(source, declaration);
        source.WriteLine("};");
        EmitValueOperations(source, declaration);
    }

    private static void EmitStructMembers(SourceWriter source, StructPlan declaration)
    {
        if (declaration.Members.Count > 0)
        {
            source.WriteLine("    // Members in IDL declaration order.");

            foreach (var member in declaration.Members)
            {
                EmitMemberDocumentation(source, declaration, member);
                var storageType = CppTypeNameFormatter.MakeRelative(member.StorageType, declaration.NamespaceSegments);
                var defaultValue = CppTypeNameFormatter.MakeRelative(member.DefaultValue, declaration.NamespaceSegments);
                source.WriteLine($"    {storageType} {member.Name} = {defaultValue};");
            }

            source.BlankLine();
        }
    }

    private static void EmitMemberDocumentation(SourceWriter source, StructPlan declaration, MemberPlan member)
    {
        source.WriteDoxygenBrief($"IDL member {member.Name}.", indentationLevel: 1);

        var details = DescribeMember(member);
        if (details.Length > 0)
        {
            source.WriteDoxygenWrapped("@details", details, indentationLevel: 1);
        }

        if (FindDirectStringType(member.Type) is { } stringType)
        {
            var manualBase = $"https://community.rti.com/static/documentation/connext-dds/{source.RtiVersion}/doc/manuals/connext_dds_professional/users_manual/";
            var stringBoundsUrl = source.RtiVersion == "7.7.0"
                ? manualBase + "users_manual/Strings_and_Wide_Strings.htm#StringsAndWideStrings"
                : manualBase + "RTI_ConnextDDS_CoreLibraries_UsersManual.pdf";
            source.WriteDoxygenSee($"RTI string bounds ({source.RtiVersion})", stringBoundsUrl, indentationLevel: 1);

            if (!member.IsOptional)
            {
                source.WriteDoxygenCode(
                    GetStringBoundaryExample(declaration.CppType, member, stringType),
                    indentationLevel: 1);
            }
        }

        if (member.IsHashId)
        {
            RtiDocumentation.WriteApiSee(source,
                "RTI DynamicType Member metadata",
                "classdds_1_1core_1_1xtypes_1_1Member.html",
                indentationLevel: 1);
        }

        if (member.IsMustUnderstand)
        {
            RtiDocumentation.WriteApiSee(source,
                "RTI Modern C++ Member must_understand(bool) metadata builder",
                "classdds_1_1core_1_1xtypes_1_1Member.html",
                indentationLevel: 1);
        }

        if (member.IsOptional)
        {
            RtiDocumentation.WriteApiSee(source,
                "RTI dds::core::optional value operations",
                "classdds_1_1core_1_1optional.html",
                indentationLevel: 1);
            source.WriteDoxygenCode(
                GetOptionalExample(member, declaration.Name, declaration.NamespaceSegments), indentationLevel: 1);
        }

        if (member.IsExternal)
        {
            RtiDocumentation.WriteApiSee(source,
                $"RTI {source.RtiVersion} dds::core::external value wrapper",
                "classdds_1_1core_1_1external.html",
                indentationLevel: 1);
        }

        if (member.TypeCodeUnit is not null)
        {
            RtiDocumentation.WriteApiSee(source,
                $"RTI {source.RtiVersion} member annotation metadata",
                "classdds_1_1core_1_1xtypes_1_1Member.html",
                indentationLevel: 1);
        }
    }

    private static void EmitConstructors(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("    // Construction and value semantics.");
        source.WriteDoxygenBrief($"Constructs a default {declaration.Name} value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}();");
        if (declaration.Members.Count > 0 || declaration.CppBaseType is not null)
        {
            source.BlankLine();
            source.WriteDoxygenBrief($"Constructs a {declaration.Name} from its values.", indentationLevel: 1);
            if (declaration.CppBaseType is not null)
            {
                source.WriteDoxygenParam("base", "Initial values for the base subobject.", indentationLevel: 1);
            }

            foreach (var member in declaration.Members)
            {
                source.WriteDoxygenParam(member.Name, $"Initial {member.Name} value.", indentationLevel: 1);
            }

            EmitValueConstructorDeclaration(source, declaration);
        }

        source.BlankLine();
        source.WriteDoxygenBrief("Copies another value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}(const {declaration.Name}&) = default;");
        source.BlankLine();
        source.WriteDoxygenBrief("Moves another value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}({declaration.Name}&&) noexcept = default;");
        source.BlankLine();
        source.WriteDoxygenBrief("Copy-assigns another value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}& operator=(const {declaration.Name}&) = default;");
        source.WriteDoxygenBrief("Move-assigns another value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}& operator=({declaration.Name}&&) noexcept = default;");
    }

    private static void EmitValueOperations(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.WriteComment("Value operations.");
        source.WriteDoxygenBrief($"Compares two {declaration.Name} values for equality.");
        source.WriteDoxygenParam("left", "First value.");
        source.WriteDoxygenParam("right", "Second value.");
        source.WriteDoxygenReturn("`true` when the values are equal.");
        EmitComparisonDeclaration(source, "==", declaration.Name);
        source.BlankLine();
        source.WriteDoxygenBrief($"Compares two {declaration.Name} values for inequality.");
        source.WriteDoxygenParam("left", "First value.");
        source.WriteDoxygenParam("right", "Second value.");
        source.WriteDoxygenReturn("`true` when the values differ.");
        EmitComparisonDeclaration(source, "!=", declaration.Name);
        source.BlankLine();
        source.WriteDoxygenBrief($"Swaps two {declaration.Name} values.");
        source.WriteDoxygenParam("left", "First value.");
        source.WriteDoxygenParam("right", "Second value.");
        source.WriteLine($"NDDSUSERDllExport void swap({declaration.Name}& left, {declaration.Name}& right) noexcept;");
        source.BlankLine();
        source.WriteDoxygenBrief("Writes a readable value representation to a stream.");
        source.WriteDoxygenParam("output", "Destination stream.");
        source.WriteDoxygenParam("value", "Value to write.");
        source.WriteDoxygenReturn("The destination stream.");
        source.WriteLine($"NDDSUSERDllExport std::ostream& operator<<(std::ostream& output, const {declaration.Name}& value);");
    }

    /// <summary>Writes an enum declaration, its numeric values, and the stream insertion signature.</summary>
    internal static void EmitEnum(SourceWriter source, EnumPlan declaration)
    {
        source.WriteDoxygenBrief($"IDL enumeration {declaration.QualifiedName}");
        source.WriteDoxygenWrapped("@details", $"The default enumerator is {declaration.DefaultMember.Name}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI DynamicType and DynamicData",
            "group__DDSXTypesModule.html");
        source.OpenBlock($"enum class {declaration.Name}");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            if (index > 0)
            {
                source.BlankLine();
            }

            source.WriteDoxygenBrief($"The <c>{member.Name}</c> enumeration value.");
            source.WriteDoxygenDetails(FormattableString.Invariant($"Numeric enum value: {member.Value}."));
            var assignment = member.HasExplicitValue ? FormattableString.Invariant($" = {member.Value}") : string.Empty;
            var separator = index == declaration.Members.Count - 1 ? string.Empty : ",";
            source.WriteLine($"{member.Name}{assignment}{separator}");
        }

        source.CloseBlock(";");
        source.BlankLine();
        source.WriteDoxygenBrief("Writes the enumerator name to a stream.");
        source.WriteDoxygenParam("output", "Destination stream.");
        source.WriteDoxygenParam("value", "Enumerator to write.");
        source.WriteDoxygenReturn("The destination stream.");
        source.WriteLine($"NDDSUSERDllExport std::ostream& operator<<(std::ostream& output, const {declaration.Name}& value);");
    }

    private static string DescribeMember(MemberPlan member)
    {
        var details = new List<string>();
        if (member.IsOptional)
        {
            var memberKind = member.Type switch
            {
                MemberTypePlan.String => "string",
                MemberTypePlan.Sequence => "sequence",
                MemberTypePlan.Array => "fixed array",
                _ => "scalar",
            };
            details.Add($"Optional {memberKind} storage preserves set and unset state. It defaults unset; assign a value to set it, call reset() to unset it, and use value() to access it when set.");
        }

        AddTypeDetails(member.Type, details);
        if (member is { Minimum: { } minimum, Maximum: { } maximum })
        {
            var range = $"[{minimum.ToString(CultureInfo.InvariantCulture)}, {maximum.ToString(CultureInfo.InvariantCulture)}]";
            details.Add($"Inclusive value range: {range}. This is documentation only; the public field does not enforce the bounds.");
        }
        else if (member.Minimum is { } minimumOnly)
        {
            details.Add($"Inclusive minimum value: {minimumOnly.ToString(CultureInfo.InvariantCulture)}. This is documentation only; the public field does not enforce the bound.");
        }
        else if (member.Maximum is { } maximumOnly)
        {
            details.Add($"Inclusive maximum value: {maximumOnly.ToString(CultureInfo.InvariantCulture)}. This is documentation only; the public field does not enforce the bound.");
        }

        if (member.TypeCodeUnit is { } unit)
        {
            details.Add($"Unit metadata: {unit}.");
        }

        if (member.HasResolveNameFalse)
        {
            details.Add("RTI name resolution remains disabled, matching @resolve_name(false).");
        }

        if (member.IsKey)
        {
            details.Add("This member is an IDL key.");
        }

        if (member.IsHashId)
        {
            details.Add($"The shared IDL binder resolved this hashed member ID as {member.MemberId}.");
        }

        if (member.IsMustUnderstand)
        {
            details.Add("This required member carries the XTypes must-understand flag in generated DynamicType metadata.");
        }

        return string.Join(" ", details);
    }

    private static MemberTypePlan.String? FindDirectStringType(MemberTypePlan type)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type as MemberTypePlan.String;
    }

    private static IReadOnlyList<string> GetStringBoundaryExample(
        string typeName,
        MemberPlan member,
        MemberTypePlan.String stringType)
    {
        var lines = new List<string> { $"auto sample = {typeName}{{}};" };
        if (stringType.MetadataBound > 0)
        {
            var character = stringType.IsWide ? "L'a'" : "'a'";
            lines.Add($"sample.{member.Name}.assign({stringType.MetadataBound}, {character});");
        }
        else
        {
            var value = stringType.IsWide ? "L\"text\"" : "\"text\"";
            lines.Add($"sample.{member.Name} = {value};");
        }

        return lines;
    }

    private static IReadOnlyList<string> GetOptionalExample(
        MemberPlan member,
        string typeName,
        IReadOnlyList<string> namespaceSegments)
    {
        var lines = new List<string> { $"{typeName} sample{{}};" };
        string Relative(string spelling) => CppTypeNameFormatter.MakeRelative(spelling, namespaceSegments);

        if (member.Type is MemberTypePlan.Sequence)
        {
            lines.Add($"if (!sample.{member.Name}.has_value()) {{ /* absent */ }}");
            lines.Add($"sample.{member.Name} = {Relative(member.Type.StorageType)}{{}}; // present and empty");
            lines.Add($"if (sample.{member.Name}.has_value()) {{");
            lines.Add($"    const auto currentValue = sample.{member.Name}.value();");
            lines.Add("}");
        }
        else
        {
            var value = member.Type switch
            {
                MemberTypePlan.String => "\"text\"",
                MemberTypePlan.Array => $"{Relative(member.Type.StorageType)}{{}}",
                _ when IsAggregateType(member.Type) => $"{Relative(member.Type.StorageType)}{{}}",
                _ => "7",
            };
            lines.Add($"sample.{member.Name} = {value};");
            lines.Add($"if (sample.{member.Name}.has_value()) {{");
            lines.Add($"    const auto currentValue = sample.{member.Name}.value();");
            lines.Add("}");
        }

        lines.Add($"sample.{member.Name}.reset();");
        return lines;
    }

    private static bool IsAggregateType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Struct or MemberTypePlan.Union => true,
        MemberTypePlan.Alias alias => IsAggregateType(alias.Target),
        _ => false,
    };

    private static void AddTypeDetails(MemberTypePlan type, ICollection<string> details)
    {
        switch (type)
        {
            case MemberTypePlan.Alias alias:
                AddTypeDetails(alias.Target, details);
                break;
            case MemberTypePlan.String stringType:
                details.Add(stringType.MetadataBound == 0
                    ? "String metadata bound is RTIXCdrLong_MAX (unbounded). The generated C++ field does not enforce a length limit when assigned."
                    : $"String metadata bound is {stringType.MetadataBound}. RTI's effective {(stringType.IsWide ? "wide-string" : "string")} maximum is {stringType.MetadataBound} {(stringType.IsWide ? "UTF-16 code units" : "bytes")}, excluding the trailing null. The generated C++ field does not enforce this limit when assigned.");
                break;
            case MemberTypePlan.Sequence sequence:
                details.Add(sequence.IsUnbounded
                    ? "Effective sequence metadata bound is RTIXCdrLong_MAX (unbounded)."
                    : $"Effective sequence metadata bound is {sequence.Bound}.");
                AddTypeDetails(sequence.ElementType, details);
                break;
            case MemberTypePlan.Array array:
                details.Add($"Fixed array dimensions are {string.Join(" x ", array.Dimensions)}.");
                AddTypeDetails(array.ElementType, details);
                break;
        }
    }

    private static void EmitComparisonDeclaration(SourceWriter source, string operation, string typeName)
    {
        var declaration = $"NDDSUSERDllExport bool operator{operation}(const {typeName}& left, const {typeName}& right);";
        if (declaration.Length <= 116)
        {
            source.WriteLine(declaration);
            return;
        }

        source.WriteLine($"NDDSUSERDllExport bool operator{operation}(");
        source.WriteLine($"    const {typeName}& left,");
        source.WriteLine($"    const {typeName}& right);");
    }

    private static void EmitValueConstructorDeclaration(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine($"    {declaration.Name}(");
        var parameters = new List<(string Type, string Name)>();

        if (declaration.CppBaseType is { } baseType)
        {
            parameters.Add(($"const {baseType}&", "base"));
        }

        parameters.AddRange(declaration.Members.Select(member => (member.ConstructorParameterType, member.Name)));

        for (var index = 0; index < parameters.Count; index++)
        {
            var (type, name) = parameters[index];
            var suffix = index == parameters.Count - 1 ? string.Empty : ",";
            var relativeType = CppTypeNameFormatter.MakeRelative(type, declaration.NamespaceSegments);
            source.WriteLine($"        {relativeType} {name}{suffix}");
        }

        source.WriteLine("    );");
    }

    /// <summary>Writes a union declaration, its discriminator, and public branch operations.</summary>
    internal static void EmitUnion(SourceWriter source, UnionPlan declaration, bool isRti612 = false)
    {
        string Relative(string spelling) =>
            CppTypeNameFormatter.MakeRelative(spelling, declaration.NamespaceSegments);

        source.WriteDoxygenBrief($"IDL union {declaration.QualifiedName}");
        source.WriteDoxygenDetails("Only a branch selected by the discriminator may be read.");
        source.WriteDoxygenWrapped(
            "@details",
            $"Discriminator type is {Relative(declaration.DiscriminatorType)}; default value is {Relative(declaration.DiscriminatorDefaultValue)}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI DynamicType and DynamicData",
            "classdds_1_1core_1_1xtypes_1_1UnionType.html");
        source.WriteLine($"class NDDSUSERDllExport {declaration.Name}");
        source.WriteLine("{");
        source.WriteLine("public:");
        source.BlankLine();
        source.WriteDoxygenBrief($"Constructs the default {declaration.Name} union value.", indentationLevel: 1);
        source.WriteLine($"    {declaration.Name}();");
        source.BlankLine();
        source.WriteDoxygenBrief("Returns the current discriminator.", indentationLevel: 1);
        source.WriteDoxygenReturn("The discriminator value.", indentationLevel: 1);
        source.WriteLine($"    {Relative(declaration.DiscriminatorType)}& _d();");
        source.WriteDoxygenBrief("Returns the current discriminator without modifying it.", indentationLevel: 1);
        source.WriteDoxygenReturn("The discriminator value.", indentationLevel: 1);
        source.WriteLine($"    const {Relative(declaration.DiscriminatorType)}& _d() const;");
        source.BlankLine();
        source.WriteDoxygenBrief("Changes the active branch by setting the discriminator.", indentationLevel: 1);
        source.WriteDoxygenParam("value", "New discriminator value.", indentationLevel: 1);
        source.WriteLine($"    void _d({Relative(declaration.DiscriminatorType)} value);");
        source.BlankLine();
        source.WriteDoxygenBrief("Returns the discriminator used by default construction.", indentationLevel: 1);
        source.WriteDoxygenReturn("The discriminator value.", indentationLevel: 1);
        source.WriteLine($"    static {Relative(declaration.DiscriminatorType)} default_discriminator();");
        foreach (var branch in declaration.Branches)
        {
            source.BlankLine();
            source.WriteDoxygenBrief($"Accesses the {branch.Name} branch when active.", indentationLevel: 1);
            var labelDetails = branch.IsDefault
                ? "Default branch is active when no explicit discriminator label matches."
                : $"Discriminator labels: {string.Join(", ", branch.LabelExpressions.Select(Relative))}.";
            source.WriteDoxygenWrapped("@details", labelDetails, indentationLevel: 1);
            source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the branch is inactive.", indentationLevel: 1);
            var branchType = Relative(branch.Type.StorageType);
            source.WriteLine($"    {branchType}& {branch.Name}();");
            source.WriteDoxygenBrief($"Accesses the {branch.Name} branch without modifying it.", indentationLevel: 1);
            source.WriteDoxygenWrapped("@details", labelDetails, indentationLevel: 1);
            source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the branch is inactive.", indentationLevel: 1);
            source.WriteLine($"    const {branchType}& {branch.Name}() const;");

            source.BlankLine();
            source.WriteDoxygenBrief($"Activates and assigns the {branch.Name} branch.", indentationLevel: 1);
            source.WriteDoxygenParam("value", "Value to assign to the branch.", indentationLevel: 1);
            if (branch.IsDefault)
            {
                source.WriteDoxygenParam("discriminator", "Unused discriminator value selecting the default branch.", indentationLevel: 1);
                source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the discriminator selects an explicit branch.", indentationLevel: 1);
            }
            EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, useRvalue: false, includeDiscriminator: branch.IsDefault);

            if (branch is { IsDefault: false, LabelExpressions.Count: > 1 })
            {
                source.WriteDoxygenBrief($"Activates and assigns the {branch.Name} branch with a selected discriminator label.", indentationLevel: 1);
                source.WriteDoxygenParam("value", "Value to assign to the branch.", indentationLevel: 1);
                source.WriteDoxygenParam("discriminator", $"One of: {string.Join(", ", branch.LabelExpressions)}.", indentationLevel: 1);
                source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the discriminator is not a branch label.", indentationLevel: 1);
                EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, includeDiscriminator: true);
            }

            if (branch.Type is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
            {
                source.WriteDoxygenBrief($"Move-assigns the {branch.Name} branch.", indentationLevel: 1);
                source.WriteDoxygenParam("value", "Value to move into the branch.", indentationLevel: 1);
                EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: branch.IsDefault);

                if (branch is { IsDefault: false, LabelExpressions.Count: > 1 })
                {
                    source.WriteDoxygenBrief($"Move-assigns the {branch.Name} branch with a selected discriminator label.", indentationLevel: 1);
                    source.WriteDoxygenParam("value", "Value to move into the branch.", indentationLevel: 1);
                    source.WriteDoxygenParam("discriminator", $"One of: {string.Join(", ", branch.LabelExpressions)}.", indentationLevel: 1);
                    source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the discriminator is not a branch label.", indentationLevel: 1);
                    EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: true);
                }
            }

            if (branch.IsDefault)
            {
                source.WriteDoxygenBrief("Assigns the already-selected default branch.", indentationLevel: 1);
                source.WriteDoxygenParam("value", "Value to assign to the branch.", indentationLevel: 1);
                source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the default branch is inactive.", indentationLevel: 1);
                EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, includeDiscriminator: false);

                if (branch.Type is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
                {
                    source.WriteDoxygenBrief("Move-assigns the already-selected default branch.", indentationLevel: 1);
                    source.WriteDoxygenParam("value", "Value to move into the branch.", indentationLevel: 1);
                    source.WriteDoxygenThrows("dds::core::PreconditionNotMetError", "when the default branch is inactive.", indentationLevel: 1);
                    EmitUnionBranchSetterDeclaration(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: false);
                }
            }
        }

        source.WriteDoxygenBrief("Compares two union values for equality.", indentationLevel: 1);
        EmitUnionComparisonDeclaration(source, "==", declaration.Name);
        source.WriteDoxygenBrief("Compares two union values for inequality.", indentationLevel: 1);
        EmitUnionComparisonDeclaration(source, "!=", declaration.Name);
        source.WriteDoxygenBrief("Swaps this union value with another.", indentationLevel: 1);
        source.WriteLine($"    void swap({declaration.Name}& other_) noexcept;");
        source.WriteDoxygenBrief("Swaps two union values through argument-dependent lookup.", indentationLevel: 1);
        source.WriteLine($"    friend NDDSUSERDllExport void swap({declaration.Name}& left, {declaration.Name}& right);");
        source.WriteDoxygenBrief("Writes a readable union value representation.", indentationLevel: 1);
        source.WriteLine(
            $"    friend NDDSUSERDllExport std::ostream& operator<<(std::ostream& stream, const {declaration.Name}& value);");
        source.BlankLine();
        source.WriteLine("private:");
        source.WriteLine($"    {Relative(declaration.DiscriminatorType)} discriminator_ = {Relative(declaration.DiscriminatorDefaultValue)};");
        foreach (var branch in declaration.Branches)
        {
            source.WriteLine($"    {Relative(branch.Type.StorageType)} {branch.Name}_ = {Relative(branch.Type.DefaultValue)};");
        }

        source.WriteLine("};");
    }

    private static void EmitUnionBranchSetterDeclaration(
        SourceWriter source,
        UnionPlan declaration,
        UnionBranchPlan branch,
        bool isRti612 = false,
        bool useRvalue = false,
        bool includeDiscriminator = true)
    {
        var parameterType = CppTypeNameFormatter.MakeRelative(
            branch.SetterParameterType(useRvalue, isRti612), declaration.NamespaceSegments);
        var discriminatorType = CppTypeNameFormatter.MakeRelative(
            declaration.DiscriminatorType, declaration.NamespaceSegments);
        var discriminatorParameter = includeDiscriminator ? $", {discriminatorType} discriminator" : string.Empty;
        source.WriteLine($"    void {branch.Name}({parameterType} value{discriminatorParameter});");
    }

    private static void EmitUnionComparisonDeclaration(SourceWriter source, string operation, string typeName)
    {
        source.WriteLine($"    bool operator{operation}(const {typeName}& other_) const;");
    }
}
