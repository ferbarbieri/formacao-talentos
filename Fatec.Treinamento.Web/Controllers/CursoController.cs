using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fatec.Treinamento.Web.Controllers
{
    public class CursoController : Controller
    {
        // GET: Curso
        public ActionResult Pesquisar(string txtPesquisaCurso)
        {
            IEnumerable<DetalhesCurso> lista = new List<DetalhesCurso>();

            using (CursoRepository repo = new CursoRepository())
            {
                lista = repo.ListarCursosPorNome(txtPesquisaCurso);
            }

            return View(lista);
        }
        
    }
}