using GESTPRO.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.persistence.manages
{

    public class UsuarioManage
    {
        private List<Usuario> listaUsuarios { get; set; }
        public UsuarioManage()
        {
            listaUsuarios = new List<Usuario>();
        }
        public List<Usuario> leerUsuarios()
        {
            Usuario user = null;
            List<Object> aux = DBBroker.obtenerAgente().leer("Select * from mydb.usuarios;");
            List<Usuario> listaProyectos = new List<Usuario>();
            foreach (List<Object> c in aux)
            {
                user = new Usuario(Convert.ToInt32(c[0]), c[1].ToString(), c[2].ToString());
                listaProyectos.Add(user);
            }
            return listaProyectos;
        }
        public void insertarUsuario(Usuario u)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            dBBroker.modificar("Insert into mydb.usuarios (nombreUsuario,password) values ('" + u.NombreUsuario + "','" + u.Contrasena + "');");
        }
        public void modificarUsuario(Usuario u)
        {
            DBBroker db = DBBroker.obtenerAgente();
            db.modificar("UPDATE mydb.usuarios SET password = '" + u.Contrasena + "' WHERE idUsuarios = " + u.IdUsuario + ";");
        }
        public void eliminarUsuario(Usuario u)
        {
            DBBroker db = DBBroker.obtenerAgente();
            db.modificar("DELETE FROM mydb.usuarios WHERE idUsuarios= " + u.IdUsuario);
        }
    }
}
