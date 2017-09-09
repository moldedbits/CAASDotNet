using System;
using System.Collections.Generic;
using System.Text;
using CAAS.Models;


namespace CAAS.Data.Query.Queries
{
  public class GetBlogPostsByAppUserIdQuery : IQuery<IEnumerable<BlogPost>>
  {
    public string AppUserId { get; set; }
  }
}
