using Application.Reports;
using Application.Services;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------
// Postavljanje connection stringa i DB konteksta
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Host=localhost;Port=5432;Database=ZalagaonicaDB;Username=postgres;Password=postgres;";
    options.UseNpgsql(connectionString);
});

// ---------------------------------------------
// Registracija servisa u DI kontejneru
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<FiskalizacijaService>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    var logger = sp.GetRequiredService<ILogger<FiskalizacijaService>>();
    var baseUri = builder.Configuration["Fiskalizacija:BaseUri"] ?? "https://api.fiskalizacija.hr";
    var authToken = builder.Configuration["Fiskalizacija:AuthToken"] ?? "";
    return new FiskalizacijaService(httpClient, logger, baseUri, authToken);
});
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<LoanService>();
builder.Services.AddScoped<CashRegisterTransactionService>();
builder.Services.AddScoped<UnitOfMeasureService>();
builder.Services.AddScoped<OutputDocumentItemService>();
builder.Services.AddScoped<ReservationStatusService>();
builder.Services.AddScoped<IncomingDocumentService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<ItemDataService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<RevenueService>();
builder.Services.AddScoped<WarehouseTypeService>();
builder.Services.AddScoped<VacationService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<VehicleStatusService>();
builder.Services.AddScoped<RepaymentService>();
builder.Services.AddScoped<PayrollService>();
builder.Services.AddScoped<SaleService>();
builder.Services.AddScoped<GeminiValuationService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<LoanRepaymentService>();
builder.Services.AddScoped<PledgeService>();
builder.Services.AddScoped<PurchaseRecordService>();
builder.Services.AddScoped<PdfReportsService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<DeliveryCostService>();
builder.Services.AddScoped<CustomerDebtService>();
builder.Services.AddScoped<DailyClosingService>();
builder.Services.AddScoped<ImportCalculationService>();
builder.Services.AddScoped<InventoryBookService>();
builder.Services.AddScoped<InventoryCountService>();
builder.Services.AddScoped<LabelService>();
builder.Services.AddScoped<PriceChangeLogService>();
builder.Services.AddScoped<WarehouseCardService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<UserManagementService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<UnifiedDocumentSearchService>();


// HttpClientFactory za vanjske HTTP pozive (npr. fiskalizacija, procjena)
builder.Services.AddHttpClient();

// ---------------------------------------------
// Dodavanje MVC kontrolera, Swagger, itd.
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClientValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReportsModule();

// ---------------------------------------------
// JWT autentikacija i autorizacija
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") ?? "SuperTajniJWTKljuc123!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ---------------------------------------------
// CORS politika (dopustiti sve za frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---------------------------------------------
// Migracije i seed admin role i usera
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Database.Migrate();

    // Seed roles
    var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Administrator");
    if (adminRole == null)
    {
        adminRole = new Role
        {
            Name = "Administrator",
            Description = "Sustavski administrator s punim pravima"
        };
        db.Roles.Add(adminRole);
    }

    var workerRole = db.Roles.FirstOrDefault(r => r.Name == "Worker");
    if (workerRole == null)
    {
        workerRole = new Role
        {
            Name = "Worker",
            Description = "Radnik zalagaonice s ograničenim pravima"
        };
        db.Roles.Add(workerRole);
    }

    db.SaveChanges();

    // Seed admin user
    var adminUser = db.UserAccounts.FirstOrDefault(u => u.Email == "admin@pawnshop.hr");
    if (adminUser == null)
    {
        var passwordHash = AuthService.HashPassword("Admin123!");
        adminUser = new UserAccount
        {
            Username = "admin",
            Email = "admin@pawnshop.hr",
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        db.UserAccounts.Add(adminUser);
        db.SaveChanges();

        db.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });
        db.SaveChanges();
    }
}

// ---------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
// Disable HTTPS redirection in Docker/Development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
