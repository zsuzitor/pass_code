

namespace PassCode.Models.BL.Interfaces
{
    public interface ICustomCommand
    {
        bool TryDo(string command);
    }
}
