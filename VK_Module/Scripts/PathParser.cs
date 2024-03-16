using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VK_Module.Scripts
{
    public class PathParser
    {        
        public string ParsePathToDir(string filepath)
        {
            List<string> full_path = filepath.Split('\\').ToList();
            full_path.Remove(full_path.Last());
            string directory_name = string.Join("\\", full_path);
            return directory_name;
        }

        public string ParsePathToFileName(string filepath)
        {
            return filepath.Split('\\').Last();
        }
        // чтение содержимого текстового файла
        public string[] ReadFromText(string filepath)
        {
            List<string> _keyword_file_content = File.ReadAllText(filepath).Split().ToList();
            _keyword_file_content.RemoveAll(x => x == " " || x == "" || x == '\n'.ToString() || x == '\r'.ToString());
            return _keyword_file_content.ToArray();
        }
    }
}
