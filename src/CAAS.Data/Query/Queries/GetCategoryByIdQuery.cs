using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{
  public class GetCategoryByIdQuery : IQuery<Category>
  {
    public long Id { get; set; }
  }
}
