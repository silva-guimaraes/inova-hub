using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Post
{
    public int IdeaId { get; set; }

    public int Id { get; set; }

    public string? Text { get; set; }

    public DateOnly CreationDate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Idea Idea { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
