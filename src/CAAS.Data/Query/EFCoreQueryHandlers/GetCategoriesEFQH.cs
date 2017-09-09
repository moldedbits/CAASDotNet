using CAAS.Data.Query.Queries;
using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAAS.EFCore;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetCategoriesEFQH : EFQHBase, IQueryHandlerAsync<GetCategoriesQuery, IEnumerable<Category>>
  {
    public GetCategoriesEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> HandleAsync(GetCategoriesQuery query)
    {
      return await Task.FromResult(_context.Categories);
    }
  }
}
