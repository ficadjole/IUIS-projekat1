using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
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

        private string txtBoxNamePlaceholder = "Enter your username";

        public MainWindow()
        {
            InitializeComponent();
            Users = serializer.DeSerializeObject<ObservableCollection<User>>("UsersRepository.xml");

            txtBoxName.Text = txtBoxNamePlaceholder;
            txtBoxName.Foreground = Brushes.Gray;


        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string inputUserName = txtBoxName.Text;
            string inputPassword = txtBoxPassword.Password;

            if (!validateFormData(inputUserName, inputPassword))
            {
                MessageBox.Show("Please fill all fields correctly!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                txtBoxName.Text = txtBoxNamePlaceholder;
                txtBoxName.Foreground = Brushes.Gray;

                txtBoxPassword.Password = string.Empty;
                UserNameErrorLable.Content = string.Empty;
                PasswordErrorLable.Content = string.Empty;


            }
            else
            {
                if (authUser(inputUserName, inputPassword))
                {
                    txtBoxName.Text = txtBoxNamePlaceholder;
                    txtBoxName.Foreground = Brushes.Gray;

                    txtBoxPassword.Password = string.Empty;
                    UserNameErrorLable.Content = string.Empty;
                    PasswordErrorLable.Content = string.Empty;
                }
                else
                {

                    MessageBox.Show("User does not exist! Please try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtBoxName.Text = txtBoxNamePlaceholder;
                    txtBoxName.Foreground = Brushes.Gray;

                    txtBoxPassword.Password = string.Empty;
                    UserNameErrorLable.Content = string.Empty;
                    PasswordErrorLable.Content = string.Empty;


                }
            }

        }

        private bool validateFormData(string userName, string password)
        {

            if (userName.Trim().Equals(string.Empty) || userName.Trim().Equals(txtBoxNamePlaceholder))
            {

                UserNameErrorLable.Content = "Username filed cannot be left empyt!";

                return false;

            }
            else if (password.Trim().Equals(string.Empty))
            {
                PasswordErrorLable.Content = "Password filed cannot be left empyt!";

                return false;
            }

            return true;
        }

        private bool authUser(string userName, string password)
        {
            bool isExisted = false;
            foreach (User user in Users)
            {
                if (user.UserName.Equals(userName) && user.Password.Equals(password))
                {
                    isExisted = true;
                    txtBoxName.Text = txtBoxNamePlaceholder;
                    txtBoxPassword.Password = string.Empty;
                    PageOfBattles pageOfBattles = new PageOfBattles(user);
                    pageOfBattles.ShowDialog();
                    break;
                }
            }

            return isExisted;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void txtBoxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text.Trim().Equals(txtBoxNamePlaceholder))
            {
                txtBoxName.Text = string.Empty;
                txtBoxName.Foreground = Brushes.Black;
            }
        }

        private void txtBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxName.Text.Trim().Equals(string.Empty))
            {
                txtBoxName.Text = txtBoxNamePlaceholder;
                txtBoxName.Foreground = Brushes.LightSlateGray;
            }
        }


    }
}