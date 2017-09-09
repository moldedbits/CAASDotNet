using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{
  public class GetAuthorByAppUserIdQuery : IQuery<Author>
  {
    public string Id { get; set; }
  }
}
