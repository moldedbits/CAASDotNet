using CAAS.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{
  public class GetBlogPostByIdViewModelQuery : IQuery<BlogPostViewModel>
  {
    public long Id { get; set; }
  }
}
