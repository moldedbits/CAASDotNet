﻿using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{

  public class GetSearchResultBlogPostsBySearchTextViewModelQuery : IQuery<IList<SearchResultBlogPostViewModel>>
  {
    public string SearchText { get; set; }
  }

}
