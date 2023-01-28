﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
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
            
            
            /*switch (dgvArticulo.SelectedRows.Count)
            {
                case 0:
                  btnModificar.Enabled = false;
                  MessageBox.Show("No puedes MODIFICAR algo que no SELECCIONASTE. Gracias.");
                  break;

                 case 1:
                   btnModificar.Enabled = true;
                   Articulo seleccionado;
                   seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                   frmAgregarModificar modificar = new frmAgregarModificar(seleccionado);
                   modificar.ShowDialog();
                   cargar();
                   break;

            }*/



           

            if (!dgvArticulo.SelectedRows.Count.Equals(0))
            {
                
            btnModificar.Enabled = true;
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;

            frmAgregarModificar modificar = new frmAgregarModificar(seleccionado);
            modificar.ShowDialog();
            cargar();     
            }
            else
                 //if (dgvArticulo.SelectedRows.Count == 0)
            {
                btnModificar.Enabled = false;
                MessageBox.Show("No puedes MODIFICAR algo que no SELECCIONASTE. Gracias.");
            }


        }
       
        




    private void btnEliminarfisico_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negoc = new ArticuloNegocio();
            Articulo seleccionado;

            if (dgvArticulo.SelectedRows.Count == 0)
            {
                btnEliminarfisico.Enabled = false;
                MessageBox.Show("No puedes ELIMINAR algo que no SELECCIONASTE. Gracias.");
            }
            else
            {
                btnEliminarfisico.Enabled = true;
                try
                {
                    DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes) //el "si"se guarda en variable respuesta. si es SI entra en IF. 
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


        }


        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloNegocio neg = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;//este return cancela la ejecucion de este evento

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

        

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;

            if (filtro.Length >= 2)
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
            if (opcion == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Descripciom")
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



        private void btnFiltroRapido_Click(object sender, EventArgs e)//super filtro
        {
             List<Articulo> listaFiltrada;
             Regex regex = new Regex(@"^[0-9]*([\.|\,]?[0-9]{0,2})?$");

             string filtro = txtFiltroRapido.Text;         


           if (filtro != "")
           {
               listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Marca.descripcionMarca.ToUpper().Contains(filtro.ToUpper()) || x.Precio.ToString().Contains(filtro));

           }                      
           else
           {
               listaFiltrada = listaArticulo;
           }




           dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = listaFiltrada;
            ocultarColumnas();
        }


   

         private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            return false;

        } // Validacion para campo y criterio       

        
    }
}
