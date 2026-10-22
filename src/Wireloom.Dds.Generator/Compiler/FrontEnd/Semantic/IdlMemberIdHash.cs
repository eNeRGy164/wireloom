using System.Security.Cryptography;
using System.Text;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Computes the shared hash-based DDS identity of struct and union members.</summary>
internal static class IdlMemberIdHash
{
    /// <summary>Computes the 28-bit DDS member ID from its UTF-8 hash source.</summary>
    internal static int Compute(string value)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
        var littleEndian = (uint)(hash[0] | (hash[1] << 8) | (hash[2] << 16) | (hash[3] << 24));
        return (int)(littleEndian & 0x0FFFFFFF);
    }
}
