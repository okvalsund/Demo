using Demo.Desktop.Model;
using Demo.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataRetriver dataRetriver;

        public MainWindow()
        {
            InitializeComponent();

            dataRetriver = new DataRetriver();
            

        }

        private void Search(object sender, RoutedEventArgs e)
        {
            var v = dataRetriver.GetFilteredPerson(searchTerm.Text);
            

            // sett datakilde.
            // skulle laget til MVVM.. pga dårlig tid ble dette ikke gjort.
            
        }
    }
}
