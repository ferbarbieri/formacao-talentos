using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fatec.Treinamento.Web.Controllers
{
    public class CursoController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<DetalhesCurso> lista = new List<DetalhesCurso>();

            using (CursoRepository repo = new CursoRepository())
            {
                lista = repo.ListarCursos();
            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult Assunto(int id)
        {
            IEnumerable<DetalhesCurso> lista = new List<DetalhesCurso>();

            using (CursoRepository repo = new CursoRepository())
            {
                lista = repo.ListarCursosPorAssunto(id);
            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult Populares()
        {
            IEnumerable<DetalhesCurso> lista = new List<DetalhesCurso>();

            using (CursoRepository repo = new CursoRepository())
            {
                lista = repo.ListarCursos();

                // Pega apenas os 5 melhores ordenados pela classificação:
                lista = (from x in lista
                    orderby x.Classificacao descending
                    select x).Take(5).ToList();

            }

            return View(lista);
        }


        public ActionResult Pesquisar(string txtPesquisaCurso)
        {
            IEnumerable<DetalhesCurso> lista = new List<DetalhesCurso>();

            using (CursoRepository repo = new CursoRepository())
            {
                lista = repo.ListarCursosPorNome(txtPesquisaCurso);
            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult Detalhes(int id)
        {
            using (CursoRepository repo = new CursoRepository())
            {
                var curso = repo.ObterDetalhesCurso(id);
                return View(curso);
            }
        }


        //get /curso/1/assistir/2
        [HttpGet]
        [Route("Curso/{idCurso}/Capitulo/{idCapitulo}/Video/{idVideo}", Name = "AssistirVideo")]   // ROTA CUSTOMIZADA
        public ActionResult Assistir(int idCurso, int idCapitulo, int idVideo)
        {
            using (CursoRepository repo = new CursoRepository())
            {
                var curso = repo.ObterDetalhesCurso(idCurso);

                // Vou devolver o ID do capítulo e video na viewbag.
                ViewBag.IdCapitulo = idCapitulo;
                ViewBag.IdVideo = idVideo;


                // pega o endereço do youtube do video que tem que rodar e devolve na viewbag tambem
                var cap = curso.Capitulos.FirstOrDefault(c => c.Id == idCapitulo);
                if (cap != null) 
                {
                    var vid = cap.Videos.FirstOrDefault(c => c.Id == idVideo);

                    if (vid != null)
                    {
                        ViewBag.CodigoYoutube = vid.CodigoYoutube;
                    }
                }

                return View(curso);
            }
        }
    }
}