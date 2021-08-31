using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;

namespace ABMMySqlCloud
{
    class ClsDal
    {
        

        SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source = bd.db; Version=3;New=False;Compress=True;");
       
        
        
        public void Abrir()
        {
            // MysqlConn.ConnectionString = "server=168.197.49.15;user id=alcancel_admin;database=alcancel_prueba;Password=violetas";
            try
            {
                sqlite_conn.Open();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void Cerrar()
        {
            sqlite_conn.Close();
        }


        public DataTable RetornaTabla(string query)
        {
            DataTable Tabla = new DataTable();
            try
            {

                sqlite_conn.Open();

                SQLiteCommand Comando = new SQLiteCommand(query, sqlite_conn);
                SQLiteDataAdapter Data = new SQLiteDataAdapter(Comando);

                Data.Fill(Tabla);
            }
            catch (SQLiteException ex)
            {

                throw ex;
            }
            finally
            {
                sqlite_conn.Close();
            }


            return Tabla;
        }

        //Retorna un Scalar
        public bool LeerScalar(string consulta)
        {

            sqlite_conn.Open();
            //uso el constructor del objeto Command
            SQLiteCommand cmd = new SQLiteCommand(consulta, sqlite_conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                int Respuesta = Convert.ToInt32(cmd.ExecuteScalar());
                sqlite_conn.Close();
                if (Respuesta > 0)
                { return true; }
                else
                { return false; }
            }
            catch (SQLiteException ex)
            { throw ex; }
        }

        //Metodo generico para escribir en la BD. Recibe un string
        //Usando transaction
        public void Escribir(string SQL)
        {

            //Abro la conexion

            sqlite_conn.Open();

            //creo el objeto transaction
            SQLiteTransaction myTrans;

            //asigno la coneccionn al objeto  transaction
            myTrans = sqlite_conn.BeginTransaction();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlite_conn;
            cmd.CommandText = SQL;

            //al objeto comand le paso la transaction
            cmd.Transaction = myTrans;

            try
            {   ///Ejecuto la consulta
                cmd.ExecuteNonQuery();
                //si esta todo ok la transaction se ejecuta
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                //si no se realizo la transaction OK se hace un rollback
                myTrans.Rollback();
                MessageBox.Show(ex.Message);
            }
            //Cierro la conexion
            sqlite_conn.Close();
        }

        /// <summary>
        /// Retorna la base entera
        /// </summary>
        /// <returns></returns>
        public DataSet RetornaDataSet()
        {
            DataSet ds = new DataSet();
            ds = CrearDataSet();
            
           
            foreach (DataTable T in ds.Tables)
            {
                string SQL = $"Select * from {T.TableName}";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlite_conn;
                cmd.CommandText = SQL;

                try
                {
                    //creo el data adapter le paso el comando
                    SQLiteDataAdapter Da = new SQLiteDataAdapter(cmd);
                    //lleno la tabla con el metodo fill
                    Da.Fill(T);
                }
                catch (SQLiteException)
                { throw new Exception("Conexion Error\n\nVerifique la cadena de conexión"); }
                catch (Exception ex)
                { throw ex; }
                finally
                { //cierro la Conexion
                    sqlite_conn.Close();
                }
            }
            return ds;
            

        }

        public void GuardarDataset(DataSet Ds, string pNombreTabla)
        {
            foreach (DataTable T in Ds.Tables)
            {
                string s = T.TableName;
            }
            //Abro la conexion
            Abrir();
            string SQL = $"SELECT * FROM {pNombreTabla}";
            //creo el objeto transaction
            SQLiteTransaction myTrans;
            //asigno la coneccionn al objeto  transaction
            myTrans = sqlite_conn.BeginTransaction();
            //SETEO DATAADAPTER CON el Store Y CONNECTIONSTRING
            SQLiteDataAdapter Da = new SQLiteDataAdapter(SQL,sqlite_conn);
            Da.SelectCommand.CommandType = CommandType.Text;
            Da.SelectCommand.Transaction = myTrans;
            //SE SETEAN LOS METODOS PARA GUARDAR DATOS EN BASE DE DATOS
            SQLiteCommandBuilder Cb = new SQLiteCommandBuilder(Da);

            Da.UpdateCommand = Cb.GetUpdateCommand();
            Da.DeleteCommand = Cb.GetDeleteCommand();
            Da.InsertCommand = Cb.GetInsertCommand();
            Da.ContinueUpdateOnError = true;

            Da.UpdateCommand.Transaction = myTrans;
            Da.DeleteCommand.Transaction = myTrans;
            Da.InsertCommand.Transaction = myTrans;
            try
            {
                //SE INTENTAN PERSISTIR LOS CAMBIOS EN LA BASE DE DATOS
                Da.Update(Ds.Tables[$"{pNombreTabla}"]);
                
                myTrans.Commit();

            }
            catch (SQLiteException ex)
            {
                //si no se realizo la transaction OK se hace un rollback
                myTrans.Rollback();
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Cerrar();
        }

        public DataSet CrearDataSet()
        {
            DataTable Tabla = new DataTable("Cliente");

            //IdCliente
            DataColumn IdCliente = new DataColumn
            {
                ColumnName = "IdCliente",
                DataType = typeof(Int32),
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1
            };
            Tabla.Columns.Add(IdCliente);

            //Nombre
            DataColumn Nombre = new DataColumn
            {
                ColumnName = "Nombre",
                DataType = typeof(System.String)
            };
            Tabla.Columns.Add(Nombre);


            //Apellido
            DataColumn Apellido = new DataColumn
            {
                ColumnName = "Apellido",
                DataType = typeof(System.String)
            };
            Tabla.Columns.Add(Apellido);

           

            //Determino cual es la clave de la tabla
            Tabla.PrimaryKey = new DataColumn[] { IdCliente };

            DataSet ds = new DataSet();
            ds.Tables.Add(Tabla);
            return ds;
        }
    }
}
