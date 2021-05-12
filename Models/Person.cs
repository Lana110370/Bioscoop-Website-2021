using System.ComponentModel.DataAnnotations;

namespace Bioscoop_Website_2021.Models
{
    public class Person
    {
        [Required]
        public string Voornaam { get; set; }
        [Required]
        public string Achternaam { get; set; }
        [Required]
        public string Email { get; set; }
    }

}
