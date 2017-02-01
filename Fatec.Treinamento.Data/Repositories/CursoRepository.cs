using Fatec.Treinamento.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fatec.Treinamento.Model.DTO;
using Fatec.Treinamento.Data.Repositories.Base;
using Dapper;
using Fatec.Treinamento.Model;

namespace Fatec.Treinamento.Data.Repositories
{
    public class CursoRepository : RepositoryBase, ICursoRepository
    {

        public IEnumerable<DetalhesCurso> ListarCursos()
        {
            return Connection.Query<DetalhesCurso>(
              @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                 FROM curso c
                 inner join assunto a on c.IdAssunto = a.id
                 inner join usuario u on c.IdAutor = u.Id
                 ORDER BY c.Nome"
            ).ToList();
        }

        public IEnumerable<DetalhesCurso> ListarCursosPorAssunto(int idAssunto)
        {
            return Connection.Query<DetalhesCurso>(
              @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                 FROM curso c
                 inner join assunto a on c.IdAssunto = a.id
                 inner join usuario u on c.IdAutor = u.Id
                 WHERE a.Id = @IdAssunto
                 ORDER BY c.Nome",
              new {IdAssunto = idAssunto}
            ).ToList();
        }

        public IEnumerable<DetalhesCurso> ListarCursosPorNome(string nome)
        {
            return Connection.Query<DetalhesCurso>(
              @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                 FROM curso c
                 inner join assunto a on c.IdAssunto = a.id
                 inner join usuario u on c.IdAutor = u.Id
                 WHERE c.nome like @Nome
                 ORDER BY c.Nome",
              param: new { Nome = "%" + nome + "%" }
            ).ToList();
        }

        public IEnumerable<DetalhesCurso> ListarCursosDetalhes()
        {
            return Connection.Query<DetalhesCurso>(
              @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                 FROM curso c
                 inner join assunto a on c.IdAssunto = a.id
                 inner join usuario u on c.IdAutor = u.Id
                 ORDER BY c.Nome"
            ).ToList();
        }

        public DetalhesCurso ObterDetalhesCurso(int id)
        {
            var curso = Connection.Query<DetalhesCurso>(
              @"SELECT
	                c.Id,
	                c.Nome,
	                a.Nome as Assunto,
	                u.Nome as Autor,
	                c.DataCriacao,
	                c.Classificacao
                 FROM curso c
                 inner join assunto a on c.IdAssunto = a.id
                 inner join usuario u on c.IdAutor = u.Id
                 WHERE c.Id = @Id
                 ORDER BY c.Nome",
              new {Id = id}
            ).FirstOrDefault();

            if (curso == null)
                return curso;

            var capitulos = Connection.Query<Capitulo>(
                @"SELECT Id, Nome, Pontos
                  FROM Capitulo
                  WHERE IdCurso = @Id",
                new {Id = id}
            ).ToList();

            // preenche os videos de cada capitulo
            foreach (var cap in capitulos)
            {
                var videos = Connection.Query<Video>(
                @"SELECT Id, Nome, Duracao, CodigoYoutube
                  FROM Video
                  WHERE IdCapitulo = @Id",
                new { Id = cap.Id }
                ).ToList();

                cap.Videos = videos;
            }
            
            curso.Capitulos = capitulos;

            return curso;

        }
    }
}
