namespace FlorFood;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MySqlConnector;
using System.Collections.ObjectModel;

public partial class Cliente : ContentPage
{
    private List<DatosPlatillos> platillosSeleccionados = new List<DatosPlatillos>();
    public static String userCliente;
    public static String passwordCliente;

    int IDNegocio2 = 0;
    public Cliente()
	{
        OpBaseDeDatos op = new OpBaseDeDatos();
        DatosCliente cliente = new DatosCliente();
        string cliente2 = "";
        InitializeComponent();
        if(op.ConsultarClienteNombre(userCliente, passwordCliente, cliente))
        {
            cliente2 = cliente.Nombre;
            NombreCliente.Text = cliente2;
            Carrito.ClienteNom = cliente2;
        }
    }
    private void MostrarPlatillos()
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        List<DatosPlatillos> platillos = op.ObtenerPlatillos(IDNegocio2);

        foreach (var platillo in platillos)
        {
            Frame frame = CreateCustomFrame(platillo);
            pedidos.Children.Add(frame);
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

        Button btnAgg = new Button
        {
            Text = "Agregar",
            BackgroundColor = Color.FromHex("#e33d8b"),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            TextColor = Colors.White,
            CornerRadius = 30,
            WidthRequest = 180,
            HeightRequest = 60,
            Margin = new Thickness(0, 0, 0, 10)
        };
        btnAgg.Clicked += (sender, e) =>
        {
            platillosSeleccionados.Add(platillo);
        };

        stackLayout.Children.Add(image);
        stackLayout.Children.Add(nombrePlatillo);
        stackLayout.Children.Add(descripcion);
        horizontalStackLayout.Children.Add(label3);
        horizontalStackLayout.Children.Add(precio);
        stackLayout.Children.Add(horizontalStackLayout);
        stackLayout.Children.Add(btnAgg);

        frame.Content = stackLayout;

        return frame;
    }

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private void llenarComboEmpresa()
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        string query = $"SELECT Nombre FROM Empresa";
        MySqlCommand COMAND = new MySqlCommand(query, op.CrearConexion());
        MySqlDataReader reader = COMAND.ExecuteReader();
        List<string> empresa = new List<string>();
        while (reader.Read())
        {
            empresa.Add(reader.GetString(0));
        }
        cbEmpresa.ItemsSource = empresa.ToArray();
    }
    private void cbEmpresa_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        DatosPlatillos dato = new DatosPlatillos();
        op.ConsultarEmpresa(Convert.ToString(cbEmpresa.SelectedItem), dato);
        IDNegocio2 = dato.IDNegocio;
        Carrito.EmpresaId = IDNegocio2;
        pedidos.Children.Clear();
        MostrarPlatillos();
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        llenarComboEmpresa();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        var carrito = new Carrito(platillosSeleccionados);
        Navigation.PushAsync(carrito);
    }
}