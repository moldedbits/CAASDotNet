﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAAS.Models.ViewModels.Home
{
  public class CategoryPostsViewModel
  {
    public Category Category { get; set; }
    public IEnumerable<BlogPost> Posts { get; set; }
  }
}
