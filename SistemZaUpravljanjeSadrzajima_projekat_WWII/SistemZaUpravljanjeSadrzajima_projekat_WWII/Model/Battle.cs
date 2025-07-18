namespace SistemZaUpravljanjeSadrzajima_projekat_WWII.Model
{
    [Serializable]
    public class Battle
    {
        public string NameOfBattle { get; set; }
        public int YearOfBattle { get; set; }
        public string ImgUrl { get; set; }
        public string RtfUrl { get; set; }
        public string TimeAdded { get; set; }

        public bool? Selected { get; set; }

        public Battle(string nameOfBattle, int yearOfBattle, string imgUrl, string rtfUrl)
        {
            NameOfBattle = nameOfBattle;
            YearOfBattle = yearOfBattle;
            ImgUrl = imgUrl;
            RtfUrl = rtfUrl;
            TimeAdded = DateTimeOffset.Now.ToString("dd-MM-yyyy");
            Selected = false;
        }

        public Battle()
        {
            NameOfBattle = string.Empty;
            YearOfBattle = 0;
            ImgUrl = string.Empty;
            RtfUrl = string.Empty;
            TimeAdded = DateTimeOffset.Now.ToString("dd-MM-yyyy");
            Selected = false;
        }


    }
}
