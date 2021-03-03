

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System.Collections.Generic;
using System.IO;

namespace PassCode.Models.BL.Commands
{
    public class SaveCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly ICoder _coder;
        private readonly IAppSettings _appSettings;
        private readonly IFileAction _fileAction;

        public SaveCommand(IOutput output, IWordContainer container, ICoder coder,
            IAppSettings appSettings, IFileAction fileAction)
        {
            _customName = "save";
            _output = output;
            _container = container;
            _coder = coder;
            _appSettings = appSettings;
            _fileAction = fileAction;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "save - encrypt and save new file - 'save <newfilename> <?key>'";
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

            string key = _appSettings?.Key;
            if (splitCommand.Length > 2)
            {
                key = splitCommand[2];
            }

            if (_container.Decoded)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new CommandHandleException($"ключ не задан");
                }
            }
            else
            {
                _output.WriteLine("внимание, данные просто пересохранены в указанный файл, дополнительное шифрование не производилось тк данные были уже зашифрованы");
            }
            

            string pathForSave =  splitCommand[1];//"./"

            if (_fileAction.Exists(pathForSave))
            {
                throw new CommandHandleException($"файл уже существует");
            }


            List<string> forWrite = new List<string>();
            foreach (var item in _container.GetAll())
            {
                string value = item.Value;
                if (_container.Decoded)//мб тут лишнее, шифровать всегда
                {
                    value = _coder.BytesToCustomString(_coder.EncryptWithByte(_coder.AddRandomizeToString(item.Value), key));
                }
                
                forWrite.Add(item.Key + "-" + value);
            }

            _fileAction.WriteAllLines(pathForSave, forWrite);

            return true;
        }
    }
}
