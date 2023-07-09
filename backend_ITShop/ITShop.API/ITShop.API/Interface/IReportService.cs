using ITShop.API.ViewModels.Report;
using static ITShop.API.Controllers.ReportController;

namespace ITShop.API.Interface
{
    public interface IReportService
    {
        public Task<byte[]> GenerateProductReportData(ProductReportParameters parameters);
        public Task<byte[]> GenerateUserReportData(UserReportParameters parameters);
        public Task<byte[]> GenerateOrderReportData(OrderReportParameters parameters);
    }
}
