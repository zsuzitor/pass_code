using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class RemoveCommand : ICustomCommand
    {
        private readonly IOutput _output;
        private readonly IWordContainer _container;

        public RemoveCommand(IOutput output, IWordContainer container)
        {
            _output = output;
            _container = container;
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith("rm"))
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
            //if (word == null)
            //{
            //    throw new CommandHandleException("не найдено");
            //}

            _output.WriteLine($"deleted");

            return true;
        }
    }
}
