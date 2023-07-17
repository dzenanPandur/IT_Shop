using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ITShop.API.HubConfig
{
    public partial class MyHub : Hub
    {

        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }
        protected IHubContext<MyHub> _context;

        public MyHub(ITShop_DBContext dBContext, UserManager<User> userManager, IAuthContext authContext, IHubContext<MyHub> context)
        {
            _dBContext = dBContext;
            _context = context;
            UserManager = userManager;
            AuthContext = authContext;
        }

        public async Task SendNewProductNotification(string name)
        {
            await _context.Clients.All.SendAsync("newProductNotification", name);
        }
    }
}
