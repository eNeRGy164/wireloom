namespace Wireloom.Dds.CSharp.Compiler.Emission;

/// <summary>Builds RTI C# API references for the selected runtime profile.</summary>
internal static class RtiCSharpDocumentation
{
    private const string ApiRoot = "https://community.rti.com/static/documentation/connext-dds";

    private static string DynamicData(RtiApiProfile profile) => ApiPage(
        profile,
        "classRti_1_1Types_1_1Dynamic_1_1DynamicData.html",
        "class_rti_1_1_types_1_1_dynamic_1_1_dynamic_data.html");

    internal static string MemberMetadata(RtiApiProfile profile) => ApiPage(
        profile,
        "classRti_1_1Types_1_1Dynamic_1_1StructMember.html",
        "class_rti_1_1_types_1_1_dynamic_1_1_struct_member.html");

    internal static string OmgTypes(RtiApiProfile profile) => ApiPage(
        profile,
        "namespaceOmg_1_1Types.html",
        "namespace_omg_1_1_types.html");

    private static string TypeSupport(RtiApiProfile profile) => ApiPage(
        profile,
        "classRti_1_1Dds_1_1Topics_1_1TypeSupport.html",
        "class_rti_1_1_dds_1_1_topics_1_1_type_support.html");

    internal static string Annotations(RtiApiProfile profile) => ApiPage(
        profile,
        "classRti_1_1Types_1_1Dynamic_1_1Annotations.html",
        "class_rti_1_1_types_1_1_dynamic_1_1_annotations.html");

    internal static void WriteTypeSupportSeeAlso(Writers.GeneratedSourceWriter writer, RtiApiProfile profile)
    {
        writer.WriteXmlSeeAlso(TypeSupport(profile), $"RTI Connext C# TypeSupport API ({profile.RtiVersion})");
        writer.WriteXmlSeeAlso(DynamicData(profile), $"RTI Connext {profile.RtiVersion} C# DynamicData API");
    }

    private static string ApiPage(RtiApiProfile profile, string currentPage, string legacyPage)
    {
        var page = profile.RtiVersion == "6.1.2" ? legacyPage : currentPage;

        return $"{ApiRoot}/{profile.RtiVersion}/doc/api/connext_dds/api_csharp/{page}";
    }
}
