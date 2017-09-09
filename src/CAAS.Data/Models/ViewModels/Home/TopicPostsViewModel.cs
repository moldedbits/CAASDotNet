﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAAS.Models.ViewModels.Home
{
  public class TopicPostsViewModel
  {
    public Topic Topic { get; set; }
    public IEnumerable<BlogPostViewModel> BlogPosts { get; set; }
  }
}
