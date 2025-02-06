using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class UsuarioAD
    {
        public static UsuarioAD _instancia = null;

        private UsuarioAD()
        {

        }

        public static UsuarioAD Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new UsuarioAD();
                }
                return _instancia;
            }
        }

        public async Task<List<Usuario>> ListaUsuarioAsync()
        {
            List<Usuario> rptListaUsuario = new List<Usuario>();
            using (SqlConnection conexion = new SqlConnection(ConexionSQL.conexionSQL))
            {
                SqlCommand cmd = new SqlCommand("ListaUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await conexion.OpenAsync();
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        rptListaUsuario.Add(new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Nombre = dr["Nombre"].ToString()
                        });
                    }
                    dr.Close();
                }
                catch(Exception ex)
                {
                    rptListaUsuario = new List<Usuario>();
                }
                finally
                {
                    conexion.Close();
                }
            }
            return rptListaUsuario;
        }
    }
}
