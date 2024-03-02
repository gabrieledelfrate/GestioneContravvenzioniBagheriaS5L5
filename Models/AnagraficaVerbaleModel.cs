using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestioneContravvenzioniBagheriaS5L5.Models
{
    public class AnagraficaVerbaleModel
    {
        // Proprietà per l'anagrafica
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Città { get; set; }
        public string CAP { get; set; }
        [Display(Name = "Codice Fiscale")]
        public string CodFisc { get; set; }

        // Proprietà per il verbale
        [Display(Name = "Data Violazione")]
        [DataType(DataType.Date)]
        public DateTime DataViolazione { get; set; }

        [Display(Name = "Indirizzo Violazione")]
        public string IndirizzoViolazione { get; set; }

        [Display(Name = "Importo")]
        public decimal Importo { get; set; }

        [Display(Name = "Decurtamento Punti")]
        public int DecurtamentoPunti { get; set; }

        [Display(Name = "Tipo Violazione")]
        [Key]
        public int IDViolazione { get; set; }

        [Display(Name = "Agente")]
        public int IDAgente { get; set; }

        [Display(Name = "Data Trascrizione Verbale")]
        [DataType(DataType.Date)]
        public DateTime DataTrascrizioneVerbale { get; set; }

        [Display(Name = "ID Anagrafica")]
        [Key]
        public int IDAnagrafica { get; set; }

        // Lista per il dropdown delle violazioni
        public List<TipoViolazioneModel> TipoViolazioni { get; set; }
    }

    public class TipoViolazioneModel
    {

        public int IDViolazione { get; set; }
        public string Descrizione { get; set; }
    }
}
