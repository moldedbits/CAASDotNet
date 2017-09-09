﻿using CAAS.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAAS.EFCore;
using CAAS.Models;

namespace CAAS.Data.Command.EFCoreCommandHandlers
{
  public class AddAuthorCommandEFCH : EFCHBase, ICommandHandlerAsync<AddAuthorCommand>
  {
    public AddAuthorCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddAuthorCommand>> ExecuteAsync(AddAuthorCommand command)
    {
      try
      {
        var author = new Author()
        {
          ApplicationUserId = command.ApplicatoinUserId,
          FirstName = command.FirstName,
          LastName = command.LastName
        };

        _context.Authors.Add(author);

        await _context.SaveChangesAsync();

        command.Author = author;

        return new CommandResult<AddAuthorCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddAuthorCommand>(command, false, ex);
      }
    }
  }
}
