using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fatec.Treinamento.Model
{
    public class Capitulo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Pontos { get; set; }

        public IEnumerable<Video> Videos { get; set; }

        // Retorna a soma total dos tempos dos videos.
        public string TempoTotal
        {
            get
            {
                int soma = 0;
                if (Videos != null)
                {
                    soma = Videos.Sum(x => x.Duracao);
                }

                TimeSpan time = TimeSpan.FromSeconds(soma);
                return time.ToString(@"hh\:mm\:ss");
            }
        }
    }
}
