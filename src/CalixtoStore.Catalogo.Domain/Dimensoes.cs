using CalixtoStore.Core.DomainObjects;

namespace CalixtoStore.Catalogo.Domain
{
    public class Dimensoes
    {
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Profundidade { get; set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            Validacoes.ValidarSeMenorQue(altura, 1, "O campo altura deve ser maior que 0");
            Validacoes.ValidarSeMenorQue(largura, 1, "O campo largura deve ser maior que 0");
            Validacoes.ValidarSeMenorQue(profundidade, 1, "O campo profundidade deve ser maior que 0");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: { Largura } x { Altura } x { Profundidade }";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }
    }
}