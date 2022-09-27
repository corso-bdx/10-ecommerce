using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDX.ECommerce.DTO
{
    public class Prodotto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public int Quantita { get; set; }
    }
}
