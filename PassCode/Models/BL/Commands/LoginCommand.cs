

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    class LoginCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IAppSettings _appSettings;

        public LoginCommand(IOutput output,  IAppSettings appSettings)
        {
            _customName = "login";

            _output = output;
            _appSettings = appSettings;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "login - login - 'login <password>'";
        }


        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var commandSplit = command.Split(" ");
            var argCount = 2;
            if (commandSplit.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            _appSettings.Key = commandSplit[1];
            if (!_appSettings.HasValideKey())
            {
                _appSettings.Key = null;
                throw new CommandHandleException("ключ не передан или не валиден");
            }

            return true;
        }
    }
}

