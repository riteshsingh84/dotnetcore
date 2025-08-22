<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->


# Copilot Instructions for SecurePdfHandling

This project is an ASP.NET Core MVC application for secure PDF upload, AES encryption, decryption, and browser display. Follow these project-specific conventions and workflows to maximize productivity:

## Architecture & Key Patterns
- **Service/Controller Separation:**
	- All PDF encryption/decryption logic is in `Services/PdfEncryptionService.cs` (implements `IPdfEncryptionService`).
	- Controllers (e.g., `Controllers/PdfController.cs`) only orchestrate requests and call service methods.
- **Dependency Injection:**
	- All services are registered in `Program.cs` using interfaces. Use DI for all service access.
- **File Handling:**
	- Only PDF files are accepted (`application/pdf` MIME, `.pdf` extension).
	- Uploaded files are renamed to `[originalName]_yyyyMMddHHmmss.pdf` for uniqueness and traceability.
	- Encrypted files are stored in the `EncryptedFiles/` directory with a `.enc` extension.
- **Encryption:**
	- AES encryption is used. The key and storage path are configured in `Program.cs` (see `PdfEncryption:Key` and `PdfEncryption:EncryptedFilesPath`).
	- The IV is prepended to the encrypted file for decryption.
- **Views:**
	- Upload and decrypt actions are handled in `Views/Pdf/Upload.cshtml`.
	- Decrypted PDFs are streamed directly to the browser; decrypted files are not saved.

## Developer Workflows
- **Build:**
	- `dotnet build` (from the `SecurePdfHandling` directory)
- **Run:**
	- `dotnet run` (default: https://localhost:5001)
- **Test:**
	- No unit tests are present in this project. (See `WebHookPoc` for test patterns.)
- **Debug:**
	- Use standard ASP.NET Core debugging. All errors are routed to `Views/Shared/Error.cshtml`.

## Project-Specific Conventions
- **Service interfaces:** Every service must have an interface and be injected via DI.
- **File validation:** Always check both MIME type and extension.
- **Dynamic file naming:** Use the provided helper in `PdfEncryptionService`.
- **Security:** Never store decrypted files; only stream to browser.
- **Documentation:** All public methods/classes are XML-documented.

## Example: Upload/Encrypt/Decrypt Flow
1. User uploads PDF via `/Pdf/Upload`.
2. Controller validates file, generates dynamic name, calls `EncryptAndSaveAsync`.
3. Encrypted file is saved as `.enc` in `EncryptedFiles/`.
4. User can decrypt/view by submitting the encrypted file name; controller streams decrypted PDF to browser.

## Reference Files
- `Services/PdfEncryptionService.cs`, `Services/IPdfEncryptionService.cs`
- `Controllers/PdfController.cs`
- `Views/Pdf/Upload.cshtml`
- `Program.cs` (for DI and config)

---
If you are unsure about a workflow or pattern, check the above files for concrete examples. For new features, follow the service/controller separation and always use DI.
