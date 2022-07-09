using CalixtoStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalixtoStore.Catalogo.Domain.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodos();

        Task<Produto> ObterPorId(Guid id);

        Task<IEnumerable<Produto>> ObterPorCategoria(int codigo);

        Task<IEnumerable<Categoria>> ObterCategorias();

        void Atualizar(Produto produto);

        void Atualizar(Categoria produto);

        void Adicionar(Produto produto);

        void Adicionar(Categoria produto);
    }
}