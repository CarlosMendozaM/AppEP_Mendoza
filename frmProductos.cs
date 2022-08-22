using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EP_MendozaMalpica
{
    public partial class frmProductos : Form
    {
        DataTable dtProducto;
        string cadenaConexion = "Server=localhost; DataBase=Parcial; Integrated security=true";
        public frmProductos()
        {
            InitializeComponent();
            dtProducto = new DataTable();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            var frm = new frmProductoEdit();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var nombre = ((TextBox)frm.Controls["txtNombre"]).Text;
                var marca = ((TextBox)frm.Controls["txtMarca"]).Text;
                var precio = ((TextBox)frm.Controls["txtPrecio"]).Text;
                var stock = ((TextBox)frm.Controls["txtStock"]).Text;
                var categoria = ((ComboBox)frm.Controls["cboCategoria"]).SelectedValue.ToString();

                using (var conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    var sql = "INSERT INTO Producto (Nombre ,Marca, Precio, " +
                        "Stock, IdCategoria) " +
                        "VALUES(@nombre, @marca, @precio, @stock, " +
                        "@categoria)";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@marca", marca);
                        comando.Parameters.AddWithValue("@precio", precio);
                        comando.Parameters.AddWithValue("@stock", stock);
                        comando.Parameters.AddWithValue("@categoria", categoria);
                        int resultado = comando.ExecuteNonQuery();
                        if (resultado > 0)
                        {
                            MessageBox.Show("El producto esta registrado", "Sistemas",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cargarDatos();
                        }

                        else
                        {
                            MessageBox.Show("El producto no ha sido registrado", "Sistemas",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void cargarDatos()
        {
            dgvListado.Rows.Clear();
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var sql = "SELECT IdProducto, A.Nombre, Marca, B.Nombre AS Categoria, Precio FROM Producto A INNER JOIN Categoria B ON A.IdCategoria = B.IdCategoria";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                dgvListado.Rows.Add(lector[0], lector[1], lector[2], lector[3], lector[4]);
                            }
                        }
                    }
                }
            }
        }

        private int getId()
        {
            try
            {
                //¿Que queremos procesar?
                DataGridViewRow filaActual = dgvListado.CurrentRow;
                if (filaActual == null)
                {
                    return 0;
                }
                return int.Parse(dgvListado.Rows[filaActual.Index].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                //¿Que hacer en caso de error?
                return 0;
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            int id = getId();
            if (id > 0)
            { 
                var frm = new frmProductoEdit();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var nombre = ((TextBox)frm.Controls["txtNombre"]).Text = dgvListado.SelectedCells[1].Value.ToString();
                var marca = ((TextBox)frm.Controls["txtMarca"]).Text = dgvListado.SelectedCells[2].Value.ToString();
                var categoria = ((ComboBox)frm.Controls["cboCategoria"]).Text = dgvListado.SelectedCells[3].Value.ToString();
                var precio = ((TextBox)frm.Controls["txtPrecio"]).Text = dgvListado.SelectedCells[4].Value.ToString();
                var stock = ((TextBox)frm.Controls["txtStock"]).Text;
                    {
                        using (var conexion = new SqlConnection(cadenaConexion))
                        {
                            conexion.Open();
                            var sql = "UPDATE Producto SET  Nombre = @nombre, Marca = @marca, Precio = @precio," +
                                "Stock = @stock, IdCategoria = @categoria WHERE IdProducto = @idProducto ";
                            using (var comando = new SqlCommand(sql, conexion))
                            {
                                comando.Parameters.AddWithValue("@idProducto", id);
                                comando.Parameters.AddWithValue("@nombre", nombre);
                                comando.Parameters.AddWithValue("@marca", marca);
                                comando.Parameters.AddWithValue("@precio", precio);
                                comando.Parameters.AddWithValue("@stock", stock);
                                comando.Parameters.AddWithValue("@categoria", categoria);
                                int resultado = comando.ExecuteNonQuery();
                                if (resultado > 0)
                                {
                                    MessageBox.Show("El producto esta registrado", "Sistemas",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    cargarDatos();
                                }

                                else
                                {
                                    MessageBox.Show("El producto no ha sido registrado", "Sistemas",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            int id = getId();
            if (id > 0)
            {
                    using (var conexion = new SqlConnection(cadenaConexion))
                    {
                    conexion.Open();
                    var sql = "DELETE FROM Producto WHERE IdProducto = @idProducto";
                    using (var comando = new SqlCommand(sql, conexion))
                        {
                        comando.Parameters.AddWithValue("@idProducto", id);
                        int resultado = comando.ExecuteNonQuery();
                        if (resultado > 0)
                            {
                                MessageBox.Show("El producto esta eliminado", "Sistemas",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                cargarDatos();
                            }

                            else
                            {
                                MessageBox.Show("El producto no ha sido eliminado", "Sistemas",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                    }
                }
        }
    }
}

