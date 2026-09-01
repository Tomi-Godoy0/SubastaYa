using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Domain.Entities
{
    public class TransaccionLedger
    {
        public int Id { get; set; }
        public int BilleteraId { get; set; }
        public string Tipo { get; set; } = string.Empty; // Deposito, Retención, Liberación, Pago, Cobro
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int? SubastaId { get; set; } // Referencia a la subasta asociada 

        //------
        public Billetera Billetera { get; set; } = null!;
        public Subasta? Subasta { get; set; }
    }
}
