using Demo.Desktop.Model;
using Demo.Desktop.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Demo.Desktop.Services
{
    public class PersonViewService
    {
        Window personView = null;

        public PersonViewService()
        { 
        }

        public void ShowDialog()
        { 
            personView = new PersonView();
            personView.Closing += PersonView_Closing;
            personView.ShowDialog();

        }

        private void PersonView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModelLocator.PersonViewModel.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Person", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Yes:
                        ViewModelLocator.PersonViewModel.SavePerson();
                        break;
                    default:
                        break;
                }
            }
        }

        public void CloseDialog()
        {
            personView?.Close();
        }
    }
}
