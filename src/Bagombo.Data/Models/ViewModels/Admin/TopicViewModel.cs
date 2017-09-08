﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bagombo.Models.ViewModels.Admin
{
  public class TopicViewModel
  {
    [Required]
    public long Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public bool ShowOnHomePage { get; set; }

    [Required]
    public string Description { get; set; }
  }
}
