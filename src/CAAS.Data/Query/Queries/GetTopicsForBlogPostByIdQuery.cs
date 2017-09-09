using System;
using System.Collections.Generic;
using System.Text;
using CAAS.Models;

namespace CAAS.Data.Query.Queries
{
  public class GetTopicsForBlogPostByIdQuery : IQuery<IEnumerable<Topic>>
  {
    public long Id { get; set; }
  }
}
