

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System;

namespace PassCode.Models.BL.Commands
{
    public class AddCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;


        public AddCommand(IOutput output, IWordContainer container, ICoder coder)
        {
            _customName = "add";

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
            return "add - add new word - 'add <key> <value>'";
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

            if (!_container.Decoded)
            {
                if (_container.HasRecords())
                {
                    throw new CommandHandleException($"попытка работы с зашифрованным списком");
                }
                else
                {
                    _container.Decoded = true;
                }
            }
            

            var key = splitCommand[1];
            if (key.Contains("-"))
            {
                throw new CommandHandleException($"символ '-' в ключах запрещен");
            }

            var value = _coder.AddRandomizeToString(splitCommand[2]);
            _container.Add(new OneWord() { Key = key, Value = value, });

            return true;
        }

        

    }
}
