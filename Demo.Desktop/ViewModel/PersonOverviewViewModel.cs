using AutoMapper;
using Demo.Desktop.Commands;
using Demo.Desktop.DAL;
using Demo.Desktop.Messaging;
using Demo.Desktop.Model;
using Demo.Desktop.Services;
using Demo.Desktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Demo.Desktop.ViewModel
{
    public class PersonOverviewViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _persons;
        public ICommand AddPersonCommand { get; set; }
        public ICommand EditPersonCommand { get; set; }
        public ICommand PersonsReportCommand { get; set; }
        public ICommand UsersReportCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public string SearchParameter { get; set; }

        private PersonViewService personViewService = null;

        public ObservableCollection<Person> Persons
        {
            get => _persons;
            set
            {
                _persons = value;
                OnPropertyChanged(nameof(Persons));
            }
        }

        private Person _selectedPerson;

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }
        public PersonOverviewViewModel()
        {
            setupCommands();
            LoadPersons();
            personViewService = new PersonViewService();
            Messenger.Default.Register<Person>(this, PersonUpdated, this);
        }

        private void PersonUpdated(Person person)
        {
            if(person == null)
            {
                this.Persons.Remove(this.SelectedPerson);
            }
            else if(person.Id > 0)
            {
                var personToUpdate = this.Persons.Where(p => p.Id == person.Id).FirstOrDefault();

                if (personToUpdate == null)
                    this.Persons.Add(person);
                else
                    personToUpdate = person;
            }
            personViewService.CloseDialog();
        }

        private async void LoadPersons(object obj = null)
        {
            this.Persons?.Clear();
            DataService r = new DataService();

            IEnumerable<Person> persons;
            if (string.IsNullOrWhiteSpace(this.SearchParameter))
                persons = await r.GetPersons();
            else
                persons = await r.GetPersonsByFilter(SearchParameter);

            this.Persons = new ObservableCollection<Person>(persons);
        }

        private void setupCommands()
        {
            PersonsReportCommand = new RelayCommand(PersonsReport, (o) => { return true; });
            UsersReportCommand = new RelayCommand(UsersReport, (o) => { return true; });
            EditPersonCommand = new RelayCommand(EditPerson, (o) => { return this.SelectedPerson != null; });
            AddPersonCommand = new RelayCommand(AddPerson, (o) => { return true; });
            SearchCommand = new RelayCommand(LoadPersons, (o) => { return true; });
        }

        private async void PersonsReport(object obj)
        {
            DataService r = new DataService();
            await r.PersonsReport();
        }

        private async void UsersReport(object obj)
        {
            DataService r = new DataService();
            await r.UsersReport();
        }


        private void AddPerson(object obj)
        {
            Messenger.Default.Send<Person>(new Person(), ViewModelLocator.PersonViewModel);
            personViewService.ShowDialog();
        }

        private void EditPerson(object obj)
        {
            Messenger.Default.Send<Person>(this.SelectedPerson, ViewModelLocator.PersonViewModel);
            personViewService.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
