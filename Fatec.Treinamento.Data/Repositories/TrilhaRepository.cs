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
        

        public Trilha Obter(int id)
        {
            return Connection.Query<Trilha>(
               "SELECT Id, Nome, Ativa FROM Trilha WHERE Id = @Id",
               param: new { Id = id }
           ).FirstOrDefault();
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
    }
}
