using System;
using TextCopy;

namespace LineReductant
{

    class Program
    {

        static void Main(string[] args)
        {
            bool isExit = false;
            Console.WriteLine("Для выхода нажмите Ctrl + C\n");
            
            while (!isExit)
            {
                var info = Console.ReadKey();

                if (info.Key == ConsoleKey.V)
                {
                    Clipboard.SetText(Clipboard.GetText().Replace(Environment.NewLine, " "));
                    Console.WriteLine("Текст в буффере обмена изменён!\n");
                }
                else
                {
                    isExit = (info.Modifiers & ConsoleModifiers.Control) != 0 &&
                             info.Key == ConsoleKey.C;
                }
            }
        }

    }
    
}
