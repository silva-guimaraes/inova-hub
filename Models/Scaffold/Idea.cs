using System;
using System.Collections.Generic;

namespace aspnet2.Models.Scaffold;

public partial class Idea
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// idea é uma subclasse de post
    /// </summary>
    public int PostId { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
