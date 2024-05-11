
namespace aspnet2.Models;

public partial class Image
{
    public string Url { get; set; } = null!;

    public int IdeaId { get; set; }

    public virtual Idea Idea { get; set; } = null!;
}
