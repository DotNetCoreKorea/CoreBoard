using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreBoard.Models.Request;
using CoreBoard.Services;
using CoreBoard.Models.Data;
using CoreBoard.Helpers;
using CoreBoard.Models.View;
using CoreBoard.Extensions;

namespace CoreBoard.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ISessionService _sessionService;

        public PostController(IPostService postService, ISessionService sessionService)
        {
            _postService = postService;
            _sessionService = sessionService;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.ListPostAsync();

            return View(posts);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostAsync(id);
            
            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostPostModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var post = await _postService.CreatePostAsync(await _sessionService.GetUserAsync(), model.Title, model.Content, model.Password, model.WriterName);

                return RedirectToAction("Details", new {id = post.Id});
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            var authorizedPost = HttpContext.Session.GetInt64("AuthorizedPost");
            var user = await _sessionService.GetUserAsync();
            
            if (post.WriterId != null && (user == null || post.WriterId != user.Id))
                return RedirectToAction("Login", "Auth", new { returnUrl = Request.Path });

            if (post.WriterId == null && authorizedPost != post.Id)
                return RedirectToAction("EditAnonymous", new { id });

            var model = new PostPostModel
            {
                Title = post.Title,
                Content = post.Content,
                Password = post.Password,
                WriterName = post.WriterName
            };

            return View(model);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostPostModel model)
        {
            try
            {
                var post = await _postService.GetPostAsync(id);

                if (post == null)
                    return RedirectToAction("Index");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var authorizedPost = HttpContext.Session.GetInt64("AuthorizedPost");
                var user = await _sessionService.GetUserAsync();

                if (post.WriterId == null && authorizedPost != post.Id)
                    return RedirectToAction("EditAnonymous", new { id });

                if (post.WriterId == user?.Id && authorizedPost == post.Id)
                {
                    await _postService.UpdatePostAsync(id, model.Title, model.Content, model.Password);
                    HttpContext.Session.Remove("AuthorizedPost");
                }
                else
                {
                    return View("Error", new ErrorViewModel { Message = "������ ������ �����ϴ�", RedirectUrl = Url.Action("Index") });
                }

                return RedirectToAction("Details", new { id });
            }
            catch
            {
                return View(model);
            }
        }

        public async Task<IActionResult> EditAnonymous(long id)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            if (post.WriterId != null)
                return RedirectToAction("Edit", new { id });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAnonymous(long id, string password)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            if (post.WriterId != null)
                return RedirectToAction("Edit", new { id });

            if (Crypto.VerifyHashedPassword(post.Password, password))
            {
                HttpContext.Session.SetInt64("AuthorizedPost", id);
            }

            return RedirectToAction("Edit", new { id });
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            var authorizedPost = HttpContext.Session.GetInt64("AuthorizedPost");
            var user = await _sessionService.GetUserAsync();
            

            if (post.WriterId != null && (user == null || post.WriterId != user.Id))
                return RedirectToAction("Login", "Auth", new { returnUrl = Request.Path });

            if (post.WriterId == null && authorizedPost != post.Id)
                return RedirectToAction("DeleteAnonymous", new { id });

            await _postService.DeletePostAsync(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeieteAnonymous(long id)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            if (post.WriterId != null)
                return RedirectToAction("Delete", new { id });

            return View("EditAnonymous");
        }

        [HttpPost]
        public async Task<IActionResult> DeieteAnonymous(long id, string password)
        {
            var post = await _postService.GetPostAsync(id);

            if (post == null)
                return RedirectToAction("Index");

            if (post.WriterId != null)
                return RedirectToAction("Deiete", new { id });

            if (Crypto.VerifyHashedPassword(post.Password, password))
            {
                HttpContext.Session.SetInt64("AuthorizedPost", id);
            }

            return RedirectToAction("Delete", new { id });
        }
    }
}