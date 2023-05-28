using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Maps;

namespace FlorFood;

public partial class VerPedidos : ContentPage
{
    private Pin pinTienda;
    private Pin pinCliente;
    private Polyline routePolyline;

    public static Int32 NegocioId;
    string clienteNom;

    double lat;
    double longi;
    String origen;
    String destino;
    public VerPedidos()
	{
		InitializeComponent();
        MostrarPlatillos();
    }
    private void MostrarPlatillos()
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        List<DatosPedido> pedidoMostrar = op.ObtenerPedidos(NegocioId);

        foreach (var pedidos in pedidoMostrar)
        {
            Frame frame = CreateCustomFrame(pedidos);
            verpedidos.Children.Add(frame);
        }
    }
    public Frame CreateCustomFrame(DatosPedido pedido)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        DatosCliente cliente = new DatosCliente();
        if(op.ConsultarClientePedido(pedido.IDCliente, cliente))
        {
            clienteNom = cliente.Nombre;
        }

        DatosEmpresa empresa = new DatosEmpresa();
        if (op.ReadEmpresaPedido(NegocioId, ref empresa))
        {
            lat = empresa.Latitud;
            longi = empresa.Longitud;
        }
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
        Label Fecha = new Label
        {
            Text = pedido.Fecha.ToString(),
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        Label IDpedido = new Label
        {
            Text = "ID del pedido: " + pedido.IDPedido.ToString(),
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        Label Cliente = new Label
        {
            Text = clienteNom,
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
            Text = "Total: ",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        Label Total = new Label
        {
            Text = pedido.Total.ToString(),
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        Label label4 = new Label
        {
            Text = "Platillos ",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        StackLayout VerticalStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center
        };
        List<DatosDetallePedido> detallesPedido = op.ObtenerDetallePedidos(pedido.IDPedido);

        foreach (DatosDetallePedido detalle in detallesPedido)
        {
            DatosPlatillos platillo = new DatosPlatillos();

            if (op.ConsultarNombrePlatillo(detalle.IDPlatillo, platillo))
            {
                Label labelPlatillo = new Label
                {
                    Text = platillo.Nombre,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                VerticalStackLayout.Children.Add(labelPlatillo);
            }
        }

        Button btnEntregar = new Button
        {
            Text = "Entregar",
            BackgroundColor = Color.FromHex("#e33d8b"),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            TextColor = Colors.White,
            CornerRadius = 30,
            WidthRequest = 180,
            HeightRequest = 60,
            Margin = new Thickness(0, 0, 0, 10)
        };
        btnEntregar.Clicked += async (sender, e) =>
        {
            if (btnEntregar.Text == "Entregar")
            {
                origen = Convert.ToString(pedido.Latitud) + "," + Convert.ToString(pedido.Longitud);
                destino = Convert.ToString(lat) + "," + Convert.ToString(longi);
                pinTienda = new Pin
                {
                    Label = "Tienda",
                    Address = "Ubicacion seleccionada",
                    Type = PinType.Place,
                    Location = new Location(lat, longi)
                };
                map.Pins.Add(pinTienda);

                pinCliente = new Pin
                {
                    Label = "Cliente",
                    Address = "Ubicacion seleccionada",
                    Type = PinType.Place,
                    Location = new Location(pedido.Latitud, pedido.Longitud)
                };
                map.Pins.Add(pinCliente);

                String apiKey = "AIzaSyDs7V_A9MHs269Tt1uhlKfI_qK25iTmtcE";

                String url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origen}&destination={destino}&key={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        String json = await response.Content.ReadAsStringAsync();

                        JObject routeData = JObject.Parse(json);

                        JArray steps = (JArray)routeData["routes"][0]["legs"][0]["steps"];

                        List<Location> routeCoordinates = new List<Location>();

                        foreach (JObject step in steps)
                        {
                            double lat = (double)step["start_location"]["lat"];
                            double lng = (double)step["start_location"]["lng"];

                            Location position = new Location(lat, lng);
                            routeCoordinates.Add(position);
                        }

                        routePolyline = new Polyline
                        {
                            StrokeColor = Colors.Red,
                            StrokeWidth = 5
                        };

                        MapSpan mapSpan = MapSpan.FromCenterAndRadius(routeCoordinates[0], Distance.FromKilometers(100));
                        map.MoveToRegion(mapSpan);

                        foreach (Location position in routeCoordinates)
                        {
                            routePolyline.Geopath.Add(position);
                        }

                        map.MapElements.Add(routePolyline);
                        btnEntregar.Text = "Entregado";
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "OCURRIO UN ERROR", "Ok");
                    }
                }
            }
            else
            {
                if(btnEntregar.Text == "Entregado")
                {
                    if(op.DeleteDetallePedido(pedido.IDPedido))
                    {
                        if (op.DeletePedido(pedido.IDPedido))
                        {
                            verpedidos.Children.Remove(frame);
                            if (map.MapElements.Contains(routePolyline))
                            {
                                map.MapElements.Remove(routePolyline);
                            }
                            if (pinTienda != null && map.Pins.Contains(pinTienda))
                            {
                                map.Pins.Remove(pinTienda);
                            }
                            if (pinCliente != null && map.Pins.Contains(pinCliente))
                            {
                                map.Pins.Remove(pinCliente);
                            }
                            await DisplayAlert("Entregado", "Pedido entregado correctamente", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", op.sLastError.ToString(), "Ok");
                    }
                }
            }
        };

        stackLayout.Children.Add(Fecha);
        stackLayout.Children.Add(IDpedido);
        stackLayout.Children.Add(Cliente);
        horizontalStackLayout.Children.Add(label3);
        horizontalStackLayout.Children.Add(Total);
        stackLayout.Children.Add(horizontalStackLayout);
        stackLayout.Children.Add(label4);
        stackLayout.Children.Add(VerticalStackLayout);
        stackLayout.Children.Add(btnEntregar);

        frame.Content = stackLayout;

        return frame;
    }
    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}