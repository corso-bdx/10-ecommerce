using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.ECommerce;

[Table("Acquisti", Schema = "Bdx")]
public class Acquisto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int IdUtente { get; set; }

    public int IdProdotto { get; set; }

    public int Quantita { get; set; }

    public Utente Utente { get; set; } = null!;

    public Prodotto Prodotto { get; set; } = null!;
}