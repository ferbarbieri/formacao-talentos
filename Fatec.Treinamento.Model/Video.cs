using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fatec.Treinamento.Model
{
    public class Video
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Duracao { get; set; }

        public string DuracaoFormatada
        {
            get
            {
                TimeSpan time = TimeSpan.FromSeconds(Duracao);
                return time.ToString(@"hh\:mm\:ss");
            }
        }

        public string CodigoYoutube { get; set; }
    }
}
