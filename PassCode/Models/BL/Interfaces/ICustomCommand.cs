

namespace PassCode.Models.BL.Interfaces
{
    public interface ICustomCommand
    {
        bool TryDo(string command);
        string GetCutomName();//ключевое слово по которому команда вызывается
        string GetShortDescription();
    }
}
