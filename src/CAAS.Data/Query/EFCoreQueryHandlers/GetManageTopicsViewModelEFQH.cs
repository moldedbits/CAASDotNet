using CAAS.Data.Query.Queries;
using CAAS.EFCore;
using CAAS.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public class GetManageTopicsViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManageTopicsViewModelQuery, ManageTopicsViewModel>
  {
    public GetManageTopicsViewModelEFQH(BlogDbContext context) : base(context) { }

    public async Task<ManageTopicsViewModel> HandleAsync(GetManageTopicsViewModelQuery query)
    {
      ManageTopicsViewModel mfvm = new ManageTopicsViewModel()
      {
        Topics = await _context.Topics.AsNoTracking().ToListAsync()
      };
      return mfvm;
    }
  }
}
