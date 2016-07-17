using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using Party_Planner.Interfaces;
using Party_Planner.Model;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Messages;

namespace Party_Planner.ViewModel
{
    public class ContactsViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;
        private IXmlService _xmlService;

        private ObservableCollection<Guest> _contactList;
        private IList _selectedContacts;
        private bool _deleteEnabled;
        private bool _editEnabled;
        private bool _deleteAllEnabled;

        private RelayCommand _goBackCommand;
        private RelayCommand _addContactCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _selectionChangedCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteAllCommand;

        private string _pathContacts = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.ContactListFilePath;
        private string _pathGuests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.GuestListFilePath;
        #endregion

        #region Constructor
        public ContactsViewModel(IFrameNavigationService navigationService, IXmlService xmlService)
        {
            _navigationService = navigationService;
            _xmlService = xmlService;
            _selectedContacts = new List<Guest>();
            Messenger.Default.Register<MessageToContacts>(this, x => HandleMessage(x.ContactCreated));
            Messenger.Default.Register<ReloadContactsMessage>(this, x => LoadData());
        }
        #endregion

        #region Proporties
        public ObservableCollection<Guest> ContactList
        {
            get
            {
                return _contactList;
            }
            set
            {
                if (_contactList != value)
                {
                    _contactList = value;
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

        public bool DeleteEnabled
        {
            get
            {
                return _deleteEnabled;
            }
            set
            {
                if (_deleteEnabled != value)
                {
                    _deleteEnabled = value;
                }
            }
        }

        public bool EditEnabled
        {
            get
            {
                return _editEnabled;
            }
            set
            {
                if (_editEnabled != value)
                {
                    _editEnabled = value;
                }
            }
        }

        public bool DeleteAllEnabled
        {
            get
            {
                return _deleteAllEnabled;
            }
            set
            {
                if (_deleteAllEnabled != value)
                {
                    _deleteAllEnabled = value;
                }
            }
        }
        #endregion

        #region Commands
        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(GoBack));
            }
        }

        public RelayCommand AddContactCommand
        {
            get
            {
                return _addContactCommand ?? (_addContactCommand = new RelayCommand(AddContact));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete));
            }
        }

        public RelayCommand DeleteAllCommand
        {
            get
            {
                return _deleteAllCommand ?? (_deleteAllCommand = new RelayCommand(DeleteAll));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand(Edit));
            }
        }

        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new RelayCommand(SelectionChanged));
            }
        }
        #endregion

        #region Private methods
        private void GoBack()
        {
            SelectedContacts.Clear();
            if (File.Exists(_pathGuests))
            {
                _navigationService.NavigateTo("PartyView");
                Messenger.Default.Send(new ReloadPartyMessage(true));
            }
            else
                _navigationService.NavigateTo("StartView");
        }

        private void AddContact()
        {
            SelectedContacts.Clear();
            _navigationService.NavigateTo("AddContactView", _contactList);
        }

        private void Delete()
        {
            List<Guest> selectedContacts = new List<Guest>();
            foreach (Guest guest in _selectedContacts)
                selectedContacts.Add(guest);
            if(selectedContacts.Count > 0)
            {
                MessageBoxResult reult = MessageBox.Show(Resources.Strings.AreYouSure, Resources.Strings.Delete, MessageBoxButton.YesNo);
                if (reult == MessageBoxResult.Yes)
                {
                    foreach (Guest guest in selectedContacts)
                        _contactList.Remove(guest);
                    _xmlService.Serialize<ObservableCollection<Guest>>(_contactList, _pathContacts);
                    CheckDeleteAllEnabled();
                }
            }
        }

        private void Edit()
        {
            _navigationService.NavigateTo("AddContactView", _contactList);
            Messenger.Default.Send<DataMessageToEditContact>(new DataMessageToEditContact(_selectedContacts[0] as Guest));
            SelectedContacts.Clear();
        }

        private void DeleteAll()
        {
            MessageBoxResult reult = MessageBox.Show(Resources.Strings.AreYouSure, Resources.Strings.Delete, MessageBoxButton.YesNo);
            if (reult == MessageBoxResult.Yes)
            {
                _contactList.Clear();
                _xmlService.Serialize<ObservableCollection<Guest>>(_contactList, _pathContacts);
                CheckDeleteAllEnabled();
            }
        }

        private void SelectionChanged()
        {
            if (SelectedContacts.Count > 0)
                DeleteEnabled = true;
            else
                DeleteEnabled = false;

            if (SelectedContacts.Count == 1)
                EditEnabled = true;
            else
                EditEnabled = false;
            RaisePropertyChanged("DeleteEnabled");
            RaisePropertyChanged("EditEnabled");
        }

        private void LoadData()
        {
            if (File.Exists(_pathContacts))
            {
                _contactList = _xmlService.Deserialize<ObservableCollection<Guest>>(_pathContacts);
                CheckDeleteAllEnabled();
            }
            else
                _contactList = new ObservableCollection<Guest>();
        }

        private void HandleMessage(bool contactCreated)
        {
            if (contactCreated)
                CheckDeleteAllEnabled();
        }

        private void CheckDeleteAllEnabled()
        {
            if (_contactList.Count > 0)
                DeleteAllEnabled = true;
            else
                DeleteAllEnabled = false;
            RaisePropertyChanged("DeleteAllEnabled");
        }
        #endregion
    }
}
