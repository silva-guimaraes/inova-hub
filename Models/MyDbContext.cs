using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace aspnet2.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Idea> Ideas { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Upvote> Upvotes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=my_db;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comment_pkey");

            entity.ToTable("comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Text)
                .HasMaxLength(4000)
                .HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_post_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_user_id_fkey");
        });

        modelBuilder.Entity<Idea>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.UserId }).HasName("idea_pkey");

            entity.ToTable("idea");

            entity.HasIndex(e => e.PostId, "header").IsUnique();

            entity.HasIndex(e => e.Id, "idea_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .HasColumnName("title");

            entity.HasOne(d => d.Post).WithOne(p => p.Idea)
                .HasPrincipalKey<Post>(p => p.Id)
                .HasForeignKey<Idea>(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("posts");

            entity.HasOne(d => d.User).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("creator");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => new { e.Url, e.PostId }).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.Url)
                .HasMaxLength(256)
                .HasColumnName("url");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Images)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("image_post_id_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => new { e.IdeaId, e.Id }).HasName("post_pkey");

            entity.ToTable("post");

            entity.HasIndex(e => e.Id, "post_id_key").IsUnique();

            entity.Property(e => e.IdeaId).HasColumnName("idea_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Text)
                .HasMaxLength(4000)
                .HasColumnName("text");

            entity.HasOne(d => d.IdeaNavigation).WithMany(p => p.Posts)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.IdeaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_idea_id_fkey");
        });

        modelBuilder.Entity<Upvote>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PostId }).HasName("upvote_pkey");

            entity.ToTable("upvote");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UpvoteDate).HasColumnName("upvote_date");

            entity.HasOne(d => d.Post).WithMany(p => p.Upvotes)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("upvote_post_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Upvotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("upvote_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Name, "user_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
