using BlazorApp13.Models;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Xml.Linq;

namespace BlazorApp13.Services
{
    public class DataServices
    {
        private readonly string _filePath;
        private readonly IJSRuntime _jsRuntime;
        private readonly string _key = "Player";

        public DataServices(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<Player> GetData()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _key);
            return json == null ? new Player() : JsonSerializer.Deserialize<Player>(json);
        }

        public async Task SaveData(Player data)
        {
           
            var json = JsonSerializer.Serialize(data);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _key, json);
        }
    }
}
