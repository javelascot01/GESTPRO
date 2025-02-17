using GESTPRO.persistence.manages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.domain
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Rol { get; set; }
        public string NombreUsuario { get; set; }
        
        public double Csr { get; set; }
        EmpleadoManage em;
        public Empleado() {
            em = new EmpleadoManage();
        }
        public Empleado(int id, string nombre, string apellidos, int rol, double csr, string nombreUsuario)
        {
            Id = id;
            Nombre = nombre;
            Apellidos = apellidos;
            Rol = rol;
            Csr = csr;
            NombreUsuario = nombreUsuario;
            em = new EmpleadoManage();
        }
        public Empleado(string nombre, string apellidos, int rol, double csr)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            Rol = rol;
            Csr = csr;
            em = new EmpleadoManage();
        }
        public void insertar()
        {
            em.insertarEmpleado(this);
        }
        public void modificar()
        {
            em.modificarEmpleado(this);
        }
        public void eliminar()
        {
            em.eliminarEmpleado(this);
        }
        public List<Empleado> listaEmpleados()
        {
            return em.leerEmpleados();
        }
        public void aniadirNombreusu()
        {
            em.aniadirNombreUsuario(this);
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
