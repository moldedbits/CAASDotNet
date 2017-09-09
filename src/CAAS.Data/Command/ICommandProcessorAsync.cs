using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Command
{
  public interface ICommandProcessorAsync
  {
    Task<CommandResult<TCommand>> ProcessAsync<TCommand>(TCommand command);
  }
}
