using GESTPRO.domain;
using GESTPRO.view;
using Mysqlx.Crud;
using Mysqlx.Cursor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GESTPRO
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        /// <summary>
        /// 
        ///  PARA QUE FUNCIONE LA APLICACION HAY QUE HACER EL INSERT INICIAL DE ESTOS DATOS:
        /// 
        /// 
        /// INSERT INTO mydb.rol (NombreRol,DescRol)values('PROGRAMADOR JUNIOR','DESC')
        /// INSERT INTO mydb.rol(NombreRol, DescRol) values('PROGRAMADOR','DESC')
        /// INSERT INTO mydb.rol(NombreRol, DescRol) values('PROGRAMADOR SENIOR','DESC')
        /// INSERT INTO mydb.rol(NombreRol, DescRol) values('JEFE PROYECTO','DESC')
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        private List<Proyecto> lstProyectos;
        private Proyecto proyecto;
        private Usuario usuario;
        private Empleado emp;
        private Holiday holiday;
        private List<ProyectoEmpleado> proyectoEmpleados;
        private List<Usuario> usuarios;
        private List<Empleado> empleados;
        private List<Holiday> holidays;
        public MainWindow()
        {
            InitializeComponent();
            proyectoEmpleados = new List<ProyectoEmpleado>();
            
            tabUsuarios.IsEnabled = false;
            tabEmpleados.IsEnabled = false;
            tabEstadisticas.IsEnabled = false;
            tabProyecto.IsEnabled = false;
            tabGestion.IsEnabled = false;
            proyecto = new Proyecto();
            emp = new Empleado();
            dataGrid.ItemsSource = null;
            lstProyectos = proyecto.gListaProyectos();
            usuario = new Usuario();
            usuarios = usuario.gListaUsuarios();
            empleados = emp.listaEmpleados();
            holiday = new Holiday();
            dataGridEmpleado.ItemsSource = empleados;
            dataGridEmpleado.Items.Refresh();
            cbRegistrado.IsEnabled = false;
            cbNoRegistrado.IsEnabled = false;
            dataGridUsers.ItemsSource = usuarios;
            dataGrid.ItemsSource = lstProyectos;
            dataGrid.Items.Refresh();
            txtRolEmple.Items.Add("PROGRAMADOR JUNIOR");
            txtRolEmple.Items.Add("PROGRAMADOR");
            txtRolEmple.Items.Add("PROGRAMADOR SENIOR");
            txtRolEmple.Items.Add("JEFE PROYECTO");
            cbEmp.ItemsSource = empleados;
            cbPro.ItemsSource = lstProyectos;
            txtRolEmple.Items.Refresh();
            generarInformesComboBox();
            start();

        }
        private void start()
        {
            txtCodPro.Text = string.Empty;
            txtFechaFin.Text = string.Empty;
            txtFechaIn.Text = string.Empty;
            txtNombrePro.Text = string.Empty;
            dataGrid.SelectedItems.Clear();
            btnAniadir.IsEnabled = true;
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
        }


        private void tabProyecto_GotFocus(object sender, RoutedEventArgs e)
        {
            tabProyecto.IsEnabled = true;
        }

        private void btnProyectos_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                Proyecto proyecto = (Proyecto)dataGrid.SelectedItems[0];
                txtCodPro.Text = proyecto.codProy.ToString();
                txtFechaIn.Text = proyecto.fechaInicio.ToString();
                txtFechaFin.Text = proyecto.fechaFin.ToString();
                txtNombrePro.Text = proyecto.nombre;
                btnAniadir.IsEnabled = false;
                btnEliminar.IsEnabled = true;
                btnModificar.IsEnabled = true;
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Quiere modificar este proyecto", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Proyecto proyecto = (Proyecto)dataGrid.SelectedItems[0];

                proyecto.codProy = txtCodPro.Text;
                proyecto.fechaFin = txtFechaFin.Text;
                proyecto.fechaInicio = txtFechaIn.Text;
                proyecto.nombre = txtNombrePro.Text;
                proyecto.modificar();
                dataGrid.Items.Refresh();
                start();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Quiere eliminar este proyecto", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Proyecto proyecto = (Proyecto)dataGrid.SelectedItems[0];
                List<Proyecto> lst = (List<Proyecto>)dataGrid.ItemsSource;
                lst.Remove(proyecto);
                dataGrid.Items.Refresh();
                dataGrid.ItemsSource = lst;
                proyecto.eliminar();
                start();
            }
        }

        private void btnAniadir_Click(object sender, RoutedEventArgs e)
        {
            // Validación de campos vacíos
            if (string.IsNullOrEmpty(txtCodPro.Text) || string.IsNullOrEmpty(txtNombrePro.Text) ||
                string.IsNullOrEmpty(txtFechaIn.Text) || string.IsNullOrEmpty(txtFechaFin.Text))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verificar si el proyecto ya existe
            if (proyectoExists(txtCodPro.Text, txtNombrePro.Text, txtFechaIn.Text, txtFechaFin.Text))
            {
                MessageBox.Show("El proyecto ya existe en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Agregar el nuevo proyecto
                Proyecto nuevoProyecto = new Proyecto(txtCodPro.Text, txtNombrePro.Text, txtFechaIn.Text, txtFechaFin.Text);
                lstProyectos.Add(nuevoProyecto);
                cbPro.ItemsSource=lstProyectos;
                cbPro.Items.Refresh();
                dataGrid.Items.Refresh(); // Refrescar el DataGrid
            }
            start(); // Limpiar los campos y restablecer botones
        }
        private bool proyectoExists(string cod, string nombre, string fechaI, string fechaFin)
        {
            foreach (Proyecto p in lstProyectos)
            {
                if (p.nombre == nombre && p.codProy == cod && p.fechaFin == fechaFin && p.fechaInicio == fechaI)
                {
                    return true;
                }
            }
            return false;
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {

            string textoBusqueda = txtBuscar.Text.ToLower();
            var proyectosFiltrados = lstProyectos.Where(p =>
                (!string.IsNullOrEmpty(p.codProy) && p.codProy.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.nombre) && p.nombre.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.fechaInicio) && p.fechaInicio.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.fechaFin) && p.fechaFin.ToLower().Contains(textoBusqueda))
            ).ToList();
            // Actualizar el DataGrid con los resultados filtrados
            dataGrid.ItemsSource = proyectosFiltrados;
        }


        private void btnCargarDatos_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                Proyecto p = proyecto.getProyectoRandom();
                p.insertar(); // Lo inserta

            }
            //Actualizo base de datos, primero leo
            dataGrid.ItemsSource = lstProyectos;
            lstProyectos = proyecto.gListaProyectos();
            cbPro.ItemsSource = lstProyectos;
            dataGrid.ItemsSource = lstProyectos;
            //List<Proyecto> lst = (List<Proyecto>)dataGrid.ItemsSource;
            cbPro.Items.Refresh();
            dataGrid.Items.Refresh();

        }

        private void tabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // Iteramos solo sobre los elementos que son TabItem
            foreach (object item in tabControl.Items)
            {
                if (item is TabItem tabItem)
                {
                    // Cambiar el fondo a un rosa oscuro cuando NO está seleccionado
                    tabItem.Background = new SolidColorBrush(Color.FromRgb(215, 47, 121)); // Rosa oscuro
                    tabItem.Foreground = Brushes.White; // Texto blanco por defecto
                }
            }

            // Cambiar el color de fondo y texto cuando se selecciona un TabItem
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                selectedTab.Background = new SolidColorBrush(Color.FromRgb(240, 160, 196)); // Rosa más claro
                selectedTab.Foreground = Brushes.Black; // Texto negro para el tab seleccionado
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 5;
        }

        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsu = txtUsuario.Text;
            string password = txtPass.Password;
            Usuario usu = new Usuario(nombreUsu, password);
            usu.insertar();
            usuarios = usu.gListaUsuarios();
            dataGridUsers.ItemsSource = usuarios;
            dataGridUsers.Items.Refresh();
        }

        private void dataGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridUsers.SelectedItems.Count > 0)
            {
                Usuario usu = (Usuario)dataGridUsers.SelectedItem;
                txtUsuario.Text = usu.NombreUsuario;
            }
        }

        private void btnDelUser_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Quiere eliminar este usuario", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Usuario usuario = (Usuario)dataGridUsers.SelectedItems[0];
                List<Usuario> lst = (List<Usuario>)dataGridUsers.ItemsSource;
                lst.Remove(usuario);
                dataGridUsers.Items.Refresh();
                dataGridUsers.ItemsSource = lst;
                usuario.eliminar();
                txtUsuario.Text = "";
            }
        }

        private void btnActPass_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem != null)
            {
                Usuario usuario = (Usuario)dataGridUsers.SelectedItems[0];
                List<Usuario> lst = (List<Usuario>)dataGridUsers.ItemsSource;
                usuario.Contrasena = txtPass.Password;
                usuario.modificar();
                dataGridUsers.Items.Refresh();
            }
        }

        private void btnAniadirEmp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombreEmple.Text) || string.IsNullOrEmpty(txtApellidosEmple.Text) || string.IsNullOrEmpty(txtRolEmple.Text) || string.IsNullOrEmpty(txtCsrEmple.Text))
            {
                MessageBox.Show("Por favor, rellene todos los campos");
            }
            else
            {
                string nombre = txtNombreEmple.Text;
                string apellidos = txtApellidosEmple.Text;
                int rol = txtRolEmple.SelectedIndex + 1;
                double csr = Convert.ToDouble(txtCsrEmple.Text);
                Empleado emp = new Empleado(nombre, apellidos, rol, csr);
                emp.insertar();
                limpiarCampos();
                empleados.Add(emp);
                dataGridEmpleado.Items.Refresh();
            }
        }
        private void limpiarCampos()
        {
            txtNombreEmple.Text = "";
            txtApellidosEmple.Text = "";
            txtRolEmple.Text = "";
            txtCsrEmple.Text = "";
        }

        private void btnModEmp_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = (Empleado)dataGridEmpleado.SelectedItem;
            emp.Nombre = txtNombreEmple.Text;
            emp.Apellidos = txtApellidosEmple.Text;
            emp.Rol = txtRolEmple.SelectedIndex + 1;
            emp.Csr = Convert.ToDouble(txtCsrEmple.Text);
            emp.modificar();
            dataGridEmpleado.Items.Refresh();
        }

        private void dataGridEmpleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Empleado emp = (Empleado)dataGridEmpleado.SelectedItem;
            txtApellidosEmple.Text = emp.Apellidos;
            txtNombreEmple.Text = emp.Nombre;
            txtRolEmple.SelectedItem = emp.Rol;
            txtCsrEmple.Text = emp.Csr.ToString();
            if (string.IsNullOrEmpty(emp.NombreUsuario))
            {
                cbNoRegistrado.IsChecked = true;
                cbRegistrado.IsChecked = false;
            }
            else
            {
                cbNoRegistrado.IsChecked = false;
                cbRegistrado.IsChecked = true;
            }
            cargarPagina(emp);
        }

        private void btnDelEmp_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = (Empleado)dataGridEmpleado.SelectedItem;
            emp.eliminar();
            empleados.Remove(emp);
            dataGridEmpleado.Items.Refresh();
        }
        private void cargarPagina(Empleado emp)
        {
            if (cbNoRegistrado.IsChecked == true)
            {
                PagUsuario pag= new PagUsuario(emp,usuarios);
                frame.Navigate(pag);
            }
            else
            {
                frame.Navigate(null);
            }
        }

        private void txtBuscarEmple_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusqueda= txtBuscarEmple.Text;
            var proyectosFiltrados = empleados.Where(p =>
                (p.Id.Equals(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.NombreUsuario) && p.NombreUsuario.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.Apellidos) && p.Apellidos.ToLower().Contains(textoBusqueda))
            ).ToList();
            // Actualizar el DataGrid con los resultados filtrados
            dataGridEmpleado.ItemsSource = proyectosFiltrados;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
        }

        private async void btnImputar_Click(object sender, RoutedEventArgs e)
        {
            
            DateTime fecha= (DateTime)txtFecha.SelectedDate;
            if (await ComprobarFecha(fecha)==false)
            {
                Empleado emp = (Empleado)cbEmp.SelectedItem;
                Proyecto pro = (Proyecto)cbPro.SelectedItem;
                int horas = Convert.ToInt32(txtHoras.Text);
                ProyectoEmpleado proyectoEmpleado = new ProyectoEmpleado(pro.codProy, pro.nombre, emp.Nombre, horas, emp.Csr, fecha);
                proyectoEmpleado.idEmpleado = emp.Id;
                proyectoEmpleado.idProyecto = pro.Id;
                proyectoEmpleado.insertar();
                proyectoEmpleados.Add(proyectoEmpleado);
                dataGridEco.ItemsSource = proyectoEmpleados;
                dataGridEco.Items.Refresh();
                
            }
            else
            {
                MessageBox.Show("No se puede añadir en dia festivo, seleccione otra fecha");
            }
            
        }
        private async Task<bool> ComprobarFecha(DateTime fecha)
        {
            bool festivo = false;
            int dia = fecha.Day;
            int mes=fecha.Month;
            int anio=fecha.Year;
            var holidays= await holiday.GetHolidays("ES", anio, mes, dia);
            if (holidays.Count > 0)
            {
                festivo=true;
            }
            return festivo;
        }

        private void btnDelEco_Click(object sender, RoutedEventArgs e)
        {
            ProyectoEmpleado proyectoEmpleado = dataGridEco.SelectedItem as ProyectoEmpleado;
            proyectoEmpleados.Remove(proyectoEmpleado);
            dataGridEco.Items.Refresh();
        }

        private async void btnModEco_Click(object sender, RoutedEventArgs e)
        {
            DateTime fecha=(DateTime)txtFecha.SelectedDate;
            if (await ComprobarFecha(fecha) == false)
            {
                ProyectoEmpleado proyectoEmpleado = dataGridEco.SelectedItem as ProyectoEmpleado;
                proyectoEmpleado.CodigoProyecto = ((Proyecto)cbPro.SelectedItem).codProy;
                proyectoEmpleado.NombrePro = ((Proyecto)cbPro.SelectedItem).nombre;
                proyectoEmpleado.NombreEmp = ((Empleado)cbEmp.SelectedItem).Nombre;
                proyectoEmpleado.Fecha = (DateTime)txtFecha.SelectedDate;
                proyectoEmpleado.Csr = ((Empleado)cbEmp.SelectedItem).Csr;

                proyectoEmpleado.Horas = Convert.ToInt32(txtHoras.Text);
                proyectoEmpleado.Coste = proyectoEmpleado.Csr * proyectoEmpleado.Horas;
                dataGridEco.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No se puede modificar en dia festivo, seleccione otra fecha");
            }

        }

        /// <summary>Handles the SelectionChanged event of the dataGridEco control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        private void dataGridEco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProyectoEmpleado proyectoEmpleado = dataGridEco.SelectedItem as ProyectoEmpleado;
            cbPro.SelectedItem = lstProyectos.FirstOrDefault(p => p.codProy == proyectoEmpleado.CodigoProyecto);
            cbEmp.SelectedItem = empleados.FirstOrDefault(emp => emp.Nombre == proyectoEmpleado.NombreEmp);
            txtFecha.SelectedDate = proyectoEmpleado.Fecha;
            txtHoras.Text = proyectoEmpleado.Horas.ToString();
        }

        /// <summary>Handles the TextChanged event of the txtBuscarEco control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs" /> instance containing the event data.</param>
        private void txtBuscarEco_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusqueda = txtBuscarEco.Text.ToLower();
            var proyectosFiltrados = proyectoEmpleados.Where(p =>
                (!string.IsNullOrEmpty(p.CodigoProyecto) && p.CodigoProyecto.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.NombrePro) && p.NombrePro.ToLower().Contains(textoBusqueda)) ||
                (!string.IsNullOrEmpty(p.NombreEmp) && p.NombreEmp.ToLower().Contains(textoBusqueda))
            ).ToList();
            // Actualizar el DataGrid con los resultados filtrados
            dataGridEco.ItemsSource = proyectosFiltrados;
        }

        /// <summary>Handles the 2 event of the Button_Click control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
        }

        /// <summary>Handles the Click event of the btnGenerar control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            
            
            crViewer.ViewerCore.ReportSource = cbInformes.SelectedItem;

        }
        /// <summary>Generars the informes ComboBox.</summary>
        private void generarInformesComboBox()
        {
            List<ProyectoEmpleado> lista;
            ProyectoEmpleado proyectoEmpleado = new ProyectoEmpleado();
            lista = proyectoEmpleado.leer();
            DataTable tabla1;
            tabla1 = new DataTable("DataTable1");
            tabla1.Columns.Add("Proyecto");
            tabla1.Columns.Add("Mes/año");
            tabla1.Columns.Add("Coste Total");
            foreach (Proyecto p in lstProyectos)
            {
                double coste = 0;
                var proyectoEmpleados = lista.Where(pe => pe.idProyecto == p.Id);
                foreach (ProyectoEmpleado pe in proyectoEmpleados)
                {
                    coste += pe.Coste;
                }
                tabla1.Rows.Add(p.codProy, p.fechaInicio, coste);
            }
            InformeProyectoCoste report = new InformeProyectoCoste();
            report.Database.Tables["DataTable1"].SetDataSource((DataTable)tabla1);
            cbInformes.Items.Add(report);
            DataTable tablaPerfiles = new DataTable("DataTable1");
            tablaPerfiles.Columns.Add("Proyecto");
            tablaPerfiles.Columns.Add("Mes/año");
            tablaPerfiles.Columns.Add("Perfil de Empleado");
            tablaPerfiles.Columns.Add("Número de Personas");
            foreach (Proyecto p in lstProyectos)
            {
                var perfiles = lista
                    .Where(pe => pe.idProyecto == p.Id)
                    .GroupBy(pe => empleados.FirstOrDefault(e => e.Id == pe.idEmpleado)?.Rol)
                    .Select(g => new
                    {
                        Perfil = g.Key,
                        NumeroPersonas = g.Count()
                    });

                foreach (var perfil in perfiles)
                {
                    string rol=txtRolEmple.Items.GetItemAt((int)(perfil.Perfil - 1)).ToString();
                    tablaPerfiles.Rows.Add(p.codProy, p.fechaInicio,rol, perfil.NumeroPersonas);
                }
            }
            NumeroPerfilesProyecto informe = new NumeroPerfilesProyecto();
            informe.Database.Tables["DataTable1"].SetDataSource(tablaPerfiles);
            cbInformes.Items.Add(informe);
        }
    }
}
