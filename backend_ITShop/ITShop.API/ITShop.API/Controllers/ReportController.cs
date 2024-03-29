﻿using iTextSharp.text.pdf;
using ITShop.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using iTextSharp.text;
using ITShop.API.Database;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ITShop.API.ViewModels.Report;
using ITShop.API.Interface;
using Microsoft.AspNetCore.Authorization;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly ITShop_DBContext _dBContext;
        private IReportService _reportService;

        public ReportController(ITShop_DBContext dBContext, IReportService _reportService)
        {
            _dBContext = dBContext;
            this._reportService = _reportService;
        }

        [HttpPost("product-report"), Authorize(Roles = "Zaposlenik")]

        public async Task<IActionResult> GenerateProductReport([FromBody] ProductReportParameters parameters)
        {
            try
            {
                byte[] reportData = await _reportService.GenerateProductReportData(parameters);

                return File(reportData, "application/pdf", "report.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while generating the report: " + ex.Message);
            }
        }

        [HttpPost("user-report"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateUserReport([FromBody] UserReportParameters parameters)
        {
            try
            {
                byte[] reportData = await _reportService.GenerateUserReportData(parameters);

                return File(reportData, "application/pdf", "report.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while generating the report: " + ex.Message);
            }
        }

        [HttpPost("order-report") , Authorize(Roles = "Zaposlenik")]
        public async Task<IActionResult> GenerateOrderReport([FromBody] OrderReportParameters parameters)
        {
            try
            {
                byte[] reportData = await _reportService.GenerateOrderReportData(parameters);

                return File(reportData, "application/pdf", "report.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while generating the report: " + ex.Message);
            }
        }
    }
}
