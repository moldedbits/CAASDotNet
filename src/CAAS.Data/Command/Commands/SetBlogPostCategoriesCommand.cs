﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CAAS.Data.Command.Commands
{
  public class SetBlogPostCategoriesCommand
  {
    public long BlogPostId { get; set; }
    public IEnumerable<long> CategoryIds { get; set; }
  }
}
