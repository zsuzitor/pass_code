

using PassCode.Models.BL.Interfaces;

namespace PassCode.Models.BL
{
    public class AppSettings : IAppSettings
    {
        public string Login { get; set; }
        public string Key { get; set; }//todo вроде есть зашифрованные строки, может использовать их?

        public bool HasValideKey()
        {
            return (!string.IsNullOrWhiteSpace(Key));
        }

        public bool HasValideLogin()
        {
            return (!string.IsNullOrWhiteSpace(Login));
        }

        public bool HasValideDataForAuth()
        {
            return HasValideKey() && HasValideLogin();
        }

        public bool LoginIsGood(string equalsLogin)
        {
            return Login == equalsLogin;
        }

        public void ClearCredit()
        {
            Key = null;
            Login = null;
        }
    }
}
