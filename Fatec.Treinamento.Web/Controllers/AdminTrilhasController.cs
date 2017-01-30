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
    public class AdminTrilhasController : Controller
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
            // Essa view precisa dos dados da trilha para exibir o nome da trilha,
            // + uma lista com todos os cursos disponíveis e também uma outra lista
            // com os cursos que já estão selecionados.
            // Por isso criamos esse viewmodel para devolver isso tudo para a view;

            var model = new VincularCursosTrilhaViewModel();

            // Obtenho a Trilha
            using (var repo = new TrilhaRepository())
            {
                model.Trilha = repo.Obter(id);
            }

            // Monto a lista com os Itens selecionados:
            model.CursosSelecionados = model.Trilha.Cursos.Select(c => c.Id).ToList();
            
            // Obs. A sintaxe acima se chama Lambda, e é usada para montar uma lista de IDs que é o que
            // o Helper precisa pra montar um ListBox de multipla seleção.
            // Poderia ser feito um FOREACH para obter essa lista.
            

            // Lista com todos os cursos
            using (var repo = new CursoRepository())
            {
                var lista = repo.ListarCursosDetalhes();

                // transforma os itens da lista nos itens para vincular com a tela:
                // Obs: Esse foreach é desnecessário. Poderia ser resolvido com 1 única linha de lambda, porém
                // usei o for para ser mais fácil de entender.
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
            // Aqui eu salvo os cursos vinculados.
            // No model, vai receber os IDs dos itens selecionados, entao é só 
            // passar isso pro método do repositório que faz a atualização.

            using (var repo = new TrilhaRepository())
            {
                repo.AtualizarCursos(model.Trilha.Id, model.CursosSelecionados.ToArray());
            }

            return RedirectToAction("Index");

        }
    }
}