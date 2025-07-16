using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vezbe2.Helpers;

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataIO serializer = new DataIO();
        public ObservableCollection<User> Users { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            Users = serializer.DeSerializeObject<ObservableCollection<User>>("UsersRepository.xml");
        
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool isExisted = false;

            string inputUserName = txtBoxName.Text;
            string inputPassword = txtBoxPassword.Text;

            if (!validateFormData(inputUserName, inputPassword))
            {
                
            }

            foreach (User user in Users) { 
                
                if(user.UserName.Equals(txtBoxName.Text) && user.Password.Equals(txtBoxPassword.Text)){
                    isExisted = true;

                    txtBoxName.Text = "Username";
                    txtBoxPassword.Text = "Password";

                    PageOfBattles pageOfBattles = new PageOfBattles(user);  

                    pageOfBattles.Show();
                        
                    break;
                }
                    
            }

            if (!isExisted) MessageBox.Show("Nepostojeci korisnik");
        }

        private bool validateFormData(string userName, string password)
        {
            if (userName.Trim().Equals(string.Empty) || password.Trim().Equals(string.Empty)) 
            {

                UserNameErrorLable.Content = "Form filed cannot be left empyt!";
                
                return false;
            }

            return true;
        }
    }
}