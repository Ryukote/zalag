using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Application.Reports;

namespace Application.Reports
{
    public static class ReportsModuleRegistrar
    {
        public static IServiceCollection AddReportsModule(this IServiceCollection services)
        {
            // QuestPDF licensing and configuration
            QuestPDF.Settings.License = LicenseType.Community;

            // Register the main ReportService
            services.AddScoped<ReportService>();

            return services;
        }
    }
}
