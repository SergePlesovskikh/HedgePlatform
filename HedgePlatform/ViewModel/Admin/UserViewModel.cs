using System.ComponentModel.DataAnnotations;

namespace HedgePlatform.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        public string Psw { get; set; }
    }
}
