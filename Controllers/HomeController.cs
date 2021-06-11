using Bioscoop_Website_2021.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Bioscoop_Website_2021.Database;
using System.Text;
using System.Security.Cryptography;

namespace Bioscoop_Website_2021.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // stel in waar de database gevonden kan worden
        //private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";
        private readonly string connectionString = "Server=172.16.160.21;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";
        // link voor in de les op school

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            ViewData["user"] = HttpContext.Session.GetString("User");

            // alle namen ophalen
            var film = GetProducts();

            // stop de namen in de html
            return View(film);
        }

        public List<string> GetNames()
        {
            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from film", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Titel"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        names.Add(Name);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }

        public List<Film> GetProducts()
        {

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Film> products = new List<Film>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from film", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Film p = new Film
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            id = Convert.ToInt32(reader["Id"]),
                            Titel = reader["Titel"].ToString(),
                            Genre = reader["Genre"].ToString(),
                            Tijdsduur = Convert.ToInt32(reader["Tijdsduur(min)"]),
                            Leeftijdsgrens = Convert.ToInt32(reader["Leeftijdsgrens"]),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Img = reader["foto"].ToString(),
                            
                        };

                        // voeg de naam toe aan de lijst met namen
                        products.Add(p);
                    }
                }
            }

            // return de lijst met namen
            return products;
        }


        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("notfound")]
        public IActionResult notfound()
        {
            return View();
        }

        [Route("Films")]
        public IActionResult Films()
        {
            // alle namen ophalen
            var film = GetProducts();            

            // stop de namen in de html
            return View(film);
        }

        [Route("Bestelpagina")]
        public IActionResult Bestelpagina()

        {
            var model = GetDates();
            var film = GetProducts();
            ViewData["film"] = film.ToList();
            return View(model);


        }

        [Route("Contactoverzicht")]
        public IActionResult Contactoverzicht()
        {
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("FAQ")]
        public IActionResult FAQ()
        {
            return View();
        }

        
        [HttpPost]
        [Route("Contact")]
        public IActionResult Contact(Person person)
        {
            // hebben we alles goed ingevuld? Dan sturen we de gebruiker door naar de succes pagina
            if (ModelState.IsValid) {

                SavePerson(person);

                return Redirect("/Success");
            }
                
           
            // niet goed? Dan sturen we de gegevens door naar de view zodat we de fouten kunnen tonen
            return View(person);
        }

        [Route("Success")]
        public IActionResult Success()
        {
            return View();
        }

        private void SavePerson(Person person)
        {
            if (person.Wachtwoord != null)
                person.Wachtwoord = ComputeSha256Hash(person.Wachtwoord);
            else
                person.Wachtwoord = "leeg";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(voornaam, achternaam, gebruikersnaam, wachtwoord, email,Telefoonnummer, bericht) VALUES(?voornaam, ?achternaam, ?gebruikersnaam, ?wachtwoord, ?email, ?Telefoonnummer, ?bericht)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.Text).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.Text).Value = person.Achternaam;
                cmd.Parameters.Add("?gebruikersnaam", MySqlDbType.Text).Value = person.Gebruikersnaam;
                cmd.Parameters.Add("?wachtwoord", MySqlDbType.Text).Value = person.Wachtwoord;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = person.Email;
                cmd.Parameters.Add("?Telefoonnummer", MySqlDbType.Int32).Value = person.Telefoonnummer;
                cmd.Parameters.Add("?bericht", MySqlDbType.Text).Value = person.Bericht;
                cmd.ExecuteNonQuery();
            }
        }


        [Route("film/{id}")]
        public IActionResult Film(string id)
        {
            var model = GetFilm(id);
            var Dates = GetDates();
            ViewData["date"] = Dates.Where(b => b.film_id == model.id).ToList();

            return View(model);

        }

        private Film GetFilm(string id)
        {
            List<Film> films = new List<Film>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from film where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Film p = new Film
                        {
                            // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                            id = Convert.ToInt32(reader["Id"]),
                            Titel = reader["Titel"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Tijdsduur = Convert.ToInt32(reader["Tijdsduur(min)"]),
                            Leeftijdsgrens = Convert.ToInt32(reader["Leeftijdsgrens"]),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Img = reader["foto"].ToString()
                        };

                        films.Add(p);
                    }
                }
            }
            return films[0];
        }

        public List<Datum> GetDates()
        {

            // maak een lege lijst waar we de namen in gaan opslaan
            List<Datum> Dates = new List<Datum>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from voorstelling", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        Datum q = new Datum

                        {
                            id = Convert.ToInt32(reader["Id"]),
                            datumtijd = DateTime.Parse(reader["datumtijd"].ToString()),
                            film_id = Convert.ToInt32(reader["film_id"]),
                            voorraad = reader["voorraad"].ToString(),

                        };

                        Dates.Add(q);
                    }
                }
            }

            // return de lijst met namen
            return Dates;
        }

        [Route("Inloggen")]
        public IActionResult Inloggen(string username, string password)
        {
            if (password == "geheim")
            {
                HttpContext.Session.SetString("User", username);
                return Redirect("/");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public IActionResult Login(string username, string password)
        {
            // hash voor "wachtwoord"
            string hash = "dc00c903852bb19eb250aeba05e534a6d211629d77d055033806b783bae09937";

            // is er een wachtwoord ingevoerd?
            if (!string.IsNullOrWhiteSpace(password))
            {

                //Er is iets ingevoerd, nu kunnen we het wachtwoord hashen en vergelijken met de hash "uit de database"
                string hashVanIngevoerdWachtwoord = ComputeSha256Hash(password);
                if (hashVanIngevoerdWachtwoord == hash)
                {
                    HttpContext.Session.SetString("User", username);
                    return Redirect("/");
                }
            }

            return View();
        }


            }


    


}



