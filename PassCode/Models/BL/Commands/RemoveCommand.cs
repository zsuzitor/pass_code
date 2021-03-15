using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class RemoveCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;

        public RemoveCommand(IOutput output, IWordContainer container)
        {
            _customName = "rm";
            _output = output;
            _container = container;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - remove word by key - {_customName} <key>";
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

            _ = _container.Delete(splitCommand[1]);


            _output.WriteLine($"deleted");

            return true;
        }
    }
}
