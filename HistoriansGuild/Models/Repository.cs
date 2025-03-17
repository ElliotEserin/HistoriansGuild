using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoriansGuild.Models
{
    public record Repository(int Id, string Location, int UserId)
    {
        public User User => AppConfig.config.Users.Find(u => u.Id == UserId)!;
    }
}
