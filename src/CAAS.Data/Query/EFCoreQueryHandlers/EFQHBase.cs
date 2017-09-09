using CAAS.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Query.EFCoreQueryHandlers
{
  public abstract class EFQHBase
  {
    protected BlogDbContext _context;

    public EFQHBase(BlogDbContext context)
    {
      _context = context;
    }
  }
}
