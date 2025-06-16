# SecurePdfHandling

This ASP.NET Core MVC application allows users to securely upload, encrypt, and decrypt PDF files. Features include:
- PDF file upload with validation
- Dynamic file naming
- AES encryption/decryption
- Encrypted file storage
- Decrypted PDF display in browser

## How to Run
1. Build the project: `dotnet build`
2. Run the project: `dotnet run`
3. Open your browser at the displayed URL (usually https://localhost:5001 or http://localhost:5000)

## Project Structure
- `Services/PdfEncryptionService.cs`: Handles PDF file operations and encryption logic
- `Controllers/PdfController.cs`: Handles upload, encrypt, decrypt, and display endpoints
- `Views/Pdf/`: Razor views for upload and decrypt actions

## Security
- Uses AES for robust encryption
- Only PDF files are accepted

## Prompt to create this project with the help of GitHub Copilot (Model:GPT4.1)

Create a C# ASP.NET Core application that can encrypt and decrypt PDF files. The application should have the following features:

1. File Upload: The user should be able to upload a PDF file via a browser.
2. File Validation: Validate the uploaded file to ensure that only PDF files are allowed. No other file formats should be accepted.
3. Dynamic File Naming: During the upload, the file name should be dynamically generated in the format [uploaded file name]_YYYYMMDDHHMMSS.pdf.
4. Encryption: Read the content of the uploaded PDF file and encrypt it using a secure encryption algorithm.
5. Save Encrypted File: Save the encrypted file to a specified file path.
6. Decryption: On clicking a button, read the encrypted file and decrypt it back to its original form. The decrypted file should not be saved.
7. Display PDF: Show the decrypted PDF file in a new browser tab when a button is clicked.
8. Security: The encryption/decryption algorithm must be robust enough to ensure the security of the PDF files.
9. Documentation: Ensure that all methods and classes are properly documented.
10. Dependency Injection: Use Dependency Injection (DI) to inject all services.
11. Service Interfaces: Each service class must have an interface, and the interface should be used for DI.

Implementation Details:
Service Class: Implement a service class to handle all the tasks related to encryption, decryption, file reading, and file saving.
Controller Class: The controller class should only call the methods from the service class to perform the required tasks.

Example Code Structure:

Service Class:
1. Method to upload and read the PDF file.
2. Method to validate the file format.
3. Method to generate a dynamic file name.
4. Method to encrypt the PDF file.
5. Method to save the encrypted file.
6. Method to decrypt the encrypted file.
7. Method to display the decrypted PDF file in a new browser tab.

Controller Class:
1. Endpoint to handle file upload, validate the file, generate the dynamic file name, and call the service method to encrypt and save the file.
2. Endpoint to handle the decryption request and call the service method to decrypt and display the file.

Additional Notes:

1. Ensure that the encryption algorithm used is secure and efficient.
2. The service class should be well-structured and modular to facilitate easy maintenance and testing.
3. Proper documentation should be provided for all methods and classes to ensure clarity and ease of understanding.
4. Use Dependency Injection (DI) to inject all services, ensuring a clean and maintainable codebase.
5. Each service class must have an interface, and the interface should be used for DI.
