using System;
using System.Collections.Generic;
using System.Text;
using CAAS.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CAAS.Data.Query;
using CAAS.Models;
using CAAS.Data.Query.EFCoreQueryHandlers;
using CAAS.Data.Query.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CAAS.tests
{
  public class CQRSToRefactor
  {
    [Fact]
    public async void GetRecentBlogPostsTest()
    {
      Container container = new Container();

      container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

      container.Register<BlogDbContext>(() => { return new BlogDbContext(new DbContextOptionsBuilder<BlogDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options); }, Lifestyle.Scoped);

      container.Register<IQueryHandlerAsync<GetRecentBlogPostsQuery, IList<BlogPost>>, GetRecentBlogPostsEFQH>();

      container.Verify();

      using (AsyncScopedLifestyle.BeginScope(container))
      { 
        var db = container.GetInstance<BlogDbContext>();

        SeedBlogDbContext.SeedData(db);

        QueryProcessorAsync qpa = new QueryProcessorAsync(container);

        GetRecentBlogPostsQuery grbp = new GetRecentBlogPostsQuery()
        {
          NumberOfPostsToGet = 2
        };

        var x = await qpa.ProcessAsync(grbp);

        Assert.Equal(x.Count, 2);

        grbp.NumberOfPostsToGet = 3;

        x = await qpa.ProcessAsync(grbp);

        Assert.Equal(x.Count, 3);
      }
    }
  }
}
