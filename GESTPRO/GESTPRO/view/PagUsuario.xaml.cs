using GESTPRO.domain;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GESTPRO.view
{
    /// <summary>
    /// Lógica de interacción para PagUsuario.xaml
    /// </summary>
    public partial class PagUsuario : Page
    {
        List<Usuario> usuarios;
        Empleado emp;
        public PagUsuario(Empleado emp,List<Usuario> users) { 
            InitializeComponent();
            this.emp = emp;
            this.usuarios = users;
            cbUsuarios.ItemsSource = usuarios;
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {

            string nombreUsuario=txtUsuario.Text;
            string password=txtPass.Password;
            Usuario usuario = new Usuario(nombreUsuario,password);
            if(usuarios.Where(u => u.NombreUsuario.Equals(nombreUsuario)).ToList().Any() ) 
            {
                usuario = (Usuario)cbUsuarios.SelectedItem;
                emp.NombreUsuario=usuario.NombreUsuario;
            }
            else
            {
                usuario.insertar();
                emp.NombreUsuario=usuario.NombreUsuario;
                List<Usuario> lista= usuario.gListaUsuarios();
                cbUsuarios.ItemsSource=lista;
                txtUsuario.Text = "";
                txtPass.Password = "";
                cbUsuarios.Items.Refresh();
            }
            emp.aniadirNombreusu();
            this.NavigationService.StopLoading();
        }
    }
}
