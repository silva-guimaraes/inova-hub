using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Idea
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int IdUser { get; set; }

    public string Text { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();
}
