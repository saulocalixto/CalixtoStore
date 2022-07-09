using CalixtoStore.Core.DomainObjects;
using System.Collections.Generic;

namespace CalixtoStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria()
        {
        }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do produto não pode estar vazio");
            Validacoes.ValidarSeIgual(Codigo, 0, "O campo Código deve ter um valor válido maior que 0");
        }
    }
}