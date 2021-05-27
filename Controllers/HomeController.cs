using Bioscoop_Website_2021.Models;
using Microsoft.AspNetCore.Mvc;using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using Bioscoop_Website_2021.Database;

namespace Bioscoop_Website_2021.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // stel in waar de database gevonden kan worden
        private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";
        //private readonly string connectionString = "Server=172.16.160.21;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";
        // link voor in de les op school

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("Index")]
        public IActionResult Index()
        {
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
                            Genre = Convert.ToInt32(reader["genre_id"]),
                            Tijdsduur = Convert.ToInt32(reader["Tijdsduur(min)"]),
                            Leeftijdsgrens = Convert.ToInt32(reader["Leeftijdsgrens"]),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Img = reader["foto"].ToString(),
                            Trailer = reader["Trailer"].ToString()
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

        [Route("Films")]
        public IActionResult Films()
        {
            // alle namen ophalen
            var film = GetProducts();

            // stop de namen in de html
            return View(film);
        }

        [Route("Evenementen")]
        public IActionResult Evenementen()
        {
            return View();
        }

        [Route("Detailpagina")]
        public IActionResult Detailpagina()
        {
            return View();
        }

        [Route("Bestelpagina")]
        public IActionResult Bestelpagina()
        {
            var Dates = GetDates();
            return View(Dates);
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

        [Route("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        [Route("Contact")]
        public IActionResult Contact(Person person)
        {
            if (ModelState.IsValid)
                return Redirect("/success");

            return View(person);
        }

        [Route("film/{id}")]
        public IActionResult Film(string id)
        {
            var model = GetFilm(id);

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
                            Genre = Convert.ToInt32(reader["genre_id"]),
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
                MySqlCommand cmd = new MySqlCommand("select * from `voorstelling-datum`", conn);

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

                        };

                        Dates.Add(q);
                    }
                }
            }

            // return de lijst met namen
            return Dates;
        }
    }
    
}

    

    