using System.Collections.Generic;

namespace VK_Module.Databases.NameFilterDbModel
{
    public interface INameFilterRepository
    {
        void AddName(string name);
        void RemoveName(string name);
        List<string> GetAllNames();
    }
}
