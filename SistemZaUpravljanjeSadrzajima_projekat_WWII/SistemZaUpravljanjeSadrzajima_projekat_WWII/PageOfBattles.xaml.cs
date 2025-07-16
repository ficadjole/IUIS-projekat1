using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
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

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII
{
    /// <summary>
    /// Interaction logic for PageOfBattles.xaml
    /// </summary>
    public partial class PageOfBattles : Window
    {
        public PageOfBattles(User user)
        {
            InitializeComponent();

            MessageBox.Show($"Prijavljen je {user.UserName}");
        }
    }
}
