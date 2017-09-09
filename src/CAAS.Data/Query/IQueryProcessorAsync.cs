using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Query
{
  public interface IQueryProcessorAsync
  {
    Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
  }
}
