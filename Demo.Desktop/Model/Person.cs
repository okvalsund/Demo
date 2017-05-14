using System.ComponentModel;

namespace Demo.Desktop.Model
{
    public class Person : INotifyPropertyChanged
    {
        private int _id;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        public int Id
        {
            get => _id; 
            set
            {
                _id = value;
                OnPropertyChanged(nameof(this.Id));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(this.FirstName));
            }
        }


        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                OnPropertyChanged(nameof(this.MiddleName));
            }
        }


        public string LastName
        {
            get => _lastName; 
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(this.LastName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
