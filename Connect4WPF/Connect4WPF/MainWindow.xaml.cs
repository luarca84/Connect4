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

namespace Connect4WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new BoardViewModel();
        }

        private void btnCol1_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(0);
        }

        private void btnCol2_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(1);
        }

        private void btnCol3_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(2);
        }

        private void btnCol4_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(3);
        }

        private void btnCol5_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(4);
        }

        private void btnCol6_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(5);
        }

        private void btnCol7_Click(object sender, RoutedEventArgs e)
        {
            BoardViewModel bvm = (BoardViewModel)this.DataContext;
            bvm.ClickBtnColumn(6);
        }
    }
}
