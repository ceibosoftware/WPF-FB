﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Lógica de interacción para WindowAgregarNCRemito.xaml
    /// </summary>
    public partial class WindowAgregarNCRemito : Window
    {

        CRUD conexion = new CRUD();
        DataTable productos;
        public List<Producto> productosparametro = new List<Producto>();
        public List<Producto> itemsNC = new List<Producto>();
        public int idRemito;
        public int idNotaCred;
        public int id;
        public WindowAgregarNCRemito()
        {
            InitializeComponent();
            loadLtsRemitos();
            loadProductosRemitos();
            LoadDgvNCRemito();
            dgvProductosNCRemito.IsReadOnly = true;
            DgvProductosRemitos.IsReadOnly = true;
        }

        public WindowAgregarNCRemito(List<Producto> ProdAmodificar, int idremito, int idNotaCredito)
        {
            InitializeComponent();
            loadLtsRemitos(idremito);
            loadProductosRemitos();
            itemsNC = ProdAmodificar;
            LoadDgvNCRemito();
            ltsRemitos.IsEnabled = false;
            idNotaCred = idNotaCredito;

        }

        public void loadLtsRemitos()
        {
            String consulta = "select * from remito";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }

        public void loadLtsRemitos(int idr)
        {
            String consulta = "select * from remito  WHERE idremitos = '" + idr + "' ";
            conexion.Consulta(consulta, tabla: ltsRemitos);
            ltsRemitos.DisplayMemberPath = "numeroRemito";
            ltsRemitos.SelectedValuePath = "idremitos";
            ltsRemitos.SelectedIndex = 0;
        }

        private void ltsRemitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Datos Remito

            String sql = "Select  t3.nombre,t1.fecha ,t2.idOrdenCompra from remito t1 , ordencompra t2, proveedor t3 where idremitos = @valor and t1.FK_idOC = t2.idOrdenCompra and t2.FK_idProveedor = t3.idProveedor";
            DataTable datos = conexion.ConsultaParametrizada(sql, ltsRemitos.SelectedValue);
     
            //consulta productos
            String consulta = "  SELECT t2.nombre , t1.CrNotaCredito, t2.idProductos from productos_has_remitos t1 inner join productos t2  on t1.FK_idProducto = t2.idProductos where t1.FK_idRemito = @valor";
            productos = conexion.ConsultaParametrizada(consulta, ltsRemitos.SelectedValue);
            productosparametro.Clear();
            for (int i = 0; i < productos.Rows.Count; i++)
            {
                productosparametro.Add(new Producto(productos.Rows[i].ItemArray[0].ToString(), (int)productos.Rows[i].ItemArray[2], (int)productos.Rows[i].ItemArray[1]));
            }

            DgvProductosRemitos.Items.Refresh();
        }

        private void loadProductosRemitos()
        {
            DgvProductosRemitos.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            DgvProductosRemitos.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            DgvProductosRemitos.Columns.Add(textColumn2);
            DgvProductosRemitos.ItemsSource = productosparametro;
        }

        private void loadProductosNCRemitos()
        {
            dgvProductosNCRemito.AutoGenerateColumns = false;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Nombre";
            textColumn.Binding = new Binding("nombre");
            dgvProductosNCRemito.Columns.Add(textColumn);
            DataGridTextColumn textColumn2 = new DataGridTextColumn();
            textColumn2.Header = "Cantidad";
            textColumn2.Binding = new Binding("cantidad");
            dgvProductosNCRemito.Columns.Add(textColumn2);
            dgvProductosNCRemito.ItemsSource = productosparametro;
        }

        private void btnAgregarProductoNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existe = false;
                Producto prod = DgvProductosRemitos.SelectedItem as Producto;

                id = prod.id;
                if (prod.cantidad > 0)
                {
                    var newW = new WindowAgregarProductoFactura();
                    for (int i = 0; i < itemsNC.Count; i++)
                    {
                        if (itemsNC[i].nombre == prod.nombre)
                        {
                            existe = true;
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                    if (prod.cantidad >= 1 && !existe)
                    {

                        newW.txtCantidad.Text = prod.cantidad.ToString();
                        newW.can = prod.cantidad;
                        newW.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("El producto ya se agrego");
                    }


                    if (newW.DialogResult == true)
                    {
                        if (int.Parse(newW.txtCantidad.Text) > 0)
                        {

                            Producto productoremito = new Producto(prod.nombre, prod.id, int.Parse(newW.txtCantidad.Text));
                            itemsNC.Add(productoremito);
                            dgvProductosNCRemito.Items.Refresh();
                            prod.cantidad = prod.cantidad - int.Parse(newW.txtCantidad.Text);
                            DgvProductosRemitos.Items.Refresh();
                  
                        }
                        else
                        {
                            MessageBox.Show("La cantidad no puede ser cero");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ya se entregaron todas las ordenes de compra de este producto");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Seleccione un producto para agregar");

            }
        }

        public void LoadDgvNCRemito()
        {
            dgvProductosNCRemito.ItemsSource = itemsNC;
        }


        public Boolean Valida()
        {

            if (dgvProductosNCRemito.HasItems == false)
            {
                MessageBox.Show("Agregue productos a la Nota de Crédito");
                return false;
            }
            else if (DgvProductosRemitos.HasItems == false)
            {
                MessageBox.Show("Agregue productos a la Nota de Crédito");
                return false;
            }
            else
            {
                return true;
            }


        }
        private void btnEliminarProductoNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Producto prod = dgvProductosNCRemito.SelectedItem as Producto;
                for (int i = 0; i < productosparametro.Count; i++)
                {
                    if (productosparametro[i].nombre == prod.nombre)
                    {
                        productosparametro[i].cantidad += prod.cantidad;
                    }

                }
                itemsNC.Remove(prod);
                dgvProductosNCRemito.Items.Refresh();
                DgvProductosRemitos.Items.Refresh();
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Seleccione un producto para eliminar");

            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            idRemito = (int)ltsRemitos.SelectedValue;
            if (Valida())
            {
                DialogResult = true;
            }
      
        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String consulta = " Select * from remito t1 where t1.fecha = @valor ";
                DataTable OCFecha = conexion.ConsultaParametrizada(consulta, dpFecha.SelectedDate);
                ltsRemitos.ItemsSource = OCFecha.AsDataView();
                ltsRemitos.DisplayMemberPath = "numeroRemito";
                ltsRemitos.SelectedValuePath = "idremitos";
                ltsRemitos.SelectedIndex = 0;
            }
            catch (NullReferenceException)
            {


            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}