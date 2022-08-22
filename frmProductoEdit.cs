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
    public partial class frmProductoEdit : Form
    {
        string cadenaConexion = "server=localhost; database=Parcial; Integrated Security=true";
        public frmProductoEdit()
        {
            InitializeComponent();
        }

        private void frmProductoEdit_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void cargarDatos()
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();

                // CARGAR DATOS DE CATEGORIA
                var sql = "SELECT * FROM Categoria";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            Dictionary<string, string> CategoriaSource = new Dictionary<string, string>();
                            while (lector.Read())
                            {
                                CategoriaSource.Add(lector[0].ToString(), lector[2].ToString());
                            }
                            cboCategoria.DataSource = new BindingSource(CategoriaSource, null);
                            cboCategoria.DisplayMember = "Value";
                            cboCategoria.ValueMember = "Key";
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double pMax = double.Parse(txtPrecio.Text);
            if (pMax > 2500)
            {
                MessageBox.Show("El precio sobrepasa los limites", "Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int sMin = int.Parse(txtStock.Text);
            if (sMin < 6)
            {
                MessageBox.Show("El stock no es suficiente", "Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }        
    } 
}
