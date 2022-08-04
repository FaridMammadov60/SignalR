using System.ComponentModel.DataAnnotations;

namespace SiqnalR.ViewModel
{
    public class LoginVM
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
