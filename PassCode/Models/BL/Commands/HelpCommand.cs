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
            return "write 'help' and get magic";
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

            //todo подумать, возможно вынести в каждую команду под метод GetInfo например, и в мейне просто перебирать
            //_output.WriteLine("help");
            //_output.WriteLine("lcf - load crypted file - 'lcf <filepath>'");
            //_output.WriteLine("dec - decode loaded file - 'dec <password>'");
            //_output.WriteLine("show - show all data - 'show'");
            //_output.WriteLine("save - encrypt and save new file - 'save <newfilename> <?key>'");
            //_output.WriteLine("add - add new word - 'add <key> <value>'");
            //_output.WriteLine("rm - remove word by key - rm <key>");
            //_output.WriteLine("get - get word by key - 'get <key>'");
            //_output.WriteLine("clear - clear word container and console - 'clear'");
            //_output.WriteLine("fdec - fast decode string - 'fdec <str> <pass>'");
            //_output.WriteLine("fenc - fast encode string - 'fenc <str> <pass>'");


            return true;
        }
    }
}
