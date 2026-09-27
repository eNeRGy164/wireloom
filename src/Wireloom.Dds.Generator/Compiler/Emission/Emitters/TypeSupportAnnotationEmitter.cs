using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits primitive range and default annotations for DynamicType members.</summary>
internal static class TypeSupportAnnotationEmitter
{
    public static void Emit(GeneratedSourceWriter writer, int index, MemberEmissionPlan field)
    {
        if (field.IsOptional || field.IsExternal)
        {
            return;
        }

        var annotation = field.PrimitiveAnnotation();
        if (annotation is null)
        {
            return;
        }

        var (TypeKind, ValueProperty, DefaultValue, Minimum, Maximum, Unit) = annotation.Value;

        writer.BlankLine();
        writer.OpenBrace();
        writer.WriteLine("var annotations = new Annotations(");
        writer.Indent();
        writer.WriteLine($"TypeKind.{TypeKind},");
        writer.WriteLine($"defaultValue: new AnnotationParameterValue {{ {ValueProperty} = {DefaultValue} }},");
        writer.WriteLine($"minValue: {(Minimum is null ? "null," : $"new AnnotationParameterValue {{ {ValueProperty} = {Minimum} }},")}");
        writer.WriteLine($"maxValue: {(Maximum is null ? "null," : $"new AnnotationParameterValue {{ {ValueProperty} = {Maximum} }},")}");
        writer.WriteLine($"unit: {Unit ?? "null"});");
        writer.Unindent();
        writer.WriteLine($"result.SetMemberAnnotations({index}, annotations);");
        writer.CloseBlock();
    }
}
