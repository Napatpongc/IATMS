using System.Security.Cryptography;
using System.Text;
namespace IATMS.Components
{
    public class Cryptography
    {
        public static string encryptStrAndToBase64(string enStr, string keyStr, string ivStr)
        {
            byte[] bytes = encrypt(Encoding.UTF8.GetBytes(enStr), keyStr, ivStr);
            return Convert.ToBase64String(bytes);
        }

        public static string decryptStrAndFromBase64(string deStr, string keyStr, string ivStr)
        {
            byte[] bytes = decrypt(Convert.FromBase64String(deStr), keyStr, ivStr);
            return Encoding.UTF8.GetString(bytes);
        }

        static byte[] decrypt(byte[] bytes, byte[] keyBytes, byte[] ivBytes)
        {
            //RijndaelManaged aes = new RijndaelManaged();
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.IV = ivBytes;
            aes.Key = keyBytes;
            //aes.Mode = CipherMode.CBC;
            //aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = aes.CreateDecryptor();
            return transform.TransformFinalBlock(bytes, 0, bytes.Length);
        }
        public static byte[] decrypt(byte[] bytes, string keyStr, string ivStr)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] ivBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(ivStr));
            byte[] keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyStr));

            return decrypt(bytes, keyBytes, ivBytes);
        }
        public static byte[] encrypt(byte[] bytes, string keyStr, string ivStr)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] ivBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(ivStr));
            byte[] keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyStr));

            return encrypt(bytes, keyBytes, ivBytes);
        }

        static byte[] encrypt(byte[] bytes, byte[] keyBytes, byte[] ivBytes)
        {
            //RijndaelManaged aes = new RijndaelManaged();
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.IV = ivBytes;
            aes.Key = keyBytes;
            //aes.Mode = CipherMode.CBC;
            //aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform transform = aes.CreateEncryptor();
            return transform.TransformFinalBlock(bytes, 0, bytes.Length);
        }
    }
}
