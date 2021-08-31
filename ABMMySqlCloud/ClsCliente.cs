using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMMySqlCloud
{
    public class ClsCliente
    {
        ClsDal bd = new ClsDal();
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public void AgregarCliente(ClsCliente pCliente)
        {
            ////Preparo la consulta
            //string query = $"INSERT INTO Cliente (Nombre,Apellido) values ('{pCliente.Nombre}','{pCliente.Apellido}')";
            ////Mando el comando a la capa DAL para que se haga cargo de ejecutarlo.
            //bd.Escribir(query);
            DataSet ds = bd.RetornaDataSet();
            ds.Tables["Cliente"].Rows.Add(null, pCliente.Nombre, pCliente.Apellido);
            bd.GuardarDataset(ds,"Cliente");
        }

        public void EliminarCliente(ClsCliente pC)
        {
            bd.Escribir($"DELETE FROM Cliente WHERE IdCliente = '{pC.IdCliente}' ");
        }

        public void ModificarCliente(ClsCliente pC)
        {
            bd.Escribir($"UPDATE Cliente SET Apellido='{pC.Apellido}',Nombre='{pC.Nombre}' WHERE IdCliente = {pC.IdCliente}");
        }

        public List<ClsCliente> ObtenerClientes()
        {
            //string query = "SELECT * FROM Cliente";
            //DataTable TablaClientes = bd.RetornaTabla(query);
            //List<ClsCliente> LC = new List<ClsCliente>();

            //foreach (DataRow fila in TablaClientes.Rows)
            //{
            //    ClsCliente C = new ClsCliente();
            //    C.IdCliente = int.Parse(fila["IdCliente"].ToString());
            //    C.Apellido = fila["Apellido"].ToString();
            //    C.Nombre = fila["Nombre"].ToString();

            //    LC.Add(C);
            //}
            List<ClsCliente> LC = new List<ClsCliente>();
            DataSet ds = bd.RetornaDataSet();

            foreach (DataRow F in ds.Tables["Cliente"].Rows)
            {
                ClsCliente C = new ClsCliente();
                C.IdCliente = int.Parse(F["IDCliente"].ToString());
                C.Nombre = F["Nombre"].ToString();
                C.Apellido = F["Apellido"].ToString();
                LC.Add(C);
            }
               
            return LC;

        }
    }
}
