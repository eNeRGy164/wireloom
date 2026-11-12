using Wireloom.Dds.Idl.Compiler.FrontEnd.Parsing;

namespace Wireloom.Dds.Idl;

/// <summary>Represents a bound IDL annotation and its source location.</summary>
[PublicAPI]
public sealed class IdlAnnotation
{
    private readonly Lazy<IReadOnlyList<IdlAnnotationArgument>> arguments;

    private IdlAnnotation(IdlAnnotationApplicationSyntax syntax)
    {
        Name = syntax.Name;
        Offset = syntax.Offset;
        Length = syntax.Length;
        arguments = new Lazy<IReadOnlyList<IdlAnnotationArgument>>(() => Array.AsReadOnly(
            syntax.ParseArguments().Select(argument => new IdlAnnotationArgument(
                argument.Name, argument.Expression, argument.RelativeOffset)).ToArray()));
    }

    /// <summary>Gets the annotation name without its leading at sign.</summary>
    public string Name { get; }

    /// <summary>Gets the zero-based source offset of the annotation.</summary>
    public int Offset { get; }

    /// <summary>Gets the source length of the annotation application.</summary>
    public int Length { get; }

    /// <summary>Gets the parsed positional or named arguments.</summary>
    public IReadOnlyList<IdlAnnotationArgument> Arguments => arguments.Value;

    internal static IdlAnnotation FromSyntax(IdlAnnotationApplicationSyntax syntax) => new(syntax);
}

/// <summary>Represents one parsed positional or named IDL annotation argument.</summary>
[PublicAPI]
public sealed class IdlAnnotationArgument(string? name, string expression, int relativeOffset)
{
    /// <summary>Gets the argument name, or null for a positional argument.</summary>
    public string? Name { get; } = name;

    /// <summary>Gets the argument expression.</summary>
    public string Expression { get; } = expression;

    /// <summary>Gets the zero-based source offset relative to the annotation.</summary>
    public int RelativeOffset { get; } = relativeOffset;
}
