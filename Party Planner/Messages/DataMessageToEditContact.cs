using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Model;

namespace Party_Planner.Messages
{
    public class DataMessageToEditContact : MessageBase
    {
        public Guest Guest { get; set; }

        public DataMessageToEditContact(Guest guest)
        {
            Guest = guest;
        }
    }
}
