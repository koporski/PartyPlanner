using System;
using GalaSoft.MvvmLight;
using Party_Planner.Interfaces;
using Party_Planner.Enums;
using GalaSoft.MvvmLight.Command;
using Party_Planner.Model;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using Party_Planner.Messages;

namespace Party_Planner.ViewModel
{
    public class AddContactViewModel : ViewModelBase
    {
        #region Private variables
        private IFrameNavigationService _navigationService;
        private IXmlService _xmlService;

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;

        private string _name;
        private string _surname;
        private int _age;
        private Gender _gender;
        private string _email;
        private Guest _contactToEdit;
        private ContactMode _mode = ContactMode.Add;
        private string _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Resources.Strings.ContactListFilePath;
        #endregion

        #region Constructor
        public AddContactViewModel(IFrameNavigationService navigationService, IXmlService xmlService)
        {
            _navigationService = navigationService;
            _xmlService = xmlService;
            Messenger.Default.Register<DataMessageToEditContact>(this, x => HandleDataMessage(x.Guest));
        }
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                }
            }
        }

        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (_age != value)
                {
                    _age = value;
                }
            }
        }

        public Gender Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
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
            string response = ValidateFields();
            if (String.IsNullOrEmpty(response))
            {
                ObservableCollection<Guest> contactList = _navigationService.Parameter as ObservableCollection<Guest>;
                Guest newGuest = new Guest()
                {
                    Name = Name,
                    Surname = Surname,
                    Age = Age,
                    Gender = Gender,
                    Email = Email,
                    Response = Response.Unknow,
                    Invited = false
                };
                if (_mode == ContactMode.Edit)
                {
                    newGuest.Response = _contactToEdit.Response;
                    newGuest.Invited = _contactToEdit.Invited;
                    contactList.Remove(_contactToEdit);
                }
                contactList.Add(newGuest);              
                _xmlService.Serialize<ObservableCollection<Guest>>(contactList, _path);
                ClearFields();
                _navigationService.GoBack();
                Messenger.Default.Send<MessageToContacts>(new MessageToContacts(true));
                _mode = ContactMode.Add;
            }
            else
                MessageBox.Show(response);
        }

        private void Cancel()
        {
            ClearFields();
            _navigationService.GoBack();
            _mode = ContactMode.Add;
        }

        private void ClearFields()
        {
            Name = String.Empty;
            Surname = String.Empty;
            Age = 0;
            Gender = Gender.Male;
            Email = String.Empty;
        }

        private string ValidateFields()
        {
            string response = "";
            if (String.IsNullOrEmpty(Name) || String.IsNullOrWhiteSpace(Name))
                response += Resources.Strings.Name + " " + Resources.Strings.Required + " ";
            if (String.IsNullOrEmpty(Surname) || String.IsNullOrWhiteSpace(Surname))
                response += Resources.Strings.Surname + " " + Resources.Strings.Required + " ";
            if (Age < 1 || Age > 100)
                response += Resources.Strings.Wrong + " ";
            if (String.IsNullOrEmpty(Email) || String.IsNullOrWhiteSpace(Email))
                response += Resources.Strings.Email + " " + Resources.Strings.Required + " ";
            return response;
        }

        private void HandleDataMessage(Guest contact)
        {
            _mode = ContactMode.Edit;
            _contactToEdit = contact;
            Name = contact.Name;
            Surname = contact.Surname;
            Age = contact.Age;
            Gender = contact.Gender;
            Email = contact.Email;            
        }
        #endregion
    }
}
