using GESTPRO.persistence.manages;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.domain
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        UsuarioManage um;
        public string EncriptarContrasena(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        public bool VerificarContrasena(string contrasena, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }
        public Usuario(int idUsuario, string nombreUsuario, string contrasena)
        {
            um = new UsuarioManage();
            this.IdUsuario = idUsuario;
            this.NombreUsuario = nombreUsuario;
            this.Contrasena = contrasena;
        }
        public Usuario()
        {
            um=new UsuarioManage();
        }
        public Usuario(string nombreUsuario, string contrasena)
        {
            um = new UsuarioManage();
            this.NombreUsuario = nombreUsuario;
            this.Contrasena = contrasena;
        }

        public void insertar()
        {
            this.Contrasena = EncriptarContrasena(this.Contrasena);
            um.insertarUsuario(this);
        }
        public void modificar()
        {
            this.Contrasena = EncriptarContrasena(this.Contrasena);
            um.modificarUsuario(this);
        }
        public void eliminar()
        {
            um.eliminarUsuario(this);
        }
        public List<Usuario> gListaUsuarios()
        {
            return um.leerUsuarios();
        }
        public override string ToString()
        {
            return NombreUsuario;
        }
    }

}
