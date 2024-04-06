using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Idea
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public int PostId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual User User { get; set; } = null!;
}
