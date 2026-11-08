using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses declared names in non-emitted interface bodies without binding types.</summary>
/// <param name="context">The destination for source-mapped identifier facts.</param>
/// <param name="input">The original IDL input.</param>
/// <param name="text">The preprocessed interface body.</param>
/// <param name="baseOffset">The body's offset in the preprocessed input.</param>
/// <param name="isDdsService">Whether the interface declares DDS service semantics.</param>
internal sealed class IdlInterfaceIdentifierParser(IdlParseContext context, IdlInput input, string text, int baseOffset, bool isDdsService)
{
    private const int MaximumTypeNesting = 64;
    private readonly List<Token> tokens = Tokenize(text, context.CancellationToken);
    private int position;
    private int typeNesting;

    /// <summary>Records interface members and their nested declaration scopes.</summary>
    /// <param name="scope">The qualified IDL name of the containing interface.</param>
    internal void Parse(string scope)
    {
        while (position < tokens.Count && !Is("}"))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            SkipAnnotations();

            if (Is("const"))
            {
                position++;
                ParseType(scope);
                RecordName(scope);
                Expect("=");
                SkipExpression(";");
                Expect(";");
            }
            else if (Is("typedef"))
            {
                position++;
                ParseType(scope);
                ParseDeclarators(scope);
            }
            else if (Is("struct") || Is("valuetype") || Is("exception") || Is("enum") || Is("union"))
            {
                ParseConstructedType(scope);
                Expect(";");
            }
            else if (Is("readonly") || Is("attribute"))
            {
                Consume("readonly");
                Expect("attribute");
                ParseType(scope);
                ParseDeclarators(scope);
            }
            else
            {
                Consume("oneway");
                ParseType(scope);
                var operation = RecordName(scope);
                Expect("(");
                ParseParameters(context.Qualify(operation, scope));
                SkipClauses();
                Expect(";");
            }
        }
    }

    private void ParseParameters(string scope)
    {
        if (Consume(")"))
        {
            return;
        }

        do
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            SkipAnnotations();
            if (Is("in") || Is("out") || Is("inout"))
            {
                position++;
            }

            ParseType(scope);
            RecordName(scope, IdlIdentifierScopeKind.Operation);
            SkipDimensions();
        }
        while (Consume(","));

        Expect(")");
    }

    private void ParseDeclarators(string scope)
    {
        do
        {
            RecordName(scope);
            SkipDimensions();
        }
        while (Consume(","));

        SkipClauses();
        Expect(";");
    }

    private void SkipClauses()
    {
        // These clauses contain references or literals, not declarations.
        while (Is("raises") || Is("context") || Is("getraises") || Is("setraises"))
        {
            position++;
            if (!Is("("))
            {
                throw Failure("Expected '(' after an interface member clause.");
            }

            SkipGroup();
        }
    }

    private void ParseType(string scope)
    {
        SkipAnnotations();
        if (Is("struct") || Is("valuetype") || Is("exception") || Is("enum") || Is("union"))
        {
            ParseConstructedType(scope);
            return;
        }

        Consume("::");
        var type = ReadIdentifier().Text;
        if (type == "unsigned")
        {
            var width = ReadIdentifier().Text;
            if (width == "long")
            {
                Consume("long");
            }
        }
        else if (type == "long")
        {
            if (!Consume("long"))
            {
                Consume("double");
            }
        }

        while (Consume("::"))
        {
            ReadIdentifier();
        }

        if (Consume("<"))
        {
            var depth = 1;
            while (depth > 0)
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                if (Is("(") || Is("["))
                {
                    SkipGroup();
                }
                else
                {
                    var token = Read();
                    if (token.Text == "<")
                    {
                        depth++;
                    }
                    else if (token.Text == ">")
                    {
                        depth--;
                    }
                    else if (token.Text == ">>" && depth >= 2)
                    {
                        depth -= 2;
                    }
                }
            }
        }
    }

    private void ParseConstructedType(string scope)
    {
        if (typeNesting >= MaximumTypeNesting)
        {
            throw Failure($"Interface type declarations exceed the supported nesting depth of {MaximumTypeNesting}.");
        }

        typeNesting++;
        try
        {
            ParseConstructedTypeBody(scope);
        }
        finally
        {
            typeNesting--;
        }
    }

    private void ParseConstructedTypeBody(string scope)
    {
        var kind = Read().Text;
        var name = RecordName(scope);
        var nestedScope = context.Qualify(name, scope);

        if (Is(";"))
        {
            return;
        }

        if (kind == "union")
        {
            Expect("switch");
            Expect("(");
            ParseType(scope);
            Expect(")");
        }
        else if (Consume(":"))
        {
            // Inheritance names are references; interface bodies do not bind them.
            SkipExpression("{");
        }

        Expect("{");
        while (!Is("}"))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            SkipAnnotations();
            if (kind == "enum")
            {
                // IDL enumerators belong to the enclosing declaration scope.
                RecordName(scope);
                if (Consume("="))
                {
                    SkipExpression(",", "}");
                }

                if (!Consume(","))
                {
                    break;
                }
            }
            else
            {
                while (Is("case") || Is("default"))
                {
                    position++;
                    SkipExpression(":");
                    Expect(":");
                }

                if (Is("public") || Is("private"))
                {
                    position++;
                }

                if (kind == "valuetype" && Consume("factory"))
                {
                    var factory = RecordName(nestedScope);
                    Expect("(");
                    ParseParameters(context.Qualify(factory, nestedScope));
                    SkipClauses();
                    Expect(";");
                    continue;
                }

                ParseType(nestedScope);
                ParseDeclarators(nestedScope);
            }
        }

        Expect("}");
    }

    private void SkipDimensions()
    {
        while (Is("["))
        {
            SkipGroup();
        }
    }

    private void SkipAnnotations()
    {
        while (Consume("@"))
        {
            ReadIdentifier();
            while (Consume("::"))
            {
                ReadIdentifier();
            }

            if (Is("("))
            {
                SkipGroup();
            }
        }
    }

    private void SkipExpression(params IReadOnlyList<string> terminators)
    {
        while (position < tokens.Count && !terminators.Contains(tokens[position].Text))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            if (Is("(") || Is("[") || Is("{"))
            {
                SkipGroup();
            }
            else if (Is("}"))
            {
                throw Failure("Unexpected end of an interface member declaration.");
            }
            else
            {
                position++;
            }
        }
    }

    private void SkipGroup()
    {
        var closingTokens = new Stack<string>();
        do
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            var token = Read().Text;
            switch (token)
            {
                case "(":
                    closingTokens.Push(")");
                    break;
                case "[":
                    closingTokens.Push("]");
                    break;
                case "{":
                    closingTokens.Push("}");
                    break;
                case ")" or "]" or "}":
                    if (closingTokens.Pop() != token)
                    {
                        throw Failure("Mismatched delimiters in an interface body.");
                    }

                    break;
            }
        }
        while (closingTokens.Count > 0);
    }

    private string RecordName(string scope, IdlIdentifierScopeKind scopeKind = IdlIdentifierScopeKind.Interface)
    {
        var token = ReadIdentifier();
        if (!isDdsService)
        {
            scopeKind = IdlIdentifierScopeKind.NonServiceInterface;
        }

        context.RecordIdentifier(input, baseOffset + token.Offset, token.Text, scope, scopeKind);
        return token.Text;
    }

    private Token ReadIdentifier()
    {
        var token = Read();
        if (!token.IsIdentifier)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + token.Offset), "Expected a declared identifier in an interface body.");
        }

        return token;
    }

    private Token Read()
    {
        if (position >= tokens.Count)
        {
            throw Failure("Unexpected end of an interface body.");
        }

        return tokens[position++];
    }

    private bool Is(string value) => position < tokens.Count && tokens[position].Text == value;

    private bool Consume(string value)
    {
        if (!Is(value))
        {
            return false;
        }

        position++;
        return true;
    }

    private void Expect(string value)
    {
        if (!Consume(value))
        {
            throw Failure($"Expected '{value}' in an interface body.");
        }
    }

    private IdlException Failure(string message)
    {
        var offset = text.Length;
        if (position < tokens.Count)
        {
            offset = tokens[position].Offset;
        }

        return new IdlException(input, context.MapOffset(baseOffset + offset), message);
    }

    private static List<Token> Tokenize(string source, CancellationToken cancellationToken)
    {
        var result = new List<Token>();
        var index = 0;
        while (index < source.Length)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (char.IsWhiteSpace(source[index]))
            {
                index++;
                continue;
            }

            var start = index++;
            var identifier = char.IsLetter(source[start]) || source[start] == '_';
            if (identifier || char.IsDigit(source[start]))
            {
                while (index < source.Length && (char.IsLetterOrDigit(source[index]) || source[index] == '_' || (!identifier && source[index] == '\'')))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    index++;
                }
            }
            else if (source[start] is '"' or '\'')
            {
                while (index < source.Length)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var character = source[index++];
                    if (character == '\\' && index < source.Length)
                    {
                        index++;
                    }
                    else if (character == source[start])
                    {
                        break;
                    }
                }
            }
            else if (index < source.Length && source[start] == source[index] && source[start] is ':' or '<' or '>')
            {
                index++;
            }

            result.Add(new Token(source.Substring(start, index - start), start, identifier));
        }

        return result;
    }

    private readonly struct Token(string text, int offset, bool isIdentifier)
    {
        /// <summary>Gets the original token spelling.</summary>
        internal string Text { get; } = text;

        /// <summary>Gets the token offset in the interface body.</summary>
        internal int Offset { get; } = offset;

        /// <summary>Gets whether this token can occupy an identifier position.</summary>
        internal bool IsIdentifier { get; } = isIdentifier;
    }
}
