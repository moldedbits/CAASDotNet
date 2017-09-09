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
  public class GetManageCategoriesViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManageCategoriesViewModelQuery, ManageCategoriesViewModel>
  {
    public GetManageCategoriesViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<ManageCategoriesViewModel> HandleAsync(GetManageCategoriesViewModelQuery query)
    {
      ManageCategoriesViewModel amcvm = new ManageCategoriesViewModel()
      {
        Categories = await _context.Categories.AsNoTracking().ToListAsync()
      };
      return amcvm;
    }
  }
}
