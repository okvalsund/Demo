using Demo.Desktop.Commands;
using Demo.Desktop.DAL;
using Demo.Desktop.Messaging;
using Demo.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Demo.Desktop.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private Person _person;
        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                LoadPersonData();
                OnPropertyChanged(nameof(Person));
                Person.PropertyChanged -= Person_PropertyChanged;
                Person.PropertyChanged += Person_PropertyChanged;
                this.IsDirty = false;
            }
        }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                IsUser = value != null;
                OnPropertyChanged(nameof(User));
            }
        }
        private bool _isUser = false;
        public bool IsUser
        {
            get { return _isUser; }
            set
            {
                _isUser = value;
                OnPropertyChanged(nameof(IsUser));
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddEmailCommand { get; set; }
        public ICommand DeleteEmailCommand { get; set; }
        public ICommand AddAddressCommand { get; set; }
        public ICommand DeleteAddressCommand { get; set; }

        private List<Email> deleteEmailBuffer;
        private List<Address> deleteAddressBuffer;

        private ObservableCollection<Email> _emails;
        public ObservableCollection<Email> Emails
        {
            get => _emails;
            set
            {
                _emails = value;
                OnPropertyChanged(nameof(Emails));
            }
        }

        private ObservableCollection<Address> _addresses;
        public ObservableCollection<Address> Addresses
        {
            get => _addresses;
            set
            {
                _addresses = value;
                OnPropertyChanged(nameof(Addresses));
            }
        }

        public Email SelectedEmail { get; set; }
        public Address SelectedAddress { get; set; }

        private Email _newEmail = null;
        public Email NewEmail
        {
            get { return _newEmail; }
            set
            {
                this._newEmail = value;
                OnPropertyChanged(nameof(NewEmail));
            }
        }

        private Address _newAddress = null;
        public Address NewAddress
        {
            get { return _newAddress; }
            set
            {
                _newAddress = value;
                OnPropertyChanged(nameof(NewAddress));
            }
        }

        public bool IsDirty { get; private set; } = false;

        //public bool IsNewPerson => this.Person.Id < 1;

        public PersonViewModel()
        {
            Messenger.Default.Register<Person>(this, (person) => { this.Person = person; }, this);
            SetupCommands();
        }

        
        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        private void SetupCommands()
        {
            SaveCommand = new RelayCommand(SavePerson, (o) => { return this.IsDirty; });
            DeleteCommand = new RelayCommand(DeletePerson, (o) => { return this.Person != null && this.Person.Id > 0; });

            AddEmailCommand = new RelayCommand(AddEmail, (o) =>
            { return this.NewEmail != null && !string.IsNullOrWhiteSpace(this.NewEmail.EmailAddress) && this.NewEmail.EmailAddress.Contains("@"); });
            DeleteEmailCommand = new RelayCommand(DeleteEmail, (o) => { return this.SelectedEmail != null; });

            AddAddressCommand = new RelayCommand(AddAddress, (o) =>
            { return this.NewAddress != null && !string.IsNullOrWhiteSpace(this.NewAddress.Street) && !string.IsNullOrWhiteSpace(this.NewAddress.City) && !string.IsNullOrWhiteSpace(this.NewAddress.ZipCode); });
            DeleteAddressCommand = new RelayCommand(DeleteAddress, (o) => { return this.SelectedAddress != null; });
        }

        private async void DeletePerson(object obj)
        {
            DataService dataService = new DataService();
            if(this.Person.Id > 0)
                await dataService.DeletePerson(this.Person);

            Messenger.Default.Send<Person>(null, ViewModelLocator.PersonOverviewViewModel);
        }

        public async void SavePerson(object obj = null)
        {
            DataService dataService = new DataService();
            //Save Person
            if (this.Person.Id == 0) // new user
                await dataService.AddPerson(this.Person);
            else
                await dataService.UpdatePerson(this.Person);

            //Save User IsUser && User
            if (this.User == null && this.IsUser)
            {
                this.User = new User() { Id = this.Person.Id };
                await dataService.AddUser(this.User);
            }
            else if (this.User != null && !this.IsUser)
            {
                await dataService.DeleteUser(this.User);
                this.User = null;
            }

            //Save Email
            foreach (var email in this.deleteEmailBuffer)
                await dataService.DeletePersonEmail(email);

            this.deleteEmailBuffer.Clear();

            var addedeEmails = this.Emails.Where(e => e.Id == 0).ToList();

            foreach (var email in addedeEmails)
            {
                email.PersonId = this.Person.Id;
                await dataService.AddPersonEmail(email);
            }

            //Save Addresses
            foreach (var address in this.deleteAddressBuffer)
                await dataService.DeletePersonAddress(address);

            var addedAddresses = this.Addresses.Where(a => a.Id == 0);

            foreach(var address in addedAddresses)
            {
                address.PersonId = this.Person.Id;
                await dataService.AddPersonAddress(address);
            }

            this.IsDirty = false;
            Messenger.Default.Send<Person>(this.Person, ViewModelLocator.PersonOverviewViewModel);
        }

        private void AddEmail(object obj)
        {
            this.Emails.Add(this.NewEmail);
            this.NewEmail = new Email();
        }

        private void AddAddress(object obj)
        {
            this.Addresses.Add(this.NewAddress);
            this.NewAddress = new Address();
        }

        private void DeleteEmail(object obj)
        {
            if (this.SelectedEmail.Id > 0)
                this.deleteEmailBuffer.Add(this.SelectedEmail);

            this.Emails.Remove(this.SelectedEmail);
        }

        private void DeleteAddress(object obj)
        {
            if (this.SelectedAddress.Id > 0)
                this.deleteAddressBuffer.Add(this.SelectedAddress);
            this.Addresses.Remove(this.SelectedAddress);
        }

        private async void LoadPersonData()
        {
            if (Person.Id <= 0) // new person
            {
                this.User = null;
                this.Emails = new ObservableCollection<Email>();
                this.Addresses = new ObservableCollection<Address>();
            }
            else
            {
                DataService dataService = new DataService();
                this.User = await dataService.GetUser(this.Person.Id);
                this.Emails = new ObservableCollection<Email>(await dataService.GetPersonEmails(this.Person.Id));
                this.Addresses = new ObservableCollection<Address>(await dataService.GetPersonAddresses(this.Person.Id));
            }

            this.NewEmail = new Email();
            this.NewAddress = new Address();
            this.deleteEmailBuffer = new List<Email>();
            this.deleteAddressBuffer = new List<Address>();

            IsDirty = false;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            IsDirty = true;
        }
    }
}
