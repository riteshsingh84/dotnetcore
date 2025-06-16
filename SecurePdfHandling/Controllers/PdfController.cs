using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using SecurePdfHandling.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SecurePdfHandling.Controllers
{
    /// <summary>
    /// Controller for handling PDF upload, encryption, decryption, and display operations.
    /// </summary>
    public class PdfController : Controller
    {
        private readonly IPdfEncryptionService _pdfService;
        private readonly string _encryptedFilesPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfController"/> class.
        /// </summary>
        /// <param name="pdfService">The PDF encryption service injected via DI.</param>
        public PdfController(IPdfEncryptionService pdfService)
        {
            _pdfService = pdfService;
            _encryptedFilesPath = pdfService.EncryptedFilesPath;
        }

        /// <summary>
        /// Displays the PDF upload form.
        /// </summary>
        /// <returns>The upload view.</returns>
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// Handles the PDF file upload, validation, encryption, and saving.
        /// </summary>
        /// <param name="file">The uploaded PDF file.</param>
        /// <returns>The upload view with status messages.</returns>
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!_pdfService.ValidatePdfFile(file))
            {
                ViewBag.Error = "Invalid file. Please upload a valid PDF.";
                return View();
            }
            var dynamicFileName = _pdfService.GenerateDynamicFileName(file.FileName);
            await _pdfService.EncryptAndSaveAsync(file, dynamicFileName);
            ViewBag.Message = "File uploaded and encrypted successfully!";
            ViewBag.EncryptedFileName = dynamicFileName + ".enc";
            return View();
        }

        /// <summary>
        /// Decrypts the specified encrypted PDF file and returns it for display in the browser.
        /// </summary>
        /// <param name="fileName">The name of the encrypted file to decrypt and display.</param>
        /// <returns>The decrypted PDF file as a FileResult, or NotFound if the file does not exist.</returns>
        [HttpGet]
        public IActionResult DecryptAndDisplay(string fileName)
        {
            var encryptedFilePath = Path.Combine(_encryptedFilesPath, fileName);
            if (!System.IO.File.Exists(encryptedFilePath))
                return NotFound();
            var decryptedBytes = _pdfService.DecryptFile(encryptedFilePath);
            return File(decryptedBytes, "application/pdf");
        }
    }
}
