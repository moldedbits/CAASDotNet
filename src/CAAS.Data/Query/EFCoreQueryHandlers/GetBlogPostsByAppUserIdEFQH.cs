using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAAS.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CAAS.Data.Query.Queries;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostsByAppUserIdEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostsByAppUserIdQuery, IEnumerable<BlogPost>>
  {
    public GetBlogPostsByAppUserIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BlogPost>> HandleAsync(GetBlogPostsByAppUserIdQuery query)
    {
      var posts = await (from a in _context.Authors
                         where a.ApplicationUserId == query.AppUserId
                         join bp in _context.BlogPosts on a.Id equals bp.AuthorId
                         select bp).ToListAsync();

      return posts;
    }
  }
}
