

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    class LoginCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IAppSettings _appSettings;

        public LoginCommand(IAppSettings appSettings)
        {
            _customName = "login";
            _appSettings = appSettings;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - login - '{_customName} <login> <password>'";
        }


        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var commandSplit = command.Split(" ");
            var argCount = 3;
            if (commandSplit.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            if (_appSettings.HasValideDataForAuth())
            {
                throw new CommandHandleException($"auth was before, for relogin user clear command");
            }
            _appSettings.Login = commandSplit[1];
            _appSettings.Key = commandSplit[2];
            if (!_appSettings.HasValideDataForAuth())
            {
                _appSettings.Login = null;
                _appSettings.Key = null;
                throw new CommandHandleException("ключ не передан или не валиден");
            }

            return true;
        }
    }
}

