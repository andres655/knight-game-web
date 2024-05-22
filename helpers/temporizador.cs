using Microsoft.AspNetCore.Components;

namespace BlazorApp13.helpers
{
    public class Temporizador : ComponentBase
    {
        private int horas;
        private int minutos;
        private int segundos;
        private bool contadorActivo;

        protected override async Task OnInitializedAsync()
        {
            horas = 0;
            minutos = 0;
            segundos = 0;
            contadorActivo = true;

            while (contadorActivo)
            {
                await Task.Delay(1000); // Espera un segundo

                segundos++; // Incrementa los segundos

                if (segundos == 60)
                {
                    minutos++; // Incrementa los minutos cuando los segundos llegan a 60
                    segundos = 0; // Reinicia los segundos
                }

                if (minutos == 60)
                {
                    horas++; // Incrementa las horas cuando los minutos llegan a 60
                    minutos = 0; // Reinicia los minutos
                }

                StateHasChanged(); // Actualiza la interfaz de usuario
            }
        }

        // Opcional: Detener el contador
        public void DetenerContador()
        {
            contadorActivo = false;
        }
    }
}
