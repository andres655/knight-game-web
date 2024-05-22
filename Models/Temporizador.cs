using Microsoft.AspNetCore.Components;

namespace BlazorApp13.Models
{
    public class Temporizador
    {
        public int horas { get; set; }
        public int minutos { get; set; }
        public  int segundos { get; set; }
        public bool contadorActivo { get; set; }


    }
}
