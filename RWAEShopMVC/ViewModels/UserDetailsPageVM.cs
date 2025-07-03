using RWAEshopDAL.Models;

namespace RWAEShopMVC.ViewModels
{
    public class UserDetailsPageVM
    {
        public UserRegisterVM User { get; set; }
        public List<OrderVM> Orders { get; set; }
    }
}
