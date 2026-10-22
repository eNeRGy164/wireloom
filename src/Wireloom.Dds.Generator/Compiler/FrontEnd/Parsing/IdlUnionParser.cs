using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL union declarations and branches.</summary>
internal sealed class IdlUnionParser
{
    private readonly IdlParseContext context;

    internal IdlUnionParser(IdlParseContext context) =>
        this.context = context;

    /// <summary>Parses a union declaration when one begins at the current position.</summary>
    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, bool useHashIds, ref int position, bool hasLocalAutoId = false)
    {
        var prefix = IdlTypeAnnotationPrefix.Read(declarations, position, input, context, baseOffset, useHashIds, hasLocalAutoId);
        var declarationStart = prefix.DeclarationStart;
        var unionDeclaration = UnionPattern.Match(declarations.Substring(declarationStart));
        if (!unionDeclaration.Success || prefix.IsTopic)
        {
            return false;
        }

        var unionName = unionDeclaration.Groups["name"].Value;
        var qualified = context.Qualify(unionName, currentNamespace);
        context.RecordIdentifier(input, baseOffset + declarationStart + unionDeclaration.Groups["name"].Index, unionName, currentNamespace);
        context.EnsureNewName(input, baseOffset + declarationStart, qualified);
        context.EnsureGeneratedCompanionNames(input, baseOffset + declarationStart, unionName, currentNamespace, includeUnmanaged: true);
        var discriminator = unionDeclaration.Groups["discriminator"];
        var discriminatorIdlType = NormalizeIdlType(discriminator.Value);
        var discriminatorOffset = context.MapOffset(baseOffset + declarationStart + discriminator.Index);
        var unionBody = unionDeclaration.Groups["body"];
        var branches = new List<IdlUnionBranch>();
        var branchNames = new HashSet<string>(StringComparer.Ordinal);
        var unionOffset = 0;

        while (unionOffset < unionBody.Length)
        {
            if (char.IsWhiteSpace(unionBody.Value[unionOffset]))
            {
                unionOffset++;
                continue;
            }

            var (Branch, Length) = ParseUnionBranch(
                input,
                unionBody.Value,
                unionOffset,
                baseOffset + declarationStart + unionBody.Index,
                currentNamespace,
                branchNames, qualified, prefix.UseHashIds);
            branches.Add(Branch);

            unionOffset += Length;
        }

        if (branches.Count == 0 || branches.Count(branch => branch.IsDefault) > 1)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + declarationStart), "The union must contain at least one branch and at most one default branch.");
        }

        var validationOffset = context.MapOffset(baseOffset + declarationStart);
        context.DeferUnionGeneratedNameCollisionValidation(input, validationOffset, unionName, branches);
        context.DeferUnionMemberIdValidation(input, validationOffset, branches);

        var parsedUnion = new IdlUnion(
            unionName,
            currentNamespace,
            discriminatorIdlType,
            input,
            discriminatorOffset,
            discriminatorIsEnum: false,
            discriminatorDefaultValue: null,
            branches,
            prefix.Extensibility ?? IdlExtensibilityKind.Extensible);
        context.Symbols.AddUnion(qualified, parsedUnion);
        context.Declarations.Add(new IdlUnionDeclaration(parsedUnion, Path.GetFileName(input.Path)));

        position = declarationStart + unionDeclaration.Length;

        return true;
    }

    private (IdlUnionBranch Branch, int Length) ParseUnionBranch(
        IdlInput input,
        string body,
        int offset,
        int sourceOffset,
        string? currentNamespace,
        HashSet<string> branchNames, string unionScope, bool useHashIds)
    {
        var branch = UnionBranchPattern.Match(body.Substring(offset));
        if (!branch.Success)
        {
            throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Unsupported union branch declaration.");
        }

        var branchName = branch.Groups["name"].Value;
        context.RecordIdentifier(input, sourceOffset + offset + branch.Groups["name"].Index, branchName, unionScope, IdlIdentifierScopeKind.Aggregate);
        if (!branchNames.Add(branchName))
        {
            throw new IdlException(input, context.MapOffset(sourceOffset + offset), $"Duplicate union branch: {branchName}");
        }

        var branchType = NormalizeIdlType(branch.Groups["type"].Value);
        // RTI accepts @optional on a union branch but omits it from the generated
        // C# contract and DynamicType. A union always selects one branch, so the
        // annotation does not make the selected branch independently absent.
        var field = ParseUnionBranchField(input, branchName, branchType, branch.Groups["dimensions"].Value, sourceOffset + offset, currentNamespace);
        if (useHashIds)
        {
            var metadata = new IdlMemberMetadata(
                memberId: IdlMemberIdHash.Compute(branchName),
                memberIdHashSource: branchName,
                usesAutoIdHash: true);
            field = new IdlMember(field.Name, field.Type, metadata, field.SourceInput, field.SourceOffset);
        }

        if (field.Type is IdlType.Sequence { Dimensions.Count: > 0 }
            && input.SequenceArrayMapping == IdlSequenceArrayMapping.RtiCSharp)
        {
            context.Diagnostics?.Add(new IdlDiagnostic("DDSG0105", input, field.SourceOffset,
                $"The C# binding does not support arrays of sequences without using a typedef; generated code for member '{branchName}' may not match IDL semantics."));
        }

        context.ValueBinder.DeferMemberValues(
            field,
            input,
            field.SourceOffset,
            currentNamespace,
            new IdlMemberAnnotationSyntax());

        var labels = new List<string>();

        if (branch.Groups[1].Success)
        {
            foreach (Match match in UnionCaseLabelPattern.Matches(branch.Groups[1].Value))
            {
                labels.Add(match.Groups["label"].Value.Trim());
            }
        }

        return (new IdlUnionBranch(field, labels, branch.Groups[2].Success), branch.Length);
    }

    private IdlMember ParseUnionBranchField(IdlInput input, string branchName, string branchType, string dimensionsText, int sourceOffset, string? currentNamespace)
    {
        var dimensions = dimensionsText.Length > 0
            ? context.TypeParser.ParseDimensions(input, sourceOffset, dimensionsText, currentNamespace)
            : [];

        if (IdlBuiltinTypeSyntax.IsSequenceType(branchType))
        {
            var (ElementType, Bound) = context.TypeParser.ParseSequenceType(input, sourceOffset, branchType, currentNamespace);
            var element = context.TypeParser.ParseCollectionElementType(input, sourceOffset, ElementType, currentNamespace, "Unknown union collection element type");
            var sequence = new IdlType.Sequence(element, Bound?.Value ?? 100, dimensions);
            Bound?.AddConsumer(sequence.SetBound);
            return new IdlMember(branchName, sequence, sourceInput: input, sourceOffset: context.MapOffset(sourceOffset));
        }

        if (dimensionsText.Length > 0)
        {
            var element = context.TypeParser.ParseCollectionElementType(input, sourceOffset, branchType, currentNamespace, "Unknown union array element type");
            return new IdlMember(branchName, new IdlType.Array(element, dimensions), sourceInput: input, sourceOffset: context.MapOffset(sourceOffset));
        }

        if (IdlBuiltinTypeSyntax.TryParseStringType(branchType, out var isWideString, out var boundExpression))
        {
            var bound = context.TypeParser.ParseStringBound(input, sourceOffset, branchType, currentNamespace, "Union string bound must be a positive Int32.");

            var stringType = new IdlType.StringType(isWideString, bound?.Value ?? 255, isBounded: boundExpression is not null);
            bound?.AddConsumer(stringType.SetBound);
            return new IdlMember(branchName, stringType, sourceInput: input, sourceOffset: context.MapOffset(sourceOffset));
        }

        if (IsPrimitive(branchType))
        {
            return new IdlMember(branchName, new IdlType.Primitive(NormalizeIdlType(branchType)), sourceInput: input, sourceOffset: context.MapOffset(sourceOffset));
        }

        return new IdlMember(branchName, context.ReferenceType(branchType, currentNamespace, input, sourceOffset, "Unknown union branch type"), sourceInput: input, sourceOffset: context.MapOffset(sourceOffset));
    }
}
