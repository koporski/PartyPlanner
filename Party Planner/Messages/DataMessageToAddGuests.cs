using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Model;
using System.Collections.ObjectModel;

namespace Party_Planner.Messages
{
    public class DataMessageToAddGuests : MessageBase
    {
        public Party Party {get;set;}
        public ObservableCollection<Guest> ContactList { get; set; }
        public DataMessageToAddGuests(Party party, ObservableCollection<Guest> contactList)
        {
            Party = party;
            ContactList = contactList;
        }
    }
}
