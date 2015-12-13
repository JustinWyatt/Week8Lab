using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Week8Lab.Models;

namespace Week8Lab.Controllers
{
    public class PostsController : Controller
    {
        private RedditContext db = new RedditContext();

        [HttpGet]
        public ActionResult UserRegistrationForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser()
        {
            return RedirectToAction();
        }

        [HttpGet]
        public ActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            return RedirectToAction();
        }

        [HttpGet]
        public PartialViewResult GetComments()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateComment(CommentVM comment)
        {
            var post = db.Posts.Find(comment.Post.PostId);
            return Content();
        }

        [HttpPost]
        public ActionResult UpVote(int postid)
        {
            var post = db.Posts.Find(postid);
            post.Upvote ++;
            post.Rank = post.Upvote - post.Downvote;
            db.SaveChanges();
            return Content(post.Rank.ToString());
        }

        [HttpPost]
        public ActionResult DownVote(int postid)
        {
            var post = db.Posts.Find(postid);
            post.Downvote ++;
            post.Rank = post.Upvote - post.Downvote;

            db.SaveChanges();
            return Content(post.Rank.ToString());
        }

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostVM post)
        {
            var newPost = new Post()
            {
                Title = post.Title,
                Body = post.Body,
                PostId = db.Posts.Max(x => x.PostId) + 1,
                DatePosted = DateTime.Now,
                DateUpdated = DateTime.Now,
                Url = post.Url

            };
            if (ModelState.IsValid)
            {
                db.Posts.Add(newPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            post.DateUpdated = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
