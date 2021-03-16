

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;

namespace PassCode.Models.BL.Commands
{
    public class FileLoaderCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly IFileAction _fileAction;
        private readonly ICoder _coder;
        private readonly IAppSettings _appSettings;

        public FileLoaderCommand(IOutput output, IWordContainer container,
            IFileAction fileAction, ICoder coder, IAppSettings appSettings)
        {
            _customName = "load";
            _output = output;
            _container = container;
            _fileAction = fileAction;
            _coder = coder;
            _appSettings = appSettings;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return $"{_customName} - load crypted file - '{_customName} <filepath>'";
        }

        public bool TryDo(string command)
        {
            if (!command.StartsWith(_customName))
            {
                return false;
            }

            var splt = command.Split(" ");
            if (splt.Length < 2)
            {
                return false;
            }

            if (_container.HasRecords())
            {
                throw new CommandHandleException("уже есть загруженные записи, сначала очистите - 'clear'");
            }

            if (!_fileAction.Exists(splt[1]))
            {
                throw new CommandHandleException("file not found");
            }

            var data = _fileAction.ReadAllLines(splt[1]);
            if (data.Length < 2)
            {
                throw new CommandHandleException("file is empty, please create new");
            }

            var encLoginSplit = data[0].Split("-");
            if (encLoginSplit.Length < 2)
            {
                throw new CommandHandleException("file is wrong sign");
            }

            var encLogin = encLoginSplit[1];

            string savedLogin = "";
            try
            {
                savedLogin = 
               _coder.DecryptFromBytes(
                   _coder.CustomStringToBytes(encLogin), _appSettings.Key);
            }
            catch
            {
            }

            if (!_appSettings.LoginIsGood(savedLogin))
            {
                throw new CommandHandleException(
                    "bad password for this db, please load any db or change password(clear command)");
            }

            foreach (var line in data[1..])
            {
                var splitLine = line.Split("-");
                var newWord = new OneWord() { Key = splitLine[0], Value = splitLine[1] };
                _container.Add(newWord);
            }

            return true;
        }

    }
}
