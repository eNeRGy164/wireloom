using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits native struct plugin contexts and their staged initialization.</summary>
internal static class StructPluginEmitter
{
    /// <summary>Writes the native plugin context for a struct.</summary>
    internal static void EmitPluginContext(
        SourceWriter source,
        StructPlan declaration,
        GenerationPlan plan)
    {
        var stagedInitialization = plan.Initialization.RequiresStagedInitialization;
        if (declaration.IsFlatData)
        {
            FlatDataPluginEmitter.EmitFlatDataPluginContext(source, declaration, plan.Profile);
            return;
        }

        if (stagedInitialization)
        {
            EmitStagedPluginContext(source, declaration, plan);
            return;
        }

        var contextName = declaration.ContextIdentifier;
        var typeName = declaration.CppType;
        source.WriteLine($"struct {contextName}");
        source.WriteLines("""
            {
                ::rti::topic::cdr::GenericTypePluginFactory& factory;
                ::dds::core::xtypes::StructType* type;
            """);
        if (declaration.IsTopic)
        {
            source.WriteLine("    ::rti::topic::cdr::CTypePlugin* type_plugin;");
        }

        if (RtiSampleAccess.HasInterpreterSampleAccess(plan, declaration)
            || RtiSampleAccess.IsSequenceAggregateElement(plan, declaration))
        {
            source.BlankLine();
            StlPluginEmitter.EmitStlSampleAccessInfo(source, declaration);
        }

        source.BlankLine();
        source.WriteLine($"    {contextName}()");
        source.WriteLine("        : factory(::rti::topic::cdr::GenericTypePluginFactory::instance())");
        source.WriteLine("        , type(factory.create_struct(");
        source.WriteLine($"              \"{declaration.QualifiedName}\",");
        source.WriteLine($"              {declaration.ExtensibilityExpression},");
        source.WriteLine($"              sizeof({typeName}),");
        source.WriteLine("              {");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
            source.WriteLine($"                  offsetof({typeName}, {member.Name}){suffix}");
        }

