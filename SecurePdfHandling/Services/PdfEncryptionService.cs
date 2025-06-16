using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecurePdfHandling.Services
{
    /// <summary>
    /// Service for handling PDF file validation, encryption, decryption, and file operations.
    /// </summary>
    public class PdfEncryptionService : IPdfEncryptionService
    {
        private readonly string _encryptionKey;
        private readonly string _encryptedFilesPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfEncryptionService"/> class.
        /// </summary>
        /// <param name="encryptionKey">The AES encryption key.</param>
        /// <param name="encryptedFilesPath">The directory path to store encrypted files.</param>
        public PdfEncryptionService(string encryptionKey, string encryptedFilesPath)
        {
            _encryptionKey = encryptionKey;
            _encryptedFilesPath = encryptedFilesPath;
        }

        /// <summary>
        /// Gets the directory path where encrypted files are stored.
        /// </summary>
        public string EncryptedFilesPath => _encryptedFilesPath;

        /// <summary>
        /// Validates if the uploaded file is a non-empty PDF file.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <returns>True if the file is a valid PDF; otherwise, false.</returns>
        public bool ValidatePdfFile(IFormFile file)
        {
            return file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ".pdf" && file.ContentType == "application/pdf";
        }

        /// <summary>
        /// Generates a dynamic file name in the format [originalName]_yyyyMMddHHmmss.pdf.
        /// </summary>
        /// <param name="originalFileName">The original file name.</param>
        /// <returns>The dynamically generated file name.</returns>
        public string GenerateDynamicFileName(string originalFileName)
        {
            var name = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{SanitizeFileName(name)}_{timestamp}.pdf";
        }

        /// <summary>
        /// Encrypts the uploaded PDF file and saves it to disk.
        /// </summary>
        /// <param name="file">The uploaded PDF file.</param>
        /// <param name="dynamicFileName">The dynamic file name to use for saving.</param>
        /// <returns>The path to the saved encrypted file.</returns>
        public async Task<string> EncryptAndSaveAsync(IFormFile file, string dynamicFileName)
        {
            var filePath = Path.Combine(_encryptedFilesPath, dynamicFileName + ".enc");
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var encrypted = Encrypt(memoryStream.ToArray());
                await File.WriteAllBytesAsync(filePath, encrypted);
            }
            return filePath;
        }

        /// <summary>
        /// Decrypts an encrypted PDF file from disk.
        /// </summary>
        /// <param name="encryptedFilePath">The path to the encrypted file.</param>
        /// <returns>The decrypted PDF file bytes.</returns>
        public byte[] DecryptFile(string encryptedFilePath)
        {
            var encryptedBytes = File.ReadAllBytes(encryptedFilePath);
            return Decrypt(encryptedBytes);
        }

        /// <summary>
        /// Encrypts the given byte array using AES encryption.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns>The encrypted data with IV prepended.</returns>
        private byte[] Encrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.GenerateIV();
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts the given byte array using AES decryption.
        /// </summary>
        /// <param name="encryptedData">The encrypted data with IV prepended.</param>
        /// <returns>The decrypted data.</returns>
        private byte[] Decrypt(byte[] encryptedData)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(encryptedData, 0, iv, 0, iv.Length);
                aes.IV = iv;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length), aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.CopyTo(ms);
                    }
                    return ms.ToArray();
                }
            }
        }

        private string SanitizeFileName(string name)
        {
            return Regex.Replace(name, "[^a-zA-Z0-9_-]", "");
        }


    }
}
