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
            //пароль в конфиге
            //название файла в конфиге для загрузки
            //при загрузке файла не давать его загрузить если есть не сохраненные данные
            //commandHandleException - должно быть только в command, все что глубже должно бросать другое

            //Encoding.UTF8.GetString(encr)
            //Convert.ToBase64String(msEncrypt.ToArray());


            var commands = new List<ICustomCommand>();
            IAppSettings appSettings = new AppSettings();
            CommandsInit(commands, appSettings);

            Console.WriteLine("автор не несет ответственность за утерю, распространение, передачу третьим лицам и тд любых введенных данных");
            Console.WriteLine("автор не несет ответственность за любые проблемы, потери возникшие в результате работы программы");
            Console.WriteLine("license - MIT");
            var command = "";
            //var g_ = (int)'z';

            do
            {
                try
                {
                    if (!appSettings.HasValideDataForAuth())
                    {

                        Console.WriteLine("Enter login: ");
                        var login = Login();
                        Console.WriteLine();
                        Console.WriteLine("Enter password: ");
                        var password = Login();
                        command = $"login {login} {password}";
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("введите команду");
                        command = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(command))
                        {
                            //Console.WriteLine("строка пустая");
                            continue;
                        }
                    }

                    bool commandDid = false;

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

        private static void CommandsInit(List<ICustomCommand> commands, IAppSettings appSettings)
        {
            IOutput outPut = new ConsoleOutput();
            ICoder coder = new AesCoder();
            IWordContainer wordContainer = new WordContainer();
            IFileAction fileActions = new CommonFileAction();

            commands.Add(new HelpCommand(outPut, commands));
            commands.Add(new AddCommand(wordContainer, coder, appSettings));
            //commands.Add(new FileDecoderCommand(outPut, wordContainer, coder, appSettings));
            commands.Add(new FileLoaderCommand(outPut, wordContainer, fileActions, coder, appSettings));
            commands.Add(new RemoveCommand(outPut, wordContainer));
            commands.Add(new SaveCommand(wordContainer, coder, appSettings, fileActions));
            commands.Add(new LoginCommand(outPut, appSettings));
            commands.Add(new ShowCommand(outPut, wordContainer, coder));
            commands.Add(new GetCommand(outPut, wordContainer, coder, appSettings));
            commands.Add(new ClearCommand(outPut, wordContainer, appSettings));
            commands.Add(new FastDecoderCommand(outPut, coder));
            commands.Add(new FastEncoderCommand(outPut, coder));
        }

        private static string Login()
        {
            ConsoleKeyInfo key;
            string login = "";

            do
            {
                key = Console.ReadKey(true);

                // Ignore any key out of range.
                if ((((int)key.Key) >= 65 && ((int)key.Key <= 90))
                    || ((int)key.Key) >= 48 && ((int)key.Key <= 57))//TODO посмотреть что тут за символы в ренже
                {
                    // Append the character to the password.
                    login += key.KeyChar;
                    Console.Write("*");
                }
                // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);
            return login;
        }

    }
}
