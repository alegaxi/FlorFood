using Microsoft.Maui.Controls.Maps;

namespace FlorFood;

public partial class Registro : ContentPage
{
    public Boolean bExiste = false;
    public Registro()
	{
		InitializeComponent();
    }
    private void cbTipo_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if(cbTipo.SelectedItem.ToString() == "Cliente")
        {
            map.IsVisible = false;
            lblLatitud.IsVisible= false;
            lblLongitud.IsVisible= false;
            lblNota.IsVisible= false;
            Apellido.IsVisible= true;
            OpBaseDeDatos opCliente = new OpBaseDeDatos();
            lblID.Text = opCliente.FolioCliente().ToString();
        }
        else 
        {
            if(cbTipo.SelectedItem.ToString() == "Empresa")
            {
                map.IsVisible = true;
                lblLatitud.IsVisible = true;
                lblLongitud.IsVisible = true;
                lblNota.IsVisible = true;
                Apellido.IsVisible = false;
                OpBaseDeDatos opEmpresa = new OpBaseDeDatos();
                lblID.Text = opEmpresa.FolioEmpresa().ToString();
            }
            
        }
    }

    private void btnExit_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void map_MapClicked(object sender, MapClickedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");
        map.Pins.Clear();
        Pin pin = new Pin
        {
            Label = "Tienda",
            Address = "Ubicacion seleccionada",
            Type = PinType.Place,
            Location = new Location(e.Location.Latitude, e.Location.Longitude)
        };
        map.Pins.Add(pin);
        lblLatitud.Text=e.Location.Latitude.ToString();
        lblLongitud.Text =e.Location.Longitude.ToString();
    }

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if(cbTipo.Text == null)
        {
            DisplayAlert("ERROR", "Elija un tipo de usuario", "Aceptar");
        }
        else
        {
            if(tbNombre.Text == null)
            {
                DisplayAlert("ERROR", "Introduzca un nombre", "Aceptar");
            }
            else
            {
                if(cbTipo.SelectedItem.ToString() == "Cliente" && tbApellido.Text == null)
                {
                    DisplayAlert("ERROR", "Introduzca un apellido", "Aceptar");
                }
                else
                {
                    if(tbTelefono.Text == null)
                    {
                        DisplayAlert("ERROR", "Introduzca un telefono", "Aceptar");
                    }
                    else
                    {
                        if(tbUser.Text == null)
                        {
                            DisplayAlert("ERROR", "Introduzca un usuario", "Aceptar");
                        }
                        else
                        {
                            if(tbPassword.Text == null)
                            {
                                DisplayAlert("ERROR", "Introduzca una contraseña", "Aceptar");
                            }
                            else
                            {
                                if(tbConfirmPassword.Text == null)
                                {
                                    DisplayAlert("ERROR", "Por favor confirme su contraseña", "Aceptar");
                                }
                                else
                                {
                                    if(tbPassword.Text != tbConfirmPassword.Text)
                                    {
                                        DisplayAlert("ERROR", "Las contraseñas no coinciden", "Aceptar");
                                    }
                                    else
                                    {
                                        if(cbTipo.SelectedItem.ToString() == "Empresa" && lblLatitud.Text == null && lblLongitud.Text == null)
                                        {
                                            DisplayAlert("ERROR", "Elija una ubicacion", "Aceptar");
                                        }
                                        else
                                        {
                                            if(cbTipo.SelectedItem.ToString() == "Cliente")
                                            {
                                                OpBaseDeDatos opCliente = new OpBaseDeDatos();
                                                DatosCliente datos = new DatosCliente()
                                                {
                                                    IDCliente = Int32.Parse(lblID.Text),
                                                    Nombre = tbNombre.Text,
                                                    Apellidos = tbApellido.Text,
                                                    Tipo = cbTipo.SelectedItem.ToString(),
                                                    Telefono = tbTelefono.Text,
                                                    Usuario = tbUser.Text,
                                                    Contrasena = tbPassword.Text
                                                };
                                                Boolean bAllOUsuario = (!bExiste) ? opCliente.InsertCliente(datos) : opCliente.UpdateCliente(datos);
                                                if (opCliente.bAllOk == true)
                                                {
                                                    DisplayAlert("CORRECTO", "Informacion almacenada correctamente", "Aceptar");
                                                    LimpiarCampos();
                                                }
                                                else
                                                {
                                                    DisplayAlert("ERROR", opCliente.sLastError, "Aceptar");
                                                }
                                            }
                                            else
                                            {
                                                if(cbTipo.SelectedItem.ToString() == "Empresa")
                                                {
                                                    OpBaseDeDatos opEmpresa = new OpBaseDeDatos();
                                                    DatosEmpresa datos = new DatosEmpresa()
                                                    {
                                                        IDEmpresa = Int32.Parse(lblID.Text),
                                                        Nombre = tbNombre.Text,
                                                        Tipo = cbTipo.SelectedItem.ToString(),
                                                        Telefono = tbTelefono.Text,
                                                        Usuario = tbUser.Text,
                                                        Contrasena = tbPassword.Text,
                                                        Latitud = Convert.ToDouble(lblLatitud.Text),
                                                        Longitud = Convert.ToDouble(lblLongitud.Text),
                                                    };
                                                    Boolean bAllOUsuario = (!bExiste) ? opEmpresa.InsertEmpresa(datos) : opEmpresa.UpdateEmpresa(datos);
                                                    if (opEmpresa.bAllOk == true)
                                                    {
                                                        DisplayAlert("CORRECTO", "Informacion almacenada correctamente", "Aceptar");
                                                        LimpiarCampos();
                                                    }
                                                    else
                                                    {
                                                        DisplayAlert("ERROR", opEmpresa.sLastError, "Aceptar");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void LimpiarCampos()
    {
        tbNombre.Text = "";
        tbApellido.Text = "";
        tbTelefono.Text = "";
        tbUser.Text = "";
        tbPassword.Text = "";
        tbConfirmPassword.Text = "";
        lblLatitud.Text = "";
        lblLongitud.Text = "";
    }
    private void btnEliminar_Clicked(object sender, EventArgs e)
    {
        OpBaseDeDatos opCliente = new OpBaseDeDatos();
        if (cbTipo.SelectedItem.ToString() == "Cliente")
        {
            if(opCliente.DeleteCliente(tbNombre.Text))
            {
                DisplayAlert("CORRECTO", "Informacion eliminada correctamente", "Aceptar");
                LimpiarCampos();
            }
            else
            {
                DisplayAlert("ERROR", opCliente.sLastError, "Aceptar");
            }
        }
        else
        {
            if(cbTipo.SelectedItem.ToString() == "Empresa")
            {
                if (opCliente.DeleteEmpresa(tbNombre.Text))
                {
                    DisplayAlert("CORRECTO", "Informacion eliminada correctamente", "Aceptar");
                    LimpiarCampos();
                }
                else
                {
                    DisplayAlert("ERROR", opCliente.sLastError, "Aceptar");
                }
            }
            else
            {
                DisplayAlert("ERROR", "Seleccione un tipo de usuario", "Aceptar");
            }
        }
    }

    private void tbNombre_Completed(object sender, EventArgs e)
    {
        if (cbTipo.SelectedItem.ToString() == "Empresa")
        {
            DatosEmpresa datos = new DatosEmpresa();
            OpBaseDeDatos opEmpresa = new OpBaseDeDatos();
            if (opEmpresa.ReadEmpresa(tbNombre.Text, ref datos))
            {
                lblID.Text = datos.IDEmpresa.ToString();
                tbTelefono.Text = datos.Telefono;
                tbUser.Text = datos.Usuario;
                tbPassword.Text = datos.Contrasena;
                lblLatitud.Text = datos.Latitud.ToString();
                lblLongitud.Text = datos.Longitud.ToString();
            }
            else
            {
                DisplayAlert("ERROR", opEmpresa.sLastError, "Aceptar");
            }
        }
        else
        {
            if (cbTipo.SelectedItem.ToString() == "Cliente")
            {
                DatosCliente datos = new DatosCliente();
                OpBaseDeDatos opCliente = new OpBaseDeDatos();
                if (opCliente.ReadCliente(tbNombre.Text, ref datos))
                {
                    lblID.Text = datos.IDCliente.ToString();
                    tbApellido.Text = datos.Apellidos;
                    tbTelefono.Text = datos.Telefono;
                    tbUser.Text = datos.Usuario;
                    tbPassword.Text = datos.Contrasena;
                }
                else
                {
                    DisplayAlert("ERROR", opCliente.sLastError, "Aceptar");
                }
            }
            else
            {
                DisplayAlert("ERROR", "Seleccione un tipo de usuario", "Aceptar");
            }
        }
    }

    private void btnLimpiar_Clicked(object sender, EventArgs e)
    {
        LimpiarCampos();
    }
}