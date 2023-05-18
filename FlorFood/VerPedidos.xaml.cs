namespace FlorFood;

public partial class VerPedidos : ContentPage
{
	public VerPedidos()
	{
		InitializeComponent();
	}

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}