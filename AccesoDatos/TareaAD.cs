using CapaDatos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class TareaAD
    {
        public static TareaAD _instancia = null;

        private TareaAD()
        {

        }

        public static TareaAD Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new TareaAD();
                }
                return _instancia;
            }
        }

        public async Task<List<Tarea>> ListaTareaAsync()
        {
            List<Tarea> rptListaTarea = new List<Tarea>();
            using (SqlConnection conexion = new SqlConnection(ConexionSQL.conexionSQL))
            {
                SqlCommand cmd = new SqlCommand("ListaTarea", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await conexion.OpenAsync();
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        rptListaTarea.Add(new Tarea()
                        {
                            IdTarea = Convert.ToInt32(dr["IdTarea"]),
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            FechaActualizacion = Convert.ToDateTime(dr["FechaActualizacion"]),
                            FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                            _Usuario = JsonConvert.DeserializeObject<Usuario>(dr["_Usuario"].ToString())
                        });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    rptListaTarea = new List<Tarea>();
                }
                finally
                {
                    conexion.Close();
                }
            }
            return rptListaTarea;
        }

        public async Task<Resultado> AgregarTareaAsync(Tarea tarea)
        {
            Resultado resultado = new Resultado();
            using (SqlConnection conexion = new SqlConnection(ConexionSQL.conexionSQL))
            {
                SqlCommand cmd = new SqlCommand("AgregarTarea", conexion);
                cmd.Parameters.AddWithValue("IdUsuario", tarea.IdUsuario);
                cmd.Parameters.AddWithValue("Nombre", tarea.Nombre);
                cmd.Parameters.AddWithValue("Descripcion", tarea.Descripcion);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await conexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    resultado = new Resultado()
                    {
                        Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value),
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString()
                    };
                }
                catch (Exception ex)
                {
                    resultado = new Resultado()
                    {
                        Respuesta = false,
                        Mensaje = ex.Message
                    };
                }
                finally
                {
                    conexion.Close();
                }
            }
            return resultado;
        }

        public async Task<Resultado> ModificarTareaAsync(Tarea tarea)
        {
            Resultado resultado = new Resultado();
            using (SqlConnection conexion = new SqlConnection(ConexionSQL.conexionSQL))
            {
                SqlCommand cmd = new SqlCommand("ModificarTarea", conexion);
                cmd.Parameters.AddWithValue("IdTarea", tarea.IdTarea);
                cmd.Parameters.AddWithValue("IdUsuario", tarea.IdUsuario);
                cmd.Parameters.AddWithValue("Nombre", tarea.Nombre);
                cmd.Parameters.AddWithValue("Descripcion", tarea.Descripcion);
                cmd.Parameters.AddWithValue("Estado", tarea.Estado);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await conexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    resultado = new Resultado()
                    {
                        Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value),
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString()
                    };
                }
                catch (Exception ex)
                {
                    resultado = new Resultado()
                    {
                        Respuesta = false,
                        Mensaje = ex.Message
                    };
                }
                finally
                {
                    conexion.Close();
                }
            }
            return resultado;
        }

        public async Task<Resultado> EliminarTareaAsync(Tarea tarea)
        {
            Resultado resultado = new Resultado();
            using (SqlConnection conexion = new SqlConnection(ConexionSQL.conexionSQL))
            {
                SqlCommand cmd = new SqlCommand("EliminarTarea", conexion);
                cmd.Parameters.AddWithValue("IdTarea", tarea.IdTarea);
                cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await conexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    resultado = new Resultado()
                    {
                        Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value),
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString()
                    };
                }
                catch (Exception ex)
                {
                    resultado = new Resultado()
                    {
                        Respuesta = false,
                        Mensaje = ex.Message
                    };
                }
                finally
                {
                    conexion.Close();
                }
            }
            return resultado;
        }
    }
}
