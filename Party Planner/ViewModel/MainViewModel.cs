using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Interfaces;
using Party_Planner.Messages;
using System;
using System.IO;

namespace Party_Planner.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;

        private RelayCommand _goToContactsCommand;
        private RelayCommand _loadedCommand;

        private string _pathGuests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.GuestListFilePath;
        #endregion

        #region Constructor
        public MainViewModel(IFrameNavigationService navigationService)
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

        public RelayCommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(Loaded));
            }
        }
        #endregion

        #region Private methods
        private void GoToContacts()
        {
            _navigationService.NavigateTo("ContactsView");
        }

        private void Loaded()
        {
            if (File.Exists(_pathGuests))
            {
                _navigationService.NavigateTo("PartyView");
                Messenger.Default.Send<ReloadPartyMessage>(new ReloadPartyMessage(true));
            }
            else
                _navigationService.NavigateTo("StartView");
        }
        #endregion
    }
}