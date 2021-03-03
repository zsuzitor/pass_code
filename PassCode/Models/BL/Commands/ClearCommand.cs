using PassCode.Models.BL.Interfaces;
using System;

namespace PassCode.Models.BL.Commands
{
    class ClearCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;

        public ClearCommand(IOutput output, IWordContainer container)
        {
            _customName = "clear";

            _output = output;
            _container = container;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "clear - clear word container and console - 'clear'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            _container.Clear();
            _container.Decoded = false;
            //_container.FileLoaded = false;
            _output.Clear();

            return true;
        }

    }
}
