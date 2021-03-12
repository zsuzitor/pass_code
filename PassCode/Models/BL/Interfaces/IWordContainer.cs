using PassCode.Models.BO;
using System.Collections.Generic;

namespace PassCode.Models.BL.Interfaces
{
    public interface IWordContainer
    {
        //public bool Decoded { get; set; }
        //public bool FileLoaded { get; set; }

        OneWord Get(string key);
        bool Delete(string key);
        bool Edit(OneWord newData);
        OneWord Add(OneWord newData);
        IEnumerable<OneWord> GetAll();
        void Clear();
        bool HasRecords();
    }
}