        source.WriteLine("              }))");
        if (declaration.IsTopic)
        {
            source.WriteLine("        , type_plugin(nullptr)");
        }
        source.WriteLine("    {");
        NativeTypeEmitter.EmitPrivateTypeCodeAnnotations(source, declaration, "        ");
        source.WriteLine("        // Bind each IDL member to its RTI DynamicType metadata.");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            DynamicTypeEmitter.EmitMemberRegistration(source, member);
            StlPluginEmitter.EmitSequenceMemberAccessInfo(
                source,
                member,
                index,
                plan,
                declaration,
                RtiSampleAccess.HasSequenceInValueGraph(plan, declaration) && RtiSampleAccess.SupportsSequenceInValueGraph(plan, declaration));
            StlPluginEmitter.EmitStlStringMemberAccessInfo(source, plan, declaration, member, index);
        }

        NativeTypeEmitter.EmitMemberTypeCodeDefaults(source, declaration, "        ");

        var createsFactoryTypePlugin = declaration.IsTopic
            && !RtiSampleAccess.HasSequenceInValueGraph(plan, declaration)
            && !RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration)
            && !RtiSampleAccess.IsReferencedByStlInterpreterGraph(plan, declaration)
            && !RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration);
        var isSequenceAggregateElement = RtiSampleAccess.IsSequenceAggregateElement(plan, declaration);
        if (createsFactoryTypePlugin && isSequenceAggregateElement)
        {
            source.BlankLine();
            source.WriteLine("        type_plugin = &factory.create_type_plugin(*type);");
        }

        StlPluginEmitter.EmitStlSampleAccessAssignments(source, plan, declaration, "        ");

        if (createsFactoryTypePlugin && !isSequenceAggregateElement)
        {
            source.BlankLine();
            source.WriteLine("        type_plugin = &factory.create_type_plugin(*type);");
        }
        source.WriteLine("    }");
        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"{contextName}& {contextName}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {contextName} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
    }

    private static void EmitStagedPluginContext(
        SourceWriter source,
        StructPlan declaration,
        GenerationPlan plan)
    {
        var contextName = declaration.ContextIdentifier;
        var typeName = declaration.CppType;
        var hasFactoryTypePlugin = declaration is { IsTopic: true, IsFlatData: false };
        source.WriteLine($"struct {contextName}");
        source.WriteLines("""
            {
                ::rti::topic::cdr::GenericTypePluginFactory& factory;
                ::std::unique_ptr<::dds::core::xtypes::StructType> owned_type;
                ::dds::core::xtypes::StructType* type;
            """);
        if (hasFactoryTypePlugin)
        {
            source.WriteLine("    ::rti::topic::cdr::CTypePlugin* type_plugin;");
        }

        if (RtiSampleAccess.HasInterpreterSampleAccess(plan, declaration)
            || RtiSampleAccess.IsSequenceAggregateElement(plan, declaration))
        {
            source.BlankLine();
            StlPluginEmitter.EmitStlSampleAccessInfo(source, declaration);
        }

        source.BlankLine();
        source.WriteLine($"    {contextName}()");
        source.WriteLine("        : factory(::rti::topic::cdr::GenericTypePluginFactory::instance())");
        source.WriteLine("        , type(nullptr)");
        if (hasFactoryTypePlugin)
        {
            source.WriteLine("        , type_plugin(nullptr)");
        }
        source.WriteLine("    {");
        source.WriteLine("    }");
        source.BlankLine();
        source.WriteLine("    void create_type()");
        source.WriteLine("    {");
        if (declaration.CppBaseType is { } baseType)
        {
            var baseContextType = PluginSymbols.ContextTypePointer(
                baseType.TrimStart(':'), ownerQualifiedName: null, optionalSelfRecursiveArray: false);
            source.WriteLine("        owned_type.reset(");
            source.WriteLine("            new ::dds::core::xtypes::StructType(");
            source.WriteLine($"                \"{declaration.QualifiedName}\",");
            source.WriteLine($"                *{baseContextType}));");
            source.WriteLine($"        owned_type->extensibility_kind({declaration.ExtensibilityExpression});");
            source.WriteLine("        type = owned_type.get();");
        }
        else if (RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
        {
            source.WriteLine("        owned_type.reset(");
            source.WriteLine($"            new ::dds::core::xtypes::StructType(\"{declaration.QualifiedName}\"));");
            source.WriteLine($"        owned_type->extensibility_kind({declaration.ExtensibilityExpression});");
            source.WriteLine("        type = owned_type.get();");
        }
        else
        {
            source.WriteLine("        type = factory.create_struct(");
            source.WriteLine($"            \"{declaration.QualifiedName}\",");
            source.WriteLine($"            {declaration.ExtensibilityExpression},");
            source.WriteLine($"            sizeof({typeName}),");
            source.WriteLine("            {");
            for (var index = 0; index < declaration.Members.Count; index++)
            {
                var member = declaration.Members[index];
                var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
                source.WriteLine($"                offsetof({typeName}, {member.Name}){suffix}");
            }

            source.WriteLine("            });");
        }

        NativeTypeEmitter.EmitPrivateTypeCodeAnnotations(source, declaration, "        ");

        source.WriteLine("    }");
        source.BlankLine();
        source.WriteLines("""
                void add_members()
                {
                    // Bind each IDL member to its RTI DynamicType metadata.
            """);
        if (declaration.HasSelfRecursiveSequence
            || RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration))
        {
            var resourceNodeType = plan.Profile.RtiVersion == "7.7.0"
                ? "::RTIXCdrResourceNode"
                : "::RTICdrResourceNode";
            var resourceNodeInitializer = resourceNodeType.TrimStart(':') + "_INITIALIZER";
            source.WriteLine($"        {resourceNodeType} wireloom_recursive_resource_node =");
            source.WriteLine($"            {resourceNodeInitializer};");
            source.WriteLines("""
                wireloom_recursive_resource_node.resource = &type->native();
                """);
        }

        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            DynamicTypeEmitter.EmitMemberRegistration(
                source,
                member,
                stagedInitialization: true,
                ownerQualifiedName: declaration.QualifiedName,
                useRecursiveResourceNode: declaration.HasSelfRecursiveSequence
                    && RtiSampleAccess.IsSelfRecursiveSequence(member.Type, declaration.CppType),
                useStructTypeApi: RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration));
            StlPluginEmitter.EmitSequenceMemberAccessInfo(
                source,
                member,
                index,
                plan,
                declaration,
                RtiSampleAccess.HasSequenceInValueGraph(plan, declaration) && RtiSampleAccess.SupportsSequenceInValueGraph(plan, declaration));
            StlPluginEmitter.EmitStlStringMemberAccessInfo(source, plan, declaration, member, index);
        }

        NativeTypeEmitter.EmitMemberTypeCodeDefaults(source, declaration, "        ");

        StlPluginEmitter.EmitStlSampleAccessAssignments(source, plan, declaration, "        ");

        source.WriteLine("    }");
        source.BlankLine();
        if (hasFactoryTypePlugin)
        {
            source.WriteLine("    void create_type_plugin()");
            source.WriteLine("    {");
            if (!RtiSampleAccess.HasSequenceInValueGraph(plan, declaration)
                && !(plan.Profile.RtiVersion == "7.3.1"
                    && RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration))
                && !RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration)
                && !RtiSampleAccess.IsReferencedByStlInterpreterGraph(plan, declaration)
                && !RtiSampleAccess.IsReferencedBySequenceInterpreterGraph(plan, declaration)
                && !RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
            {
                source.WriteLine("        type_plugin = &factory.create_type_plugin(*type);");
            }
            source.WriteLine("    }");
        }
        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"{contextName}& {contextName}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {contextName} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
    }

    /// <summary>Writes the shared staged initializer for the file's native type contexts.</summary>
    internal static void EmitStructTypeInitialization(
        SourceWriter source,
        GenerationPlan plan,
        bool metadataOnlyTypeInitialization)
    {
        source.WriteLine("namespace");
        source.WriteLine("{");
        if (metadataOnlyTypeInitialization)
        {
            source.WriteLine("std::once_flag wireloom_dynamic_types_once;");
        }

        source.WriteLine("std::once_flag wireloom_type_plugins_once;");
        if (metadataOnlyTypeInitialization)
        {
            source.WriteLine("void wireloom_initialize_dynamic_types()");
        }
        else
        {
            source.WriteLine("void wireloom_initialize_type_plugins()");
        }

        source.WriteLine("{");
        source.WriteLine(
            "    std::call_once("
            + $"{(metadataOnlyTypeInitialization ? "wireloom_dynamic_types_once" : "wireloom_type_plugins_once")}, []");
        source.WriteLine("    {");
        source.WriteLine("        // Stage 1: create all struct types without inheritance.");
        if (plan.Unions.Count > 0)
        {
            source.WriteLine("        // Create union shells in the same stage to support cross-type metadata references.");
        }
        foreach (var declaration in plan.Structs.Where(declaration => declaration.CppBaseType is null))
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "create_type();", 8);
        }

        foreach (var declaration in plan.Unions)
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "create_type();", 8);
        }

        foreach (var declaration in plan.Initialization.StructShellsBeforeMetadata)
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "create_type();", 8);
        }

        source.BlankLine();
        source.WriteLine(
            $"{(plan.Unions.Count > 0 ? "        // Stage 2: register metadata in dependency order after all shells exist." : "        // Stage 2: register metadata in dependency order; recursive collections use the existing type shell.")}");
        foreach (var entry in plan.Initialization.TypeInitializationOrder)
        {
            if (entry.Struct is { } structure)
            {
                if (structure.CppBaseType is not null && !plan.Initialization.StructShellsBeforeMetadata.Contains(structure))
                {
                    PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(structure), "create_type();", 8);
                }

                PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(structure), "add_members();", 8);
            }
            else if (entry.Union is { } union)
            {
                PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(union), "add_members();", 8);
            }
        }

        if (metadataOnlyTypeInitialization)
        {
            source.WriteLine("    });");
            source.WriteLine("}");
            source.BlankLine();
            source.WriteLines("""
                void wireloom_initialize_type_plugins()
                {
                    wireloom_initialize_dynamic_types();
                    std::call_once(wireloom_type_plugins_once, []
                    {
                """);
        }
        else
        {
            source.BlankLine();
        }

        source.WriteLine("        // Stage 3: create plugins only after every DynamicType is complete.");
        foreach (var declaration in plan.Structs.Where(declaration => declaration is { IsTopic: true, IsFlatData: false }))
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "create_type_plugin();", 8);
        }

        foreach (var declaration in plan.Structs.Where(declaration => declaration is { IsTopic: true, IsFlatData: true }))
        {
            PluginSymbols.WriteContextMemberCall(
                source,
                PluginSymbols.ContextFunctionName(declaration),
                "create_plain_helper_type_plugin();",
                8);
        }

        foreach (var declaration in plan.Unions)
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "create_type_plugin();", 8);
        }

        source.WriteLines("""
                });
            }
            } // namespace
            """);
    }
}
