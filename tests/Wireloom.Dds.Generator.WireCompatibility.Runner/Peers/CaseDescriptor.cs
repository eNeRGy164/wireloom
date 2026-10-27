namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Discovered type names and IDL for one corpus case.</summary>
/// <param name="CaseId">Stable corpus identifier.</param>
/// <param name="WireloomType">Managed qualified type name, or '-' for RTI-only cases.</param>
/// <param name="CppType">Native qualified type name.</param>
/// <param name="Idl">Corpus-relative IDL path.</param>
[PublicAPI]
internal sealed record CaseDescriptor(string CaseId, string WireloomType, string CppType, string Idl)
{
    /// <summary>Describes the existing optional-aggregate RTI-only control case.</summary>
    internal static CaseDescriptor RtiOnly(string caseId) => new(caseId, "-", "CorpusOptionalAggregate::Holder", "idl/features/09-optional-aggregate-member.idl");
}
