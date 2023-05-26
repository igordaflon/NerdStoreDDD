using NerdStoreDDD.Core.DomainObjects;

namespace NerdStoreDDD.Catalogo.Domain.Events;

public class ProdutoAbaixoEstoqueEvent : DomainEvent
{
    public int _quantidadeRestante { get; private set; }

    public ProdutoAbaixoEstoqueEvent(Guid aggregateId, int quantidadeRestante) : base(aggregateId)
    {
        _quantidadeRestante = quantidadeRestante;
    }
}
