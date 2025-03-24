using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW04.Library.Services
{
    interface IFileIO
    {
        public byte[] Concatenate(string[] paths);
        public byte[] ReadFrom(string path);
        public void WriteTo(string path, byte[] content);
    }
}
