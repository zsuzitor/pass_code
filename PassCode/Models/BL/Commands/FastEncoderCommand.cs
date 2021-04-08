
using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class FastEncoderCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly ICoder _coder;
        public FastEncoderCommand(IOutput output, ICoder coder)
        {
            _customName = "fenc";

            _output = output;
            _coder = coder;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - fast encode string - '{_customName} <some count of str> <last part is pass>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var splitCommand = command.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var argCount = 3;
            if (splitCommand.Length < argCount)
            {
                throw new CommandHandleException($"{argCount} аргумента");
            }

            var values = splitCommand[1..^1];
            var value = string.Join(" ", values);
            var str = _coder.EncryptWithString(value, splitCommand[splitCommand.Length - 1]);
            _output.WriteLine(str);

            return true;

        }
    }
}
