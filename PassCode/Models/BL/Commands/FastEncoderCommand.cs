﻿
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
            return $"{_customName} - fast encode string - '{_customName} <str> <pass>'";
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

            var str = _coder.EncryptWithString(splitCommand[1], splitCommand[2]);
            _output.WriteLine(str);

            return true;

        }
    }
}
