using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlorFood
{
    public class DatosCliente
    {
        public Int32 IDCliente = 0;
        public String Nombre = " ";
        public String Apellidos = " ";
        public String Tipo = " ";
        public String Telefono = " ";
        public String Usuario = " ";
        public String Contrasena = " ";

    }
    public class DatosEmpresa
    {
        public Int32 IDEmpresa = 0;
        public String Nombre = " ";
        public String Tipo = " ";
        public String Telefono = " ";
        public String Usuario = " ";
        public String Contrasena = " ";
        public Double Latitud = 0;
        public Double Longitud = 0;

    }
    public class DatosPlatillos
    {
        public Int32 IDPlatillo = 0;
        public Int32 IDNegocio = 0;
        public String Nombre = " ";
        public Double Precio = 0.0;
        public String Descripcion = " ";
        public byte[] Imagen = null;
    }

    public class DatosPedido
    {
        public DateTime Fecha = DateTime.Now;
        public Int32 IDPedido = 0;
        public Int32 IDCliente = 0;
        public Int32 IDNegocio = 0;
        public Double Total = 0.0;
        public Double Latitud = 0.0;
        public Double Longitud = 0.0;
    }
    public class DatosDetallePedido
    {
        public Int32 IDPedido = 0;
        public Int32 IDPlatillo = 0;
        public Double PrecioUnitario = 0.0;
    }
    public class OpBaseDeDatos
    {
        public Boolean bAllOk = false;
        public String sLastError = "";

        public MySqlConnection CrearConexion()
        {
            string cadenaConexion = $"DataSource=192.168.137.202; Port=3306; User Id=FlorGax; Password=081278; Database=florFood; SSL Mode=None;";
            MySqlConnection conn = new MySqlConnection(cadenaConexion);
            conn.Open();
            return conn;
        }
        //PANTALLA REGISTRO
        public bool InsertCliente(DatosCliente datos)
        {
            try
            {
                string query = $"INSERT INTO Clientes (IDCliente, Nombre, Apellido, Tipo, Telefono, Usuario, Contrasena)" +
                    $"VALUES({datos.IDCliente}, '{datos.Nombre}', '{datos.Apellidos}', '{datos.Tipo}', '{datos.Telefono}', '{datos.Usuario}', '{datos.Contrasena}')";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool UpdateCliente(DatosCliente datos)
        {
            try
            {
                string query = "UPDATE Clientes " +
                                 $"SET Nombre = '{datos.Nombre}', " +
                                     $"Apellidos = '{datos.Apellidos}', " +
                                     $"Tipo = '{datos.Tipo}', "+
                                     $"Telefono = '{datos.Telefono}', " +
                                     $"Usuario = '{datos.Usuario}', " +
                                     $"Contrasena = '{datos.Contrasena}'," +
                                 $"WHERE IDCliente = {datos.IDCliente} ";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool DeleteCliente(String Nombre)
        {
            try
            {
                string query = "DELETE FROM Clientes " +
                                 $"WHERE Nombre = '{Nombre}' ";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ReadCliente(String nombre, ref DatosCliente datos)
        {
            try
            {
                string query = $"SELECT IDCliente, Nombre, Apellido, Tipo, Telefono, Usuario, Contrasena FROM Clientes WHERE Nombre = '{nombre}'";
                MySqlCommand cmd = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    datos.IDCliente = Int32.Parse(reader["IDCliente"].ToString());
                    datos.Nombre = reader["Nombre"].ToString();
                    datos.Apellidos = reader["Apellido"].ToString();
                    datos.Tipo = reader["Tipo"].ToString();
                    datos.Telefono = reader["Telefono"].ToString();
                    datos.Usuario = reader["Usuario"].ToString();
                    datos.Contrasena = reader["Contrasena"].ToString();
                    bAllOk = true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public Int32 FolioCliente()
        {
            Int32 iFolioSiguiente = 0;

            try
            {
                String sCmd = "SELECT MAX(IDCliente) FROM Clientes";
                MySqlCommand cmd = new MySqlCommand(sCmd, CrearConexion());
                iFolioSiguiente = (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            return iFolioSiguiente;
        }
        public bool InsertEmpresa(DatosEmpresa datos)
        {
            try
            {
                string query = $"INSERT INTO Empresa (IDEmpresa, Nombre, Tipo, Telefono, Usuario, Contrasena, Latitud, Longitud)" +
                    $"VALUES({datos.IDEmpresa}, '{datos.Nombre}', '{datos.Tipo}', '{datos.Telefono}', '{datos.Usuario}', '{datos.Contrasena}', {datos.Latitud}, {datos.Longitud})";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool UpdateEmpresa(DatosEmpresa datos)
        {
            try
            {
                string query = "UPDATE Empresa " +
                                 $"SET Nombre = '{datos.Nombre}', " +
                                     $"Tipo = '{datos.Tipo}', " +
                                     $"Telefono = '{datos.Telefono}', " +
                                     $"Usuario = '{datos.Usuario}', " +
                                     $"Contrasena = '{datos.Contrasena}'," +
                                     $"Latitud = {datos.Latitud}, " +
                                     $"Longitud = {datos.Longitud}, " +
                                 $"WHERE IDEmpresa = {datos.IDEmpresa}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool DeleteEmpresa(String Nombre)
        {
            try
            {
                string query = "DELETE FROM Empresa " +
                                 $"WHERE Nombre = '{Nombre}'";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ReadEmpresa(String nombre, ref DatosEmpresa datos)
        {
            try
            {
                string query = $"SELECT IDEmpresa, Nombre, Tipo, Telefono, Usuario, Contrasena, Latitud, Longitud FROM Empresa WHERE Nombre = '{nombre}'";
                MySqlCommand cmd = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    datos.IDEmpresa = Int32.Parse(reader["IDEmpresa"].ToString());
                    datos.Nombre = reader["Nombre"].ToString();
                    datos.Tipo = reader["Tipo"].ToString();
                    datos.Telefono = reader["Telefono"].ToString();
                    datos.Usuario = reader["Usuario"].ToString();
                    datos.Contrasena = reader["Contrasena"].ToString();
                    datos.Latitud = Convert.ToDouble(reader["Latitud"].ToString());
                    datos.Longitud = Convert.ToDouble(reader["Longitud"].ToString());
                    bAllOk = true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public Int32 FolioEmpresa()
        {
            Int32 iFolioSiguiente = 0;

            try
            {
                String sCmd = "SELECT MAX(IDEmpresa) FROM Empresa";
                MySqlCommand cmd = new MySqlCommand(sCmd, CrearConexion());
                iFolioSiguiente = (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            return iFolioSiguiente;
        }

        //PANTALLA LOGIN
        public bool ValInicioSesionCliente(DatosCliente datos)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM Clientes WHERE Usuario = '{datos.Usuario}' and Contrasena = '{datos.Contrasena}' and Tipo = 'Cliente';";
                MySqlCommand cmd = new MySqlCommand(query, CrearConexion());
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                if (i == 1)
                {
                    bAllOk = true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }

        public bool ValInicioSesionEmpresa(DatosEmpresa datos)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM Empresa WHERE Usuario = '{datos.Usuario}' and Contrasena = '{datos.Contrasena}' and Tipo = 'Empresa';";
                MySqlCommand cmd = new MySqlCommand(query, CrearConexion());
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                if (i == 1)
                {
                    bAllOk = true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }

        //PANTALLA PLATILLOS
        public bool InsertPLatillo(DatosPlatillos datos)
        {
            try
            {
                string query = "INSERT INTO Platillo (IDPlatillo, IDNegocio, Nombre, Precio, Descripcion, Imagen) " +
                    "VALUES (@IDPlatillo, @IDNegocio, @Nombre, @Precio, @Descripcion, @Imagen)";

                using (MySqlConnection connection = CrearConexion())
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IDPlatillo", datos.IDPlatillo);
                    command.Parameters.AddWithValue("@IDNegocio", datos.IDNegocio);
                    command.Parameters.AddWithValue("@Nombre", datos.Nombre);
                    command.Parameters.AddWithValue("@Precio", datos.Precio);
                    command.Parameters.AddWithValue("@Descripcion", datos.Descripcion);
                    command.Parameters.AddWithValue("@Imagen", datos.Imagen);

                    command.ExecuteNonQuery();
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ReadPlatillo(string nombre, int id, ref DatosPlatillos datos)
        {
            try
            {
                string query = "SELECT IDPlatillo, IDNegocio, Nombre, Precio, Descripcion, Imagen FROM Platillo WHERE Nombre = @Nombre AND IDNegocio = @IDNegocio";
                using (MySqlCommand cmd = new MySqlCommand(query, CrearConexion()))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@IDNegocio", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idPlatilloOrdinal = reader.GetOrdinal("IDPlatillo");
                            int idNegocioOrdinal = reader.GetOrdinal("IDNegocio");
                            int nombreOrdinal = reader.GetOrdinal("Nombre");
                            int precioOrdinal = reader.GetOrdinal("Precio");
                            int descripcionOrdinal = reader.GetOrdinal("Descripcion");
                            int imagenOrdinal = reader.GetOrdinal("Imagen");

                            datos.IDPlatillo = reader.GetInt32(idPlatilloOrdinal);
                            datos.IDNegocio = reader.GetInt32(idNegocioOrdinal);
                            datos.Nombre = reader.GetString(nombreOrdinal);
                            datos.Precio = reader.GetDouble(precioOrdinal);
                            datos.Descripcion = reader.GetString(descripcionOrdinal);
                            byte[] imageData = (byte[])reader.GetValue(imagenOrdinal);
                            datos.Imagen = imageData;
                            bAllOk = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool UpdatePlatillo(DatosPlatillos datos)
        {
            try
            {
                string query = "UPDATE Platillo " +
                                 $"SET IDNegocio = {datos.IDNegocio}, " +
                                     $"Nombre = '{datos.Nombre}', " +
                                     $"Precio = {datos.Precio}, " +
                                     $"Descripcion = '{datos.Descripcion}', " +
                                     $"Imagen = '{datos.Imagen}'," +
                                 $"WHERE IDPlatillo = {datos.IDPlatillo}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool DeletePlatillo(String Nombre, int id)
        {
            try
            {
                string query = "DELETE FROM Platillo " +
                                 $"WHERE Nombre = '{Nombre}' AND IDNegocio = {id}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public Int32 FolioPlatillo()
        {  Int32 iFolioSiguiente = 0;

            try
            {
                String sCmd = "SELECT MAX(IDPlatillo) FROM Platillo";
                MySqlCommand cmd = new MySqlCommand(sCmd, CrearConexion());
                iFolioSiguiente = (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            return iFolioSiguiente;
        }
          
        public bool ConsultarEmpresa(String Nombre, DatosPlatillos empresa)
        {
            try
            {
                string query = $"SELECT IDEmpresa FROM Empresa WHERE Nombre = '{Nombre}'";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    empresa.IDNegocio = Int32.Parse(reader["IDEmpresa"].ToString());
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ConsultarEmpresaNombre(String User, String Password, DatosEmpresa empresa)
        {
            try
            {
                string query = $"SELECT Nombre FROM Empresa WHERE Usuario = '{User}' and Contrasena = '{Password}'";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    empresa.Nombre = reader["Nombre"].ToString();
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }

        //PANTALLA CLIENTE
        public bool ConsultarClienteNombre(String User, String Password, DatosCliente cliente)
        {
            try
            {
                string query = $"SELECT Nombre FROM Clientes WHERE Usuario = '{User}' and Contrasena = '{Password}'";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente.Nombre = reader["Nombre"].ToString();
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public List<DatosPlatillos> ObtenerPlatillos(Int32 Id)
        {
            List<DatosPlatillos> platillos = new List<DatosPlatillos>();

            try
            {
                string query = $"SELECT IDPlatillo, Nombre, Precio, Descripcion, Imagen FROM Platillo WHERE IDNegocio = {Id}";
                using (MySqlCommand cmd = new MySqlCommand(query, CrearConexion()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatosPlatillos platillo = new DatosPlatillos();
                            platillo.IDPlatillo = reader.GetInt32("IDPlatillo");
                            platillo.Nombre = reader.GetString("Nombre");
                            platillo.Precio = reader.GetDouble("Precio");
                            platillo.Descripcion = reader.GetString("Descripcion");
                            byte[] imageData = (byte[])reader.GetValue("Imagen");
                            platillo.Imagen = imageData;

                            platillos.Add(platillo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }

            return platillos;
        }

        // PANTALLA CARRITO
        public bool InsertPedido(DatosPedido datos)
        {
            try
            {
                string query = "INSERT INTO Pedido (Fecha, IDPedido, IDCliente, IDNegocio, Total, Latitud, Longitud)" +
                        "VALUES(@Fecha, @IDPedido, @IDCliente, @IDNegocio, @Total, @Latitud, @Longitud)";

                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.Parameters.AddWithValue("@Fecha", datos.Fecha);
                command.Parameters.AddWithValue("@IDPedido", datos.IDPedido);
                command.Parameters.AddWithValue("@IDCliente", datos.IDCliente);
                command.Parameters.AddWithValue("@IDNegocio", datos.IDNegocio);
                command.Parameters.AddWithValue("@Total", datos.Total);
                command.Parameters.AddWithValue("@Latitud", datos.Latitud);
                command.Parameters.AddWithValue("@Longitud", datos.Longitud);

                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public Int32 FolioPedido()
        {
            Int32 iFolioSiguiente = 0;

            try
            {
                String sCmd = "SELECT MAX(IDPedido) FROM Pedido";
                MySqlCommand cmd = new MySqlCommand(sCmd, CrearConexion());
                iFolioSiguiente = (Convert.ToInt32(cmd.ExecuteScalar()) + 1);
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            return iFolioSiguiente;
        }
        public bool InsertDetallePedido(DatosDetallePedido datos)
        {
            try
            {
                string query = $"INSERT INTO DetallePedido (IDPedido, IDPlatillo, PrecioUnitario)" +
                    $"VALUES({datos.IDPedido}, {datos.IDPlatillo}, {datos.PrecioUnitario})";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ConsultarCliente(String Nombre, DatosPedido cliente)
        {
            try
            {
                string query = $"SELECT IDCliente FROM Clientes WHERE Nombre = '{Nombre}'";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente.IDCliente = Int32.Parse(reader["IDCliente"].ToString());
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }

        //PANTALLA VER PEDIDOS
        public List<DatosPedido> ObtenerPedidos(Int32 Id)
        {
            List<DatosPedido> pedidos = new List<DatosPedido>();

            try
            {
                string query = $"SELECT Fecha, IDPedido, IDCliente, IDNegocio, Total, Latitud, Longitud FROM Pedido WHERE IDNegocio = {Id} ORDER BY Fecha ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, CrearConexion()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatosPedido pedido = new DatosPedido();
                            pedido.Fecha = reader.GetDateTime("Fecha");
                            pedido.IDPedido = reader.GetInt32("IDPedido");
                            pedido.IDCliente = reader.GetInt32("IDCliente");
                            pedido.IDNegocio = reader.GetInt32("IDNegocio");
                            pedido.Total = reader.GetFloat("Total");
                            pedido.Latitud = reader.GetDouble("Latitud");
                            pedido.Longitud = reader.GetDouble("Longitud");

                            pedidos.Add(pedido);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }

            return pedidos;
        }
        public List<DatosDetallePedido> ObtenerDetallePedidos(Int32 Id)
        {
            List<DatosDetallePedido> detallepedidos = new List<DatosDetallePedido>();

            try
            {
                string query = $"SELECT IDPlatillo FROM DetallePedido WHERE IDPedido = {Id}";
                using (MySqlCommand cmd = new MySqlCommand(query, CrearConexion()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatosDetallePedido pedido = new DatosDetallePedido();
                            pedido.IDPlatillo = reader.GetInt32("IDPlatillo");

                            detallepedidos.Add(pedido);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }

            return detallepedidos;
        }
        public bool ConsultarNombrePlatillo(Int32 id, DatosPlatillos platillo)
        {
            try
            {
                string query = $"SELECT Nombre FROM Platillo WHERE IDPlatillo = {id}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    platillo.Nombre = reader["Nombre"].ToString();
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ConsultarClientePedido(Int32 id, DatosCliente cliente)
        {
            try
            {
                string query = $"SELECT Nombre FROM Clientes WHERE IDCliente = {id}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cliente.Nombre =reader["Nombre"].ToString();
                }
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool ReadEmpresaPedido(Int32 nombre, ref DatosEmpresa datos)
        {
            try
            {
                string query = $"SELECT Latitud, Longitud FROM Empresa WHERE IDEmpresa = {nombre}";
                MySqlCommand cmd = new MySqlCommand(query, CrearConexion());

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    datos.Latitud = Convert.ToDouble(reader["Latitud"].ToString());
                    datos.Longitud = Convert.ToDouble(reader["Longitud"].ToString());
                    bAllOk = true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool DeletePedido(Int32 id)
        {
            try
            {
                string query = "DELETE FROM Pedido " +
                                 $"WHERE IDPedido = {id}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
        public bool DeleteDetallePedido(Int32 id)
        {
            try
            {
                string query = "DELETE FROM DetallePedido " +
                                 $"WHERE IDPedido = {id}";
                MySqlCommand command = new MySqlCommand(query, CrearConexion());
                command.ExecuteNonQuery();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            return bAllOk;
        }
    }
}
