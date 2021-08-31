using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ABMMySqlCloud
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ColorGrilla1(DataGridView pD) // FUNCION PARA CONFIGURAR LAS GRILLAS
        {
            pD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            pD.MultiSelect = false;
            pD.EnableHeadersVisualStyles = false; //Anula el estilo visual de Windows
            pD.AlternatingRowsDefaultCellStyle.BackColor = Color.PeachPuff; // Color de la alternancia
            pD.ColumnHeadersDefaultCellStyle.BackColor = Color.Maroon; // Encabezado de Columnas
            pD.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Color de Fuente del encabezado
            pD.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.Maroon;
            pD.RowHeadersDefaultCellStyle.BackColor = Color.IndianRed; // Color del encabezado de Fila     
            pD.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Firebrick;
            pD.DefaultCellStyle.SelectionBackColor = Color.Firebrick; // Color de fondo de Selección de la fila
            pD.DefaultCellStyle.SelectionForeColor = Color.White; // Color de la fuente de la fila seleccionada
            pD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; // Celdas Autoajustables
            pD.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //Alineación del Texto
            pD.BackgroundColor = Color.PeachPuff; // Color de Fondo     
            pD.RowHeadersWidth = 30;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ColorGrilla1(GrillaClientes);
            MostrarClientes();
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            ClsCliente C = new ClsCliente();

            C.Nombre = Interaction.InputBox("Nombre: ", "Nombre");
            C.Apellido = Interaction.InputBox("Apellido: ", "Apellido");

            C.AgregarCliente(C);
            MostrarClientes();
        }

        public void MostrarClientes()
        {
            ClsCliente C = new ClsCliente();
            GrillaClientes.DataSource = null;
            GrillaClientes.DataSource = C.ObtenerClientes();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            try
            {
                ClsCliente C = GrillaClientes.SelectedRows[0].DataBoundItem as ClsCliente;
                C.EliminarCliente(C);
                MostrarClientes();
            }
            catch (Exception)
            {

                MessageBox.Show("No hay Clinetes");
            }
            
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            try
            {
                ClsCliente C = GrillaClientes.SelectedRows[0].DataBoundItem as ClsCliente;

                C.Nombre = Interaction.InputBox("Nombre: ", "Nombre", C.Nombre);
                C.Apellido = Interaction.InputBox("Apellido: ", "Apellido", C.Apellido);
                C.ModificarCliente(C);
                MostrarClientes();
            }
            catch (Exception)
            {

                MessageBox.Show("No hay Clinetes");
            }
          
        }
    }
}
