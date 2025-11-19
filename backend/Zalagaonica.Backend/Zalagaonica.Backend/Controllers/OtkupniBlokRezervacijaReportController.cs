//using Application.Reports;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/reports/otkupni-blok-rezervacija")]
//public class OtkupniBlokRezervacijaReportController : ControllerBase
//{
//    private readonly IReportService _reportService;

//    public OtkupniBlokRezervacijaReportController(IReportService reportService)
//    {
//        _reportService = reportService;
//    }

//    [HttpGet("{id}")]
//    public IActionResult Get(int id)
//    {
//        var data = _reportService.GetOtkupniBlokRezervacijaData(id);
//        var pdfStream = _reportService.GenerateOtkupniBlokRezervacijaPdf(data);
//        return File(pdfStream, "application/pdf", $"OtkupniBlokRabDobaraRezervacija_{id}.pdf");
//    }
//}
