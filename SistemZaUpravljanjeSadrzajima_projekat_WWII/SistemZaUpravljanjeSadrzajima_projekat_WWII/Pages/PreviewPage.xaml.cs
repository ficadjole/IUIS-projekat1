using SistemZaUpravljanjeSadrzajima_projekat_WWII.Model;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII
{
    /// <summary>
    /// Interaction logic for PreviewPage.xaml
    /// </summary>
    public partial class PreviewPage : Window
    {
        public PreviewPage(Battle battle)
        {
            InitializeComponent();
             
            txtBoxBattleName.Text = " - " + battle.NameOfBattle;
            txtBoxBattleDate.Text = " - " + battle.YearOfBattle.ToString();

            if (!string.IsNullOrEmpty(battle.ImgUrl))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(battle.ImgUrl, UriKind.RelativeOrAbsolute));
                    imgBattle.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(battle.RtfUrl))
            {
                try
                {
                    
                    FlowDocument flowDoc = new FlowDocument();
                    TextRange textRange = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
                    using (var stream = new System.IO.FileStream(battle.RtfUrl, System.IO.FileMode.Open))
                    {   
                        textRange.Load(stream, DataFormats.Rtf);
                    }
                    EditorRichTextBox.Document = flowDoc;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading RTF document: " + ex.Message);
                }
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
