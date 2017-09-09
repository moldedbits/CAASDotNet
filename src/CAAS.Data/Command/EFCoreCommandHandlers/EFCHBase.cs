using CAAS.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Command.EFCoreCommandHandlers
{
  public abstract class EFCHBase
  {
    protected BlogDbContext _context;

    public EFCHBase(BlogDbContext context)
    {
      _context = context;
    }
  }
}
