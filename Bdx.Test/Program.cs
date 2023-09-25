using Bdx.ECommerce;
using Bdx.ECommerce.Repository;
using Microsoft.EntityFrameworkCore;

string connectionstring = "Server=localhost\\SQLEXPRESS;Database=ECOMMERCE;Trusted_Connection=True";

DbContextOptionsBuilder<ECommerceDbContext> optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
optionsBuilder.UseSqlServer(connectionstring);

using ECommerceDbContext eCommerceDbContext = new ECommerceDbContext(optionsBuilder.Options);
ECommerceRepository eCommerceRepository = new ECommerceRepository(eCommerceDbContext);

bool esci = false;

// funzione che permette di chiedere un parametro.
// Se il paramentro "index" è stato indicato da linea di comando usa quello,
// altrimenti mostra un "messaggio" all'utente chiedendo il parametro.
string ChiediParametro(int index, string messaggio)
{
    if (args.Length > index)
    {
        return args[index];
    }

    Console.Write(messaggio);
    string? input = Console.ReadLine();
    if (input == null)
    {
        throw new Exception("Nessun input");
    }

    return input;
}

Dictionary<string, Action> operazioni = new Dictionary<string, Action>()
{
    ["aggiungiUtente"] = () =>
    {
        string username = ChiediParametro(1, "Username: ");
        string nome = ChiediParametro(2, "Nome: ");
        string cognome = ChiediParametro(3, "Cognome: ");
        eCommerceRepository.AggiungiUtente(username, nome, cognome);

        WriteLineOK("Inserimento utente concluso correttamente");
    },

    ["aggiungiProdotto"] = () =>
    {
        string nome = ChiediParametro(1, "Nome: ");
        if (!int.TryParse(ChiediParametro(2, "Quantità: "), out int quantita))
            throw new Exception("Quantità non valida");

        eCommerceRepository.AggiungiProdotto(nome, quantita);

        WriteLineOK("Inserimento prodotto concluso correttamente");
    },

    ["cercaProdotto"] = () =>
    {
        string nome = ChiediParametro(1, "Nome: ");
        List<Bdx.ECommerce.Dto.Prodotto> ListaProdotto = eCommerceRepository.CercaProdotto_BonusDto(nome);

        foreach (Bdx.ECommerce.Dto.Prodotto prodotto in ListaProdotto)
        {
            Console.WriteLine($"\t{prodotto.Id}: {prodotto.Nome} {prodotto.Quantita}");
        }

        if (ListaProdotto.Count == 0)
            WriteLineWarning("Nessun prodotto trovato");
    },

    ["acquista"] = () =>
    {
        if (!int.TryParse(ChiediParametro(1, "IdUtente: "), out int idUtente))
            throw new Exception("IdUtente non valido");

        if (!int.TryParse(ChiediParametro(2, "IdProdotto: "), out int idProdotto))
            throw new Exception("IdUtente non valido");

        if (!int.TryParse(ChiediParametro(3, "Quantità: "), out int quantita))
            throw new Exception("Quantità non valida");

        eCommerceRepository.Acquista(idUtente, idProdotto, quantita);
    },

    ["cercaProdottoBonus"] = () =>
    {
        string nome = ChiediParametro(1, "Nome: ");
        List<Prodotto> ListaProdotto = eCommerceRepository.CercaProdotto_Bonus(nome);

        foreach (Prodotto prodotto in ListaProdotto)
        {
            Console.WriteLine($"\t{prodotto.Id}: {prodotto.Nome} {prodotto.Quantita}");
        }

        if (ListaProdotto.Count == 0)
            WriteLineWarning("Nessun prodotto trovato");
    },

    ["acquistaBonus"] = () =>
    {
        if (!int.TryParse(ChiediParametro(1, "IdUtente: "), out int idUtente))
            throw new Exception("IdUtente non valido");

        if (!int.TryParse(ChiediParametro(2, "IdProdotto: "), out int idProdotto))
            throw new Exception("IdUtente non valido");

        if (!int.TryParse(ChiediParametro(3, "Quantità: "), out int quantita))
            throw new Exception("Quantità non valida");

        eCommerceRepository.Acquista_Bonus(idUtente, idProdotto, quantita);

        WriteLineOK("Acquisto concluso correttamente");
    },

    ["cercaProdottoBonusDto"] = () =>
    {
        string nome = ChiediParametro(1, "Nome: ");
        List<Bdx.ECommerce.Dto.Prodotto> ListaProdotto = eCommerceRepository.CercaProdotto_BonusDto(nome);

        foreach (Bdx.ECommerce.Dto.Prodotto prodotto in ListaProdotto)
        {
            Console.WriteLine($"\t{prodotto.Id}: {prodotto.Nome} {prodotto.Quantita}");
        }

        if (ListaProdotto.Count == 0)
            WriteLineWarning("Nessun prodotto trovato");
    },

    ["esci"] = () =>
    {
        esci = true;

        Console.WriteLine($"\n\n\nCIAO\n\n\n");
    }
};

void WriteLineOK(string msg)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(msg);
    Console.ResetColor();
}

void WriteLineWarning(string msg)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine(msg);
    Console.ResetColor();
}

void WriteLineKO(Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"{ex.ToString()}\n\n");
    Console.ResetColor();
}

while (!esci)
{
    try
    {
        Console.Clear();

        // Chiede all'utente che operazione vuole eseguire.
        string operazione = ChiediParametro(0, $"Operazioni disponibili:\n\n{string.Join("\n", operazioni.Keys.Select(k => $"\t{k}"))}\n\nCosa vuoi fare? ");
        if (!operazioni.TryGetValue(operazione, out Action? action))
            throw new Exception("Comando non valido.");

        action();
    }
    catch (Exception ex)
    {
        WriteLineKO(ex);
    }

    Console.WriteLine("\n\nPremi un tasto per continuare");
    Console.ReadKey();
}
