using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bdx.ECommerce;

[Table("Prodotti", Schema = "Bdx")]
public class Prodotto
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int Quantita { get; set; }

    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
}