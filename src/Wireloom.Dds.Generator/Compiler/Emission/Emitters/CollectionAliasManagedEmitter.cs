using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits managed collection and value typedef declarations.</summary>
internal static class CollectionAliasManagedEmitter
{
    /// <summary>Emits the managed typedef document.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlTypedef declaration, GeneratedTypeNames names, CollectionAliasEmissionPlan plan, string sourceIdlFileName)
    {
        var typeName = names.ManagedTypeName;
        string? resolvedElement;
        if (plan.IsString)
        {
            resolvedElement = "string";
        }
        else
        {
            resolvedElement = plan.ElementType;
        }

        var elementReference = IdlNaming.TypeReference(resolvedElement, declaration.Namespace);
        var requiresNullForgivingValueInitializer = !declaration.IsCollection && plan is { IsAggregate: true, IsPrimitive: false, IsEnum: false };
        var writer = EmissionSupport.CreateSource(declaration.Namespace, EmissionSupport.DataTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> IDL typedef declared in <c>{sourceIdlFileName}</c>.");
        writer.OpenBlock($"public partial class {typeName} : global::System.IEquatable<{typeName}>");

        if (plan.IsSequence)
        {
            var sequenceSummary = $"Gets the sequence value represented by this typedef.{(declaration.Bound is int bound ? $" Its maximum number of elements is <c>{bound}</c>." : string.Empty)}";
            writer.WriteXmlSummary(sequenceSummary);
            writer.WriteXmlRemarks("Use the mutable sequence instance to add or remove elements; the property itself is getter-only. For an unbounded IDL sequence, Wireloom currently generates an effective limit of 100 elements. RTI uses the bound from the type metadata when processing DDS data.");
            writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_csharp/namespaceOmg_1_1Types.html", "RTI Connext 7.7.0 ISequence API");
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
            writer.WriteXmlRemarks("The typedef stores the supplied sequence reference; it does not make a copy.");
            writer.OpenBlock($"public {typeName}(ISequence<{elementReference}> Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another sequence typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.WriteXmlRemarks("The constructor creates a new sequence container. If <paramref name=\"other\"/> is null, it returns without creating the sequence and <see cref=\"Value\"/> remains null.");
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
            writer.WriteLine("var hash = new global::System.HashCode();");
            writer.BlankLine();
            writer.WriteLine("hash.Add(Value.Count);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same sequence values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.WriteXmlReturns("<see langword=\"true\"/> when both sequences contain equal values in the same order; otherwise, <see langword=\"false\"/>.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.WriteLine("return other is not null");
            writer.Indent();
            writer.WriteLine("&& (global::System.Object.ReferenceEquals(this, other) || global::System.Linq.Enumerable.SequenceEqual(Value, other.Value));");
            writer.Unindent();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Formats this typedef as readable text.");
            writer.WriteXmlReturns("A readable string formatted by this typedef's type-support instance.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else if (plan.IsArray)
        {
            var arrayElementIsAggregate = plan.CollectionElementIsAggregate;
            var arrayType = $"{elementReference}[{new string(',', declaration.Dimensions.Count - 1)}]";

            writer.WriteXmlSummary("Gets or sets the array value represented by this typedef.");
            writer.WriteLine($"public {arrayType} Value {{ get; set; }} = new {elementReference}[{string.Join(", ", declaration.Dimensions)}];");
            writer.BlankLine();

            var arrayElementCount = declaration.Dimensions.Aggregate(1, (count, dimension) => count * dimension);
            writer.WriteXmlSummary($"Initializes a fixed-size array typedef with dimensions {string.Join(" × ", declaration.Dimensions)} ({arrayElementCount} elements); each element starts at its default value.");
            writer.OpenBlock($"public {typeName}()");

            if (arrayElementIsAggregate)
            {
                ArraySourceEmitter.EmitAggregateInitialization(writer, "Value", elementReference, declaration.Dimensions);
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with an array value.");
            writer.WriteXmlParam("Value", "The array value to store.");
            writer.WriteXmlRemarks("The typedef stores the supplied array reference; it does not make a copy.");
            writer.OpenBlock($"public {typeName}({arrayType} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another array typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.WriteXmlRemarks($"The constructor creates a new array container{(arrayElementIsAggregate ? " and copies aggregate elements into new instances" : string.Empty)}. When <paramref name=\"other\"/> is null, the initially allocated array remains in place.");
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
            writer.WriteLine("var hash = new global::System.HashCode();");
            writer.BlankLine();
            writer.WriteLine($"hash.Add(Value[{firstElementIndex}]);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same array values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.WriteXmlReturns("<see langword=\"true\"/> when both arrays have the same dimensions and equal values; otherwise, <see langword=\"false\"/>.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (global::System.Object.ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();

            if (declaration.Dimensions.Count == 1)
            {
                writer.WriteLine("return global::System.Linq.Enumerable.SequenceEqual(Value, other.Value);");
            }
            else
            {
                writer.WriteLine("return Value.Rank == other.Value.Rank");
                writer.Indent();
                writer.WriteLine("&& global::System.Linq.Enumerable.All(global::System.Linq.Enumerable.Range(0, Value.Rank), dimension => Value.GetLength(dimension) == other.Value.GetLength(dimension))");
                writer.WriteLine($"&& global::System.Linq.Enumerable.SequenceEqual(global::System.Linq.Enumerable.Cast<{elementReference}>(Value), global::System.Linq.Enumerable.Cast<{elementReference}>(other.Value));");
                writer.Unindent();
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Formats this typedef as readable text.");
            writer.WriteXmlReturns("A readable string formatted by this typedef's type-support instance.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else
        {
            var valueSummary = "Gets or sets the value represented by this typedef.";
            if (declaration is { IsString: true, IsStringBounded: true })
            {
                valueSummary += $" Its IDL bound is <c>{declaration.StringBound}</c> characters.";
            }

            writer.WriteXmlSummary(valueSummary);

            if (declaration.IsString)
            {
                var stringKind = declaration.IsWideString ? "wide" : "narrow";
                var encoding = declaration.IsWideString ? "UTF-16" : "UTF-8";
                var stringRemarks = declaration.IsStringBounded
                    ? $"The bound on this {stringKind} IDL string counts characters. RTI encodes {stringKind} IDL strings as {encoding} by default. The generated C# property does not check the bound when assigned."
                    : $"This unbounded {stringKind} IDL string has an effective limit of 255 characters. RTI encodes {stringKind} IDL strings as {encoding} by default. The generated C# property does not enforce the effective limit when assigned.";
                writer.WriteXmlRemarks(stringRemarks);
                writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm", "RTI Connext 7.7.0 string and wide-string bounds");
            }

            if (declaration.IsString)
            {
                writer.WriteLine($"[Bound({declaration.StringBound})]");
            }

            writer.WriteLine($"public {elementReference} Value {{ get; set; }}{(declaration.IsString ? " = string.Empty;" : requiresNullForgivingValueInitializer ? " = null!;" : string.Empty)}");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef value to its default value.");
            writer.OpenBlock($"public {typeName}()");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with a value.");
            writer.WriteXmlParam("Value", "The value to store.");
            if (requiresNullForgivingValueInitializer || plan.IsString)
            {
                writer.WriteXmlRemarks("The constructor stores the supplied reference as provided; it does not make a copy.");
            }
            writer.OpenBlock($"public {typeName}({elementReference} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another typedef value.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.WriteXmlRemarks("When <paramref name=\"other\"/> is null, the constructor returns without copying and keeps its property initializer. An aggregate value is copied through its generated copy constructor.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is not null)");
            if (plan.IsAggregate)
            {
                writer.WriteLine($"Value = other.Value is null ? null! : new {elementReference}(other.Value);");
            }
            else
            {
                writer.WriteLine("Value = other.Value;");
            }

            writer.CloseBlock();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("var hash = new global::System.HashCode();");
            writer.BlankLine();
            writer.WriteLine("hash.Add(Value);");
            writer.BlankLine();
            writer.WriteLine("return hash.ToHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same value as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.WriteXmlReturns("<see langword=\"true\"/> when both typedefs have equal values; otherwise, <see langword=\"false\"/>.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (global::System.Object.ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Value.Equals(other.Value);");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Formats this typedef as readable text.");
            writer.WriteXmlReturns("A readable string formatted by this typedef's type-support instance.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        writer.CloseBlock();

        return [new GeneratedIdlSource(names.Managed.HintName, writer.ToString())];
    }

}
