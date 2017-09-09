﻿using System;
using System.Collections.Generic;
using System.Text;
using CAAS.Models;
using Microsoft.EntityFrameworkCore;
using CAAS.Data.Command.Commands;
using CAAS.EFCore;
using System.Threading.Tasks;
using System.Linq;

namespace CAAS.Data.Command.EFCoreCommandHandlers
{
  public class AddTopicCommandEFCH : EFCHBase, ICommandHandlerAsync<AddTopicCommand>
  {
    public AddTopicCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddTopicCommand>> ExecuteAsync(AddTopicCommand command)
    {
      try
      {
        if (command.Topic.ShowOnHomePage == true)
        {
          var curTopicOnHomePage = await _context.Topics.Where(t => t.ShowOnHomePage == true).FirstOrDefaultAsync();

          if (curTopicOnHomePage != null)
          {
            curTopicOnHomePage.ShowOnHomePage = false;

            await _context.SaveChangesAsync();
          }
        }

        _context.Topics.Add(command.Topic);
        await _context.SaveChangesAsync();

        return new CommandResult<AddTopicCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddTopicCommand>(command, false, ex);
      }
    }
  }
}
