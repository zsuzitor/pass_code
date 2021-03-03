

using PassCode.Models.BL.Interfaces;

namespace PassCode.Models.BL.Commands
{
    public class ShowCommand : ICustomCommand
    {
        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;

        public ShowCommand(IOutput output, IWordContainer container, ICoder coder)
        {
            _output = output;
            _container = container;
            _coder = coder;
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith("show"))
            {
                return false;
            }

            foreach (var word in _container.GetAll())
            {
                _output.WriteLine($"{word.Key} - {_coder.RemoveRandomizeFromString(word.Value)}");
            }

            return true;
        }
    }
}
