using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Command.Commands
{
  public class AddCategoryCommand
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
