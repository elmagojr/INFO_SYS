using DDigital.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ProyectoDIGITALPERSONA
{
    class ODBC_CONN
    {
        public readonly string connectionString;
        OdbcConnection db_con;
        private string UID = "HID";
        private string PWD = "hid@123456";
       
        
        public ODBC_CONN() {
            if (!probarConexion(UID, PWD))
            {
               // MessageBox.Show(UT.HAS_ERROS("DB00000"), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
           
            }
            
            connectionString = string.Format("DSN=SISC;UID={0};PWD={1};", UID, PWD);
            db_con = new OdbcConnection(connectionString);
              
            
        }
        public bool probarConexion(string usr, string pass)
        {
            try
            {
                using (OdbcConnection connection = new OdbcConnection(string.Format("DSN=SISC;UID={0};PWD={1};", usr, pass)))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;

            }
        }

              
        public int EjecutarParametrizada(string sql, Dictionary<string, object> parametros = null) //DB00001
        {
            int rowAffected = 0;
            try
            {
                using (db_con)
                {
                    db_con.Open();
                    using (OdbcCommand cmd = db_con.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (parametros != null)
                        {
                            foreach (KeyValuePair<string, object> parametro in parametros)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }
                        rowAffected = cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("SE REALIZÒ LA CONSULTA CON EXITO");
            }
            catch (Exception E)
            {
               // MessageBox.Show(UT.HAS_ERROS("DB00001") + E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (db_con.State == ConnectionState.Open)
                {
                    db_con.Close();
                }
            }
            return rowAffected;
        }
        public DataTable EjecutarConsultaSelect(string consulta, Dictionary<string, object> parametros = null)
        {
             DataTable dataTable = new DataTable();
            try
            {
                using (db_con)
                {
                    db_con.Open();
                    using (OdbcCommand cmd = db_con.CreateCommand())
                    {
                        cmd.CommandText = consulta;

                        if (parametros != null)
                        {
                            foreach (KeyValuePair<string, object> parametro in parametros)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }

                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                Console.WriteLine("Consulta SELECT ejecutada con éxito");
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
           
                //MessageBox.Show(UT.HAS_ERROS("DB00002") + E.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Console.WriteLine("Error en la consulta SELECT: " + E.Message);
            }
            finally
            {
                db_con.Close();
            }
            return dataTable;
        }

        public int CountSelect(string consulta, Dictionary<string, object> parameters=null)
        {
            int contador=0;
            try
            {
                using (db_con)
                {
                    db_con.Open();
                    using (OdbcCommand cmd = new OdbcCommand(consulta, db_con))
                    {
                     
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parametro in parameters)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }
                        object result = cmd.ExecuteScalar();

                        if (result != null && result !=DBNull.Value)
                        {
                            contador = Convert.ToInt32(result);
                        }
                    }
                }
                Console.WriteLine("SE REALIZÒ LA CONSULTA CON EXITO");
            }
            catch (Exception E)
            {
               // MessageBox.Show(UT.HAS_ERROS("DB00003") + E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                db_con.Close();
            }
            return contador;
        }

        public string primeraCol(string consulta, Dictionary<string, object> parameters = null)
        {
            string valor="";
            try
            {
                using (db_con)
                {
                    db_con.Open();
                    using (OdbcCommand cmd = new OdbcCommand(consulta, db_con))
                    {

                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parametro in parameters)
                            {
                                cmd.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            valor = result.ToString();
                        }
                    }
                }
                Console.WriteLine("SE REALIZÒ LA CONSULTA CON EXITO");
            }
            catch (Exception E)
            {
               // MessageBox.Show(UT.HAS_ERROS("DB00004") + E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                db_con.Close();
            }
            return valor;
        }

        public bool SelectExisteElvalor( string consulta, Dictionary<string, string> ValorComparar= null)
        {
            try
            {
                using (db_con)
                {
                    db_con.Open();
                    // string query = $"select 1 from {nombreTabla} where {nombrecampo} = ?";
                    using (OdbcCommand cdm = new OdbcCommand(consulta, db_con))
                    {
                        if (ValorComparar != null)
                        {
                            foreach (KeyValuePair<string, string> parametro in ValorComparar)
                            {
                                cdm.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }

                        using (OdbcDataReader reader = cdm.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }

                }
            }
            catch (Exception E)
            {
               // MessageBox.Show(UT.HAS_ERROS("DB00004") + E.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
                           
        }

      

    }



}
