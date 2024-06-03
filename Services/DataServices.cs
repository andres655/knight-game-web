using BlazorApp13.Models;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorApp13.Services
{
    public class DataServices
    {
        private readonly string _filePath;
        private readonly IJSRuntime _jsRuntime;
        private readonly  string _key = "Level";

        public DataServices(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<Player> GetData(string level)
        {
            string keyLevel = _key + level;
            Console.WriteLine(level);
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", keyLevel);
            return json == null ? new Player() : JsonSerializer.Deserialize<Player>(json);
        }

        public async Task SaveData(Player data)
        {
          
            
            string keyLevel= _key + data.level.ToString()
           ;
            var json = JsonSerializer.Serialize(data);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", keyLevel, json);
        }
    }
}
