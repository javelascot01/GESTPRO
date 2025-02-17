using GESTPRO.domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GESTPRO.persistence
{
    internal class ProyectoPersistence
    {
        private DataTable table { get; set; }
        private List<Proyecto> listaProyectos { get; set; }

        public ProyectoPersistence()
        {
            table = new DataTable();
            listaProyectos = new List<Proyecto>();
        }

        /*public List<Proyecto> leerProyectos()
{
    List<Proyecto> listaProyectos = new List<Proyecto>();
    string query = @"SELECT p.IDProyecto, p.CodigoProv, p.NombreProv, p.DescProy, p.Presupuesto,
                            e.IDEmpleado, e.Nombre, e.Apellidos, e.CSR,
                            r.IDRol, r.NombreRol
                     FROM proyecto p
                     JOIN proyectoempleado pe ON p.IDProyecto = pe.IDProyecto
                     JOIN empleado e ON pe.IDEmpleado = e.IDEmpleado
                     JOIN rol r ON e.IDRol = r.IDRol;";

    List<Object> aux = DBBroker.obtenerAgente().leer(query);
    
    foreach (List<Object> c in aux)
    {
        Proyecto proyecto = new Proyecto(
            Convert.ToInt32(c[0]), c[1].ToString(), c[2].ToString(),
            c[3].ToString(), Convert.ToSingle(c[4])
        );

        Empleado empleado = new Empleado(
            Convert.ToInt32(c[5]), c[6].ToString(), c[7].ToString(),
            Convert.ToSingle(c[8])
        );

        Rol rol = new Rol(
            Convert.ToInt32(c[9]), c[10].ToString()
        );

        proyecto.Empleado = empleado;
        empleado.Rol = rol;

        listaProyectos.Add(proyecto);
    }

    return listaProyectos;
}*/
        public List<Proyecto> leerProyectos()
        {
            Proyecto proyecto = null;
            List<Object> aux = DBBroker.obtenerAgente().leer("Select * from mydb.proyectos;");
            List <Proyecto> listaProyectos = new List<Proyecto>();
            foreach (List<Object> c in aux)
            {
                proyecto = new Proyecto(Convert.ToInt32(c[0]), c[1].ToString(), c[2].ToString(), c[3].ToString(), c[4].ToString());
                listaProyectos.Add(proyecto);
            }
            //List <Proyecto> listaProyectos=new List<Proyecto>();
            return listaProyectos;
        }
        public void insertarProyecto(Proyecto p)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            dBBroker.modificar("Insert into mydb.proyectos (codigoProy,nombreProy,fechaInicio,fechaFin) values ('" + p.codProy + "','" + p.nombre + "','"+p.fechaInicio+"','"+p.fechaFin+"');");
           // dBBroker.modificar("Insert into mydb.proyectos (codigoproy,nombreProy,fechaInicio,fechaFin) values (" + p.codProy + ",'" + p.nombre + "','" + p.fechaInicio + "'," + p.fechaFin + ");");
        }
        public void modificarProyecto(Proyecto p)
        {
            DBBroker db = DBBroker.obtenerAgente();
            db.modificar("UPDATE mydb.proyectos SET codigoProy = '" + p.codProy + "', nombreProy = '" + p.nombre + "', fechaInicio = '" + p.fechaInicio + "', fechaFin = '"+ p.fechaFin+"' WHERE idProyecto = " + p.Id + ";");
        }
        public void eliminarProyecto(Proyecto p)
        {
            DBBroker db= DBBroker.obtenerAgente();
            db.modificar("DELETE FROM mydb.proyectos WHERE idProyecto= " + p.Id);
        }
        public static List<string> listaEmpresas()
        {
            List <string> listaEmpresas=new List<string>();
            listaEmpresas.Add("ALLEGRO");
            listaEmpresas.Add("VELNEO");
            listaEmpresas.Add("SAP");
            listaEmpresas.Add("NATURGY");
            listaEmpresas.Add("SANTANDER");
            listaEmpresas.Add("MUTUAMADRILEÑA");
            //Console.WriteLine(listaEmpresas[0]);
            return listaEmpresas;
        }
    }
}
