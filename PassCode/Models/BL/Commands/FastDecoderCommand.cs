

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class FastDecoderCommand : ICustomCommand
    {
        private readonly IOutput _output;
        private readonly ICoder _coder;

        public FastDecoderCommand(IOutput output, ICoder coder)
        {
            _output = output;
            _coder = coder;
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith("fdec"))
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
            _output.WriteLine(str);

            return true;
        }
    }
}
