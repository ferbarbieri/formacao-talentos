using Fatec.Treinamento.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fatec.Treinamento.Model;
using Fatec.Treinamento.Data.Repositories.Base;
using Dapper;
using Fatec.Treinamento.Model.DTO;

namespace Fatec.Treinamento.Data.Repositories
{
    public class TrilhaRepository : RepositoryBase, ITrilhaRepository
    {
        
        public Trilha Inserir(Trilha trilha)
        {
            var id = Connection.ExecuteScalar<int>(
               @"INSERT INTO Trilha (Nome, Ativa) 
                 VALUES (@Nome, @Ativa); 
               SELECT SCOPE_IDENTITY()",
               param: new
               {
                   trilha.Nome,
                   trilha.Ativa
               }
           );

            trilha.Id = id;
            return trilha;
        }

        public IEnumerable<Trilha> Listar()
        {
            return Connection.Query<Trilha>(
              @"SELECT Id, Nome, Ativa FROM Trilha
                ORDER BY Nome ASC"
            ).ToList();
        }

        /// <summary>
        /// Retorna a trilha já com seus cursos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Trilha Obter(int id)
        {
            var trilha = Connection.Query<Trilha>(
               "SELECT Id, Nome, Ativa FROM Trilha WHERE Id = @Id",
               param: new { Id = id }
            ).FirstOrDefault();


            if (trilha == null)
            {
                return trilha;
            }

            // Obtém os cursos dessa trilha:
            var cursos = Connection.Query<DetalhesCurso>(
                @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                    FROM Trilha_Curso tc
	                inner join curso c on tc.idCurso = c.id
                    inner join assunto a on c.IdAssunto = a.id
                    inner join usuario u on c.IdAutor = u.Id
                WHERE tc.IdTrilha = @Id
                ORDER BY c.Nome",
                param: new { Id = id }
            ).ToList();

            trilha.Cursos = cursos;

            return trilha;
        }


        public Trilha Atualizar(Trilha trilha)
        {
            Connection.Execute(
               @"UPDATE Trilha SET 
                    Nome = @Nome,
                    Ativa = @Ativa
                WHERE Id = @Id",
               param: new
               {
                   trilha.Nome,
                   trilha.Ativa,
                   trilha.Id
               }
            );

            return trilha;
        }
        
        public void Excluir(Trilha trilha)
        {
            Connection.Execute(
                "DELETE FROM Trilha WHERE Id = @Id",
                param: new { Id = trilha.Id }
            );
        }

        /// <summary>
        /// Recebe uma lista de cursos e atualiza a trilha para conter esses cursos
        /// </summary>
        /// <param name="idTrilha"></param>
        /// <param name="cursos"></param>
        public void AtualizarCursos(int idTrilha, int[] cursos)
        {
            // Cria uma lista contento IdTrilha ,IdCurso, seguindo o modelo a inserir
            var lista = (from x in cursos
                         select new
                         {
                             IdTrilha = idTrilha,
                             IdCurso = x
                         }).ToList();

            var trans = Connection.BeginTransaction();

            try
            {
                // primeiro exclui os cursos daquela trilha na tabela de relacionamento
                // para inserir tudo novamente
                Connection.Execute(
                   "DELETE FROM Trilha_Curso WHERE IdTrilha = @Id",
                   param: new { Id = idTrilha },
                   transaction: trans
               );

                // Agora insere a lista
                Connection.Execute(
                    @"INSERT INTO Trilha_Curso (IdTrilha, IdCurso) 
                      VALUES(@IdTrilha, @IdCurso)",
                    param: lista,
                    transaction: trans
                );

                // deu certo, faz commit da transação.
                trans.Commit();
            }
            catch
            {
                trans.Rollback();// caso de erro, reverte a transação e lança o erro
                throw;
            }
            finally
            {
                // Finaliza a transação
                trans.Dispose();
            }
        }
    }
}
