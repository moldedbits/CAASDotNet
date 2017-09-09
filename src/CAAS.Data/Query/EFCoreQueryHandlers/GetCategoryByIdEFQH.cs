using System;
using System.Collections.Generic;
using System.Text;
using CAAS.EFCore;
using CAAS.Data.Query.Queries;
using CAAS.Models;
using System.Threading.Tasks;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetCategoryByIdEFQH : EFQHBase, IQueryHandlerAsync<GetCategoryByIdQuery, Category>
  {
    public GetCategoryByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Category> HandleAsync(GetCategoryByIdQuery query)
    {
      var c = await _context.Categories.FindAsync(query.Id);

      return c;
    }
  }
}
