using CAAS.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{

  public class GetSearchResultBlogPostsBySearchTextViewModelQuery : IQuery<IList<SearchResultBlogPostViewModel>>
  {
    public string SearchText { get; set; }
  }

}
