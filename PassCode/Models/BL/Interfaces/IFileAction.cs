

using System.Collections.Generic;

namespace PassCode.Models.BL.Interfaces
{
    public interface IFileAction
    {
        bool Exists(string path);
        void WriteAllLines(string path, IEnumerable<string> contents);
        string[] ReadAllLines(string path);
    }
}
