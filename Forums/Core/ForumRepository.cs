using Forums.Models;
using Forums.StopWords;
using Forums.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forums.Core
{
    public class ForumRepository : IForumRepository
    {
        private readonly ProjectContext _context;

        public ForumRepository(ProjectContext context)
        {
            _context = context;
        }
        public void AddForum(Forum forum)
        {
            _context.Forums.Add(forum);
        }

        public List<Forum> GetAllForums()
        {
            return _context.Forums
                .Include(f => f.level)
                .ToList();
        }
        public void DeleteForum(Forum forum)
        {
            _context.Forums.Remove(forum);
        }
        public Forum GetForum(int id, bool levels, bool comments = false)
        {
            if (levels && comments)
            {
                return _context.Forums
                .Where(x => x.Id == id)
                .Include(x => x.level)
                .Include(x => x.UserForum)
                .SingleOrDefault();
            }
            else if (levels && !comments)
            {
                return _context.Forums
                .Where(f => f.Id == id)
                .Include(x => x.level)
                .SingleOrDefault();
            }
            return _context.Forums
                .Where(f => f.Id == id)
                .SingleOrDefault();
        }
        public List<UserForum> GetComments(int forumId, int? page, int count = 3)
        {
            if (!page.HasValue)
            {
                return _context.UserForums.Where(x => x.forumId == forumId && x.ParentId == null)
                .Include(c => c.Replies)
                .ToList();
            }
            var list = _context.UserForums.Where(x => x.forumId == forumId && x.ParentId == null)
                .Include(c => c.Replies)
                .Include(x => x.user)
                .OrderByDescending(x => x.Id)
                .Skip(count * page.Value).Take(count).ToList();
            foreach (var comment in list)
            {
                comment.Replies = comment.Replies.OrderByDescending(x => x.Id).Take(count).ToList();
            }
            return list;
        }
        public int GetRepliesCount(int ParentId)
        {
            var Comments = _context.UserForums.Where(x => x.ParentId == ParentId).ToList();
            return Comments.Count;
        }
        public UserForum GetCommentById(int commentId)
        {
            return _context.UserForums.Where(x => x.Id == commentId).Include(x => x.user).SingleOrDefault();
        }
        public void AddComment(UserForum userForum)
        {
            _context.UserForums.Add(userForum);
        }
        public List<Level> GetLevels()
        {
            return _context.levels.ToList();
        }

        public List<UserForum> GetReplies(int parentId, int? page, int count = 3)
        {
            if (!page.HasValue)
            {
                return _context.UserForums.Where(x => x.ParentId == parentId).Include(x => x.user).ToList();
            }
            else
            {
                return _context.UserForums
                    .Where(x => x.ParentId == parentId)
                    .Include(x => x.user)
                    .OrderByDescending(x => x.Id)
                    .Skip(page.Value * count).Take(count)
                    .ToList();
            }
        }
        public void DeleteComment(List<UserForum> userForum)
        {
            _context.RemoveRange(userForum);
        }
        public void DeleteReply(UserForum userForum)
        {
            _context.Remove(userForum);
        }

        public int GetReplyCountByDate(int? id, DateTime? date, bool Equal = true)
        {
            if (!id.HasValue)
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId != null && x.CreatedDate.Date == date)
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId != null && x.CreatedDate.Date >= date)
                        .Count();
                }
                else
                    return _context.UserForums
                        .Where(x => x.ParentId != null)
                        .Count();
            }
            else
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId != null && x.CreatedDate.Date == date && x.forumId == id)
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId != null && x.CreatedDate.Date >= date && x.forumId == id)
                        .Count();
                }
                else
                    return _context.UserForums
                        .Where(x => x.ParentId != null && x.forumId == id)
                        .Count();
            }
        }

        public int GetCommentCountByDate(int? id, DateTime? date, bool Equal = true)
        {
            if (!id.HasValue)
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId == null && x.CreatedDate.Date == date.Value)
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId == null && x.CreatedDate.Date >= date.Value)
                        .Count();
                }
                else
                    return _context.UserForums
                        .Where(x => x.ParentId == null)
                        .Count();
            }
            else
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId == null && x.CreatedDate.Date == date.Value && x.forumId == id)
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.ParentId == null && x.CreatedDate.Date >= date.Value && x.forumId == id)
                        .Count();
                }
                else
                    return _context.UserForums
                        .Where(x => x.ParentId == null && x.forumId == id)
                        .Count();
            }

        }

        public int GetUsersCountByDate(int? id, DateTime? date, bool Equal = true)
        {
            if (!id.HasValue)
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.CreatedDate.Date == date)
                        .Select(x => x.userId).Distinct()
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.CreatedDate.Date >= date)
                        .Select(x => x.userId).Distinct()
                        .Count();
                }
                else
                    return _context.UserForums
                        .Select(x => x.userId).Distinct()
                        .Count();
            }
            else
            {
                if (date.HasValue && Equal)
                {
                    return _context.UserForums
                        .Where(x => x.CreatedDate.Date == date && x.forumId == id)
                        .Select(x => x.userId).Distinct()
                        .Count();
                }
                else if (date.HasValue && !Equal)
                {
                    return _context.UserForums
                        .Where(x => x.CreatedDate.Date >= date && x.forumId == id)
                        .Select(x => x.userId).Distinct()
                        .Count();
                }
                else
                    return _context.UserForums
                        .Where(x => x.forumId == id)
                        .Select(x => x.userId).Distinct()
                        .Count();
            }
        }
        public List<UserForum> SearchComments(string[] query, int userLevel, int page, int count = 3)
        {
            IEnumerable<UserForum> list = new List<UserForum>();
            Expression<Func<UserForum, bool>> expression = x => true;
            if (userLevel == 0)
            {
                foreach (string term in query)
                {
                    list = list.Concat(
                    (IEnumerable<UserForum>)_context.UserForums
                    .Include(x => x.forum)
                    .Include(c => c.Replies)
                    .Include(x => x.user)
                    .Where(x => x.Comment.Contains(term) && x.ParentId == null)
                    ).ToList();
                }
            }
            else
            {
                foreach (string term in query)
                {
                    list = list.Concat(
                    (IEnumerable<UserForum>)_context.UserForums
                    .Include(x => x.forum)
                    .Include(c => c.Replies)
                    .Include(x => x.user)
                    .Where(x => x.Comment.Contains(term) && x.ParentId == null && x.forum.levelId <= userLevel)
                    ).ToList();
                }
            }
            list = list.OrderByDescending(x => x.Id)
                .Skip(count * page).Take(count);

            foreach (var comment in list)
            {
                comment.Replies = comment.Replies.OrderByDescending(x => x.Id).Take(count).ToList();
            }

            return list.ToList();
        }
    }
}
