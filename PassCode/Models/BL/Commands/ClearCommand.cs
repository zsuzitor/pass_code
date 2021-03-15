using PassCode.Models.BL.Interfaces;
using System;

namespace PassCode.Models.BL.Commands
{
    class ClearCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly IAppSettings _appSettings;

        public ClearCommand(IOutput output, IWordContainer container, IAppSettings appsettings)
        {
            _customName = "clear";

            _output = output;
            _container = container;
            _appSettings = appsettings;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - clear word container and console - '{_customName}'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            _container.Clear();
            //_container.Decoded = false;
            _appSettings.ClearCredit();
            //_container.FileLoaded = false;
            _output.Clear();

            return true;
        }

    }
}
