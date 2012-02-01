using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace ResourceCollectorXNA
{
#if WINDOWS || XBOX
   public  static class Program
   {
       public static string help_regex
       {
           get
           {
return
@"Большинство символов в регулярном выражении представляют сами себя за исключением специальных символов [ ] \ ^ $ . | ? * + ( ) { }, которые могут быть предварены символом \ для представления их самих в качестве символов текста. Можно экранировать целую последовательность символов, заключив её между \Q и \E.
        Пример   Соответствие
           a\.?    a. или a
         a\\\\b    a\\b
         a\[F\]    a[F]
       \Q+-*/\E    +-*/

.            любой символ
.*           любая последовательность символов
[01234abcf]  один из этих символов
[0-9A-Z]     цифра или буква от A до Z
[^a-z]       не маленькие буквы
\d  ~цифре. Эквивалентно [0-9]
\D  ~нецифровому символу. Эквивалентно [^0-9]
\s  ~любому пробельному символу. Эквивалентно [ \f\n\r\t\v]
\S  ~любому непробельному символу. Эквивалентно [^ \f\n\r\t\v]
\w  ~любому буквенному символу, цифровому и знаку подчеркивания. Эквивалентно [[:word:]]
\W  ~любому символу, кроме буквенного символа, цифрового или подчеркивания. Эквивалентно [^[:word:]]

Следующие символы позволяют спозиционировать регулярное выражение относительно элементов текста: начала и конца строки, границ слова.
^               Начало строки
$               Конец строки
\b              Граница слова
\B              Не граница слова
\G              Предыдущий успешный поиск

Количество символов:
{n}     Ровно n раз	
{m,n}   От m до n включительно
{m,}    Не менее m
{,n}    Не более n
*   Ноль или более  == {0,}
+   Одно или более  == {1,}
?   Ноль или одно   == {0,1}
                        ";
           }
       }

       public static string help
       {
           get
           {
               return help_regex + 
                   "\n\nPlease insert HELP here";
           }
       }

        public static MyGame game;
    
       
       [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    if (arg == "-git")
                    {
                        string git_commander = @"C:\Users\shpengler\Desktop\git\GitCommander\GitCommander\bin\Debug\GitCommander.exe";
                        Process.Start(git_commander, @"-w C:\Users\shpengler\Desktop\git\ResourceCollector");
                    }
                }
            }
            catch { }

            ResourceCollector.ElementType.Init();
            game = new MyGame();
            game.Run();
        }
    }
#endif
}

