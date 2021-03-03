using PassCode.Models.BL;
using PassCode.Models.BL.Commands;
using PassCode.Models.BL.Interfaces;
using PassCode.Models.BO;
using System;
using System.Collections.Generic;

namespace PassCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //todo конфиг
            //флаг сессионности, вводишь пароль 1 раз и он запоминается пока не закроешь апу
            //пароль в конфиге
            //название файла в конфиге для загрузки

            //при загрузке файла не давать его загрузить если есть не сохраненные данные

            //commandHandleException - должно быть только в command, все что глубже должно бросать другое

            var commands = new List<ICustomCommand>();
            CommandsInit(commands);

            Console.WriteLine("автор не несет ответственность за утерю, распространение, передачу третьим лицам и тд любых введенных данных");
            Console.WriteLine("автор не несет ответственность за любые проблемы, потери возникшие в результате работы программы");
            Console.WriteLine("license - MIT");
            var command = "";
            //File.read
            do
            {
                try
                {
                    bool commandDid = false;
                    Console.WriteLine("введите команду");

                    command = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        //Console.WriteLine("строка пустая");
                        continue;
                    }

                    foreach (var cmd in commands)
                    {
                        if (cmd.TryDo(command))
                        {
                            commandDid = true;
                            break;
                        }
                    }

                    if (!commandDid)
                    {
                        Console.WriteLine("комманда не найдена, список команд - 'help'");
                    }
                }
                catch (CommandHandleException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (!command.StartsWith("end"));


        }

        private static void CommandsInit(List<ICustomCommand> commands)
        {
            IOutput outPut = new ConsoleOutput();
            ICoder coder = new AesCoder();
            IWordContainer wordContainer = new WordContainer();
            IAppSettings appSettings = new AppSettings();
            IFileAction fileActions = new CommonFileAction();

            commands.Add(new HelpCommand(outPut, commands));
            commands.Add(new AddCommand(outPut, wordContainer, coder));
            commands.Add(new FileDecoderCommand(outPut, wordContainer, coder, appSettings));
            commands.Add(new FileLoaderCommand(outPut, wordContainer, fileActions));
            commands.Add(new RemoveCommand(outPut, wordContainer));
            commands.Add(new SaveCommand(outPut, wordContainer, coder, appSettings, fileActions));
            commands.Add(new ShowCommand(outPut, wordContainer, coder));
            commands.Add(new GetCommand(outPut, wordContainer, coder));
            commands.Add(new ClearCommand(outPut, wordContainer));
            commands.Add(new FastDecoderCommand(outPut, coder));
            commands.Add(new FastEncoderCommand(outPut, coder));
        }

    }
}
