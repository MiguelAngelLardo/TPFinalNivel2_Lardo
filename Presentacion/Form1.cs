
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using Presentacion;

namespace presentacion
{
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulo; //es privado para poder usar la lista en otros EVENTOS  

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
            cboCampo.Items.Add("Marca");
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulo = negocio.listar();
                dgvArticulo.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulo.Columns["Id"].Visible = false;
            dgvArticulo.Columns["ImagenUrl"].Visible = false;
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem; 
            cargarImagen(seleccionado.ImagenUrl);
            }
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarModificar agregar = new frmAgregarModificar();
            agregar.ShowDialog();
            cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;

            frmAgregarModificar modificar = new frmAgregarModificar(seleccionado);
            modificar.ShowDialog();
            cargar(); 
        }

        private void btnEliminarfisico_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negoc = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {    
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ( respuesta ==DialogResult.Yes) //el "si"se guarda en variable respuesta. si es SI entra en IF. 
                {
                seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                negoc.eliminar(seleccionado.Id);
                cargar();

                }

            }
            catch (Exception ex) 
            {
            MessageBox.Show(ex.ToString());                
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio neg = new ArticuloNegocio();
            try
            {
              string campo = cboCampo.SelectedItem.ToString();
              string criterio = cboCriterio.SelectedItem.ToString();
              string filtro = txtFiltroAvanzado.Text;
               dgvArticulo.DataSource = neg.filtrar(campo, criterio, filtro);


            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());                
            }
            

        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >=2 )
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.ToString().ToUpper().Contains(filtro.ToUpper()) || x.Marca.ToString().ToUpper().Contains(filtro.ToUpper()) || x.Precio.ToString().ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }


            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = listaArticulo;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
             string opcion = cboCampo.SelectedItem.ToString();
             if(opcion == "Nombre")
             {                
                 cboCriterio.Items.Clear();
                 cboCriterio.Items.Add("Comienza con");
                 cboCriterio.Items.Add("Termina con");
                 cboCriterio.Items.Add("Contiene");
             }
             else if(opcion == "Descripciom")
             {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
             }
             else
             {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }




        }

        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
