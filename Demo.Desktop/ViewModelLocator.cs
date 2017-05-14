using Demo.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Desktop
{
    public class ViewModelLocator
    {
        private static PersonOverviewViewModel personOverviewViewModel = new PersonOverviewViewModel();
        private static PersonViewModel personViewModel = new PersonViewModel();

        public static PersonOverviewViewModel PersonOverviewViewModel => personOverviewViewModel;
        public static PersonViewModel PersonViewModel => personViewModel;
    }
}
