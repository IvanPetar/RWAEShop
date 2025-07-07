using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApp.ViewModels
{
    public class LogViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Log Message")]
        public string? Message { get; set; }

        [Display(Name = "Log Level")]
        public int Level { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
