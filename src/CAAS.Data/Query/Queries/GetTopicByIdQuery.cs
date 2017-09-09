using CAAS.Models;
using CAAS.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.Queries
{
  public class GetTopicByIdQuery : IQuery<Topic>
  {
    public long Id { get; set; }
  }
}
