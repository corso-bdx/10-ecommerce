
namespace BDX.ECommerce.Repository
{
    public interface IECommerceRepository
    {
        void AggiungiUtente(string username, string nome, string cognome);

        void AggiungiProdotto(string nome, int quantita);

        List<Prodotto> CercaProdotto(string nome);

        void Acquista(int idUtente, int idProdotto, int quantita);

        List<Prodotto> CercaProdotto_Bonus(string nome);

        void Acquista_Bonus(int idUtente, int idProdotto, int quantita);

        List<DTO.Prodotto> CercaProdotto_BonusDTO(string nome);
    }
}
