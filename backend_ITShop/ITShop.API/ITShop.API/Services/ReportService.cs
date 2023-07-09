using iTextSharp.text.pdf;
using ITShop.API.Entities;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Report;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using ITShop.API.Database;
using ITShop.API.ViewModels.Role;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Text;

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

                products = products.OrderBy(p => p.Price).ToList();

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
                    var userrole = await _dBContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
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

        public async Task<byte[]> GenerateOrderReportData(OrderReportParameters parameters)
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

                Paragraph title = new Paragraph("Orders report generated at: " + DateTime.Now);
                document.Add(title);

                PdfPTable table = new PdfPTable(6); // Adjust the number of columns as per your requirements
                table.SpacingBefore = 10;
                float totalWidth = 600f;
                table.TotalWidth = totalWidth;

                float[] columnWidths = { 5f, 20f, 15f, 12f, 40f, 20f }; // Adjust the column widths as per your requirements
                table.SetWidths(columnWidths);
                table.WidthPercentage = 100f;

                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);

                PdfPCell header1 = new PdfPCell(new Phrase("ID", boldFont));
                PdfPCell header2 = new PdfPCell(new Phrase("Customer", boldFont));
                PdfPCell header3 = new PdfPCell(new Phrase("Order Date", boldFont));
                PdfPCell header4 = new PdfPCell(new Phrase("Total Price", boldFont));
                PdfPCell header5 = new PdfPCell(new Phrase("Order Items", boldFont));
                PdfPCell header6 = new PdfPCell(new Phrase("Shipping Address", boldFont));

                table.AddCell(header1);
                table.AddCell(header2);
                table.AddCell(header3);
                table.AddCell(header4);
                table.AddCell(header5);
                table.AddCell(header6);

                IQueryable<Order> ordersQuery = _dBContext.Orders.Include(o => o.User).Include(o => o.OrderItems);

                if (parameters != null)
                {
                    // Filter by date range
                    if (parameters.FromDate.HasValue && parameters.ToDate.HasValue)
                    {
                        DateTime fromDate = parameters.FromDate.Value.Date;
                        DateTime toDate = parameters.ToDate.Value.Date.AddDays(1);
                        ordersQuery = ordersQuery.Where(o => o.OrderDate >= fromDate && o.OrderDate < toDate);
                    }

                    // Filter by price range
                    if (parameters.MinPrice.HasValue)
                    {
                        ordersQuery = ordersQuery.Where(o => o.TotalTotalPrice >= parameters.MinPrice.Value);
                    }

                    if (parameters.MaxPrice.HasValue)
                    {
                        ordersQuery = ordersQuery.Where(o => o.TotalTotalPrice <= parameters.MaxPrice.Value);
                    }
                }
                else
                {
                    // No price range specified, load all orders
                    ordersQuery = ordersQuery;
                }
                List<Order> orders = await ordersQuery.ToListAsync();

                foreach (Order order in orders)
                {
                    StringBuilder productsText = new StringBuilder();
                    int itemNumber = 1;

                    foreach (OrderItems item in order.OrderItems)
                    {
                        Product product = await _dBContext.Products.FindAsync(item.ProductID);

                        if (product != null)
                        {
                            string productInfo = $"{itemNumber}: {product.Name}, Qty: {item.Quantity}, Price: {item.TotalPrice} KM";
                            productsText.AppendLine(productInfo);
                            itemNumber++;
                        }
                    }

                    PdfPCell orderIdCell = new PdfPCell(new Phrase(order.Id.ToString()));
                    PdfPCell customerCell = new PdfPCell(new Phrase($"{order.User?.FirstName} {order.User?.LastName}"));
                    PdfPCell orderDateCell = new PdfPCell(new Phrase(order.OrderDate.ToString()));
                    PdfPCell totalPriceCell = new PdfPCell(new Phrase(order.TotalTotalPrice.ToString() + " KM"));
                    PdfPCell productsCell = new PdfPCell(new Phrase(productsText.ToString()));
                    PdfPCell shippingAddressCell = new PdfPCell(new Phrase(order.ShippingAdress));

                    table.AddCell(orderIdCell);
                    table.AddCell(customerCell);
                    table.AddCell(orderDateCell);
                    table.AddCell(totalPriceCell);
                    table.AddCell(productsCell);
                    table.AddCell(shippingAddressCell);
                }

                document.Add(table);

                document.Close();

                byte[] reportData = stream.ToArray();
                return reportData;
            }
        }
    }
}