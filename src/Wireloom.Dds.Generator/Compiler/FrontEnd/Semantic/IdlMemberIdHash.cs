using System.Security.Cryptography;
using System.Text;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Computes the shared hash-based DDS identity of struct and union members.</summary>
internal static class IdlMemberIdHash
{
    /// <summary>Computes the 28-bit DDS member ID from its UTF-8 hash source.</summary>
    internal static int Compute(string value)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(value));
        var littleEndian = unchecked((uint)(hash[0] | (hash[1] << 8) | (hash[2] << 16) | (hash[3] << 24)));
        return (int)(littleEndian & 0x0FFFFFFF);
    }
}
