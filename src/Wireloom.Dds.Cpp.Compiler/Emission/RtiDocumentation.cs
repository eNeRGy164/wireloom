namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Writes versioned references to the RTI Modern C++ API documentation.</summary>
internal static class RtiDocumentation
{
    /// <summary>Writes a reference to a page or fragment in the RTI Modern C++ API.</summary>
    internal static void WriteApiSee(SourceWriter source, string description, string page, int indentationLevel = 0) =>
        source.WriteDoxygenSee(description,
            $"https://community.rti.com/static/documentation/connext-dds/{source.RtiVersion}/doc/api/connext_dds/api_cpp2/{page}",
            indentationLevel);
}
