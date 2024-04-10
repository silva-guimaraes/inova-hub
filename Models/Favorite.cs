using System;
using System.Collections.Generic;

namespace aspnet2.Models;

public partial class Favorite
{
    public int IdUser { get; set; }

    public int IdIdea { get; set; }

    /// <summary>
    /// vai automaticamente gerar data atual quando linha for inserida
    /// </summary>
    public DateOnly FavoriteDate { get; set; }

    public virtual Idea IdIdeaNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
