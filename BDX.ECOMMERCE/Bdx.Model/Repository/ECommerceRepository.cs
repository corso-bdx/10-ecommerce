namespace BDX.ECommerce.Repository
{
    public class ECommerceRepository : IECommerceRepository, IDisposable
    {
        private readonly ECommerceDbContext context;

        public ECommerceRepository(ECommerceDbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void AggiungiUtente(string username, string nome, string cognome)
        {
            Utente utente = new Utente();
            utente.Username = username;
            utente.Nome = nome;
            utente.Cognome = cognome;

            context.Add(utente);

            context.SaveChanges();
        }

        public void AggiungiProdotto(string nome, int quantita)
        {
            Prodotto prodotto = new Prodotto();
            prodotto.Nome = nome;
            prodotto.Quantita = quantita;

            context.Add(prodotto);

            context.SaveChanges();
        }

        public List<Prodotto> CercaProdotto(string nome)
        {
            IQueryable<Prodotto> IQueryableProdotto = context.ListaProdotto;

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

            context.Add(acquisto);

            context.SaveChanges();
        }

        public List<Prodotto> CercaProdotto_Bonus(string nome)
        {
            IQueryable<Prodotto> IQueryableProdotto = context.ListaProdotto.Where(p => p.Quantita > 0);

            if (!string.IsNullOrWhiteSpace(nome))
                IQueryableProdotto = IQueryableProdotto.Where(p => p.Nome.Contains(nome));

            return IQueryableProdotto.ToList();
        }

        public void Acquista_Bonus(int idUtente, int idProdotto, int quantita)
        {
            Prodotto? prodottoDaAcquistare = context.ListaProdotto.SingleOrDefault(p => p.Id == idProdotto);

            if (prodottoDaAcquistare is null)
                throw new Exception($"Prodotto id {idProdotto} non trovato");

            if (prodottoDaAcquistare.Quantita < quantita)
                throw new Exception($"Prodotto id {idProdotto} esaurito");

            Acquisto acquisto = new Acquisto();
            acquisto.IdUtente = idUtente;
            acquisto.IdProdotto = idProdotto;
            acquisto.Quantita = quantita;

            context.Add(acquisto);

            prodottoDaAcquistare.Quantita -= quantita;

            context.SaveChanges();

            /*
             
                è necessario utilizzare una transazione?
                  NO: possiamo eseguire le due istruzioni con una sola SaveChanges()
                  che utilizza implicitamente una transazione e ci assicura l'atomicità
                  delle due modifiche
             
                In un contesto di concorrenza, ovvero quando il repository è utilizzato
                da più utenti contemporaneamente, si può comunque fare a meno delle transazioni e
                utilizzare altri strumenti (tipo i vincoli di tupla, vedi slide) per garantire che
                la quantità del prodotto non vada mai sotto lo 0.
             
             */

        }

        /// <summary>
        /// Alternativa per evitare di esporre il model Prodotto
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public List<DTO.Prodotto> CercaProdotto_BonusDTO(string nome)
        {
            IQueryable<Prodotto> IQueryableProdotto = context.ListaProdotto.Where(p => p.Quantita > 0);

            if (!string.IsNullOrWhiteSpace(nome))
                IQueryableProdotto = IQueryableProdotto.Where(p => p.Nome.Contains(nome));

            return IQueryableProdotto.Select(p => new DTO.Prodotto
            {
                Id = p.Id,
                Nome = p.Nome,
                Quantita = p.Quantita
            }).ToList();
        }
    }
}
