

using PassCode.Models.BL.Interfaces;

namespace PassCode.Models.BL.Commands
{
    public class ShowCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;

        public ShowCommand(IOutput output, IWordContainer container)
        {
            _customName = "show";
            _output = output;
            _container = container;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - show all data(or keys only) - '{_customName} <?all>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var showOnlyKeys = !command.Contains("all");
            foreach (var word in _container.GetAll())
            {
                var decrString = "...";
                if (!showOnlyKeys)
                {
                    //decrString = _coder.RemoveRandomizeFromString(word.Value);
                    decrString = word.Value;
                }
                _output.WriteLine($"{word.Key} - {decrString}");
            }

            return true;
        }
    }
}
