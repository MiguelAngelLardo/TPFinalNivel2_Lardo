
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio; 

namespace presentacion
{
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulo;   

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulo = negocio.listar(); 
            dgvArticulo.DataSource = listaArticulo;
            dgvArticulo.Columns["ImagenUrl"].Visible = false; 
            cargarImagen(listaArticulo[0].ImagenUrl);

        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem; 
            cargarImagen(seleccionado.ImagenUrl);
        }

        private void cargarImagen(string imagen)
        {
           
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://www.ncenet.com/wp-content/uploads/2020/04/No-image-found.jpg");
            }

        }

    }
}
