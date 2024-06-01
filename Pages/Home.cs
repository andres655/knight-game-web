using BlazorApp13.Models;
using BlazorApp13.Services;
using Blazorise;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp13.Pages
{
    public partial class Home
    {

        // Propiedades y estados del componente
        public Board board = new Board();
        public int id = 0;
        public int contador = 0;
        public bool firstMovent = true;
        public List<Square> squares { get; set; } = new List<Square>();
        public Player player = new Player();
        public Level level { get; set; }
        public int x = 5;
        public int y = 5;

        // Método de inicialización del componente
        protected override void OnInitialized()
        {
            level = LevelService.CurrentLevel;
            if (level != null)
            {
                // Usa el objeto CurrentLevel según sea necesario
            }

            // Obtiene datos y crea el tablero inicial
            getData();
            x = level.Row;
            y = level.Column;
            createBoard(level.Row, level.Column);
        }

        // Actualiza las columnas del tablero según la selección del usuario
        private void UpdateGridColumns(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value.ToString(), out int newColumnCount))
            {
                x = newColumnCount;
                y = newColumnCount;
                squares.Clear();
                createBoard(x, y);
                StateHasChanged();
            }
        }

        // Obtiene datos del jugador de forma asíncrona
        public async Task getData()
        {
            player = await data.GetData();
        }

        // Crea el tablero de juego
        public void createBoard(int rows, int columns)
        {
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    string color = (column + row) % 2 == 0 ? "white" : "black";
                    id++;

                    squares.Add(new Square
                    {
                        Id = id,
                        x = row,
                        y = column,
                        color = color
                    });
                }
            }
        }

        // Maneja el evento de levantar el ratón sobre un cuadrado
        private async Task MouseUp(Square square)
        {
            var newSquare = squares.FirstOrDefault(s => s.Id == square.Id && (firstMovent || s.Style == "nextMovimiento"));
            Console.WriteLine(firstMovent);
            if (newSquare != null)
            {
                if (firstMovent)
                {
                    firstMovent = false;
                }

                // Actualiza el estilo de los cuadrados
                var lastSquares = squares.Where(s => s.Style == "nextMovimiento");
                foreach (var item in lastSquares)
                {
                    item.Style = "";
                }

                var character = squares.FirstOrDefault(s => s.Style == "character");
                if (character != null)
                {
                    character.Style = "beforeMovent";
                }

                // Actualiza el cuadrado actual y busca los próximos movimientos posibles
                newSquare.Style = "character";
                contador++;
                newSquare.contador = contador;
                board.contador = contador;

                GetKnight(newSquare.y, newSquare.x);
            }
        }

        // Calcula y marca los movimientos válidos del caballo
        private void GetKnight(int column, int row)
        {
            bool foundMatchingSquare = false;
            int[] moveX = { 1, -1, 2, -2, 1, -1, 2, -2 };
            int[] moveY = { 2, 2, 1, 1, -2, -2, -1, -1 };

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

        // Maneja el caso donde no hay más movimientos disponibles
        private async void Finnish()
        {
            if (board.contador + 1 >= (x * y))
            {
                var resultado = await swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Felicidades, Haz ganado",
                    Text = "Your Score is: " + contador,
                    Icon = SweetAlertIcon.Success,
                    InputLabel = "Ingresa tu nombre",
                    Input = SweetAlertInputType.Text,
                    Position = SweetAlertPosition.Top,
                    ConfirmButtonText = "Ingresar",
                    
                    
                }) ;

                // Aquí podrías guardar el nombre del jugador si es necesario
            }
            else
            {
                if (contador > player.Score)
                {
                    player.Name = "invictado";
                    player.Score = contador;
                    player.Date = DateTime.UtcNow;
                }

                await data.SaveData(player);
                await swal.FireAsync(new SweetAlertOptions
                {
                    Title = "You Lose",
                    Text = "Your Score is:" + contador,
                    Icon = SweetAlertIcon.Info,
                    ConfirmButtonText = "OK",
                    Position = SweetAlertPosition.Top,
                });

                Navigation.NavigateTo("/");
            }
        }

        public  void exit (){
            Navigation.NavigateTo("/");

        }

        public void reset()
        {
            board = new Board();
         id = 0;
        contador = 0;
         firstMovent = true;
       squares  = new List<Square>();
         player = new Player();

            x = level.Row;
            y = level.Column;
            squares.Clear();
            createBoard(x, y);
            StateHasChanged();
            Navigation.NavigateTo("/levels");
           

        }
    }
}
