using CurrieTechnologies.Razor.SweetAlert2;


namespace BlazorApp13.Services
{
   
    public class ShowAlertServices
    {

        private readonly SweetAlertService _sweetAlertService;

        public ShowAlertServices(SweetAlertService sweetAlertService)
        {
            _sweetAlertService = sweetAlertService;
        }

        public async Task Tutorial()
        {
            string resumen = @"
                <div class='steps'>
                    <p><strong>Reglas Básicas</strong></p>
                    <div class='step'>
                        <span class='step-number'>1: </span>  
                        El caballo se mueve en forma de 'L': dos casillas en una dirección (vertical u horizontal) y una casilla en la dirección perpendicular, o viceversa.
                    </div>
                    <div class='step'>
                        <span class='step-number'>2: </span>  
                        Movimiento Único por Casilla: El caballo debe moverse a cada casilla del tablero exactamente una vez durante el recorrido.
                    </div>
                    <div class='step'>
                        <span class='step-number'>3: </span>  
                        Ganas cuando hayas recorrido todas las casillas del tablero.
                    </div>
                    <div class='step'>
                        <span class='step-number'>Paso 1:</span> Elije la casilla donde deseas iniciar.
                    </div>
                    <div class='step'>
                        <span class='step-number'>Paso 2:</span> Selecciona tu próximo movimiento.
                    </div>
                    <div class='step'>
                        <span class='step-number'>Paso 3:</span> Completa el Knight's Tour.
                    </div>
                </div>";

            await ShowSweetAlert("Tutorial del Knight's Tour", "", SweetAlertIcon.Info, false, resumen);
        }

        public async Task ShowSweetAlert(string title, string text, SweetAlertIcon icon, bool isWinner = false, string? html = null)
        {
            var options = new SweetAlertOptions
            {
                Title = title,
                Html = html,
                Text = text,
                Icon = icon,
                ConfirmButtonText = "OK",
                Position = SweetAlertPosition.Top
            };

            if (isWinner)
            {
                options.Background = "#fff url(/winner.gif)";
                options.ConfirmButtonText = "Next";
                options.Backdrop = true;
            }

            await _sweetAlertService.FireAsync(options);
        }
    }
}

