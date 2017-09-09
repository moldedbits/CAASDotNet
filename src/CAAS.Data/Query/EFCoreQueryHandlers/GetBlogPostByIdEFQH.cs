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
  public class GetBlogPostByIdEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostByIdQuery, BlogPost>
  {
    public GetBlogPostByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<BlogPost> HandleAsync(GetBlogPostByIdQuery query)
    {
      return await _context.BlogPosts.Where(bp => bp.Id == query.Id).Include(bp => bp.Author).FirstOrDefaultAsync();
    }
  }
}
