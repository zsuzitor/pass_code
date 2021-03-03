

using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System.IO;

namespace PassCode.Models.BL.Commands
{
    public class FileLoaderCommand : ICustomCommand
    {
        private readonly string _customName;

        private readonly IOutput _output;
        private readonly IWordContainer _container;
        private readonly IFileAction _fileAction;

        public FileLoaderCommand(IOutput output, IWordContainer container, IFileAction fileAction)
        {
            _customName = "lcf";
            _output = output;
            _container = container;
            _fileAction = fileAction;
        }

        public string GetCutomName()
        {
            return _customName;
        }

        public string GetShortDescription()
        {
            return "lcf - load crypted file - 'lcf <filepath>'";
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
            foreach (var line in data)
            {
                var splitLine = line.Split("-");
                var newWord = new OneWord() { Key = splitLine[0], Value = splitLine[1] };
                _container.Add(newWord);
            }

            //_container.FileLoaded = true;

            return true;
        }

    }
}
