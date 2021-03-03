using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System.Collections.Generic;
using System.Linq;

namespace PassCode.Models.BL
{
    public class WordContainer : IWordContainer
    {

        public bool Decoded { get; set; }
        //public bool FileLoaded { get; set; }


        private readonly List<OneWord> _data;


        public WordContainer()
        {
            _data = new List<OneWord>();
            Decoded = false;
        }


        public bool Delete(string key)
        {
            var count = _data.RemoveAll(x => x.Key == key);
            return count > 0;
        }

        public bool Edit(OneWord newData)
        {
            if (newData == null)
            {
                return false;
            }

            var oldVal = _data.FirstOrDefault(x => x.Key == newData.Key);
            if (oldVal == null)
            {
                return false;
            }

            oldVal.Value = newData.Value;
            return true;
        }

        public OneWord Add(OneWord newData)
        {
            if (newData == null)
            {
                return null;
            }

            _data.Add(newData);
            return newData;
        }

        public OneWord Get(string key)
        {
            return _data.FirstOrDefault(x => x.Key == key);
        }

        public IEnumerable<OneWord> GetAll()
        {
            return _data.AsReadOnly();
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool HasRecords()
        {
            return _data.Any();
        }
    }
}
