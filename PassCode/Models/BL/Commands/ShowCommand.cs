

using PassCode.Models.BL.Interfaces;

namespace PassCode.Models.BL.Commands
{
    public class ShowCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;

        public ShowCommand(IOutput output, IWordContainer container, ICoder coder)
        {
            _customName = "show";
            _output = output;
            _container = container;
            _coder = coder;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "show - show all data(or keys only) - 'show <?keys>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var showOnlyKeys = command.Contains("keys");
            foreach (var word in _container.GetAll())
            {
                var decrString = "...";
                if (!showOnlyKeys)
                {
                    decrString = _coder.RemoveRandomizeFromString(word.Value);
                }
                _output.WriteLine($"{word.Key} - {decrString}");
            }

            return true;
        }
    }
}
