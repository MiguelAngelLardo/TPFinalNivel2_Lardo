using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Presentacion
{
    public partial class frmAgregarModificar : Form
    {

        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAgregarModificar()
        {
            InitializeComponent();
        } //Esto esto es para Agregar

        public frmAgregarModificar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }// Este es para Modificar

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio negocio = new ArticuloNegocio();

           // articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
            //articulo.Marca = (Marca)cboMarca.SelectedItem;
            //articulo.ImagenUrl = txtUrlImagen.Text;

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
                //articulo.Precio = Decimal.Parse(txtPrecio.Text);

                decimal precio;
                if (Decimal.TryParse(txtPrecio.Text, out precio))
                {
                    articulo.Precio = precio;
                    lblSoloPrecio.Visible = false;
                }
                else
                {
                    lblSoloPrecio.Visible = true;
                    txtPrecio.Text = "";                    
                    return;
                }

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

                //Guardo imagen si la levanto localmente:
                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))   
                 File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folfer"] + archivo.SafeFileName);

                Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAgregarModificar_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "descripcionMarca";




                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo.ToString(); // si chilla ver q pasa sacando ToString
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedValue = articulo.Marca.ToString();
                    cboMarca.SelectedValue = articulo.Marca.Id; 
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtPrecio.Text = articulo.Precio.ToString();
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg; |png|*.png";
            archivo.ShowDialog();
            if (archivo.ShowDialog() == DialogResult.OK)
            {
              txtUrlImagen.Text = archivo.FileName;
              cargarImagen(archivo.FileName);



            }
        }

       

        




    }
}
