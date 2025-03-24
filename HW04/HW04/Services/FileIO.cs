using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW04.Library.Services
{
    public class FileIO : IFileIO
    {
        public static readonly string DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public FileIO()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
        }

        public byte[] Concatenate(string[] paths) =>
            paths.SelectMany(path => ReadFrom(path)).ToArray();

        public byte[] ReadFrom(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            return File.ReadAllBytes(path);
        }

        public void WriteTo(string path, byte[] content)
        {
            int number = 1;
            string fileName = $"concatenated_{number}.txt";
            while (File.Exists(Path.Join(DataPath, fileName)))
            {
                number++;
                fileName = $"concatenated_{number}.txt";
            }
            File.WriteAllBytes(Path.Join(DataPath, fileName), content);
        }
    }
}
