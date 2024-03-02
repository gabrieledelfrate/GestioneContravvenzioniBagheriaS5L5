namespace GestioneContravvenzioniBagheriaS5L5.Models
{
    public class ViolazioniImportoMaggiore400Model
    {
        public decimal Importo { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public DateTime DataViolazione { get; set; }
        public int DecurtamentoPunti { get; set; }
    }
}
