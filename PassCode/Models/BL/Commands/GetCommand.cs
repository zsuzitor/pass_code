

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class GetCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;
        private readonly IAppSettings _appSettings;

        public GetCommand(IOutput output, IWordContainer container, ICoder coder, IAppSettings appSettings)
        {
            _customName = "get";
            _output = output;
            _container = container;
            _coder = coder;
            _appSettings = appSettings;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - get word by key - '{_customName} <word_key> <?'dec'>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var splitCommand = command.Split(" ");
            var argCount = 2;
            if (splitCommand.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            var word = _container.Get(splitCommand[1]);
            if (word == null)
            {
                throw new CommandHandleException("не найдено");
            }

            if (splitCommand.Length > 2 && splitCommand[2] == "dec")
            {
                var decodedVal = 
                    _coder.DecryptFromBytes(
                        _coder.CustomStringToBytes(word.Value), _appSettings.Key);
                _output.WriteLine($"{word.Key} - {decodedVal}");
            }
            else
            {
                _output.WriteLine($"{word.Key} - {word.Value}");
            }

            return true;
        }
    }
}
