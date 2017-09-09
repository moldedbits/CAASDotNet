using CAAS.Data.Command.Commands;
using CAAS.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAAS.Data.Command.EFCoreCommandHandlers
{
  public class DeleteTopicCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteTopicCommand>
  {
    public DeleteTopicCommandEFCH(BlogDbContext context) : base(context) { }

    public async Task<CommandResult<DeleteTopicCommand>> ExecuteAsync(DeleteTopicCommand command)
    {
      try
      {
        var topic = await _context.Topics.FindAsync(command.Id);

        _context.Topics.Remove(topic);

        await _context.SaveChangesAsync();

        return new CommandResult<DeleteTopicCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<DeleteTopicCommand>(command, false, ex);
      }
    }
  }
}
