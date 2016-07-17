using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Party_Planner.Interfaces;
using Party_Planner.Services;
using System;

namespace Party_Planner.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SetupNavigation();
            SimpleIoc.Default.Register<IXmlService, XmlService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<StartViewModel>();
            SimpleIoc.Default.Register<ContactsViewModel>(true);
            SimpleIoc.Default.Register<AddContactViewModel>(true);
            SimpleIoc.Default.Register<PartyViewModel>(true);
            SimpleIoc.Default.Register<AddGuestsViewModel>(true);
        }

        private void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("MainWindow", new Uri("../View/MainWindow.xaml", UriKind.Relative));
            navigationService.Configure("StartView", new Uri("../View/StartView.xaml", UriKind.Relative));
            navigationService.Configure("ContactsView", new Uri("../View/ContactsView.xaml", UriKind.Relative));
            navigationService.Configure("AddContactView", new Uri("../View/AddContactView.xaml", UriKind.Relative));
            navigationService.Configure("PartyView", new Uri("../View/PartyView.xaml", UriKind.Relative));
            navigationService.Configure("AddGuestsView", new Uri("../View/AddGuestsView.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public StartViewModel StartViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StartViewModel>();
            }
        }

        public ContactsViewModel ContactsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContactsViewModel>();
            }
        }

        public AddContactViewModel AddContactViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddContactViewModel>();
            }
        }

        public PartyViewModel PartyViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PartyViewModel>();
            }
        }

        public AddGuestsViewModel AddGuestsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddGuestsViewModel>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}