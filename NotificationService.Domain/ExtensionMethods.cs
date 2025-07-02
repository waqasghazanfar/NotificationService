using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks if the List<string> is null or empty.
        /// </summary>
        /// <param name="list">The List<string> to check.</param>
        /// <returns>True if the list is null or contains no elements; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this List<string> list)
        {
            return list == null || list.Count == 0;
        }

        public static string ToCommaSeparatedString(this List<string> list, bool includeNullOrEmptyStrings = false)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            // If we don't want to include null or empty strings, filter them out first
            if (!includeNullOrEmptyStrings)
            {
                // Using LINQ's Where to filter out null or empty strings
                IEnumerable<string> filteredList = list.Where(s => !string.IsNullOrEmpty(s));
                return string.Join(",", filteredList);
            }
            else
            {
                // If including null/empty strings, ensure nulls are represented as empty strings for Join
                // This prevents "System.String,System.String," if there's a null in the list
                IEnumerable<string> formattedList = list.Select(s => s ?? "");
                return string.Join(",", formattedList);
            }
        }



        // --- Configuration ---
        private const string EncryptionKey = "abc123xyz"; // Replace with a strong, securely stored key.
        private static readonly byte[] Salt = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }; // "Ivan Medvedev" in ASCII

        /// <summary>
        /// Encrypts a string using AES (Advanced Encryption Standard).
        /// </summary>
        /// <param name="clearText">The string to encrypt. This is the extension parameter.</param>
        /// <returns>A Base64 encoded encrypted string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        public static string Encrypt(this string clearText)
        {
            if (clearText == null)
            {
                throw new ArgumentNullException(nameof(clearText));
            }

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                // Use Rfc2898DeriveBytes to generate a key and IV from the password and salt.
                // This adds a layer of protection against dictionary attacks.
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt, 10000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32); // 256-bit key
                encryptor.IV = pdb.GetBytes(16);  // 128-bit IV

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        // The 'using' statement will automatically call cs.FlushFinalBlock() and close the stream.
                    }
                    // Return the encrypted data as a Base64 string for easy transport.
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a string that was encrypted using the corresponding Encrypt method.
        /// </summary>
        /// <param name="cipherText">The Base64 encoded encrypted string to decrypt.</param>
        /// <returns>The original, decrypted string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        /// <exception cref="FormatException">Thrown if the input is not a valid Base64 string.</exception>
        public static string Decrypt(this string cipherText)
        {
            if (cipherText == null)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }

            // The input string might contain spaces if transmitted, which are not valid in Base64.
            // Replace them with '+' characters.
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, Salt, 10000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }

    }


  
}
