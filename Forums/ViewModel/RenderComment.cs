namespace Forums.ViewModel
{
    public class RenderComment
    {
        public ICollection<GetUserForums> UserForums { get; set; }
        public bool showAllLink { get; set; }
        public bool showReply { get; set; }
    }
}
