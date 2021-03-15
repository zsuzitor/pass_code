

namespace PassCode.Models.BL.Interfaces
{
    public interface IAppSettings
    {
        public string Key { get; set; }
        public string Login { get; set; }
        public bool HasValideKey();
        public bool HasValideLogin();
        public bool HasValideDataForAuth();
        bool LoginIsGood(string equalsLogin);
        void ClearCredit();
    }
}
