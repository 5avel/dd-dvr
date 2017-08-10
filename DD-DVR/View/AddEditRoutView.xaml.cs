using DD_DVR.ViewModel;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using DD_DVR.Data.Model;
using System.Collections.ObjectModel;

namespace DD_DVR.View
{

    public partial class AddEditRoutView : MetroWindow
    {

        public string Str { get; set; }
        public AddEditRoutView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Str = strtb.Text;
            // str не пустое и не null
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Str = null;
            this.Close();
        }
    }
}
