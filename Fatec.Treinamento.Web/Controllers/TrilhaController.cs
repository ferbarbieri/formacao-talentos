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
    public class TrilhaController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var repo = new TrilhaRepository();

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
        public ActionResult Criar(Trilha trilha)
        {
            using (var repo = new TrilhaRepository())
            {
                var inserido = repo.Inserir(trilha);

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
            using (var repo = new TrilhaRepository())
            {
                var trilha = repo.Obter(id);

                return View(trilha);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Trilha trilha)
        {
            using (var repo = new TrilhaRepository())
            {
                trilha = repo.Atualizar(trilha);

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Excluir(int id)
        {
            using (var repo = new TrilhaRepository())
            {
                var trilha = repo.Obter(id);

                return View(trilha);
            }
        }

        [HttpPost]
        public ActionResult Excluir(Trilha trilha)
        {
            using (var repo = new TrilhaRepository())
            {
                repo.Excluir(trilha);

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult VincularCursos(int id)
        {

            // Essa view precisa dos dados da trilha, + uma lista com todos os cursos disponíveis.
            // Por isso criamos esse viewmodel para devolver isso para a voew;

            var model = new VincularCursosTrilhaViewModel();

            using (var repo = new TrilhaRepository())
            {
                model.Trilha = repo.Obter(id);
            }

            // Itens selecionados:
            model.CursosSelecionados = model.Trilha.Cursos.Select(c => c.Id).ToList();

            // Lista com todos os cursos
            using (var repo = new CursoRepository())
            {
                var lista = repo.ListarCursosDetalhes();

                // transforma os itens da lista nos itens para vincular com a tela:
                foreach (var curso in lista)
                {
                    var item = new SelectListItem
                    {
                        Text = curso.Nome,
                        Value = curso.Id.ToString()
                    };
                    model.CursosDisponiveis.Add(item);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VincularCursos(VincularCursosTrilhaViewModel model)
        {

            using (var repo = new TrilhaRepository())
            {
                repo.AtualizarCursos(model.Trilha.Id, model.CursosSelecionados.ToArray());
            }

            return RedirectToAction("Index");

        }
    }
}