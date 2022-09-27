using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.ECommerce;

[Table("Utenti", Schema = "Bdx")]
public class Utente
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public List<Acquisto> Acquisti { get; set; } = new List<Acquisto>();
}