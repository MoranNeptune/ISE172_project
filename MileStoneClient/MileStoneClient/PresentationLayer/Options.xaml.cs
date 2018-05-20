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

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
  
    public partial class Options : Page
    {
        ObservableObject _main = new ObservableObject();

        public Options()
        {
            InitializeComponent();
            this.DataContext = _main;
            //initialize with ascending sort, none filter and sort by timestamp
            _main.AscendingIsChecked = true;
            _main.DescendingIsChecked = false;
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
}
