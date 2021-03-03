using PassCode.Models.BL.Interfaces;
using System;

namespace PassCode.Models.BL.Commands
{
    class ClearCommand : ICustomCommand
    {
        private readonly IOutput _output;
        private readonly IWordContainer _container;

        public ClearCommand(IOutput output, IWordContainer container)
        {
            _output = output;
            _container = container;
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith("clear"))
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
