using MySqlConnector;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.IO;

namespace FlorFood;

public partial class AgregarPlatillos : ContentPage
{
    public Boolean bExiste = false;
    public static String userEmpresa;
    public static String passEmpresa;
    int IDNegocio2 = 0;
	public AgregarPlatillos()
	{
        OpBaseDeDatos op = new OpBaseDeDatos();
        DatosEmpresa empresa = new DatosEmpresa();
		InitializeComponent();
        lblID.Text = op.FolioPlatillo().ToString();
        if(op.ConsultarEmpresaNombre(userEmpresa, passEmpresa, empresa))
        {
            lblNombreEmpresa.Text = empresa.Nombre;
        }
        DatosPlatillos dato = new DatosPlatillos();
        op.ConsultarEmpresa(lblNombreEmpresa.Text, dato);
        IDNegocio2 = dato.IDNegocio;
        VerPedidos.NegocioId = IDNegocio2;
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
        Boolean bAllOUsuario = (!bExiste) ? op.InsertPLatillo(datos) : op.UpdatePlatillo(datos);
        if (op.bAllOk == true)
        {
            await DisplayAlert("CORRECTO", "Informacion almacenada correctamente", "Aceptar");
        }
        else
        {
            await DisplayAlert("ERROR", op.sLastError, "Aceptar");
        }
    }
    private void limpiar()
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        lblID.Text = op.FolioPlatillo().ToString();
        tbNombre.Text = "";
        tbPrecio.Text = "";
        tbDescripcion.Text = "";
        ImagenGuardada.Source = null;
        tbImageSource.Text = "";

    }
    private void tbNombre_Completed(object sender, EventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();

        DatosPlatillos datos = new DatosPlatillos();

        if (op.ReadPlatillo(tbNombre.Text, IDNegocio2, ref datos))
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

    private void btnLimpiar_Clicked(object sender, EventArgs e)
    {
        limpiar();
    }

    private void btnEliminar_Clicked(object sender, EventArgs e)
    {
        OpBaseDeDatos op = new OpBaseDeDatos();
        if (op.DeletePlatillo(tbNombre.Text, IDNegocio2))
        {
            DisplayAlert("CORRECTO", "Informacion eliminada correctamente", "Aceptar");
            limpiar();
        }
        else
        {
            DisplayAlert("ERROR", op.sLastError, "Aceptar");
        }
    }
}