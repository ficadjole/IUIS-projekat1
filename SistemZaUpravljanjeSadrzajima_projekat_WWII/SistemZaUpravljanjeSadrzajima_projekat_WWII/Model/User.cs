using static SistemZaUpravljanjeSadrzajima_projekat_WWII.Enums.TypeOfUser;

namespace SistemZaUpravljanjeSadrzajima_projekat_WWII.Model
{
    [Serializable]
    public class User
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserRole UserRole { get; set; }

        public User() { }

        public User(string username, string password, UserRole userRole)
        {
            UserName = username;
            Password = password;
            UserRole = userRole;
        }
    }
}
