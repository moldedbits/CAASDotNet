using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Query
{
  public interface IQueryHandlerAsync<TQuery, TResult> where TQuery : IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}
