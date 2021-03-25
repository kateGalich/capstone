using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BetterBooks.Models;
using Microsoft.AspNet.Identity;

namespace BetterBooks.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Title,Author,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                book.OwnerId = userId;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Description,OwnerId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST: Books/RequestBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult RequestBook(int id, string returnAction, string returnController = null, int? returnId = null)
        {
            Book book = db.Books.Find(id);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (book.RequestedByUsers.Contains(user))
            {
                book.RequestedByUsers.Remove(user);
            }
            else
            {
                book.RequestedByUsers.Add(user);
            }
            db.SaveChanges();

            if (returnId.HasValue)
            {
                return RedirectToAction(returnAction, returnController, new { id = returnId.Value });
            }
            else
            {
                return RedirectToAction(returnAction, returnController);
            }
        }

        [HttpGet]
        // Please Use this contrroller for review Action 
        public ActionResult Review(int? id)
        {
            //BookReview bookReview = db.BookReviews.Find(id);
            //dynamic myModel = new ExpandoObject();
            //myModel.bookList = db.BookReviews.Where(p => p.BookId == id).ToList();
            List<BookReview> bookList = db.BookReviews.Where(p => p.BookId == id).ToList();
            ViewData["book"] = id;
            ViewData["book1"] = db.BookReviews.Find(id);

            if (bookList != null)
            {
                return View(bookList);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Review([Bind(Include = "Review")] BookReview bookReview, int id)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                bookReview.UserId = userId;
                bookReview.BookId = id;
                db.BookReviews.Add(bookReview);
                db.SaveChanges();
                return RedirectToAction("Review");
            }

            return View();
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
