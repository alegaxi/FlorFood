using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;

namespace FlorFood;

public partial class Carrito : ContentPage
{
    private List<DatosPlatillos> platillosSeleccionados;
    public static String ClienteNom;
    public static int EmpresaId;
    double precioTotal = 0;
    double dLatitud = 0.0;
    double dLongitud = 0.0;
    int ClienteId;
    DateTime fecha = DateTime.Now;
    int pedidoId = 0;
    public Carrito(List<DatosPlatillos> platillos)
	{
		InitializeComponent();
        platillosSeleccionados = platillos;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        foreach (var platillo in platillosSeleccionados)
        {
            Frame frame = CreateCustomFrame(platillo);
            precioTotal += platillo.Precio;
            tbTotal.Text = precioTotal.ToString();
            pedidos2.Children.Add(frame);        
        }
    }
    public Frame CreateCustomFrame(DatosPlatillos platillo)
    {
        Frame frame = new Frame
        {
            BorderColor = Color.FromHex("#e33d8b"),
            Padding = new Thickness(10)
        };

        StackLayout stackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center
        };

        Image image = new Image
        {
            Source = ImageSource.FromStream(() => new MemoryStream(platillo.Imagen)),
            Aspect = Aspect.AspectFill,
            HeightRequest = 150,
            WidthRequest = 150
        };

        Label nombrePlatillo = new Label
        {
            Text = platillo.Nombre,
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        Label descripcion = new Label
        {
            Text = platillo.Descripcion,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        StackLayout horizontalStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center
        };

        Label label3 = new Label
        {
            Text = "Precio: ",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        Label precio = new Label
        {
            Text = platillo.Precio.ToString(),
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };
        Button btnQuitar = new Button
        {
            Text = "Quitar",
            BackgroundColor = Color.FromHex("#e33d8b"),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            TextColor = Colors.White,
            CornerRadius = 30,
            WidthRequest = 180,
            HeightRequest = 60,
            Margin = new Thickness(0, 0, 0, 10)
        };
        btnQuitar.Clicked += (sender, e) =>
        {
            Frame frameToRemove = (Frame)((Button)sender).Parent.Parent;
            precioTotal -= platillo.Precio;
            tbTotal.Text = precioTotal.ToString();
            platillosSeleccionados.Remove(platillo);
            pedidos2.Children.Remove(frameToRemove);
        };

        stackLayout.Children.Add(image);
        stackLayout.Children.Add(nombrePlatillo);
        stackLayout.Children.Add(descripcion);
        horizontalStackLayout.Children.Add(label3);
        horizontalStackLayout.Children.Add(precio);
        stackLayout.Children.Add(horizontalStackLayout);
        stackLayout.Children.Add(btnQuitar);

        frame.Content = stackLayout;

        return frame;
    }
    private void btnPedir_Clicked(object sender, EventArgs e)
    {
        if(dLatitud == 0 && dLongitud == 0)
        {
            DisplayAlert("ERROR", "Selecciona una ubicacion en el mapa", "Aceptar");
        }
        else
        {
            OpBaseDeDatos opPedido = new OpBaseDeDatos();
            DatosPedido dato = new DatosPedido();
            pedidoId = opPedido.FolioPedido();
            if (opPedido.ConsultarCliente(ClienteNom, dato))
            {
                ClienteId = dato.IDCliente;
            }

            DatosPedido pedido = new DatosPedido
            {
                Fecha = fecha,
                IDPedido = pedidoId,
                IDCliente = ClienteId,
                IDNegocio = EmpresaId,
                Total = precioTotal,
                Latitud = dLatitud,
                Longitud = dLongitud,
            };
            opPedido.InsertPedido(pedido);

            OpBaseDeDatos opDetalle = new OpBaseDeDatos();
            foreach (var platillo in platillosSeleccionados)
            {
                DatosDetallePedido detalle = new DatosDetallePedido
                {
                    IDPedido = pedidoId,
                    IDPlatillo = platillo.IDPlatillo,
                    PrecioUnitario = platillo.Precio,
                };
                opDetalle.InsertDetallePedido(detalle);
            }
            if (opPedido.bAllOk == true && opDetalle.bAllOk == true)
            {
                DisplayAlert("CORRECTO", "Su compra ha sido realizada", "Aceptar");
            }
            else
            {
                DisplayAlert("ERROR EN PEDIDO", opPedido.sLastError, "Aceptar");
                DisplayAlert("ERROR EN DETALLE", opDetalle.sLastError, "Aceptar");
            }
        }
    }

    private void btnRegresar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void map_MapClicked(object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");
        map.Pins.Clear();
        Pin pin = new Pin
        {
            Label = "Cliente",
            Address = "Ubicacion seleccionada",
            Type = PinType.Place,
            Location = new Location(e.Location.Latitude, e.Location.Longitude)
        };
        map.Pins.Add(pin);
        dLatitud = e.Location.Latitude;
        dLongitud = e.Location.Longitude;
    }
}