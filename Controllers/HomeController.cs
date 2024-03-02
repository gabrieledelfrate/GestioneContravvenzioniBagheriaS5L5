using GestioneContravvenzioniBagheriaS5L5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace GestioneContravvenzioniBagheriaS5L5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string connectionString = "Data Source=GABRIELE-PORTAT\\SQLEXPRESS;Initial Catalog=GestioneContravvenzioniBagheria2;Integrated Security=True;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult TrasgressoriTotale()
        {
            var model = GetTrasgressoriTotale();
            return View(model);
        }

        public IActionResult PuntiDecurtatiTotale()
        {
            var model = GetPuntiDecurtatiTotale();
            return View(model);
        }

        public IActionResult ViolazioniSopra10Punti()
        {
            var model = GetViolazioniSopra10Punti();
            return View(model);
        }

        public IActionResult ViolazioniImportoMaggiore400()
        {
            var model = GetViolazioniImportoMaggiore400();
            return View(model);
        }


        public ActionResult AnagraficaVerbale()
        {
            var model = new AnagraficaVerbaleModel
            {
                TipoViolazioni = GetTipoViolazioni()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult InserisciAnagrafica(AnagraficaVerbaleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    InsertAnagrafica(model);
                    TempData["SuccessMessage"] = "Anagrafica registrata con successo!";
                    return RedirectToAction("AnagraficaVerbale");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Si è verificato un errore durante la registrazione dell'anagrafica.";
                }
            }

            model.TipoViolazioni = GetTipoViolazioni();
            return View("AnagraficaVerbale", model);
        }

        [HttpPost]
        public ActionResult InserisciVerbale(AnagraficaVerbaleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    InsertVerbale(model);

                    TempData["SuccessMessage"] = "Verbale registrato con successo!";
                    return RedirectToAction("AnagraficaVerbale");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Si è verificato un errore durante la registrazione del verbale.";
                }
            }
            model.TipoViolazioni = GetTipoViolazioni();
            return View("AnagraficaVerbale", model);
        }

        private List<TipoViolazioneModel> GetTipoViolazioni()
        {
            var tipoViolazioni = new List<TipoViolazioneModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM TIPO_VIOLAZIONE", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tipoViolazioni.Add(new TipoViolazioneModel
                            {
                                IDViolazione = Convert.ToInt32(reader["IDViolazione"]),
                                Descrizione = Convert.ToString(reader["Descrizione"])
                            });
                        }
                    }
                }
            }

            return tipoViolazioni;
        }

        private void InsertAnagrafica(AnagraficaVerbaleModel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Città, CAP, CodFisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @CodFisc)", connection))
                {
                    command.Parameters.AddWithValue("@Cognome", model.Cognome);
                    command.Parameters.AddWithValue("@Nome", model.Nome);
                    command.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                    command.Parameters.AddWithValue("@Città", model.Città);
                    command.Parameters.AddWithValue("@CAP", model.CAP);
                    command.Parameters.AddWithValue("@CodFisc", model.CodFisc);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertVerbale(AnagraficaVerbaleModel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, IDAgente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IDAnagrafica, IDViolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @IDAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IDAnagrafica, @IDViolazione)", connection))
                {
                    command.Parameters.AddWithValue("@DataViolazione", model.DataViolazione);
                    command.Parameters.AddWithValue("@IndirizzoViolazione", model.IndirizzoViolazione);
                    command.Parameters.AddWithValue("@IDAgente", model.IDAgente);
                    command.Parameters.AddWithValue("@DataTrascrizioneVerbale", model.DataTrascrizioneVerbale);
                    command.Parameters.AddWithValue("@Importo", model.Importo);
                    command.Parameters.AddWithValue("@DecurtamentoPunti", model.DecurtamentoPunti);
                    command.Parameters.AddWithValue("@IDAnagrafica", model.IDAnagrafica);
                    command.Parameters.AddWithValue("@IDViolazione", model.IDViolazione);

                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpPost]
        public ActionResult RecuperaIdAnagrafica(string codiceFiscale)
        {
            int idAnagrafica = GetIdAnagraficaByCodiceFiscale(codiceFiscale);
            return Json(idAnagrafica);
        }

        private int GetIdAnagraficaByCodiceFiscale(string codiceFiscale)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT IDAnagrafica FROM ANAGRAFICA WHERE CodFisc = @CodiceFiscale", connection))
                {
                    command.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);
                    return (int)(command.ExecuteScalar() ?? 0);
                }
            }
        }

        private List<TrasgressoreTotaleModel> GetTrasgressoriTotale()
        {
            List<TrasgressoreTotaleModel> result = new List<TrasgressoreTotaleModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Cognome, Nome, COUNT(IDVerbale) as TotaleVerbali FROM ANAGRAFICA INNER JOIN VERBALE ON ANAGRAFICA.IDAnagrafica = VERBALE.IDAnagrafica GROUP BY Cognome, Nome", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new TrasgressoreTotaleModel
                            {
                                Cognome = Convert.ToString(reader["Cognome"]),
                                Nome = Convert.ToString(reader["Nome"]),
                                TotaleVerbali = Convert.ToInt32(reader["TotaleVerbali"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        private List<PuntiDecurtatiTotaleModel> GetPuntiDecurtatiTotale()
        {
            List<PuntiDecurtatiTotaleModel> result = new List<PuntiDecurtatiTotaleModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Cognome, Nome, SUM(DecurtamentoPunti) as TotalePuntiDecurtati FROM ANAGRAFICA INNER JOIN VERBALE ON ANAGRAFICA.IDAnagrafica = VERBALE.IDAnagrafica GROUP BY Cognome, Nome", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new PuntiDecurtatiTotaleModel
                            {
                                Cognome = Convert.ToString(reader["Cognome"]),
                                Nome = Convert.ToString(reader["Nome"]),
                                TotalePuntiDecurtati = Convert.ToInt32(reader["TotalePuntiDecurtati"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        private List<ViolazioniSopra10PuntiModel> GetViolazioniSopra10Punti()
        {
            List<ViolazioniSopra10PuntiModel> result = new List<ViolazioniSopra10PuntiModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Importo, Cognome, Nome, DataViolazione, DecurtamentoPunti FROM ANAGRAFICA INNER JOIN VERBALE ON ANAGRAFICA.IDAnagrafica = VERBALE.IDAnagrafica WHERE DecurtamentoPunti > 10", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ViolazioniSopra10PuntiModel
                            {
                                Importo = Convert.ToDecimal(reader["Importo"]),
                                Cognome = Convert.ToString(reader["Cognome"]),
                                Nome = Convert.ToString(reader["Nome"]),
                                DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                                DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        private List<ViolazioniImportoMaggiore400Model> GetViolazioniImportoMaggiore400()
        {
            List<ViolazioniImportoMaggiore400Model> result = new List<ViolazioniImportoMaggiore400Model>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Importo, Cognome, Nome, DataViolazione, DecurtamentoPunti FROM ANAGRAFICA INNER JOIN VERBALE ON ANAGRAFICA.IDAnagrafica = VERBALE.IDAnagrafica WHERE Importo > 400", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ViolazioniImportoMaggiore400Model
                            {
                                Importo = Convert.ToDecimal(reader["Importo"]),
                                Cognome = Convert.ToString(reader["Cognome"]),
                                Nome = Convert.ToString(reader["Nome"]),
                                DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                                DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"])
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
