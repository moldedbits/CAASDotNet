using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CAAS.EFCore;

namespace CAAS.Data.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CAAS.Models.Author", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Biography");

                    b.Property<string>("Blurb");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("ImageLink");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("CAAS.Models.BlogPost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<bool>("Public");

                    b.Property<DateTime>("PublishOn");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("BlogPost");
                });

            modelBuilder.Entity("CAAS.Models.BlogPostCategory", b =>
                {
                    b.Property<long>("BlogPostId");

                    b.Property<long>("CategoryId");

                    b.HasKey("BlogPostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BlogPostCategory");
                });

            modelBuilder.Entity("CAAS.Models.BlogPostTopic", b =>
                {
                    b.Property<long>("TopicId");

                    b.Property<long>("BlogPostId");

                    b.HasKey("TopicId", "BlogPostId");

                    b.HasIndex("BlogPostId");

                    b.ToTable("BlogPostTopic");
                });

            modelBuilder.Entity("CAAS.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("CAAS.Models.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("ShowOnHomePage");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("CAAS.Models.BlogPost", b =>
                {
                    b.HasOne("CAAS.Models.Author", "Author")
                        .WithMany("BlogPosts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("CAAS.Models.BlogPostCategory", b =>
                {
                    b.HasOne("CAAS.Models.BlogPost", "BlogPost")
                        .WithMany("BlogPostCategory")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAAS.Models.Category", "Category")
                        .WithMany("BlogPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAAS.Models.BlogPostTopic", b =>
                {
                    b.HasOne("CAAS.Models.BlogPost", "BlogPost")
                        .WithMany("BlogPostTopic")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAAS.Models.Topic", "Topic")
                        .WithMany("BlogPosts")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
