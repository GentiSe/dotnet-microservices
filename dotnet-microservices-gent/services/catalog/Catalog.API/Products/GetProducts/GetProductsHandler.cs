using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;
using System.Collections;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber= 1, int? PageSize = 10) 
        : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductsHandler(IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        private readonly IDocumentSession _session = session;
        public async Task<GetProductResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _session.Query<Product>()
                .ToPagedListAsync(request.PageNumber.Value,request.PageSize.Value,cancellationToken);

            return new GetProductResult(products);
        }
    }
}
