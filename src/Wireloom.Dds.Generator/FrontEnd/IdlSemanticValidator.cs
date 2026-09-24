using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Wireloom;

using static Wireloom.IdlCompiler;

/// <summary>Owns front-end validation that does not produce target code.</summary>
internal sealed class IdlSemanticValidator
{
    private readonly IdlSymbolTable symbols;

    public IdlSemanticValidator(IdlSymbolTable symbols)
    {
        this.symbols = symbols;
    }

    public void EnsureNewName(IdlInput input, int offset, string name)
    {
        if (!symbols.AddName(name) || symbols.ContainsEnum(name) || symbols.ContainsTypedef(name))
        {
            throw new IdlException(input, offset, $"Duplicate type: {name}");
        }
    }

    public void ValidateTypedef(IdlInput input, int offset, string name)
    {
        ValidateTypedef(input, offset, name, new HashSet<string>(StringComparer.Ordinal));
    }

    public int ResolveBound(
        IdlInput input,
        int offset,
        string text,
        string? currentNamespace,
        string diagnosticName = "Collection bound")
    {
        BigInteger value;
        try
        {
            value = IdlConstantExpressionEvaluator.Evaluate(text, symbols, currentNamespace);
        }
        catch (FormatException)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must resolve to a positive constant: {text.Trim()}");
        }

        if (value <= 0 || value > int.MaxValue)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must be a positive Int32: {text.Trim()}");
        }

        return (int)value;
    }

    public static void ValidateMemberIds(IdlInput input, int offset, IReadOnlyList<IdlMember> fields)
    {
        var duplicateId = fields
            .Where(field => field.Metadata.MemberId is not null)
            .GroupBy(field => field.Metadata.MemberId!.Value)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateId is not null)
        {
            throw new IdlException(input, offset, $"Duplicate member ID: {duplicateId.Key}");
        }
    }

    private void ValidateTypedef(IdlInput input, int offset, string name, HashSet<string> activeAliases)
    {
        if (!symbols.TryGetTypedef(name, out var alias))
        {
            return;
        }

        if (!activeAliases.Add(name))
        {
            throw new IdlException(input, offset, $"Typedef alias cycle detected: {name}");
        }

        if (alias.IsString)
        {
            activeAliases.Remove(name);
            return;
        }

        var target = alias.IsCollection ? alias.ElementType! : alias.Target;
        if (!target.StartsWith("string", StringComparison.Ordinal)
            && !target.StartsWith("wstring", StringComparison.Ordinal)
            && !IsPrimitive(target))
        {
            var qualifiedTarget = ResolveTypeName(target, alias.Namespace);
            if (symbols.ContainsTypedef(qualifiedTarget))
            {
                ValidateTypedef(input, offset, qualifiedTarget, activeAliases);
            }
            else if (!symbols.ContainsEnum(qualifiedTarget) && !symbols.ContainsName(qualifiedTarget))
            {
                throw new IdlException(input, offset, $"Unknown typedef target: {target}");
            }
        }

        activeAliases.Remove(name);
    }
}
