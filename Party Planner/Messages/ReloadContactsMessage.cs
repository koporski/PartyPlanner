using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Party_Planner.Messages
{
    public class ReloadContactsMessage
    {
        public bool Reload { get; set; }

        public ReloadContactsMessage(bool reload)
        {
            Reload = reload;
        }
    }
}
