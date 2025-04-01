using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_catalogue.Models
{
    public class Nota
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdCurs { get; set; }
        public int NotaValoare { get; set; }
    }
}
