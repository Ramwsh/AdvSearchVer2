using System.Collections.Generic;

namespace VK_Module.Databases.OKGroupsLoadDbModel
{
    public interface IOKGroupsLoadRepository
    {
        void AddGroupLink(string groupLink);
        void RemoveGroupLink(string groupLink);
        List<string> GetAllGroupLinks();
    }
}
