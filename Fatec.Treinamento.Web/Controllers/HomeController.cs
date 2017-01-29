using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model;
using System.Web.Mvc;

namespace Fatec.Treinamento.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            
        }

        public PartialViewResult MenuAssuntos()
        {
            using(AssuntoRepository repo = new AssuntoRepository())
            {
                // Obter a lista de assuntos
                var model = repo.ListarTotalCursosPorAssunto();
                
                return PartialView("~/Views/Shared/ItensMenuAssuntos.cshtml", model);
            }
        }
    }
}