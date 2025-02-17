using GESTPRO.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.persistence.manages
{
    public class EmpleadoManage
    {
        private List<Empleado> listaEmpleados { get; set; }
        public EmpleadoManage()
        {
            listaEmpleados = new List<Empleado>();
        }
        public List<Empleado> leerEmpleados()
        {
            Empleado user = null;
            List<Object> aux = DBBroker.obtenerAgente().leer("Select * from mydb.empleados;");
            List<Empleado> listaProyectos = new List<Empleado>();
            foreach (List<Object> c in aux)
            {
                user = new Empleado(Convert.ToInt32(c[0]), c[1].ToString(), c[2].ToString(), Convert.ToInt32(c[3]), Convert.ToDouble(c[4]), c[5].ToString());
                listaProyectos.Add(user);
            }
            return listaProyectos;
        }
        public void insertarEmpleado(Empleado em)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            dBBroker.modificar("Insert into mydb.empleados (nombre,apellidos,rol,csr) values ('" + em.Nombre + "','" + em.Apellidos + "'," + em.Rol + "," + em.Csr + ");");
        }
        public void modificarEmpleado(Empleado em)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            dBBroker.modificar("Update mydb.empleados set nombre = '" + em.Nombre + "', apellidos='"+em.Apellidos+"', rol='"+em.Rol+"',  csr="+em.Csr+" WHERE idEmpleados = " + em.Id + ";");
        }
        public void eliminarEmpleado(Empleado em)
        {
            DBBroker db = DBBroker.obtenerAgente();
            db.modificar("DELETE FROM mydb.empleados WHERE idEmpleados= " + em.Id);
        }
        public void aniadirNombreUsuario(Empleado em)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            dBBroker.modificar("Update mydb.empleados set nombreUsuario = '" + em.NombreUsuario + "' WHERE idEmpleados = " + em.Id + ";");
        }

    }
    }

