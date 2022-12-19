using Forums.Models;
using Forums.ViewModel;

namespace Forums.Core
{
    public interface IForumRepository
    {
        public void AddForum(Forum forum);
        public void DeleteForum(Forum forum);
        public List<Forum> GetAllForums();
        public Forum GetForum(int id, bool levels, bool comments = false);
        public List<Level> GetLevels();
        public void AddComment(UserForum userForum);
        public void DeleteComment(List<UserForum> userForum);
        public List<UserForum> GetComments(int forumId, int? page, int count = 3);
        public List<UserForum> SearchComments(string[] query, int userLevel, int page, int count = 3);
        public UserForum GetCommentById(int commentId);
        public List<UserForum> GetReplies(int parentId, int? page, int count = 3);
        public int GetRepliesCount(int parentId);
        public void DeleteReply(UserForum userForum);
        public int GetReplyCountByDate(int? id, DateTime? date = null, bool Equal = true);
        public int GetCommentCountByDate(int? id, DateTime? date = null, bool Equal = true);
        public int GetUsersCountByDate(int? id, DateTime? date = null, bool Equal = true);
    }
}
