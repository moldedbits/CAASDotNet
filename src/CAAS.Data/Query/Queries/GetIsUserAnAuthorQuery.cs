using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{
  public class GetIsUserAnAuthorQuery : IQuery<bool>
  {
    public string Id { get; set; }
  }
}
