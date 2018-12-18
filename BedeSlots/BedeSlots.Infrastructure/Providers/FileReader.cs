using BedeSlots.Infrastructure.Providers.Interfaces;
using System.IO;

namespace BedeSlots.Infrastructure.Providers
{
    public class FileReader : IFileReader
    {
        public string ReadAllFrom(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
