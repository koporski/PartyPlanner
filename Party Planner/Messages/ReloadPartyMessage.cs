using GalaSoft.MvvmLight.Messaging;

namespace Party_Planner.Messages
{
    public class ReloadPartyMessage : MessageBase
    {
        public bool Reload { get; set; }

        public ReloadPartyMessage(bool reload)
        {
            Reload = reload;
        }
    }
}
