using System;
using System.Collections.Generic;

namespace aspnet2.Models.Scaffold;

public partial class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
