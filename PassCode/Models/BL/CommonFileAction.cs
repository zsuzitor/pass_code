using PassCode.Models.BL.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace PassCode.Models.BL
{
    public class CommonFileAction : IFileAction
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            File.WriteAllLines(path, contents);
        }
    }
}
