using System.Security.Cryptography;
using System.Text;

namespace Raccoon.Ninja.Tools.Uuid;

public static class Guid5
{
    private static readonly UTF8Encoding Utf8EncodingWithoutBom = new (encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    private static readonly SHA256 Sha256 = SHA256.Create();
    
    /// <summary>
    /// Generates a deterministic GUID (UUID v5) based on the arguments passed to the method.
    /// </summary>
    /// <param name="args">The arguments to be used to generate the GUID.</param>
    /// <returns>The generated GUID.</returns>
    /// <throws>ArgumentNullException if no arguments are passed to the function.</throws>
    public static Guid NewGuid(params object[] args)
    {
        if (args is null || args.Length == 0)
            throw new ArgumentNullException(nameof(args), "Arguments must not be empty.");

        var hash = Sha256.ComputeHash(Utf8EncodingWithoutBom.GetBytes(string.Join("_", args)));
        return new Guid(hash[..16]);
    }
}