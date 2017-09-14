﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CAAS.Models;
using CAAS.Models.ViewModels.Author;
using CAAS.EFCore;
using CommonMark;
using CAAS.Data.Query;
using CAAS.Data.Command;
using CAAS.Data.Query.Queries;
using CAAS.Data.Command.Commands;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CAAS.Controllers
{
  [Authorize(Roles = "Authors")]
  public class AuthorController : Controller
  {
    private IQueryProcessorAsync _qpa;
    private ICommandProcessorAsync _cp;
    private UserManager<ApplicationUser> _userManager;
    private ILogger _logger;

    public AuthorController(IQueryProcessorAsync qpa,
                            ICommandProcessorAsync cp,
                            UserManager<ApplicationUser> userManager,
                            ILogger<AuthorController> logger)
    {
      _qpa = qpa;
      _cp = cp;
      _userManager = userManager;
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult>EditProfile()
    {
      var gabauid = new GetAuthorByAppUserIdQuery
      {
        Id = await _userManager.GetUserIdAsync(
            await _userManager.GetUserAsync(User)
          )
      };

      var author = await _qpa.ProcessAsync(gabauid);

      var profileViewModel = new ProfileViewModel
      {
        Id = author.Id,
        FirstName = author.FirstName,
        LastName = author.LastName,
        Blurb = author.Blurb,
        Biography = author.Biography
      };

      return View(profileViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>EditProfile(ProfileViewModel model)
    {
      if (ModelState.IsValid)
      {
        var eapc = new EditAuthorProfileCommand
        {
          Id = model.Id,
          FirstName = model.FirstName,
          LastName = model.LastName,
          Blurb = model.Blurb,
          Biography = model.Biography,
          ImageLink = model.ImageLink
        };

        var result = await _cp.ProcessAsync(eapc);

        if (result.Succeeded)
        {
          _logger.LogInformation("Successfully updated author profile for id {0}", model.Id);
          ViewData["SavedMessage"] = "Profile saved.";
          return View(model);
        }
        else
        {
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

    [HttpGet]
    public async Task<IActionResult> ManagePosts()
    {
      AuthorManagePostsViewModel ampvm = new AuthorManagePostsViewModel();

      var curUser = await _userManager.GetUserAsync(User);

      var gbpbauid = new GetBlogPostsByAppUserIdQuery
      {
        AppUserId = curUser.Id
      };

      var posts = await _qpa.ProcessAsync(gbpbauid);

      ampvm.posts = posts;

      return View(ampvm);
    }

    [HttpGet]
    public IActionResult AddPost()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPost(AddBlogPostViewModel model)
    {
      var content = Request.Form["editor1"];

      if (ModelState.IsValid)
      {
        var curUser = await _userManager.GetUserAsync(User);

        var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = curUser.Id });

        var abpc = new AddBlogPostCommand
        {
          Author = author,
          Title = model.Title,
          //Content = model.Content,
          Content = content,
          Description = model.Description,
          CreatedAt = DateTime.Now,
          ModifiedAt = DateTime.Now,
          Public = false,
          PublishOn = DateTime.Now.AddDays(7)
        };

        var result = await _cp.ProcessAsync(abpc);

        if (result.Succeeded)
        {
          _logger.LogInformation("Added BlogPost {0} by {1} {2}", model.Title, author.FirstName, author.LastName);

          return RedirectToAction("EditPost", new { id = result.Command.Id });
        }
        else
        {
          _logger.LogWarning("Unable to add BlogPost {0} by {1} {2}", model.Title, author.FirstName, author.LastName);

          ModelState.AddModelError("", "Error saving to database!");
          return View(model);
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPost(long id)
    {
      var post = await _qpa.ProcessAsync(new GetBlogPostByIdQuery { Id = id });

      if (post != null)
      {
        EditBlogPostViewModel ebpvm = new EditBlogPostViewModel
        {
          Id = id,
          Title = post.Title,
          Content = post.Content,
          Description = post.Description,
          PublishOn = post.PublishOn,
          Public = post.Public,
          TopicsList = new List<TopicsCheckBox>(),
          CategoriesList = new List<CategoriesCheckBox>()
        };

        var postHasCategories = await _qpa.ProcessAsync(new GetCategoriesForBlogPostByIdQuery { Id = id });

        foreach (var category in await _qpa.ProcessAsync(new GetCategoriesQuery()))
        {
          var categoryCheckBox = new CategoriesCheckBox()
          {
            CategoryId = category.Id,
            Name = category.Name,
            IsSelected = false
          };

          if (postHasCategories.Contains(category))
          {
            categoryCheckBox.IsSelected = true;
          }

          ebpvm.CategoriesList.Add(categoryCheckBox);
        }

        var postHasTopics = await _qpa.ProcessAsync(new GetTopicsForBlogPostByIdQuery { Id = id });

        foreach (var topic in await _qpa.ProcessAsync(new GetTopicsQuery()))
        {
          var topicCheckBox = new TopicsCheckBox()
          {
            TopicId = topic.Id,
            Title = topic.Title,
            IsSelected = false
          };

          if (postHasTopics.Contains(topic))
          {
            topicCheckBox.IsSelected = true;
          }

          ebpvm.TopicsList.Add(topicCheckBox);
        }

        return View(ebpvm);
      }
      else
      {
        _logger.LogWarning("EditPost called for non-existant BlogPost {0}", id);

        return NotFound();
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(EditBlogPostViewModel model)
    {

      var post = await _qpa.ProcessAsync(new GetBlogPostByIdQuery { Id = model.Id });

      if (post != null && ModelState.IsValid)
      {
        var ubpc = new UpdateBlogPostCommand();

        if (post.Author == null)
        {
          var curUser = await _userManager.GetUserAsync(User);

          var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = curUser.Id });

          ubpc.NewAuthor = author;
        }
        else
        {
          ubpc.NewAuthor = post.Author; // keep the same author
        }

        ubpc.Id = model.Id;
        ubpc.NewTitle = model.Title;
        ubpc.NewDescription = model.Description;
        ubpc.NewContent = model.Content;
        ubpc.LastModifiedAt = DateTime.Now;
        ubpc.NewPublishOn = model.PublishOn;
        ubpc.NewPublic = model.Public;

        // update the post

        var ubpcResult = await _cp.ProcessAsync(ubpc);

        if (ubpcResult.Succeeded)
        {
          // do nothing
          _logger.LogInformation("Successfully updated BlogPost Id {0}", model.Id);
        }
        else
        {
          _logger.LogWarning("Unable to update BlogPost {0}", model.Id);
          // an error
          return NotFound();
        }

        List<long> topicIds = new List<long>();

        if (model.TopicsList != null)
        {
          foreach (var topic in model.TopicsList)
          {
            if (topic.IsSelected)
            {
              topicIds.Add(topic.TopicId);
            }
          }

          var addTopicsResult =
            await _cp.ProcessAsync(new SetBlogPostTopicsCommand
            {
              BlogPostId = post.Id,
              TopicIds = topicIds
            });

          if (addTopicsResult.Succeeded)
          {
            // do nothing
            _logger.LogInformation("Successfully set Topics for BlogPost {0}", model.Id);
          }
          else
          {
            // log the error
            _logger.LogInformation("Unable to set Topics for BlogPost {0}", model.Id);

            return NotFound();
          }
        }
        else
        {
          model.TopicsList = new List<TopicsCheckBox>();
        }

        List<long> categoryIds = new List<long>();

        if (model.CategoriesList != null)
        {
          foreach (var category in model.CategoriesList)
          {
            if (category.IsSelected)
            {
              categoryIds.Add(category.CategoryId);
            }
          }

          var addCategoriesResult =
            await _cp.ProcessAsync(new SetBlogPostCategoriesCommand
            {
              BlogPostId = post.Id,
              CategoryIds = categoryIds
            });

          if (addCategoriesResult.Succeeded)
          {
            // do nothing
            _logger.LogInformation("Successfully set Categories for BlogPost {0}", model.Id);
          }
          else
          {
            // log the error
            _logger.LogWarning("Unable to set Categories for BlogPost {0}", model.Id);
            return NotFound();
          }
        }
        else
        {
          model.CategoriesList = new List<CategoriesCheckBox>();
        }

        ViewData["SavedMessage"] = "Post saved.";

        return View(model); 
      }
      else
      {
        if (post == null)
        {
          _logger.LogWarning("Post to EditPost for update invoked for a non-existant BlogPost Id {0}", model.Id);
          return NotFound();
        }

        // these are needed for rendering the view when there 
        // are not categories or topics created in the engine
        if (model.CategoriesList == null)
        {
          model.CategoriesList = new List<CategoriesCheckBox>();
        }
        if (model.TopicsList == null)
        {
          model.TopicsList = new List<TopicsCheckBox>();
        }

        return View(model);        
      }
    }

    [HttpPost]
    public ContentResult GetPreviewHtml(string content)
    {
      return new ContentResult()
      {
        Content = CommonMark.CommonMarkConverter.Convert(content),
        ContentType = "text/html"
      };
    }

  }

}
