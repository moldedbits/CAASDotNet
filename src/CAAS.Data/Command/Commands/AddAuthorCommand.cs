using CAAS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Command.Commands
{
  public class AddAuthorCommand
  {
    public string ApplicatoinUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Return Author
    public Author Author { get; set; }
  }
}
