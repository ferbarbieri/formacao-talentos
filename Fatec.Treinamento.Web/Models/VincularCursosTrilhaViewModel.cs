using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fatec.Treinamento.Model;

namespace Fatec.Treinamento.Web.Models
{
    /// <summary>
    /// ViewModel para vinculo de cursos e trilhas.
    /// Deriva de Trilha para ficar mais fácil já que uso todas as propriedades da Trilha
    /// </summary>
    public class VincularCursosTrilhaViewModel
    {
        public Trilha Trilha { get; set; }

        public IList<SelectListItem> CursosDisponiveis { get; set; }

        [DisplayName("Cursos Selecionados")]
        public IList<int> CursosSelecionados { get; set; }

        public VincularCursosTrilhaViewModel()
        {
            CursosDisponiveis = new List<SelectListItem>();
            CursosSelecionados = new List<int>();
        }


    }
}