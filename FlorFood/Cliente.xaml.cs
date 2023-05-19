namespace FlorFood;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MySqlConnector;

public partial class Cliente : ContentPage
{
    int IDNegocio2 = 0;
    public Cliente()
	{
		InitializeComponent();
        pedidos.Add(CreateCustomFrame());
        //AddContent();
    }

    public Frame CreateCustomFrame()
    {
        Frame frame = new Frame
        {
            BorderColor = Color.FromHex("#e33d8b"),
            Padding = new Thickness(10)
        };

        StackLayout stackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical
        };

        Image image = new Image
        {
            Source = "hamburguesa.jpg",
            Aspect = Aspect.AspectFill,
            HeightRequest = 150,
            WidthRequest = 150
        };

        Label label1 = new Label
        {
            Text = "Platillo 1",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        Label label2 = new Label
        {
            Text = "Descripción del platillo 1",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        StackLayout horizontalStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal
        };

        Label label3 = new Label
        {
            Text = "                    Precio: ",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        Label label4 = new Label
        {
            Text = "150",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };

        Button button = new Button
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

        stackLayout.Children.Add(image);
        stackLayout.Children.Add(label1);
        stackLayout.Children.Add(label2);
        horizontalStackLayout.Children.Add(label3);
        horizontalStackLayout.Children.Add(label4);
        stackLayout.Children.Add(horizontalStackLayout);
        stackLayout.Children.Add(button);
        frame.Content = stackLayout;

        return frame;
    }


/*
public Frame AddContent()
{
    Frame frame = new Frame
    {
        BorderColor = Color.FromHex("#e33d8b"),
        Padding = 10
    };
    Image image = new Image
    {
        Source = "hamburguesa.jpg",
        Aspect = Aspect.AspectFill,
        HeightRequest = 150,
        WidthRequest = 150
    };
    pedidos.Children.Add(image);

    Label label1 = new Label
    {
        Text = "Hamburguesa",
        FontSize = 20,
        FontAttributes = FontAttributes.Bold,
        HorizontalOptions = LayoutOptions.Center
    };
    pedidos.Children.Add(label1);

    Label label2 = new Label
    {
        Text = "Descripción del platillo 1",
        FontSize = 16,
        HorizontalOptions = LayoutOptions.Center
    };
    pedidos.Children.Add(label2);

    StackLayout horizontalStackLayout = new StackLayout
    {
        Orientation = StackOrientation.Horizontal
    };
    Label label3 = new Label
    {
        Text = "Precio: ",
        FontSize = 16,
        HorizontalOptions = LayoutOptions.Center
    };
    horizontalStackLayout.Children.Add(label3);

    Label label4 = new Label
    {
        Text = "150",
        FontSize = 16,
        HorizontalOptions = LayoutOptions.Center
    };
    horizontalStackLayout.Children.Add(label4);

    pedidos.Children.Add(horizontalStackLayout);

    Button button = new Button
    {
        Text = "Agregar",
        BackgroundColor = Color.FromHex("#e33d8b"),
        HorizontalOptions = LayoutOptions.CenterAndExpand,
        TextColor = Color.FromHex("#ffffff"),
        CornerRadius = 30,
        WidthRequest = 180,
        HeightRequest = 60,
        Margin = new Thickness(0, 0, 0, 10)
    };
    pedidos.Children.Add(button);

    frame.Content = pedidos;

    Content = frame;

    return frame;
}*/




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
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        llenarComboEmpresa();
    }
}