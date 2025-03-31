using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_catalogue.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Rol { get; set; }
        public string Parola { get; set; }
        public string Email { get; set; }
    }
}
