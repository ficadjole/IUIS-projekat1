using Microsoft.Win32;
using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII
{
    /// <summary>
    /// Interaction logic for AddNewBattle.xaml
    /// </summary>
    public partial class AddNewBattle : Window
    {
        ObservableCollection<Battle> battles;

        private string battleNamePlaceHolder = "Enter battle name";
        private string battleDatePlaceHolder = "Enter year of battle";

        public AddNewBattle(ObservableCollection<Battle> battles)
        {
            InitializeComponent();

            txtBoxBattleDate.Text = battleDatePlaceHolder;
            txtBoxBattleName.Text = battleNamePlaceHolder;
            txtBoxBattleName.Foreground = Brushes.LightSlateGray;
            txtBoxBattleDate.Foreground = Brushes.LightSlateGray;


            this.battles = battles;
            FontFamilyCombox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            btlImgErrorLbl.Content = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                imgBattle.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }


        private void FontFamilyCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyCombox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {

                EditorRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyCombox.SelectedItem);
            }
        }
        private void FontSizeCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeCombox.SelectedItem != null && !EditorRichTextBox.Selection.IsEmpty)
            {

                ComboBoxItem selectedFontSize = FontSizeCombox.SelectedItem as ComboBoxItem;

                if (double.TryParse(selectedFontSize.Content.ToString(), out double FontSize))
                {
                    EditorRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, FontSize);
                }

            }
        }

        private void EditorRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object fontWeight = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);

            BoldButton.IsChecked = (fontWeight != DependencyProperty.UnsetValue && fontWeight.Equals(FontWeights.Bold));

            object fontStyle = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);

            FontFamilyCombox.SelectedItem = fontStyle;

            object fontItalic = EditorRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);

            ItalicButton.IsChecked = (fontItalic != DependencyProperty.UnsetValue && fontItalic.Equals(FontStyles.Italic));

            object fontSize = EditorRichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            //ovo radimo jer mi varaca vrednost fontSize da je double i da bih mogao da promenim u ComboBoxu da pise ta velicina fonta iz RichTextBoxa
            //morao sam da prodjem kroz sve elemente FontSizeCombox-a i da ga onda postavim da bude selectovan
            foreach (ComboBoxItem item in FontSizeCombox.Items)
            {
                if (item.Content.ToString() == fontSize.ToString())
                {
                    FontSizeCombox.SelectedItem = item;
                    break;
                }
            }

            object foregroundColor = EditorRichTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);

            if (foregroundColor is SolidColorBrush solidColorBrush)
            {
                ColorPickerText.SelectedColor = solidColorBrush.Color;
            }


        }

        private void ColorPickerText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

            EditorRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush((Color)ColorPickerText.SelectedColor));

        }

        private void btnAddBattle_Click(object sender, RoutedEventArgs e)
        {

            if (!BattleValidation())
            {
                return;
            }
            else
            {
                battles.Add(new Battle
                {
                    NameOfBattle = txtBoxBattleName.Text,
                    YearOfBattle = int.Parse(txtBoxBattleDate.Text),
                    RtfUrl = writeInRTF(),
                    ImgUrl = imgBattle.Source.ToString(),
                    TimeAdded = DateTime.Now.ToString("dd-MM-yyyy")
                });
                MessageBox.Show("Battle added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BattleNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxBattleName.Text.Trim().Equals(battleNamePlaceHolder))
            {
                txtBoxBattleName.Text = string.Empty;
                txtBoxBattleName.Foreground = Brushes.Black;
            }

            btlNameErrorLbl.Content = string.Empty;
            txtBoxBattleNameBorder.BorderBrush = Brushes.Transparent;
            txtBoxBattleNameBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void BattleNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxBattleName.Text.Trim().Equals(string.Empty))
            {
                txtBoxBattleName.Text = battleNamePlaceHolder;
                txtBoxBattleName.Foreground = Brushes.LightSlateGray;
            }
        }

        private void BattleDateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxBattleDate.Text.Trim().Equals(battleDatePlaceHolder))
            {
                txtBoxBattleDate.Text = string.Empty;
                txtBoxBattleDate.Foreground = Brushes.Black;
            }

            btlDateErrorLbl.Content = string.Empty;
            txtBoxBattleDateBorder.BorderBrush = Brushes.Transparent;
            txtBoxBattleDateBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void BattleDateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBoxBattleDate.Text.Trim().Equals(string.Empty))
            {
                txtBoxBattleDate.Text = battleDatePlaceHolder;
                txtBoxBattleDate.Foreground = Brushes.LightSlateGray;
            }
        }

        private string writeInRTF()
        {

            string rtfFilePath = @"C:\Users\Filip\OneDrive\Desktop\SistemZaUpravljanjeSadrzajima_projekat_WWII\SistemZaUpravljanjeSadrzajima_projekat_WWII\RTFs\" + txtBoxBattleName.Text.Trim() + ".rtf";

            TextRange textRange = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd);

            using (var stream = new System.IO.FileStream(rtfFilePath, System.IO.FileMode.Create))
            {
                textRange.Save(stream, DataFormats.Rtf);
            }

            return rtfFilePath;
        }

        private bool BattleValidation()
        {
            bool isValid = false;

            if (txtBoxBattleName.Text.Trim().Equals(battleNamePlaceHolder))
            {
                MessageBox.Show("Please write battle name before adding a battle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btlNameErrorLbl.Content = "Battle name cannot be empty!";
                txtBoxBattleNameBorder.BorderBrush = Brushes.Red;
                txtBoxBattleNameBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else if (txtBoxBattleDate.Text.Trim().Equals(battleDatePlaceHolder))
            {
                MessageBox.Show("Please write battle year before adding a battle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btlDateErrorLbl.Content = "Year of battle cannot be empty!";
                txtBoxBattleDateBorder.BorderBrush = Brushes.Red;
                txtBoxBattleDateBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else if (!int.TryParse(txtBoxBattleDate.Text, out int year) || year < 1939 || year > 1945)
            {
                MessageBox.Show("Please enter a valid year for the battle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btlDateErrorLbl.Content = "Year must be between 1939 and 1945!";
            }
            else if (imgBattle.Source == null)
            {
                MessageBox.Show("Please add battle image before adding a battle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btlImgErrorLbl.Content = "Image cannot be empty!";
            }
            else if (validationEditorRichTextBox())
            {
                MessageBox.Show("Please write battle description before adding a battle.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                richTxtBoxErrorLbl.Content = "Description cannot be empty!";
                EditorRichTextBox.BorderBrush = Brushes.Red;
                EditorRichTextBox.BorderThickness = new Thickness(1, 1, 1, 1);

                
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        private bool validationEditorRichTextBox() { 
        
            string text = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd).Text.Trim(); //uzimamo ceo tekst iz richTextBox-a

            if (string.IsNullOrWhiteSpace(text)) {
                return true;
            }
            else { return false; }

        }

        private void EditorRichTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int numberOfWords = 0;

            string text = new TextRange(EditorRichTextBox.Document.ContentStart, EditorRichTextBox.Document.ContentEnd).Text.Trim();

            if (!string.IsNullOrEmpty(text))
            {
                numberOfWords = text.Split(new char[] { ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
            }

            lblWordCount.Content = $"Words: {numberOfWords}";
        }
    }
}
