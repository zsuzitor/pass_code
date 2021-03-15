//using PassCode.Models.BL.Interfaces;
//using PassCode.Models.BO;
//using System;
//using System.Linq;

//namespace PassCode.Models.BL.Commands
//{
//    public class FileDecoderCommand : ICustomCommand
//    {
//        private readonly string _customName;

//        private readonly IOutput _output;
//        private readonly IWordContainer _container;
//        private readonly ICoder _coder;
//        private readonly IAppSettings _appSettings;

//        public FileDecoderCommand(IOutput output, IWordContainer container, ICoder coder, IAppSettings appSettings)
//        {
//            _customName = "dec";

//            _output = output;
//            _container = container;
//            _coder = coder;
//            _appSettings = appSettings;
//        }

//        public string GetCutomName()
//        {
//            return _customName;
//        }

//        public string GetShortDescription()
//        {
//            return "dec - decode loaded file - 'dec <password>'";
//        }


//        public bool TryDo(string command)
//        {
//            if (!command.StartsWith(_customName))
//            {
//                return false;
//            }

//            var commandSplit = command.Split(" ");
//            //var argCount = 2;
//            //if (commandSplit.Length < argCount)
//            //{
//            //    throw new CommandHandleException($"{argCount} аргумента");
//            //}

//            string key = _appSettings.Key;
//            if (commandSplit.Length > 1)
//            {
//                key = commandSplit[1];
//            }

//            if (string.IsNullOrWhiteSpace(key))
//            {
//                throw new CommandHandleException($"ключ не задан");
//            }


//            foreach (var word in _container.GetAll())
//            {
//                var bytes = _coder.CustomStringToBytes(word.Value);
//                var encodedStr = _coder.DecryptFromBytes(bytes.ToArray(), key);
//                encodedStr = _coder.RemoveRandomizeFromString(encodedStr);
//                word.Value = encodedStr;
//                _container.Edit(word);
//            }

//            _container.Decoded = true;

//            return true;
//        }
//    }
//}
