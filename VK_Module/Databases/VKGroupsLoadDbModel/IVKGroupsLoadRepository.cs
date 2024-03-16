using System.Collections.Generic;

namespace VK_Module.Databases.VKGroupsLoadDbModel
{
    public interface IVKGroupsLoadRepository
    {
        void AddGroupLink(string groupLink);
        void RemoveGroupLink(string groupLink);
        List<string> GetAllGroupLinks();
    }
}
