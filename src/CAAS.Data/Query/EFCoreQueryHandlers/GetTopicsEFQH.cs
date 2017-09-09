using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CAAS.Models;
using CAAS.Data.Query.Queries;
using System.Threading.Tasks;
using CAAS.EFCore;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetTopicsEFQH : EFQHBase, IQueryHandlerAsync<GetTopicsQuery, IEnumerable<Topic>>
  {
    public GetTopicsEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Topic>> HandleAsync(GetTopicsQuery query)
    {
      return await Task.FromResult(_context.Topics);
    }
  }
}
