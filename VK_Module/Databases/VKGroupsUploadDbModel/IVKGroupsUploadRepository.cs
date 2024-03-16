using System.Collections.Generic;

namespace VK_Module.Databases.VKGroupsUploadDbModel
{
    public interface IVKGroupsUploadRepository
    {
        void AddGroupLink(string groupLink);
        void RemoveGroupLink(string groupLink);
        List<string> GetAllGroupLinks();
    }
}
