using System;
using System.Diagnostics;
using System.Linq;
using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fatec.Treinamento.Testes.Data.Tests
{
    [TestClass]
    public class TrilhaRepositoryTests
    {
        
        [TestMethod]
        public void TestarCrudTrilha()
        {
            var id = InserirTrilha();
            var trilha = ObterTrilha(id);
            trilha = AtualizarTrilha(trilha);
            Excluir(trilha);
        }

        private int InserirTrilha()
        {
            using (TrilhaRepository repo = new TrilhaRepository())
            {
                var trilha = new Trilha
                {
                    Nome = "Demo teste 1",
                    Ativa = true
                };

                int id = repo.Inserir(trilha).Id ;

                Assert.IsTrue(id > 0);
                return id;
            }
        }
        
        public Trilha ObterTrilha(int id)
        {
            using (TrilhaRepository repo = new TrilhaRepository())
            {
               
                var trilha = repo.Obter(id);
                
                Assert.AreEqual("Demo teste 1", trilha.Nome);
                
                return trilha;

            }
        }


        private Trilha AtualizarTrilha(Trilha trilha)
        {
            using (TrilhaRepository repo = new TrilhaRepository())
            {

                trilha.Nome = "Nome Atualizado";
                repo.Atualizar(trilha);

                var trilhaAtualizado = repo.Obter(trilha.Id);

                Assert.AreEqual("Nome Atualizado", trilhaAtualizado.Nome);
                
                return trilhaAtualizado;
            }
        }
                
        private void Excluir(Trilha trilha)
        {
            using (TrilhaRepository repo = new TrilhaRepository())
            {
                repo.Excluir(trilha);
                var trilhaRet = repo.Obter(trilha.Id);
                Assert.IsTrue(trilhaRet == null);
            }
        }
    }
}
