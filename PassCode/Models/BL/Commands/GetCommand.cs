

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class GetCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;

        public GetCommand(IOutput output, IWordContainer container, ICoder coder)
        {
            _customName = "get";
            _output = output;
            _container = container;
            _coder = coder;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "get - get word by key - 'get <key>'";
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

            var word = _container.Get(splitCommand[1]);
            if (word == null)
            {
                throw new CommandHandleException("не найдено");
            }

            _output.WriteLine($"{word.Key} - {_coder.RemoveRandomizeFromString(word.Value)}");


            return true;
        }
    }
}
