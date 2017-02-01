using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fatec.Treinamento.Web.Models;

namespace Fatec.Treinamento.Web.Controllers
{
    // Obs: Colocando o atributo [Authorize] no controller, garante que todas as actions so podem ser
    // acessadas quando o usuário estiver autorizado a utilizar.
    // Quando informo o parametro "Roles", indico que só quem tiver o perfil administrador poderá acessar.

    [Authorize(Roles="Administrador")]
    public class AdminAssuntosController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var repo = new AssuntoRepository();

            var lista = repo.Listar();

            return View(lista);
        }

        [HttpGet]
        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar(Assunto assunto)
        {
            using (var repo = new AssuntoRepository())
            {
                var inserido = repo.Inserir(assunto);

                if (inserido.Id == 0)
                {
                    ModelState.AddModelError("", "Erro");
                }

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            using (var repo = new AssuntoRepository())
            {
                var assunto = repo.Obter(id);

                return View(assunto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Assunto assunto)
        {
            using (var repo = new AssuntoRepository())
            {
                assunto = repo.Atualizar(assunto);

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Excluir(int id)
        {
            using (var repo = new AssuntoRepository())
            {
                var assunto = repo.Obter(id);

                return View(assunto);
            }
        }

        [HttpPost]
        public ActionResult Excluir(Assunto assunto)
        {
            try
            {
                using (var repo = new AssuntoRepository())
                {
                    repo.Excluir(assunto);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erro", ex.Message);
                return View(assunto);
            }
        }
    }
}