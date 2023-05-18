using MySqlConnector;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.IO;

namespace FlorFood;

public partial class AgregarPlatillos : ContentPage
{
    int IDNegocio2 = 0;
	public AgregarPlatillos()
	{
        OpBaseDeDatos op = new OpBaseDeDatos();
		InitializeComponent();
        lblID.Text = op.FolioPlatillo().ToString();
	}

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnImagen_Clicked(object sender, EventArgs e)
    {
        tbImageSource.Text = "";
        ImagenGuardada.Source = "";

        var file = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Selecciona una imagen"
        });

        if(file != null) 
        {
            tbImageSource.Text = file.FullPath;
            ImagenGuardada.Source = ImageSource.FromFile(file.FullPath);
            byte[] datosImagen= File.ReadAllBytes(file.FullPath);
        }
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
    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();

        FileImageSource imageSource = (FileImageSource)ImagenGuardada.Source;
        byte[] arreglo;
        using (FileStream fs = new FileStream(imageSource.File, FileMode.Open, FileAccess.Read))
        {
            arreglo = new byte[fs.Length];
            await fs.ReadAsync(arreglo, 0, (int)fs.Length);
        }

        DatosPlatillos datos = new DatosPlatillos()
        {
            IDPlatillo = Int32.Parse(lblID.Text),
            IDNegocio = IDNegocio2,
            Nombre = tbNombre.Text,
            Precio = Convert.ToDouble(tbPrecio.Text),
            Descripcion= tbDescripcion.Text,
            Imagen = arreglo
        };

        if(op.InsertPLatillo(datos))
        {
            await DisplayAlert("CORRECTO", "Informacion almacenada correctamente", "Aceptar");
        }
        else
        {
            await DisplayAlert("ERROR", op.sLastError, "Aceptar");
        }
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        llenarComboEmpresa();
    }

    private void cbEmpresa_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        DatosPlatillos dato = new DatosPlatillos();
        op.ConsultarEmpresa(Convert.ToString(cbEmpresa.SelectedItem), dato);
        IDNegocio2 = dato.IDNegocio;
    }

    private void tbNombre_Completed(object sender, EventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();

        DatosPlatillos datos = new DatosPlatillos();

        if (op.ReadPlatillo(tbNombre.Text, ref datos))
        {
            lblID.Text = datos.IDPlatillo.ToString();
            tbNombre.Text = datos.Nombre;
            tbPrecio.Text = datos.Precio.ToString();
            tbDescripcion.Text = datos.Descripcion;
            byte[] bytes = datos.Imagen;
            ImageSource imageSource = null;

            if (bytes != null && bytes.Length > 0)
            {
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, bytes);
                imageSource = ImageSource.FromFile(tempFilePath);
            }

            ImagenGuardada.Source = imageSource;
        }
        else
        {
            DisplayAlert("ERROR", op.sLastError, "Aceptar");
        }
    }
}