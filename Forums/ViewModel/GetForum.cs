using Forums.Models;

namespace Forums.ViewModel
{
    public class GetForum
    {
        public int Id { get; set; }
        public string levelName { get; set; }
        public int levelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
