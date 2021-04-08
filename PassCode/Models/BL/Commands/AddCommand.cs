

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
            return $"{_customName} - add new word - '{_customName} <key> <some count of value>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            if (command.Contains(Consts.FileDataStringSeparate))
            {
                throw new CommandHandleException($"символ '{Consts.FileDataStringSeparate}' в ключах запрещен");
            }

            var splitCommand = command.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var argCount = 3;
            if (splitCommand.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            var key = splitCommand[1];

            var values = splitCommand[2..];//убираем ключевое слово и key
            var value = string.Join(" ", values);
            var encodedValString = _coder.EncryptWithString(value, _appSettings.Key);
            _container.Add(new OneWord() { Key = key, Value = encodedValString, });

            return true;
        }

    }
}
