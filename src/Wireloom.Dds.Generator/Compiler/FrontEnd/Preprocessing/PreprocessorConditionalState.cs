namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Tracks active conditional-preprocessor branches and validates their nesting.</summary>
internal sealed class PreprocessorConditionalState
{
    private readonly Stack<ConditionalFrame> frames = new();

    internal bool IsActive { get; private set; } = true;

    /// <summary>Enters a nested conditional branch.</summary>
    internal void Enter(bool condition)
    {
        var parentActive = IsActive;
        var branchActive = parentActive && condition;
        frames.Push(new ConditionalFrame(parentActive, branchActive, false));
        IsActive = branchActive;
    }

    /// <summary>Selects an else-if branch and validates its nesting.</summary>
    internal void SelectElseIf(Func<bool> condition, IdlInput input, int offset)
    {
        if (frames.Count == 0)
        {
            throw new IdlException(input, offset, "Unexpected #elif.");
        }

        var frame = frames.Pop();
        if (frame.ElseSeen)
        {
            throw new IdlException(input, offset, "Unexpected #elif after #else.");
        }

        var branchActive = frame is { ParentActive: true, BranchTaken: false } && condition();
        frames.Push(new ConditionalFrame(frame.ParentActive, frame.BranchTaken || branchActive, false));
        IsActive = branchActive;
    }

    /// <summary>Selects the else branch and validates its nesting.</summary>
    internal void SelectElse(IdlInput input, int offset)
    {
        if (frames.Count == 0)
        {
            throw new IdlException(input, offset, "Unexpected #else.");
        }

        var frame = frames.Pop();
        if (frame.ElseSeen)
        {
            throw new IdlException(input, offset, "Unexpected second #else.");
        }

        var branchActive = frame is { ParentActive: true, BranchTaken: false };
        frames.Push(new ConditionalFrame(frame.ParentActive, true, true));
        IsActive = branchActive;
    }

    /// <summary>Closes the current conditional branch.</summary>
    internal void End(IdlInput input, int offset)
    {
        if (frames.Count == 0)
        {
            throw new IdlException(input, offset, "Unexpected #endif.");
        }

        IsActive = frames.Pop().ParentActive;
    }

    /// <summary>Ensures that all conditional branches have been closed.</summary>
    internal void EnsureComplete(IdlInput input)
    {
        if (frames.Count != 0)
        {
            throw new IdlException(input, input.Text.Length, "Unterminated preprocessor conditional.");
        }
    }

    private sealed class ConditionalFrame(bool parentActive, bool branchTaken, bool elseSeen)
    {
        public bool ParentActive { get; } = parentActive;
        public bool BranchTaken { get; } = branchTaken;
        public bool ElseSeen { get; } = elseSeen;
    }
}
