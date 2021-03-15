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
        // private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";
        private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110370;Uid=110370;Pwd=inf2021sql;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
                            Leeftijdsgrens = Convert.ToInt32(reader["Leeftijdsgrens"]),
                            Beschrijving = reader["Beschrijving"].ToString(),
                        };

                        // voeg de naam toe aan de lijst met namen
                        products.Add(p);
                    }
                }
            }

            // return de lijst met namen
            return products;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ShowAll()
        {
            return View();
        }

        public IActionResult Films()
        {
            return View();
        }

        public IActionResult Evenementen()
        {
            return View();
        }

        public IActionResult Detailpagina()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
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
                MySqlCommand cmd = new MySqlCommand($"select * from product where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Film p = new Film
                        {
                            id = Convert.ToInt32(reader["Id" ]),
                            //Titel = reader["Titel"].ToString(),
                            //Beschrijving = reader["Beschrijving"].ToString()
                        };
                        films.Add(p);
                    }
                }
            }
            return films[0];
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}