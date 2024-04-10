using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Upvote
{
    public int UserId { get; set; }

    public int IdIdea { get; set; }

    /// <summary>
    /// vai automaticamente gerar data atual quando linha for inserida
    /// </summary>
    public DateOnly? UpvoteDate { get; set; }

    public virtual Idea IdIdeaNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
