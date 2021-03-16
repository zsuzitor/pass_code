

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class AddCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IWordContainer _container;
        private readonly ICoder _coder;
        private readonly IAppSettings _appSettings;


        public AddCommand(IWordContainer container, ICoder coder, IAppSettings appSettings)
        {
            _customName = "add";
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
            return $"{_customName} - add new word - '{_customName} <key> <value>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var splitCommand = command.Split(" ");
            var argCount = 3;
            if (splitCommand.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            var key = splitCommand[1];
            if (key.Contains("-"))
            {
                throw new CommandHandleException($"символ '-' в ключах запрещен");
            }

            var value = splitCommand[2];
            var encodedValString = _coder.EncryptWithString(value, _appSettings.Key);
            _container.Add(new OneWord() { Key = key, Value = encodedValString, });

            return true;
        }

    }
}
