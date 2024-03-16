using System.Collections.Generic;

namespace VK_Module.Databases.CianPagesLoadDbModel
{
    public interface ICianPagesLoadRepository
    {
        void AddPageLink(string cianPageLink);
        void RemovePageLink(string cianPageLink);
        List<string> GetAllPageLinks();
    }
}
