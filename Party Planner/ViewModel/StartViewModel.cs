using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Interfaces;
using Party_Planner.Messages;

namespace Party_Planner.ViewModel
{
    public class StartViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;

        private RelayCommand _goToContactsCommand;
        private RelayCommand _goToPartyCommand;
        #endregion

        #region Constructor
        public StartViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Commands
        public RelayCommand GoToContactsCommand
        {
            get
            {
                return _goToContactsCommand ?? (_goToContactsCommand = new RelayCommand(GoToContacts));
            }
        }

        public RelayCommand GoToPartyCommand
        {
            get
            {
                return _goToPartyCommand ?? (_goToPartyCommand = new RelayCommand(GoToParty));
            }
        }
        #endregion

        #region Private methods
        private void GoToContacts()
        {
            _navigationService.NavigateTo("ContactsView");
            Messenger.Default.Send<ReloadContactsMessage>(new ReloadContactsMessage(true));
        }

        private void GoToParty()
        {
            _navigationService.NavigateTo("PartyView");
            Messenger.Default.Send<ReloadPartyMessage>(new ReloadPartyMessage(true));
        }
        #endregion
    }
}
