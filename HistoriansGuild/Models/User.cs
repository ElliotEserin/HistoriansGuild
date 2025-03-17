using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoriansGuild.Models
{ 
    public record User(int Id, string Name, string Email, string Password);
}
