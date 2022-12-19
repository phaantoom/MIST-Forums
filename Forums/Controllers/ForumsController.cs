using AutoMapper;
using Forums.Core;
using Forums.Mappers;
using Forums.Models;
using Forums.StopWords;
using Forums.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;

namespace Forums.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumRepository _repo;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly UserManager<Users> _userManager;

        public ForumsController(IForumRepository repo, IMapper mapper, UserManager<Users> userManager, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unit = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            var lists = _repo.GetAllForums();
            List<GetForum> forumList = _mapper.Map<List<Forum>, List<GetForum>>(lists);
            if (forumList != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.IsInRoleAsync(user, "Admin");

                if (!roles)
                    forumList = forumList.Where(f => f.levelId <= user.levelId).ToList();
            }
            return View(forumList);
        }
        [Authorize]
        public async Task<IActionResult> GetForumAsync(int id)
        {
            var Forum = _repo.GetForum(id, true);
            Forum.UserForum = _repo.GetComments(id, 0);
            if (Forum != null)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.IsInRoleAsync(user, "Admin");

                if (roles || user.levelId >= Forum.levelId)
                {
                    ForumsWithComments ViewForum = _mapper.Map<Forum, ForumsWithComments>(Forum);
                    ViewForum.UserForum.Select(x => x.RepliesCount = _repo.GetRepliesCount(x.Id)).ToList();
                    return View(ViewForum);
                }
            }
            return RedirectToAction("Error");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult NewForum()
        {
            ViewBag.LevelsListItem = GetLevels();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult NewForum(AddForum forum)
        {
            var Newforum = new Forum
            {
                Description = forum.Description,
                Title = forum.Title,
                levelId = forum.levelId
            };
            _repo.AddForum(Newforum);
            _unit.Save();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditForum(int id)
        {
            Forum? Forum = _repo.GetForum(id, true);

            ViewBag.LevelsListItem = GetLevels();
            if (Forum != null)
            {
                EditForum editForum = _mapper.Map<Forum, EditForum>(Forum);
                return View(editForum);
            }
            return View("Error");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditForum(EditForum EditForum)
        {
            Forum Forum = _repo.GetForum(EditForum.Id, false);
            if (Forum != null)
            {
                _mapper.Map(EditForum, Forum);
                _unit.Save();
                return RedirectToAction("Index");
            }
            return View("Error");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteForum(int id)
        {
            Forum Forum = _repo.GetForum(id, false);
            if (Forum != null)
            {
                _repo.DeleteForum(Forum);
                _unit.Save();
                List<UserForum> commentList = _repo.GetComments(id, null);
                _repo.DeleteComment(commentList);
                _unit.Save();
                return Json(true);
            }
            return View(false);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddComment(int forumId, string comment)
        {
            var newComment = new UserForum
            {
                Comment = comment,
                userId = _userManager.GetUserId(User),
                forumId = forumId
            };
            _repo.AddComment(newComment);
            _unit.Save();

            newComment = _repo.GetCommentById(newComment.Id);
            GetUserForums addedComment = _mapper.Map<UserForum, GetUserForums>(newComment);

            return Json(new { data = addedComment });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddReply(int forumId, string comment, int parentId)
        {
            var newReply = new UserForum
            {
                Comment = comment,
                userId = _userManager.GetUserId(User),
                forumId = forumId,
                ParentId = parentId
            };
            _repo.AddComment(newReply);
            _unit.Save();

            return RedirectToAction("GetCommentReplies", new { parentId });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteComment(int commentId)
        {
            var Reply = _repo.GetReplies(commentId, null);
            Reply.Add(_repo.GetCommentById(commentId));
            _repo.DeleteComment(Reply);
            _unit.Save();

            return Json(true);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteReply(int commentId)
        {
            var Reply = _repo.GetCommentById(commentId);
            _repo.DeleteReply(Reply);
            _unit.Save();

            return Json(true);
        }
        [HttpGet]
        public IActionResult GetForumsComment(int forumId, int page)
        {
            var forumsList = _repo.GetComments(forumId, page);
            List<GetUserForums> getUserForums = _mapper.Map<List<UserForum>, List<GetUserForums>>(forumsList);
            getUserForums.ForEach(x => x.RepliesCount = _repo.GetRepliesCount(x.Id));

            return Json(getUserForums);
        }
        [HttpGet]
        public IActionResult GetComment(int id)
        {
            var forumsList = _repo.GetCommentById(id);
            GetUserForums getUserForums = _mapper.Map<UserForum, GetUserForums>(forumsList);
            getUserForums.RepliesCount = _repo.GetRepliesCount(getUserForums.Id);

            return View(getUserForums);
        }
        public async Task<IActionResult> SearchCommentAsync(string query)
        {
            query = StopWordsExtension.RemoveStopWords(query, "en");
            var terms = query.Split(' ');
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.IsInRoleAsync(user, "Admin");
            List<UserForum> list;
            if (roles)
                list = _repo.SearchComments(terms, 0, 0);
            else
                list = _repo.SearchComments(terms, user.levelId, 0);

            List<GetUserForums> getComments = _mapper.Map<List<UserForum>, List<GetUserForums>>(list);
            ViewBag.query = query;

            return View(getComments);
        }
        public async Task<IActionResult> GetNextSearchCommentAsync(string query, int page)
        {
            query = StopWordsExtension.RemoveStopWords(query, "en");
            var terms = query.Split(' ');
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.IsInRoleAsync(user, "Admin");
            List<UserForum> list;
            if (roles)
                list = _repo.SearchComments(terms, 0, page);
            else
                list = _repo.SearchComments(terms, user.levelId, page);

            List<GetUserForums> getComments = _mapper.Map<List<UserForum>, List<GetUserForums>>(list);

            return Json(getComments);
        }
        [HttpGet]
        public IActionResult GetCommentReplies(int parentId, int? page = 0)
        {
            var forumsList = _repo.GetReplies(parentId, page);
            List<GetUserForums> getUserForums = _mapper.Map<List<UserForum>, List<GetUserForums>>(forumsList);

            return Json(getUserForums);
        }
        [HttpGet]
        public IActionResult GetForumStatistics(int? id)
        {
            //count of comment/ count of replies/ count of engaging users.
            ForumStatisticsObj statisticsObj = new ForumStatisticsObj();
            DateTime todayDate = DateTime.Now.Date;
            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            DateTime threeMonths = DateTime.Now.AddMonths(-3).Date;

            statisticsObj.Today = new()
            {
                NoComment = _repo.GetCommentCountByDate(id, todayDate),
                NoReplies = _repo.GetReplyCountByDate(id, todayDate),
                NoUser = _repo.GetUsersCountByDate(id, todayDate)
            };
            statisticsObj.Yesterday = new()
            {
                NoComment = _repo.GetCommentCountByDate(id, yesterday),
                NoReplies = _repo.GetReplyCountByDate(id, yesterday),
                NoUser = _repo.GetUsersCountByDate(id, yesterday)
            };
            statisticsObj.LastThreeMonths = new()
            {
                NoComment = _repo.GetCommentCountByDate(id, threeMonths, false),
                NoReplies = _repo.GetReplyCountByDate(id, threeMonths, false),
                NoUser = _repo.GetUsersCountByDate(id, threeMonths, false)
            };
            statisticsObj.Total = new()
            {
                NoComment = _repo.GetCommentCountByDate(id),
                NoReplies = _repo.GetReplyCountByDate(id),
                NoUser = _repo.GetUsersCountByDate(id)
            };

            return PartialView("_ForumStatistics", statisticsObj);
        }
        [NonAction]
        public List<SelectListItem> GetLevels()
        {
            List<Level> level = _repo.GetLevels();
            List<SelectListItem> selectList = new();
            level.ForEach(l =>
            {
                selectList.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.TypeName
                });
            });
            return selectList;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
