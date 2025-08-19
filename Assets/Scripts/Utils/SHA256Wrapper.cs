using Cysharp.Text;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

internal sealed class SHA256Wrapper : MonoBehaviour {
    public static string Hash(in string target) {
        string hashResult = string.Empty;
        using (SHA256 sha256 = SHA256.Create()) {
            byte[] byteCodes = Encoding.UTF8.GetBytes(target);
            byte[] hashedCodes = sha256.ComputeHash(byteCodes);
            Utf8ValueStringBuilder stringBuilder = ZString.CreateUtf8StringBuilder();
            foreach (byte b in hashedCodes) {
                stringBuilder.Append(b.ToString("x2"));
            }
            hashResult = stringBuilder.ToString();
        }
        return hashResult;
    }
}