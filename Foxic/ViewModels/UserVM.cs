using Foxic.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foxic.ViewModels
{
    public class UserVM
    {
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }

    }
}
