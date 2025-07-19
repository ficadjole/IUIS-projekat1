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

            addNewBattleWindow.ShowDialog();
        }

        private void SaveDataAsXML()
        {
            serializer.SerializeObject(Battles, "BattlesRepository.xml");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (!validationDelete())
            {
                return; //ako validacija nije prosla, nece se izvrsiti brisanje
            }


            if (chckBoxSelectAll.IsChecked == true) //slucaj ako su svi selektovani
            {

                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete all battles?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var battle in Battles.ToList())
                    {
                        File.Delete(battle.RtfUrl); //brise rtf fajl bitke
                    }
                    Battles.Clear(); //uklanja sve bitke iz kolekcije

                    MessageBox.Show("All battles have been successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (result == MessageBoxResult.No)
                {
                    chckBoxSelectAll.IsChecked = false; //ako korisnik odustane od brisanja, restartovati da ne bude selektovano

                    foreach (var battle in Battles)
                    {
                        battle.Selected = false; //ako korisnik odustane od brisanja, restartovati da ne budu selektovani
                    }

                }

            }
            else
            {
                //slucaj ako je bar jedan selektovan, jer na primer mozda je korisnik greskom uzeo Kursk i Sutjesku, a hteo je samo Kursk, moci ce da ne izbrise Sutjesku

                foreach (var battle in Battles.ToList())
                {
                    if ((bool)battle.Selected)
                    {

                        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the battle: {battle.NameOfBattle}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {

                            Battles.Remove(battle); //uklanja bitku iz kolekcije

                            File.Delete(battle.RtfUrl); //brise rtf fajl bitke

                            MessageBox.Show($"Battle {battle.NameOfBattle} has been successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (result == MessageBoxResult.No)
                        {

                            battle.Selected = false; //ako korisnik odustane od brisanja, nece se ukloniti iz kolekcije
                            continue;
                        }

                    }
                }
            }

            dataGridBattles.Items.Refresh();
            SaveDataAsXML();

        }

        private bool validationDelete()
        {

            bool isAnySelected = false;

            if (Battles.Count == 0)
            {
                MessageBox.Show("There are no battles to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (var battle in Battles)
            {
                if (battle.Selected == true)
                {
                    isAnySelected = true;
                    break;
                }
            }

            if (!isAnySelected && chckBoxSelectAll.IsChecked == false) //slucaj ako nijedna nije selektovana
            {
                MessageBox.Show("Please select at least one battle to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

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
                    editBattle.ShowDialog();

                    dataGridBattles.Items.Refresh();
                }

            }


        }
    }
}
