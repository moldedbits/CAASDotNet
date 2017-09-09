using CAAS.Data.Query.Queries;
using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAAS.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetAuthorByAppUserIdEFQH : EFQHBase, IQueryHandlerAsync<GetAuthorByAppUserIdQuery, Author>
  {
    public GetAuthorByAppUserIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Author> HandleAsync(GetAuthorByAppUserIdQuery query)
    {
      // This needs to be returned as tracking or another query needs to be made
      return await _context.Authors.Where(a => a.ApplicationUserId == query.Id).FirstOrDefaultAsync();
    }
  }
}
