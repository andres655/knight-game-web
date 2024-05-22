using BlazorApp13.Models;

namespace BlazorApp13.Services
{
    public interface IDataServices
    {
        Player GetData();
        void SaveData(Player data);
    }
}
