using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestDnsApp
{
    public class Tree
    {
        public Tree(string startDir)
        {
            start_dir = startDir;
        }
        public void Print()
        {
            PrintTree(start_dir);
        }

        public string start_dir { get; }
        public bool show_size { get; set; } = false;
        public int max_depth { get; set; } = int.MaxValue;

        public void PrintTree(string start_dir, string prefix = "", int depth = 0) 
        {
            if (depth >= max_depth)
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(start_dir);
            var fileItems = dirInfo.GetFileSystemInfos()
                .Where(f => !f.Name.StartsWith("."))
                .OrderBy(f => f.Name)
                .ToList();


            foreach (var fileItem in fileItems.Take(fileItems.Count-1))
            {
                Console.Write(prefix + "├── "); // U+02EB ˫
                Console.Write($"{fileItem.Name} {GetSize(fileItem)} \n");
                if (fileItem.isDirectory())
                    PrintTree(fileItem.FullName, prefix + "│   ", depth + 1);
            }

            var lastFileItem = fileItems.LastOrDefault();
            if (lastFileItem != null)
            {
                Console.Write(prefix + "└── ");
                Console.Write($"{lastFileItem.Name} {GetSize(lastFileItem)} \n");
                if (lastFileItem.isDirectory())
                    PrintTree(lastFileItem.FullName, prefix + "    ", depth + 1);
            }

        }

        public string GetSize(FileSystemInfo fileItem) 
        {
            if (show_size & !fileItem.isDirectory())
            {
                var path = fileItem.FullName;
                var fileInfos = new FileInfo(path);
                long fileSize = fileInfos.Length;

                return ConverterSize(fileSize);
            }
            return "";
        }

        public string ConverterSize(long fileSize)
        {
            
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB" };
            if (fileSize == 0)
                return "(empty)";
            long bytes = Math.Abs(fileSize);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"({(Math.Sign(fileSize) * num).ToString()} {suf[place]})";
            
            // Сначала попробовал простым способом, потом стало интересно получить точное значение
            /*
            long actualSize;

            if (fileSize == 0)
                return "(empty)";
            else if (fileSize >= 1073741824) 
            {
                actualSize = fileSize / 1073741824;
                return $"({actualSize} GB)";
            }
            else if (fileSize >= 1048576)
            {
                actualSize = fileSize / 1048576;
                return $"({actualSize} MB)";
            }
            else if (fileSize >= 1024)
            {
                actualSize = fileSize / 1024;
                return $"({actualSize} KB)";
            }
            else
            {
                return $"({fileSize} B)";
            }
            */
        }
        

    }

    public static class FileSystemInfoExtensions
    {
        public static bool isDirectory(this FileSystemInfo fileItem) =>
            (fileItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
    }
}
