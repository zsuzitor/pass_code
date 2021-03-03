

using PassCode.Models.BL.Interfaces;

namespace PassCode.Models.BL
{
    public class AppSettings : IAppSettings
    {
        public string Key { get; set; }//todo вроде есть зашифрованные строки, может использовать их?
    }
}
