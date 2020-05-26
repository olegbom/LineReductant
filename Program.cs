using System;
using System.Text;
using TextCopy;

namespace LineReductant
{

    class Program
    {

        static void Main(string[] args)
        {
            bool isExit = false;
            Console.WriteLine("Для выхода нажмите Ctrl + C");

            while (!isExit)
            {
                var info = Console.ReadKey();

                if (info.Key == ConsoleKey.V)
                {
                    Clipboard.SetText(Clipboard.GetText().Replace(Environment.NewLine, " "));
                    Console.WriteLine("Текст в буффере обмена изменён!");
                }

                if (info.Key == ConsoleKey.R)
                {
                    Cp1251Replace();
                    Console.WriteLine("Текст в буффере обмена изменён!");
                }
                else
                {
                    isExit = (info.Modifiers & ConsoleModifiers.Control) != 0 &&
                             info.Key == ConsoleKey.C;
                }
            }
        }

        static void Cp1251Replace()
        {
            string text = Clipboard.GetText();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding(1251);
            byte[] bytes = win1251.GetBytes(text);

            byte[] outBytes = new byte[bytes.Length * 3];
            int k = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
            {
                if ( bytes[i] == 0x27 && 
                     bytes[i + 1] > 127 &&
                     bytes[i + 2] == 0x27)
                {
                    string numberChars = $"(char){bytes[i + 1]}/*";
                    foreach (var c in numberChars)
                    {
                        outBytes[k] = (byte) c;
                        k++;
                    }

                    outBytes[k] = bytes[i + 1];
                    k++;
                    string commentEnd = "*/";
                    foreach (var c in commentEnd)
                    {
                        outBytes[k] = (byte)c;
                        k++;
                    }
                    i += 2;
                }
                else
                {
                    outBytes[k] = bytes[i];
                    k++;
                }
            }

            for (int i = bytes.Length - 2; i < bytes.Length; i++)
            {
                outBytes[k] = bytes[i];
                k++;
            }
            string outText = win1251.GetString(outBytes, 0, k);
            Clipboard.SetText(outText);
        }

    }
    
}
