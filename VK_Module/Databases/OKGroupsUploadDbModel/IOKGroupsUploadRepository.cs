using System.Collections.Generic;

namespace VK_Module.Databases.OKGroupsUploadDbModel
{
    public interface IOKGroupsUploadRepository
    {
        void AddGroupLink(string groupLink);
        void RemoveGroupLink(string groupLink);
        List<string> GetAllGroupLinks();
    }
}
