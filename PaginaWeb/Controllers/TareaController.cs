using AccesoDatos;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PaginaWeb.Controllers
{
    public class TareaController : Controller
    {
        // GET: Tarea
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListaUsuarioASync()
        {
            List<Usuario> Lista = await UsuarioAD.Instancia.ListaUsuarioAsync();
            return Json(new { data = Lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ListaTareaASync()
        {
            List<Tarea> Lista = await TareaAD.Instancia.ListaTareaAsync();
            return Json(new { data = Lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarTareaASync(Tarea tarea)
        {
            Resultado _resultado;
            if (tarea.IdTarea == 0)
            {
                _resultado = await TareaAD.Instancia.AgregarTareaAsync(tarea);
            }
            else
            {
                _resultado = await TareaAD.Instancia.ModificarTareaAsync(tarea);
            }
            return Json(new { data = _resultado });
        }

        [HttpPost]
        public async Task<JsonResult> EliminarTareaASync(Tarea tarea)
        {
            Resultado _resultado = await TareaAD.Instancia.EliminarTareaAsync(tarea);
            return Json(new { data = _resultado });
        }
    }
}