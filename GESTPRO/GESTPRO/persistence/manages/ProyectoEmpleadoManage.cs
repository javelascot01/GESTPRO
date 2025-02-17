using GESTPRO.domain;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTPRO.persistence.manages
{
    public class ProyectoEmpleadoManage
    {
        private List<ProyectoEmpleado> listaProyectoEmpleados { get; set; }
        public ProyectoEmpleadoManage()
        {
            listaProyectoEmpleados = new List<ProyectoEmpleado>();
        }
        public List<ProyectoEmpleado> leerEmpleados()
        {
            ProyectoEmpleado proyectoEmpleado = null;
            List<Object> aux = DBBroker.obtenerAgente().leer("Select p.codigoProy,p.nombreProy,e.nombre,pe.horas,e.csr,pe.fecha,p.idProyecto, e.idEmpleados FROM mydb.proyectos p" +
                " JOIN mydb.proyectoempleado pe on p.idProyecto=pe.idProyecto" +
                " JOIN mydb.empleados e ON pe.idEmpleados=e.idEmpleados;");

            List<ProyectoEmpleado> listaProyectos = new List<ProyectoEmpleado>();
            foreach (List<Object> c in aux)
            {
                proyectoEmpleado = new ProyectoEmpleado(c[0].ToString(), c[1].ToString(), c[2].ToString(),Convert.ToInt32(c[3]),Convert.ToDouble(c[4]), Convert.ToDateTime(c[5]));
                proyectoEmpleado.idProyecto= Convert.ToInt32(c[6]);
                proyectoEmpleado.idEmpleado= Convert.ToInt32(c[7]);
                listaProyectos.Add(proyectoEmpleado);
            }//(string codigoProyecto, string nombrePro, string nombreEmp, int horas, double csr,DateTime fecha
            return listaProyectos;
        }
        public void insertarProyectoEmpleado(ProyectoEmpleado em)
        {
            DBBroker dBBroker = DBBroker.obtenerAgente();
            string fechaFormateada = em.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
            string query = $"Insert into mydb.proyectoempleado (idProyecto, idEmpleados, Horas, Costes, Fecha) values ({em.idProyecto}, {em.idEmpleado}, {em.Horas}, {em.Coste}, '{fechaFormateada}');";
            dBBroker.modificar(query);
        }


    }
}

