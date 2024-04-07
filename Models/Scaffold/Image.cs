using System;
using System.Collections.Generic;

namespace aspnet2.Models.Scaffold;

public partial class Image
{
    public string Url { get; set; } = null!;

    public int PostId { get; set; }

    public virtual Post Post { get; set; } = null!;
}
