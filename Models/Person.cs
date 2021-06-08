using System.ComponentModel.DataAnnotations;

namespace Bioscoop_Website_2021.Models
{
    public class Person
    {
        [Required(ErrorMessage = "U bent verplicht uw voornaam in te vullen")]
        public string Voornaam { get; set; }
        [Required(ErrorMessage = "U bent verplicht uw achternaam in te vullen")]
        public string Achternaam { get; set; }
       
        public string Wachtwoord { get; set; }

        [Required(ErrorMessage = "U bent verplicht uw emailadres in te vullen")]
        public string Email { get; set; }
        public int Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Gelieve een bericht in te vullen")]
        public string Bericht { get; set; }
        
    }

}
