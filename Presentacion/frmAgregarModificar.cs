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

namespace Presentacion
{
    public partial class frmAgregarModificar : Form
    {

        private Articulo articulo = null;
        public frmAgregarModificar()
        {
            InitializeComponent();
        }

        public frmAgregarModificar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Articulo art = new Articulo(); esto habbria q borrarlo
            ArticuloNegocio negocio = new ArticuloNegocio();

            articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
            articulo.Marca = (Marca)cboMarca.SelectedItem;
            articulo.ImagenUrl = txtUrlImagen.Text;

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Precio = int.Parse(txtPrecio.Text);

                if (articulo.Id != 0)//Si es distinto de cero asumimos que ya existe en la base de datos, por lo tanto llamamos al metodo "modificar".
                                     //Caso contrario, llamamos al metodo "crear", porque no existe
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }



                Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void frmAgregarModificar_Load(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                cboMarca.DataSource = articuloNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = articuloNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo.ToString(); // si chilla ver q pasa sacando ToString
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;




                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {

            try
            {
                pbxArticuloAgregar.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticuloAgregar.Load("https://www.ncenet.com/wp-content/uploads/2020/04/No-image-found.jpg");
            }
        }
    }
}
