using iTextSharp.text.pdf;
using ITShop.API.Entities;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Report;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using ITShop.API.Database;
using ITShop.API.ViewModels.Role;

namespace ITShop.API.Services
{
    public class ReportService : IReportService
    {
        public readonly ITShop_DBContext _dBContext;

        public ReportService(ITShop_DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<byte[]> GenerateProductReportData(ProductReportParameters parameters)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font bigFont = new Font(font, 36, Font.BOLD);

                Paragraph header = new Paragraph("IT Shop", bigFont);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                Paragraph title = new Paragraph("Product report generated at: " + DateTime.Now);
                document.Add(title);

                PdfPTable table = new PdfPTable(4);
                table.SpacingBefore = 10;
                float totalWidth = 500f;
                table.TotalWidth = totalWidth;
                table.WidthPercentage = 100f;
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);

                PdfPCell header1 = new PdfPCell(new Phrase("Product", boldFont));
                PdfPCell header2 = new PdfPCell(new Phrase("Producer", boldFont));
                PdfPCell header3 = new PdfPCell(new Phrase("Category", boldFont));
                PdfPCell header4 = new PdfPCell(new Phrase("Price", boldFont));

                table.AddCell(header1);
                table.AddCell(header2);
                table.AddCell(header3);
                table.AddCell(header4);

                List<Product> products = await _dBContext.Products.ToListAsync();

                products = products.OrderBy(p=>p.Price).ToList();

                foreach (Product product in products)
                {
                    ProductCategory category = await _dBContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.CategoryID);
                    ProductProducer producer = await _dBContext.ProductProducers.FirstOrDefaultAsync(x => x.Id == product.ProducerID);

                    bool match = true;

                    if (parameters != null)
                    {
                        if (parameters.Category != "All")
                        {
                            if (category.Name != parameters.Category)
                                match = false;
                        }

                        if (parameters.Producer != "All" && producer != null)
                        {
                            if (producer.Name != parameters.Producer)
                                match = false;
                        }

                        if (parameters.MinPrice.ToString() != "" && product.Price < parameters.MinPrice)
                            match = false;

                        if (parameters.MaxPrice.ToString() != "" && product.Price > parameters.MaxPrice)
                            match = false;

                    }

                    if (match)
                    {
                        table.AddCell(product.Name);
                        table.AddCell(producer?.Name);
                        table.AddCell(category?.Name);
                        table.AddCell(product.Price.ToString() + " KM");
                    }
                }

                document.Add(table);

                document.Close();

                byte[] reportData = stream.ToArray();
                return reportData;
            }
        }

        public async Task<byte[]> GenerateUserReportData(UserReportParameters parameters)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font bigFont = new Font(font, 36, Font.BOLD);


                Paragraph header = new Paragraph("IT Shop", bigFont);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                Paragraph title = new Paragraph("Users report generated at: " + DateTime.Now);
                document.Add(title);


                PdfPTable table = new PdfPTable(7);
                table.SpacingBefore = 10;
                float totalWidth = 500f;
                table.TotalWidth = totalWidth;
                table.WidthPercentage = 100f;

                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);

                PdfPCell header1 = new PdfPCell(new Phrase("First name", boldFont));
                PdfPCell header2 = new PdfPCell(new Phrase("Last name", boldFont));
                PdfPCell header3 = new PdfPCell(new Phrase("Role", boldFont));
                PdfPCell header4 = new PdfPCell(new Phrase("Username", boldFont));
                PdfPCell header5 = new PdfPCell(new Phrase("E-mail", boldFont));
                PdfPCell header6 = new PdfPCell(new Phrase("Phone number", boldFont));
                PdfPCell header7 = new PdfPCell(new Phrase("Date created", boldFont));

                table.AddCell(header1);
                table.AddCell(header2);
                table.AddCell(header3);
                table.AddCell(header4);
                table.AddCell(header5);
                table.AddCell(header6);
                table.AddCell(header7);

                List<User> users = await _dBContext.Users.ToListAsync();
                

                foreach (User user in users)
                {
                    var userrole = await _dBContext.UserRoles.FirstOrDefaultAsync(x=>x.UserId == user.Id);
                    Role role = await _dBContext.Roles.FirstOrDefaultAsync(x => x.Id == userrole.RoleId);

                    bool match = true;

                    if (parameters != null)
                    {
                        if (parameters.Role != "All")
                        {
                            if (role.Name != parameters.Role)
                                match = false;
                        }

                    }

                    if (parameters.FromDate.HasValue && parameters.ToDate.HasValue)
                    {
                        if (user.CreatedDate < parameters.FromDate.Value || user.CreatedDate > parameters.ToDate.Value)
                            match = false;
                    }

                    if (match)
                    {
                        table.AddCell(user.FirstName);
                        table.AddCell(user.LastName);
                        table.AddCell(role.Name);
                        table.AddCell(user.UserName);
                        table.AddCell(user.Email);
                        table.AddCell(user.PhoneNumber);
                        table.AddCell(user.CreatedDate.ToString());
                        
                    }
                }

                document.Add(table);

                document.Close();

                byte[] reportData = stream.ToArray();
                return reportData;
            }
        }
    }
}
