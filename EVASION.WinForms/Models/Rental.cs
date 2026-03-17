namespace EVASION.WinForms.Models;

public class Rental
{
    public int Id { get; set; }
    public string CodeClient { get; set; } = string.Empty;
    public string Immatriculation { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; } = DateTime.Today;
    public DateTime DateFin { get; set; } = DateTime.Today.AddDays(1);
    public DateTime? DateRetour { get; set; }
    public int? KmParcourus { get; set; }
    public decimal? Montant { get; set; }
}