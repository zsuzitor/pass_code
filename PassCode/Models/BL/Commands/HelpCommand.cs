using PassCode.Models.BL.Interfaces;
using System.Collections.Generic;

namespace PassCode.Models.BL.Commands
{
    public class HelpCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly List<ICustomCommand> _commands;

        public HelpCommand(IOutput output, List<ICustomCommand> commands)
        {
            _customName = "help";
            _output = output;
            _commands = commands;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"write '{_customName}' and get magic";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            foreach (var cmd in _commands)
            {
                _output.WriteLine(cmd.GetShortDescription());
            }

            return true;
        }
    }
}
