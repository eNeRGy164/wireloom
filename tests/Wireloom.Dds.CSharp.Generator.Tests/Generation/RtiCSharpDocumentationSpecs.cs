using Wireloom.Dds.CSharp.Compiler.Emission;
using static Wireloom.Dds.CSharp.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class RtiCSharpDocumentationSpecs
{
    [Theory]
    [InlineData("6.1.2", "namespace_omg_1_1_types.html", "class_rti_1_1_types_1_1_dynamic_1_1_dynamic_data.html", "class_rti_1_1_dds_1_1_topics_1_1_type_support.html", "class_rti_1_1_types_1_1_dynamic_1_1_struct_member.html", "")]
    [InlineData("7.3.1", "namespaceOmg_1_1Types.html", "classRti_1_1Types_1_1Dynamic_1_1DynamicData.html", "classRti_1_1Dds_1_1Topics_1_1TypeSupport.html", "classRti_1_1Types_1_1Dynamic_1_1StructMember.html", "")]
    [InlineData("7.7.0", "namespaceOmg_1_1Types.html", "classRti_1_1Types_1_1Dynamic_1_1DynamicData.html", "classRti_1_1Dds_1_1Topics_1_1TypeSupport.html", "classRti_1_1Types_1_1Dynamic_1_1StructMember.html", "classRti_1_1Types_1_1Dynamic_1_1Annotations.html")]
    public void GeneratedDocsLinkToVersionedRtiCSharpApis(
        string version,
        string omgTypesPage,
        string dynamicDataPage,
        string typeSupportPage,
        string memberMetadataPage,
        string? annotationsPage)
    {
        // Arrange
        var input = Input("rti-api-doc-links.idl", "module Example { enum State { Idle, Active }; union Choice switch(long) { case 1: long value; }; struct Sample { @key long id; @optional long value; @default(3) long annotated; }; };");
        var profile = IdlCompiler.GetProfileForRuntimeVersion(Version.Parse(version)) switch
        {
            IdlCompilerProfile.Rti612 => RtiApiProfile.Rtiddsgen312,
            IdlCompilerProfile.Rti731 => RtiApiProfile.Rtiddsgen431,
            IdlCompilerProfile.Rti770 => RtiApiProfile.Rtiddsgen470,
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, "The RTI API profile is not supported.")
        };

        // Act
        var documents = CompileSourcesWithDiagnostics([input], null, TestContext.Current.CancellationToken, profile);

        // Assert
        var managed = documents["Example.Sample.g.cs"].Source;
        managed.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{omgTypesPage}");
        managed.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{memberMetadataPage}");
        if (!string.IsNullOrEmpty(annotationsPage))
        {
            managed.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{annotationsPage}");
        }
        else
        {
            managed.ShouldNotContain("/api_csharp/classRti_1_1Types_1_1Dynamic_1_1Annotations.html");
            managed.ShouldNotContain("/api_csharp/class_rti_1_1_types_1_1_dynamic_1_1_annotations.html");
        }
        var support = documents["Example.SampleSupport.g.cs"].Source;
        support.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{typeSupportPage}");
        support.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{dynamicDataPage}");
        foreach (var supportName in new[] { "Example.StateSupport.g.cs", "Example.ChoiceSupport.g.cs" })
        {
            var generatedSupport = documents[supportName].Source;
            generatedSupport.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{typeSupportPage}");
            generatedSupport.ShouldContain($"https://community.rti.com/static/documentation/connext-dds/{version}/doc/api/connext_dds/api_csharp/{dynamicDataPage}");
        }
    }
}
