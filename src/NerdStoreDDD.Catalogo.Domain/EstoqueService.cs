﻿using NerdStoreDDD.Catalogo.Domain.Events;
using NerdStoreDDD.Core.Bus;

namespace NerdStoreDDD.Catalogo.Domain;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMediatrHandler _bus;

    public EstoqueService(IProdutoRepository produtoRepository, IMediatrHandler bus)
    {
        _produtoRepository = produtoRepository;
        _bus = bus;
    }

    public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if(produto is null) return false;

        if(!produto.PossuiEstoque(quantidade)) return false;

        produto.DebitarEstoque(quantidade);

        if(produto.QuantidadeEstoque < 10)        
            await _bus.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));        

        _produtoRepository.Atualizar(produto);
        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if (produto is null) return false;

        produto.ReporEstoque(quantidade);

        _produtoRepository.Atualizar(produto);
        return await _produtoRepository.UnitOfWork.Commit();
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
    }
}

public interface IEstoqueService : IDisposable
{
    Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
    Task<bool> ReporEstoque(Guid produtoId, int quantidade);
}
