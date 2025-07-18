using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
using SistemZaUpravljanjeSadrzajima_projekat_WWII.Pages;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Vezbe2.Helpers;

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII
{
    /// <summary>
    /// Interaction logic for PageOfBattles.xaml
    /// </summary>
    public partial class PageOfBattles : Window
    {
        private DataIO serializer = new DataIO();
        private User currentUser { get; set; }
        public ObservableCollection<Battle> Battles { get; set; }
        public PageOfBattles(User user)
        {
            InitializeComponent();

            currentUser = user;

            if (user.UserRole == Enums.TypeOfUser.UserRole.ADMIN)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                dataGridColumnDelete.Visibility = Visibility.Visible;
            }


            Battles = serializer.DeSerializeObject<ObservableCollection<Battle>>("BattlesRepository.xml");
            Battles = this.Battles;
            this.DataContext = this;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SaveDataAsXML();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewBattle addNewBattleWindow = new AddNewBattle(Battles);

            addNewBattleWindow.Owner = this;

            addNewBattleWindow.ShowDialog();
        }

        private void SaveDataAsXML()
        {
            serializer.SerializeObject(Battles, "BattlesRepository.xml");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            

            bool isAnySelected = false;

            if (Battles.Count == 0)
            {
                MessageBox.Show("There are no battles to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var battle in Battles.ToList())
            {
                if ((bool)battle.Selected)
                {
                    Battles.Remove(battle); //uklanja bitku iz kolekcije

                    File.Delete(battle.RtfUrl); //brise rtf fajl bitke

                    isAnySelected = true;
                }
            }

            if (!isAnySelected)
            {
                MessageBox.Show("No battles selected for deletion!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show("Selected battles have been deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);



                dataGridBattles.Items.Refresh();
                SaveDataAsXML();
            }



        }

        private void chckBoxSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (chckBoxSelectAll.IsChecked == true)
            {
                foreach (var battle in Battles)
                {
                    battle.Selected = true;

                }
            }
            else
            {
                foreach (var battle in Battles)
                {
                    battle.Selected = false;

                }
            }

            dataGridBattles.Items.Refresh();
        }

        private void chckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.DataContext is Battle battle)
            {
                battle.Selected = checkBox.IsChecked == true;
            }
        }

        private void HyperLinkItem_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;

            if (currentUser.UserRole != Enums.TypeOfUser.UserRole.ADMIN)
            {
                if (hyperlink.DataContext is Battle battle)
                {
                    PreviewPage previewPage = new PreviewPage(battle);
                    previewPage.ShowDialog();
                }

            }
            else
            {

                if (hyperlink.DataContext is Battle battle)
                {
                    EditBattle editBattle = new EditBattle(battle);
                    editBattle.Owner = this;
                    editBattle.ShowDialog();

                    dataGridBattles.Items.Refresh();
                }

            }


        }
    }
}
