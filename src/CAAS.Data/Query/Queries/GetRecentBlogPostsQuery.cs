using System;
using System.Collections.Generic;
using System.Text;
using CAAS.Models;
using CAAS.Models.ViewModels;
using CAAS.Models.ViewModels.Home;

namespace CAAS.Data.Query.Queries
{
  public class GetRecentBlogPostsQuery : IQuery<IList<BlogPost>>
  {
    public int NumberOfPostsToGet { get; set; }
  }

}
