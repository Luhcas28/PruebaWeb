using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Tarea
    {
        public int IdTarea { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public Usuario _Usuario { get; set; }
        public string Color
        {
            get
            {
                if (Estado)
                {
                    return "danger";
                }
                else
                {
                    return "success";
                }
            }
        }
        public string Icono
        {
            get
            {
                if (Estado)
                {
                    return "lock";
                }
                else
                {
                    return "unlock";
                }
            }
        }
    }
}
