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
  public class GetCategoriesForBlogPostByIdEFQH : EFQHBase, IQueryHandlerAsync<GetCategoriesForBlogPostByIdQuery, IEnumerable<Category>>
  {
    public GetCategoriesForBlogPostByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> HandleAsync(GetCategoriesForBlogPostByIdQuery query)
    {
      return await (from bpc in _context.BlogPostCategory
                    where bpc.BlogPostId == query.Id
                    join c in _context.Categories on bpc.CategoryId equals c.Id
                    select c).ToListAsync();
    }
  }
}
