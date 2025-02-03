using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;
using MediatR;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByResult>;
    public record GetProductByResult(Product Product);
    internal class GetProductByIdHandler(IDocumentSession session)
        : IRequestHandler<GetProductByIdQuery, GetProductByResult>
    {
        private readonly IDocumentSession _session = session;
        public async Task<GetProductByResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);
            return product is null ? throw new ProductNotFoundException(request.Id) 
                : new GetProductByResult(product);
        }
    }
}
