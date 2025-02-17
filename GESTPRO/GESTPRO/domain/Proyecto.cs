using GESTPRO.persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GESTPRO.domain
{
    internal class Proyecto
    {
        public int Id { get; set; }
        public string codProy { get; set; }
        public string nombre { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        private List<Proyecto> proyectos;
        public ProyectoPersistence pp;
        public Proyecto() { 
            pp= new ProyectoPersistence();
        }
        public Proyecto(int id,string codProy, string nombre, string fechaInicio, string fechaFin)
        {
            pp = new ProyectoPersistence();
            this.Id = id;
            this.codProy = codProy;
            this.nombre = nombre;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
        }
        public Proyecto(string codProy, string nombre, string fechaInicio, string fechaFin)
        {
            pp = new ProyectoPersistence();
            this.codProy = codProy;
            this.nombre = nombre;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
        }
        public List<Proyecto> gListaProyectos()
        {
            proyectos = pp.leerProyectos();
            return proyectos;

        }
        public void insertar()
        {
            pp.insertarProyecto(this);
        }
        public void modificar()
        {
            pp.modificarProyecto(this);
        }
        public void eliminar()
        {
            pp.eliminarProyecto(this);
        }
        public Proyecto getProyectoRandom()
        {
            Proyecto p = new Proyecto();
            List<string> list = ProyectoPersistence.listaEmpresas();
            //Obtener numero entre 0 y 5 para las empresas
            int num=Helpers.random(0, 5);
            int anio=DateTime.Now.Year;
            string strAnio=anio.ToString();
            string strAnioDos = "" + strAnio[2] + strAnio[3];
            string codigoPro = "MTR" + num.ToString() + list[num] + strAnioDos;
            string nombre = list[num];
            string fechaInicio = DateTime.Now.AddDays(-num).ToString();
            string fechaFin = DateTime.Now.AddDays(num).ToString();
            p.codProy = codigoPro;
            p.nombre = nombre;
            p.fechaFin = fechaFin;
            p.fechaInicio=fechaInicio;
            return p;

        }
        public override string ToString()
        {
            return codProy;
        }
    }
}
