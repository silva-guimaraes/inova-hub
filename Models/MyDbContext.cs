using Microsoft.EntityFrameworkCore;

namespace aspnet2.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext() { }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Idea> Ideas { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Upvote> Upvotes { get; set; }

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdIdea }).HasName("favorite_pk");
            entity.ToTable("favorite");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdIdea).HasColumnName("id_idea");
            entity.Property(e => e.FavoriteDate)
                .HasDefaultValueSql("CURRENT_DATE")
                // mentira!!!!!!!
                .HasComment("vai automaticamente gerar data atual quando linha for inserida")
                .HasColumnName("favorite_date");
            entity.HasOne(d => d.IdIdeaNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.IdIdea)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("idea_fk");
            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_fk");
        });

        modelBuilder.Entity<Idea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idea_pk");
            entity.ToTable("idea");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Text)
                .HasMaxLength(4000)
                .HasColumnName("text");
            entity.Property(e => e.Content)
                .HasMaxLength(10000)
                .HasColumnName("content");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .HasColumnName("title");
            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_fk");
            entity.HasMany(e => e.Images)
                .WithOne(e => e.Idea)
                .HasForeignKey(e => e.IdeaId)
                .IsRequired();
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => new { e.Url }).HasName("image_pkey");
            entity.ToTable("image");
            entity.Property(e => e.Url)
                .HasMaxLength(256)
                .HasColumnName("url");
            entity.Property(e => e.IdeaId).HasColumnName("idea_id");
            entity.HasOne(e => e.Idea)
                .WithMany(e => e.Images)
                .HasForeignKey(e => e.IdeaId)
                .IsRequired();
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
            entity.HasOne(d => d.Idea).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdeaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_idea_id_fkey");
        });

        modelBuilder.Entity<Upvote>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.IdIdea }).HasName("upvote_pk");
            entity.ToTable("upvote");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IdIdea).HasColumnName("id_idea");
            entity.Property(e => e.UpvoteDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasComment("vai automaticamente gerar data atual quando linha for inserida")
                .HasColumnName("upvote_date");
            entity.HasOne(d => d.IdIdeaNavigation).WithMany(p => p.Upvotes)
                .HasForeignKey(d => d.IdIdea)
                .HasConstraintName("idea_fk");
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
        modelBuilder.HasSequence("comment_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("idea_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("post_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("user_id_seq").HasMax(2147483647L);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
