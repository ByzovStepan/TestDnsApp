using System;

namespace TestDnsApp
{
    class Program
    {
        //Справка
        static void Help()
        {
            Console.WriteLine(@"Аргументы при запуске программы:
                            — задают глубину вложенности (-d или --depth)
                            — показывают размер объектов (-s или --size)
                            — справка по использованию (-? или --help)
                        Используйте: TestDnsApp [dir] [-s] [-d depth]

                        Примеры:
                            TestDnsApp C:\Users\Admin\source\repos\TestDnsApp -s
                            TestDnsApp \\ADMIN-PC\Shared
                            TestDnsApp C:\Users\Admin\source\repos\ByzovCRM -s -d 4
                            ");
        }

        static void Main(string[] args)
        {
            //Help();

            string startDir = null;
            bool showSize = false;
            int maxDepth = int.MaxValue;


            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-s" || arg == "--size")
                    showSize = true;
                else if (arg == "-d" || arg == "--depth")
                {
                    maxDepth = int.Parse(args[i + 1]);
                    i++;
                }
                else if (arg.EndsWith("-?") || arg.EndsWith("--help"))
                { 
                    Help();
                    return;
                }  
                else
                {
                    if (startDir == null)
                        startDir = arg;
                    else
                    {
                        Console.WriteLine("Неизвестный атрибут {0}", arg);
                        Help();
                        return;
                    }
                }
            }

            if (String.IsNullOrEmpty(startDir))
            {
                startDir = ".";
            }

            new Tree(startDir)
            {
                show_size = showSize,
                max_depth = maxDepth,
            }.Print();
        }

    }
}
