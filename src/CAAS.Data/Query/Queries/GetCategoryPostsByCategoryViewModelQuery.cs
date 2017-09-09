using System;
using System.Collections.Generic;
using System.Text;
using CAAS.Models.ViewModels.Home;

namespace CAAS.Data.Query.Queries
{
  public class GetCategoryPostsByCategoryViewModelQuery : IQuery<CategoryPostsViewModel>
  {
    public long Id { get; set; }
  }
}
