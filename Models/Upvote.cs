﻿using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Upvote
{
    public int UserId { get; set; }

    public int IdIdea { get; set; }

    public DateOnly? UpvoteDate { get; set; }

    public virtual Idea IdIdeaNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
