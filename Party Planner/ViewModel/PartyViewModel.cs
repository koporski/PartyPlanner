using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using Party_Planner.Interfaces;
using System.Collections.ObjectModel;
using Party_Planner.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Messages;
using System.IO;
using System.Collections;
using System.Windows;
using Party_Planner.Enums;
using System.Windows.Media;
using System.Windows.Forms;

namespace Party_Planner.ViewModel
{
    public class PartyViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;
        private IXmlService _xmlService;

        private Party _party;
        private bool _deleteEnabled;
        private bool _deleteAllEnabled;
        private IList _selectedGuests;
        private ObservableCollection<Guest> _contactList;
        private string _status = "";
        private Brush _statusColor = Brushes.Black;

        private RelayCommand _addGuestsCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _selectionChangedCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _deleteAllCommand;
        private RelayCommand _exportCommand;
        private RelayCommand _deletePartyCommand;
        private RelayCommand _contactsCommand;

        private string _pathGuests = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.GuestListFilePath;
        private string _pathContacts = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.ContactListFilePath;
        #endregion

        #region Constructor
        public PartyViewModel(IFrameNavigationService navigationService, IXmlService xmlService)
        {
            _navigationService = navigationService;
            _xmlService = xmlService;
            _selectedGuests = new List<Guest>();
            Messenger.Default.Register<ReloadPartyMessage>(this, x => LoadData());
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
                if(_party.Name != value)
                {
                    _party.Name = value;
                }
            }
        }

        public ObservableCollection<Guest> GuestsList
        {
            get
            {
                return _party.GuestList;
            }
            set
            {
                if (_party.GuestList != value)
                {
                    _party.GuestList = value;
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
                
                    _deleteEnabled = value;
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

                    _deleteAllEnabled = value;
            }
        }

        public IList SelectedGuests
        {
            get
            {
                return _selectedGuests;
            }
            set
            {
                if (_selectedGuests != value)
                {
                    _selectedGuests = value;
                }
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                }
            }
        }
        
        public Brush StatusColour
        {
            get
            {
                return _statusColor;
            }
            set
            {
                if(_statusColor != value)
                {
                    _statusColor = value;
                }
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddGuestsCommand
        {
            get
            {
                return _addGuestsCommand ?? (_addGuestsCommand = new RelayCommand(AddGuests));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(Save));
            }
        }

        public RelayCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new RelayCommand(SelectionChanged));
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

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand ?? (_exportCommand = new RelayCommand(Export));
            }
        }

        public RelayCommand DeletePartyCommand
        {
            get
            {
                return _deletePartyCommand ?? (_deletePartyCommand = new RelayCommand(DeleteParty));
            }
        }

        public RelayCommand ContactsCommand
        {
            get
            {
                return _contactsCommand ?? (_contactsCommand = new RelayCommand(GoToContacts));
            }
        }
        #endregion

        #region Private methods
        private void AddGuests()
        {
            SelectedGuests.Clear();
            _navigationService.NavigateTo("AddGuestsView");
            Messenger.Default.Send<DataMessageToAddGuests>(new DataMessageToAddGuests(_party, _contactList));
        }

        private void Save()
        {
            SelectedGuests.Clear();
            _xmlService.Serialize(_party, _pathGuests);
            _xmlService.Serialize(_contactList, _pathContacts);
        }

        private void Delete()
        {
            List<Guest> selectedGuests = new List<Guest>();
            foreach (Guest guest in _selectedGuests)
                selectedGuests.Add(guest);
            if (selectedGuests.Count > 0)
            {
                MessageBoxResult reult = System.Windows.MessageBox.Show(Resources.Strings.AreYouSure, Resources.Strings.Delete, MessageBoxButton.YesNo);
                if (reult == MessageBoxResult.Yes)
                {
                    foreach (Guest guest in selectedGuests)
                    {
                        guest.Invited = false;
                        guest.Response = Enums.Response.Unknow;
                        _party.GuestList.Remove(guest);
                    }                        
                    _xmlService.Serialize(_party, _pathGuests);
                    _xmlService.Serialize(_contactList, _pathContacts);
                    CheckDeleteAllEnabled();
                    CalculateStatus();
                }
            }
        }

        private void DeleteAll()
        {
            MessageBoxResult reult = System.Windows.MessageBox.Show(Resources.Strings.AreYouSure, Resources.Strings.Delete, MessageBoxButton.YesNo);
            if (reult == MessageBoxResult.Yes)
            {
                foreach(Guest guest in _party.GuestList)
                {
                    guest.Invited = false;
                    guest.Response = Enums.Response.Unknow;
                }
                _party.GuestList.Clear();
                _xmlService.Serialize(_party, _pathGuests);
                _xmlService.Serialize(_contactList, _pathContacts);
                CheckDeleteAllEnabled();
                CalculateStatus();
            }
        }

        private void Export()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();
            if(result == DialogResult.OK)
            {
                _xmlService.Serialize(_party, folderBrowser.SelectedPath + "\\" + Resources.Strings.GuestListFilePath);
            }

        }

        private void DeleteParty()
        {
            MessageBoxResult reult = System.Windows.MessageBox.Show(Resources.Strings.AreYouSure, Resources.Strings.Delete, MessageBoxButton.YesNo);
            if (reult == MessageBoxResult.Yes)
            {
                foreach (Guest guest in _party.GuestList)
                {
                    guest.Invited = false;
                    guest.Response = Response.Unknow;
                }
                _party.GuestList.Clear();
                _xmlService.Serialize(_contactList, _pathContacts);
                if (File.Exists(_pathGuests))
                    File.Delete(_pathGuests);
                _navigationService.NavigateTo("StartView");
            }
        }

        private void GoToContacts()
        {
            _navigationService.NavigateTo("ContactsView");
            Messenger.Default.Send<ReloadContactsMessage>(new ReloadContactsMessage(true));
        }

        private void SelectionChanged()
        {
            if (SelectedGuests.Count > 0)
                DeleteEnabled = true;
            else
                DeleteEnabled = false;
            RaisePropertyChanged("DeleteEnabled");
        }

        private void LoadData()
        {
            if (File.Exists(_pathContacts))
                _contactList = _xmlService.Deserialize<ObservableCollection<Guest>>(_pathContacts);
            else
                _contactList = new ObservableCollection<Guest>();

            if (File.Exists(_pathGuests))
            {
                Party tempParty = _xmlService.Deserialize<Party>(_pathGuests);
                _party = new Party()
                {
                    Name = tempParty.Name,
                    GuestList = new ObservableCollection<Guest>()
                };
                foreach (Guest guest in _contactList)
                {
                    if (guest.Invited)
                        _party.GuestList.Add(guest);
                }
            }
            else
            {
                _party = new Party()
                {
                    Name = Resources.Strings.NewParty,
                    GuestList = new ObservableCollection<Guest>()
                };
                _xmlService.Serialize(_party, _pathGuests);
            }
            CheckDeleteAllEnabled();
            CalculateStatus();
        }

        private void CheckDeleteAllEnabled()
        {
            if (_party.GuestList.Count > 0)
                DeleteAllEnabled = true;
            else
                DeleteAllEnabled = false;
            RaisePropertyChanged("DeleteAllEnabled");
        }

        private void CalculateStatus()
        {
            int maleCount = 0, femaleCount = 0, odds = 0;
            foreach (Guest guest in _party.GuestList)
                if (guest.Gender == Gender.Male)
                    maleCount++;
            femaleCount = _party.GuestList.Count - maleCount;
            odds = Math.Abs(maleCount - femaleCount);

            if (odds < 3)
            {
                Status = Resources.Strings.Ok;
                StatusColour = Brushes.Green;
            }
            else if (odds >= 3 && odds < 5)
            {
                Status = Resources.Strings.Poor;
                StatusColour = Brushes.Yellow;
            }
            else
            {
                Status = Resources.Strings.Bad;
                StatusColour = Brushes.Red;
            }
            RaisePropertyChanged("Status");
            RaisePropertyChanged("StatusColour");
        }
        #endregion
    }
}
