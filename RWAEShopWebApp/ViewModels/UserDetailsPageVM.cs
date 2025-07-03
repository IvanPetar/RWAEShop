using RWAEshopDAL.Models;

namespace RWAEShopWebApp.ViewModels
{
    public class UserDetailsPageVM
    {
        public UserRegisterVM User { get; set; }
        public List<OrderVM> Orders { get; set; }
    }
}
