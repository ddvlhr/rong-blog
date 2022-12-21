using System.Security.Cryptography;
using System.Text;
using Rong.Core.Models;

namespace Rong.Infra.Helper;

public static class EncryptHelper
{
    /// <summary>
    /// AES 加密
    /// </summary>
    /// <param name="text">原始字符串</param>
    /// <returns>加密字符串</returns>
    public static string AesEncrypt(string text)
    {
        var settings = AppConfigurationHelper.GetSection<EncryptSettings>(nameof(EncryptSettings));
        var sourceBytes = Encoding.UTF8.GetBytes(text);
        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Encoding.UTF8.GetBytes(settings.Key);
        aes.IV = Encoding.UTF8.GetBytes(settings.Iv);
        var transform = aes.CreateEncryptor();
        return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
    }
    
    /// <summary>
    /// AES 解密
    /// </summary>
    /// <param name="text">加密字符串</param>
    /// <returns>原始字符串</returns>
    public static string AesDecrypt(string text)
    {
        var settings = AppConfigurationHelper.GetSection<EncryptSettings>(nameof(EncryptSettings));
        var sourceBytes = Convert.FromBase64String(text);
        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Encoding.UTF8.GetBytes(settings.Key);
        aes.IV = Encoding.UTF8.GetBytes(settings.Iv);
        var transform = aes.CreateDecryptor();
        return Encoding.UTF8.GetString(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
    }
}