using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
   public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> Lista = new List<Usuario>();
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo from usuario";
                    SqlCommand cmd = new SqlCommand(query, oconnection);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                    Activo = Convert.ToBoolean(dr["Activo"])


                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Usuario>();
            }

            return Lista;
        }


        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idauto = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("dp_RegistrarUsuario", oconnection);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    idauto = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                   
                }
            }
            catch (Exception ex)
            {

                idauto = 0;
                Mensaje = ex.Message;
            }
            return idauto;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
           bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconnection);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);  
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconnection.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar (int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconnection = new SqlConnection(Connection.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where IdUsuario = @id", oconnection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconnection.Open();
                    //total de filas afectadas mayor a 0
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
