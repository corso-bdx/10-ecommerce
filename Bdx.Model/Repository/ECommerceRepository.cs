namespace Bdx.ECommerce.Repository;

public class ECommerceRepository : IECommerceRepository
{
    private readonly ECommerceDbContext _context;

    public ECommerceRepository(ECommerceDbContext context)
    {
        _context = context;
    }

    public void AggiungiUtente(string username, string nome, string cognome)
    {
        Utente utente = new Utente();
        utente.Username = username;
        utente.Nome = nome;
        utente.Cognome = cognome;

        _context.Add(utente);

        _context.SaveChanges();
    }

    public void AggiungiProdotto(string nome, int quantita)
    {
        Prodotto prodotto = new Prodotto();
        prodotto.Nome = nome;
        prodotto.Quantita = quantita;

        _context.Add(prodotto);

        _context.SaveChanges();
    }

    public List<Prodotto> CercaProdotto(string nome)
    {
        IQueryable<Prodotto> IQueryableProdotto = _context.ListaProdotto;

        if (!string.IsNullOrWhiteSpace(nome))
            IQueryableProdotto = IQueryableProdotto.Where(p => p.Nome.Contains(nome));

        return IQueryableProdotto.ToList();
    }

    public void Acquista(int idUtente, int idProdotto, int quantita)
    {
        Acquisto acquisto = new Acquisto();
        acquisto.IdUtente = idUtente;
        acquisto.IdProdotto = idProdotto;
        acquisto.Quantita = quantita;

        _context.Add(acquisto);

        _context.SaveChanges();
    }

    public List<Prodotto> CercaProdotto_Bonus(string nome)
    {
        IQueryable<Prodotto> IQueryableProdotto = _context.ListaProdotto.Where(p => p.Quantita > 0);

        if (!string.IsNullOrWhiteSpace(nome))
            IQueryableProdotto = IQueryableProdotto.Where(p => p.Nome.Contains(nome));

        return IQueryableProdotto.ToList();
    }

    public void Acquista_Bonus(int idUtente, int idProdotto, int quantita)
    {
        Prodotto? prodottoDaAcquistare = _context.ListaProdotto.SingleOrDefault(p => p.Id == idProdotto);

        if (prodottoDaAcquistare is null)
            throw new Exception($"Prodotto id {idProdotto} non trovato");

        if (prodottoDaAcquistare.Quantita < quantita)
            throw new Exception($"Prodotto id {idProdotto} esaurito");

        Acquisto acquisto = new Acquisto();
        acquisto.IdUtente = idUtente;
        acquisto.IdProdotto = idProdotto;
        acquisto.Quantita = quantita;

        _context.Add(acquisto);

        prodottoDaAcquistare.Quantita -= quantita;

        _context.SaveChanges();

        // è necessario utilizzare una transazione?
        // NO: possiamo eseguire le due istruzioni con una sola SaveChanges()
        // che utilizza implicitamente una transazione e ci assicura l'atomicità
        // delle due modifiche
        //
        // In un contesto di concorrenza, ovvero quando il repository è utilizzato
        // da più utenti contemporaneamente, si può comunque fare a meno delle transazioni e
        // utilizzare altri strumenti (tipo i vincoli di tupla, vedi slide) per garantire che
        // la quantità del prodotto non vada mai sotto lo 0.
    }

    /// <summary>
    /// Alternativa per evitare di esporre il model Prodotto
    /// </summary>
    /// <param name="nome"></param>
    /// <returns></returns>
    public List<Dto.Prodotto> CercaProdotto_BonusDto(string nome)
    {
        IQueryable<Prodotto> IQueryableProdotto = _context.ListaProdotto.Where(p => p.Quantita > 0);

        if (!string.IsNullOrWhiteSpace(nome))
            IQueryableProdotto = IQueryableProdotto.Where(p => p.Nome.Contains(nome));

        return IQueryableProdotto.Select(p => new Dto.Prodotto
        {
            Id = p.Id,
            Nome = p.Nome,
            Quantita = p.Quantita
        }).ToList();
    }
}
