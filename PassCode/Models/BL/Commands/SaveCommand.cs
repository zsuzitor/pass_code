

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
            //
            return $"{_customName} - encrypt and save new file, dont change current credentials " +
                $"- '{_customName} <newfilename> [?<login> <key>]'";
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
                throw new CommandHandleException($"min {argCount} аргумента");
            }

            string loginForSave = _appSettings.Login;
            string passwordForSave = _appSettings.Key;
            bool differentCredit = false;

            if (splitCommand.Length > 2)
            {
                if (splitCommand.Length < 4)
                {
                    throw new CommandHandleException($"2 or 4 arg");
                }
                else
                {
                    loginForSave = splitCommand[2];
                    passwordForSave = splitCommand[3];
                    differentCredit = true;
                }

            }


            string pathForSave = splitCommand[1];//"./"

            if (_fileAction.Exists(pathForSave))
            {
                throw new CommandHandleException($"file is exist");
            }


            List<string> forWrite = new List<string>();
            forWrite.Add("check" + "-"
                + _coder.BytesToCustomString(
                    _coder.EncryptWithByte(
                        _coder.AddRandomizeToString(loginForSave), passwordForSave)));
            foreach (var item in _container.GetAll())
            {
                string value = item.Value;
                if (!differentCredit)
                {
                    value = item.Value;
                }
                else
                {
                    value = _coder.BytesToCustomString(
                        _coder.EncryptWithByte(
                            _coder.DecryptFromBytes(
                                _coder.CustomStringToBytes(value), _appSettings.Key), passwordForSave));
                }

                forWrite.Add(item.Key + "-" + value);
            }

            _fileAction.WriteAllLines(pathForSave, forWrite);

            return true;
        }
    }
}
