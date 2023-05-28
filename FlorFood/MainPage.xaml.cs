namespace FlorFood;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();
	}
    private void LimpiarCampos()
    {
        tbPassword.Text = "";
        tbUser.Text = "";
    }
    private void btnAcept_Clicked(object sender, EventArgs e)
    {
        if(tbUser.Text != null && tbPassword != null)
        {
            OpBaseDeDatos op = new OpBaseDeDatos();
            DatosCliente datoscliente = new DatosCliente()
            {
                Usuario= tbUser.Text,
                Contrasena= tbPassword.Text,
            };
            DatosEmpresa datosempresa = new DatosEmpresa()
            {
                Usuario = tbUser.Text,
                Contrasena = tbPassword.Text,
            };
            if(op.ValInicioSesionCliente(datoscliente))
            {
                Cliente.userCliente = tbUser.Text;
                Cliente.passwordCliente = tbPassword.Text;
                var cliente = new Cliente();
                Navigation.PushAsync(cliente);
                LimpiarCampos();
            }
            else
            {
                if(op.ValInicioSesionEmpresa(datosempresa))
                {
                    AgregarPlatillos.userEmpresa = tbUser.Text;
                    AgregarPlatillos.passEmpresa = tbPassword.Text;
                    var negocio = new Negocio();
                    Navigation.PushAsync(negocio);
                    LimpiarCampos();
                }
                else
                {
                    DisplayAlert("INCORRECTO", "Usuario o contraseña incorrectos", "Aceptar");
                }
            }
        }
        else
        {
            DisplayAlert("INCORRECTO", "Usuario o contraseña incorrectos", "Aceptar");
        }
    }

    private async void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        var registro = new Registro();
        await Navigation.PushAsync(registro);
    }
}

