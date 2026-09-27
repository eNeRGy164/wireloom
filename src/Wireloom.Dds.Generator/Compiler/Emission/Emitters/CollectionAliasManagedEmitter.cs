using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits managed collection and value typedef declarations.</summary>
internal static class CollectionAliasManagedEmitter
{
    public static void Emit(CompilationContext compilation, IdlTypedef declaration, string sourceIdlFileName)
    {
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        string? element;

        if (declaration.IsString)
        {
            element = "string";
        }
        else if (declaration.IsCollection)
        {
            element = declaration.ElementType!;
        }
        else
        {
            element = declaration.Target;
        }

        if (declaration is { IsCollection: false, IsString: false })
        {
            element = compilation.ResolveUnderlyingType(element, declaration.Namespace);
        }

        string? resolvedElement;
        if (declaration.IsString || IsStringType(element))
        {
            resolvedElement = "string";
        }
        else if (IdlNaming.IsPrimitive(element))
        {
            resolvedElement = IdlNaming.MapPrimitive(element);
        }
        else
        {
            resolvedElement = IdlNaming.EscapeQualifiedIdentifier(IdlNaming.ResolveTypeName(element, declaration.Namespace));
        }

        var elementReference = IdlNaming.TypeReference(resolvedElement, declaration.Namespace);
        var requiresNullForgivingValueInitializer = !declaration.IsCollection && !IdlNaming.IsPrimitive(element) && !compilation.IsEnum(element, declaration.Namespace);
        var typedefUsings = declaration.IsCollection ? EmissionSupport.DataTypeUsings.Concat(["System.Linq"]) : EmissionSupport.DataTypeUsings;

        var writer = EmissionSupport.CreateSource(declaration.Namespace, typedefUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> IDL typedef declared in <c>{sourceIdlFileName}</c>.");
        writer.OpenBlock($"public partial class {typeName} : IEquatable<{typeName}>");

        if (declaration.IsSequence)
        {
            var sequenceSummary = $"Gets the sequence value represented by this typedef.{(declaration.Bound is int bound ? $" Its maximum number of elements is <c>{bound}</c>." : string.Empty)}";
            writer.WriteXmlSummary(sequenceSummary);
            writer.WriteLine($"[Bound({declaration.Bound ?? 0})]");
            writer.WriteLine($"public ISequence<{elementReference}> Value {{ get; }} = null!;");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty sequence typedef.");
            writer.OpenBlock($"public {typeName}()");
            writer.WriteLine($"Value = new Sequence<{elementReference}>();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with a sequence value.");
            writer.WriteXmlParam("Value", "The sequence value to store.");
            writer.OpenBlock($"public {typeName}(ISequence<{elementReference}> Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another sequence typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine($"Value = new Sequence<{elementReference}>(other.Value);");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("var hash = new HashCode();");
            writer.BlankLine();
            writer.WriteLine("hash.Add(Value.Count);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same sequence values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.WriteLine("return other is not null");
            writer.Indent();
            writer.WriteLine("&& (ReferenceEquals(this, other) || Value.SequenceEqual(other.Value));");
            writer.Unindent();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else if (declaration.IsArray)
        {
            var arrayElementIsAggregate = !IdlNaming.IsPrimitive(element) && !IsCSharpPrimitive(element) && !compilation.IsEnum(element, declaration.Namespace);
            var arrayType = $"{elementReference}[{new string(',', declaration.Dimensions.Count - 1)}]";

            writer.WriteXmlSummary("Gets or sets the array value represented by this typedef.");
            writer.WriteLine($"public {arrayType} Value {{ get; set; }} = new {elementReference}[{string.Join(", ", declaration.Dimensions)}];");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty array typedef.");
            writer.OpenBlock($"public {typeName}()");

            if (arrayElementIsAggregate)
            {
                ArraySourceEmitter.EmitAggregateInitialization(writer, "Value", elementReference, declaration.Dimensions);
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with an array value.");
            writer.WriteXmlParam("Value", "The array value to store.");
            writer.OpenBlock($"public {typeName}({arrayType} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another array typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine($"Value = ({arrayType})other.Value.Clone();");

            if (arrayElementIsAggregate)
            {
                ArraySourceEmitter.EmitAggregateCopy(writer, "Value", "other.Value", elementReference, declaration.Dimensions);
            }

            writer.CloseBlock();
            writer.BlankLine();

            var firstElementIndex = string.Join(", ", declaration.Dimensions.Select(_ => "0"));
            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("var hash = new HashCode();");
            writer.BlankLine();
            writer.WriteLine($"hash.Add(Value[{firstElementIndex}]);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same array values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();

            if (declaration.Dimensions.Count == 1)
            {
                writer.WriteLine("return Value.SequenceEqual(other.Value);");
            }
            else
            {
                writer.WriteLine("return Value.Rank == other.Value.Rank");
                writer.Indent();
                writer.WriteLine("&& Enumerable.Range(0, Value.Rank).All(dimension => Value.GetLength(dimension) == other.Value.GetLength(dimension))");
                writer.WriteLine($"&& Value.Cast<{elementReference}>().SequenceEqual(other.Value.Cast<{elementReference}>());");
                writer.Unindent();
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else
        {
            var valueSummary = $"Gets or sets the value represented by this typedef.{(declaration.IsString ? $" Its maximum length is <c>{declaration.StringBound}</c>." : string.Empty)}";
            writer.WriteXmlSummary(valueSummary);

            if (declaration.IsString)
            {
                writer.WriteLine($"[Bound({declaration.StringBound})]");
            }

            writer.WriteLine($"public {elementReference} Value {{ get; set; }}{(declaration.IsString ? " = string.Empty;" : requiresNullForgivingValueInitializer ? " = null!;" : string.Empty)}");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty typedef value.");
            writer.OpenBlock($"public {typeName}()");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with a value.");
            writer.WriteXmlParam("Value", "The value to store.");
            writer.OpenBlock($"public {typeName}({elementReference} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another typedef value.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is not null)");
            writer.WriteLine("Value = other.Value;");
            writer.CloseBlock();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("var hash = new HashCode();");
            writer.BlankLine();
            writer.WriteLine("hash.Add(Value);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same value as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Value.Equals(other.Value);");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(declaration.Namespace, declaration.Name), writer.ToString()));
    }

    private static bool IsStringType(string? typeName) =>
        typeName is not null
        && (typeName.StartsWith("string", StringComparison.Ordinal) || typeName.StartsWith("wstring", StringComparison.Ordinal));

    private static bool IsCSharpPrimitive(string typeName) => typeName is
        "sbyte" or "byte" or "short" or "ushort" or "int" or "uint" or "long" or
        "ulong" or "char" or "bool" or "float" or "double";
}
