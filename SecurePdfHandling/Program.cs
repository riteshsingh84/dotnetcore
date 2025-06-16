using SecurePdfHandling.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IPdfEncryptionService with DI
builder.Services.AddScoped<IPdfEncryptionService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var encryptionKey = config["PdfEncryption:Key"] ?? "ThisIsASecretKey1234567890123456";
    var encryptedFilesPath = config["PdfEncryption:EncryptedFilesPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "EncryptedFiles");
    
    if (!Directory.Exists(encryptedFilesPath))
        Directory.CreateDirectory(encryptedFilesPath);

    return new PdfEncryptionService(encryptionKey, encryptedFilesPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
