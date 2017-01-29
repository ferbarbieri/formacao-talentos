using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fatec.Treinamento.Web.Controllers
{
    public class TrilhaController : Controller
    {
        // GET: Trilha
        public ActionResult Index()
        {
            var repo = new TrilhaRepository();

            var lista = repo.Listar();
            
            return View(lista);
        }

        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Criar(Trilha trilha)
        {
            using(var repo = new TrilhaRepository())
            {
                var inserido = repo.Inserir(trilha);

                if (inserido.Id == 0)
                {
                    ModelState.AddModelError("", "Erro");
                }

            }
            
            return RedirectToAction("Index");
        }
    }
}