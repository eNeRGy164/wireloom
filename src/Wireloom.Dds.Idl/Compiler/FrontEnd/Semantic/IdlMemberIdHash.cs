using System.Text;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Computes the shared hash-based DDS identity of struct and union members.</summary>
internal static class IdlMemberIdHash
{
    /// <summary>Computes the 28-bit DDS member ID from its UTF-8 hash source.</summary>
    internal static int Compute(string value, Func<byte[], byte[]> computeMd5)
    {
        byte[] hash;
        try
        {
            hash = computeMd5(Encoding.UTF8.GetBytes(value));
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("The MD5 provider failed while computing a DDS member ID.", exception);
        }

        if (hash is null || hash.Length != 16)
        {
            throw new InvalidOperationException("The MD5 provider must return exactly 16 digest bytes.");
        }

        var littleEndian = unchecked((uint)(hash[0] | (hash[1] << 8) | (hash[2] << 16) | (hash[3] << 24)));
        return (int)(littleEndian & 0x0FFFFFFF);
    }
}
