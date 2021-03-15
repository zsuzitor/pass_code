

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class FastDecoderCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly ICoder _coder;

        public FastDecoderCommand(IOutput output, ICoder coder)
        {
            _customName = "fdec";

            _output = output;
            _coder = coder;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - fast decode string - '{_customName} <str> <pass>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var splitCommand = command.Split(" ");
            var argCount = 3;
            if (splitCommand.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            var bytes = _coder.CustomStringToBytes(splitCommand[1]);
            var str = _coder.DecryptFromBytes(bytes, splitCommand[2]);
            _output.WriteLine(_coder.RemoveRandomizeFromString(str));

            return true;
        }
    }
}
