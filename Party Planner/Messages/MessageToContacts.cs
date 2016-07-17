using GalaSoft.MvvmLight.Messaging;

namespace Party_Planner.Messages
{
    public class MessageToContacts : MessageBase
    {
        public bool ContactCreated { get; set; }

        public MessageToContacts(bool contactCreated)
        {
            ContactCreated = contactCreated;
        }
    }
}
