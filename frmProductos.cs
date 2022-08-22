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
                var sql = "SELECT A.Nombre, Marca, B.Nombre AS Categoria, Precio FROM Producto A INNER JOIN Categoria B ON A.IdCategoria = B.IdCategoria";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                dgvListado.Rows.Add(lector[0], lector[1], lector[2], lector[3]);
                            }
                        }
                    }
                }
            }
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            var frm = new frmProductoEdit();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var nombre = ((TextBox)frm.Controls["txtNombre"]).Text;
                var marca = ((TextBox)frm.Controls["txtMarca"]).Text;
                var precio = ((TextBox)frm.Controls["txtPrecio"]).Text;
                var stock = ((TextBox)frm.Controls["txtStock"]).Text;
                var categoria = ((ComboBox)frm.Controls["cboCategoria"]).ToString();
                {
                    using (var conexion = new SqlConnection(cadenaConexion))
                    {
                        conexion.Open();
                        var sql = "UPDATE Producto SET  Nombre='Mouse' WHERE Nombre = @nombre";
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
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
                var filaActual = dgvListado.CurrentRow;
                if (filaActual != null)
                {
                    using (var conexion = new SqlConnection(cadenaConexion))
                    {
                    var sql = "DELETE FROM Producto WHERE Nombre ='Mouse'";
                    var frm = new frmProductoEdit();
                    var nombre = ((TextBox)frm.Controls["txtNombre"]).Text;
                    conexion.Open();
                        using (var comando = new SqlCommand(sql, conexion))
                        {
                        comando.Parameters.AddWithValue("@nombre", nombre);
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

