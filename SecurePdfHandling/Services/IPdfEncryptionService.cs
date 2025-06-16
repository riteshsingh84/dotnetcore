using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecurePdfHandling.Services
{
    /// <summary>
    /// Interface for PDF encryption and decryption services.
    /// </summary>
    public interface IPdfEncryptionService
    {
        /// <summary>
        /// Gets the directory path where encrypted files are stored.
        /// </summary>
        string EncryptedFilesPath { get; }

        /// <summary>
        /// Validates if the uploaded file is a non-empty PDF file.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <returns>True if the file is a valid PDF; otherwise, false.</returns>
        bool ValidatePdfFile(IFormFile file);

        /// <summary>
        /// Generates a dynamic file name in the format [originalName]_yyyyMMddHHmmss.pdf.
        /// </summary>
        /// <param name="originalFileName">The original file name.</param>
        /// <returns>The dynamically generated file name.</returns>
        string GenerateDynamicFileName(string originalFileName);

        /// <summary>
        /// Encrypts the uploaded PDF file and saves it to disk.
        /// </summary>
        /// <param name="file">The uploaded PDF file.</param>
        /// <param name="dynamicFileName">The dynamic file name to use for saving.</param>
        /// <returns>The path to the saved encrypted file.</returns>
        Task<string> EncryptAndSaveAsync(IFormFile file, string dynamicFileName);

        /// <summary>
        /// Decrypts an encrypted PDF file from disk.
        /// </summary>
        /// <param name="encryptedFilePath">The path to the encrypted file.</param>
        /// <returns>The decrypted PDF file bytes.</returns>
        byte[] DecryptFile(string encryptedFilePath);
    }
}
