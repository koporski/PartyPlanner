using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using Party_Planner.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Messages;
using Party_Planner.Model;
using System.Collections.ObjectModel;
using System.Collections;
using GalaSoft.MvvmLight.Command;

namespace Party_Planner.ViewModel
{
    public class AddGuestsViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;
        private IXmlService _xmlService;

        private IList _selectedContacts;
        private Party _party;
        private ObservableCollection<Guest> _uninvitedContactList;
        private ObservableCollection<Guest> _contactList;

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;

        private string _pathContacts = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.ContactListFilePath;
        private string _pathGuests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.GuestListFilePath;
        #endregion

        #region Constructor
        public AddGuestsViewModel(IFrameNavigationService navigationService, IXmlService xmlService)
        {
            _navigationService = navigationService;
            _xmlService = xmlService;
            _selectedContacts = new List<Guest>();
            Messenger.Default.Register<DataMessageToAddGuests>(this, x => HandleDataMessage(x.Party, x.ContactList));
        }
        #endregion

        #region Properties
        public string PartyName
        {
            get
            {
                return _party.Name;
            }
            set
            {
                if (_party.Name != value)
                {
                    _party.Name = value;
                }
            }
        }

        public ObservableCollection<Guest> UninvitedContactList
        {
            get
            {
                return _uninvitedContactList;
            }
            set
            {
                if (_uninvitedContactList != value)
                {
                    _uninvitedContactList = value;
                }
            }
        }

        public IList SelectedContacts
        {
            get
            {
                return _selectedContacts;
            }
            set
            {
                if (_selectedContacts != value)
                {
                    _selectedContacts = value;
                }
            }
        }
        #endregion

        #region Commands
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(Save));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));
            }
        }
        #endregion

        #region Private methods
        private void Save()
        {
            foreach (Guest guest in _selectedContacts)
            {
                guest.Invited = true;
                _party.GuestList.Add(guest);
            }
            _xmlService.Serialize<ObservableCollection<Guest>>(_contactList, _pathContacts);
            _xmlService.Serialize<Party>(_party, _pathGuests);
            _selectedContacts.Clear();
            _navigationService.NavigateTo("PartyView");
            Messenger.Default.Send<ReloadPartyMessage>(new ReloadPartyMessage(true));                        
        }

        private void Cancel()
        {
            _navigationService.GoBack();
        }

        private void HandleDataMessage(Party party, ObservableCollection<Guest> contactList)
        {
            _party = party;
            _contactList = contactList;
            _uninvitedContactList = new ObservableCollection<Guest>();
            foreach (Guest guest in _contactList)
            {
                if (guest.Invited == false)
                    _uninvitedContactList.Add(guest);
            }
        }
        #endregion
    }
}
