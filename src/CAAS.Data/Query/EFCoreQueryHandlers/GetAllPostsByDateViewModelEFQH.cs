using CAAS.Data.Query.Queries;
using CAAS.EFCore;
using CAAS.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetAllPostsByDateViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetAllPostsByDateViewModelQuery, AllPostsViewModel>
  {
    public GetAllPostsByDateViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<AllPostsViewModel> HandleAsync(GetAllPostsByDateViewModelQuery query)
    {
      return new AllPostsViewModel()
      {
        PostsByDate = await _context.BlogPosts
                                      .AsNoTracking()
                                      .Where(bp => bp.Public == true && bp.PublishOn < DateTime.Now)
                                      .Include(bp => bp.Author)
                                      .OrderByDescending(bp => bp.ModifiedAt)
                                      .ThenByDescending(bp => bp.PublishOn)
                                      .ToListAsync(),
        SortBy = 2,
        Categories = null
      };
    }
  }
}
