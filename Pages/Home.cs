using BlazorApp13.Componets;
using BlazorApp13.Models;
using BlazorApp13.Services;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BlazorApp13.Pages
{
    public partial class Home
    {
        public Board board = new Board();
        public int id = 0;
        public int contador = 0;
        public bool firstMovent = true;
        public List<Square> squares { get; set; } = new List<Square>();
        public Player player = new Player();
        public Level level { get; set; }
        public int x = 5;
        public int y = 5;
        private int volume = 10; // Volumen inicial

        protected override void OnInitialized()
        {
            level = LevelService.CurrentLevel;
            if (level == null)
            {
                Navigation.NavigateTo("/");
                return;
            }

            InitializeGame();
        }

        private async void InitializeGame()
        {
           
            x = level.Row;
            y = level.Column;
            createBoard(x, y);
            PlayBackgroundSound();
            ResetGame();
            await getData();
        }

        private void PlayBackgroundSound()
        {
            double normalizedVolume = volume / 100.0;
            JS.InvokeVoidAsync("playBackgroundSound", "Sound/fondo.mp3", normalizedVolume);
        }

        private async Task getData()
        {
            player = await data.GetData(level.Word.ToString());
            StateHasChanged();
        }

        private void createBoard(int rows, int columns)
        {
            id = 0;
            squares.Clear();
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    string color = (column + row) % 2 == 0 ? "white" : "black";
                    squares.Add(new Square { Id = ++id, x = row, y = column, color = color });
                }
            }
        }

        private async Task MouseUp(Square square)
        {
            var newSquare = squares.FirstOrDefault(s => s.Id == square.Id && (firstMovent || s.Style == "nextMovimiento"));
            if (newSquare == null) return;

            firstMovent = false;
            ClearStyles("nextMovimiento");

            var character = squares.FirstOrDefault(s => s.Style == "character");
            if (character != null)
            {
                character.Style = "beforeMovent";
            }

            newSquare.Style = "character";
            contador++;
            newSquare.contador = contador;
            board.contador = contador;

            GetKnight(newSquare.y, newSquare.x);
            await PlaySound("Sound/movent.mp3");
        }

        private void ClearStyles(string style)
        {
            foreach (var item in squares.Where(s => s.Style == style))
            {
                item.Style = "";
            }
        }

        private void GetKnight(int column, int row)
        {
            int[] moveX = { 1, -1, 2, -2, 1, -1, 2, -2 };
            int[] moveY = { 2, 2, 1, 1, -2, -2, -1, -1 };
            bool foundMatchingSquare = false;

            for (int i = 0; i < 8; i++)
            {
                int nextX = row + moveX[i];
                int nextY = column + moveY[i];

                var matchingSquare = squares.FirstOrDefault(s => s.x == nextX && s.y == nextY);
                if (matchingSquare != null && matchingSquare.Style != "beforeMovent")
                {
                    matchingSquare.Style = "nextMovimiento";
                    foundMatchingSquare = true;
                }
            }

            if (!foundMatchingSquare)
            {
                Finnish();
            }
        }

        private async Task Tutorial()
        {
            await showAlert.Tutorial();
        }
        private async void Finnish()
        {
            if (contador > player.Score)
            {
                player.Name = "invictado";
                player.Score = contador;
                player.Date = DateTime.UtcNow;
                player.level = level.Word;
            }

            await data.SaveData(player);

            if (board.contador >= (x * y))
            {
                await PlaySound("Sound/winner.mp3");
                await showAlert.ShowSweetAlert("Felicidades, Haz ganado", "Your Score is: " + contador, SweetAlertIcon.Success, true);
                await AdvanceToNextLevel();
            }
            else
            {
                await PlaySound("Sound/loser.mp3");
                await showAlert.ShowSweetAlert("You Lose", "Your Score is:" + contador, SweetAlertIcon.Info);
            }
            ResetGame();
        }
     
   

        private async Task AdvanceToNextLevel()
        {
            if (level == null) return;

            var levels = await Http.GetFromJsonAsync<Level[]>("Level.json");
            var nextLevel = levels?.Where(l => l.Word > level.Word).OrderBy(l => l.Word).FirstOrDefault();
            LevelService.CurrentLevel = nextLevel;
            level = LevelService.CurrentLevel;

            ResetGame();
        }

        private async void ResetGame()
        {
            board = new Board();
            id = 0;
            contador = 0;
            firstMovent = true;
            squares.Clear();
            player = new Player();

            if (level != null)
            {
                x = level.Row;
                y = level.Column;
                createBoard(x, y);
            }

            StateHasChanged();
            Navigation.NavigateTo("/levels");
            await getData();
        }

      

        private async Task PlaySound(string soundPath)
        {
            await JS.InvokeVoidAsync("playSound", soundPath);
        }

        public void Exit()
        {
            Navigation.NavigateTo("/");
        }

        public void Back()
        {
            if (contador <= 0) return;

            ModifySquare(
                current =>
                {
                    current.Style = "";
                    current.contador = 0;
                },
                last => last.Style = ""
            );
        }

        public async void ModifySquare(Action<Square> modifyCurrent, Action<Square> modifyLast)
        {
            var sortedSquares = squares.OrderByDescending(s => s.contador).ToList();
            if (sortedSquares.Count < 2) return;

            var current = sortedSquares[0];
            var last = sortedSquares[1];
            var next = squares.Where(s => s.Style == "nextMovimiento").ToList();

            firstMovent = true;
            modifyCurrent(current);
            contador = contador - 2;

            if (contador >= 0)
            {
                await MouseUp(last);
            }
            else
            {
                foreach (var item in next)
                {
                    contador = 0;
                    modifyLast(item);
                }
            }
        }
    }
}
