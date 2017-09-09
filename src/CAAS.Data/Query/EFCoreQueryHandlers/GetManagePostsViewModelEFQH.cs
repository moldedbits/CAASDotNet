using CAAS.Data.Query.Queries;
using CAAS.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAAS.EFCore;
using Microsoft.EntityFrameworkCore;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetManagePostsViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManagePostsViewModelQuery, ManagePostsViewModel>
  {
    public GetManagePostsViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<ManagePostsViewModel> HandleAsync(GetManagePostsViewModelQuery query)
    {
      ManagePostsViewModel mpvm = new ManagePostsViewModel()
      {
        posts = await _context.BlogPosts.AsNoTracking().Include(a => a.Author).ToListAsync()
      };
      return mpvm;
    }
  }
}
