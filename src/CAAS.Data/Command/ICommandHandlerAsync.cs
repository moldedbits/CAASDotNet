﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Command
{
  interface ICommandHandlerAsync<TCommand>
  {
    Task<CommandResult<TCommand>> ExecuteAsync(TCommand command);
  }
}
