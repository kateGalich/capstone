using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BetterBooks.Models;
using Microsoft.AspNet.Identity;

namespace BetterBooks.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(string order, string searchOrder)
        {
            var bookSort = from b in db.Books
                               .Include(book => book.Owner)
                               .Include(book => book.RequestedByUsers)
                           select b;
            ViewData["TitleSort"] = string.IsNullOrEmpty(order) ? "title_desc" : "";
            ViewData["AuthorSort"] = order == "Author" ? "Author_desc" : "Author";
            ViewData["DateSort"] = order == "DateAsc" ? "DateDesc" : "DateAsc";
            if (!String.IsNullOrEmpty(searchOrder))
            {
                bookSort = bookSort.Where(s => s.Title.Contains(searchOrder));
            }
            switch (order)
            {
                case "title_desc":
                    bookSort = bookSort.OrderByDescending(b => b.Title);
                    break;
                case "Author":
                    bookSort = bookSort.OrderBy(b => b.Author);
                    break;
                case "Author_desc":
                    bookSort = bookSort.OrderByDescending(b => b.Author);
                    break;
                case "DateAsc":
                    bookSort = bookSort.OrderBy(b => b.DateAdded);
                    break;
                case "DateDesc":
                    bookSort = bookSort.OrderByDescending(b => b.DateAdded);
                    break;
                default:
                    bookSort = bookSort.OrderBy(b => b.Title);
                    break;
            }
            return View(bookSort);
        }

        // GET: Books/Image/5
        public ActionResult Image(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            if (book.Image != null)
            {
                return File(book.Image, "image");
            }
            else
            {
                return File("~/Content/default_cover.jpg", "image");
            }
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
        public ActionResult Create([Bind(Include = "Title,Author,Year,Description,NewImage")] Book book)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                book.OwnerId = userId;
                book.DateAdded = DateTime.Now;

                if (book.NewImage != null)
                {
                    var bytes = new byte[book.NewImage.ContentLength];
                    book.NewImage.InputStream.Read(bytes, 0, bytes.Length);
                    book.Image = bytes;
                }

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
            Book book = db.Books.Find(id);
            var currentUser = User.Identity.GetUserId();
            var userID = book.OwnerId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (book == null)
            {
                return HttpNotFound();
            }
            //This code stops them from anyone but the owner from editing the book - Ajay
            if (currentUser != userID)
            {
                Response.Write("<script>alert('Unathorized access! Returning to catalogue.');window.location.href='https://betterbooks20210320133241.azurewebsites.net/';</script>");
                //return RedirectToAction("Details/" + id);
            }

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Year,Description,OwnerId,DateAdded,NewImage")] Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.NewImage != null)
                {
                    var bytes = new byte[book.NewImage.ContentLength];
                    book.NewImage.InputStream.Read(bytes, 0, bytes.Length);
                    book.Image = bytes;
                }
                else
                {
                    book.Image = db.Books.Where(b => b.Id == book.Id).Select(b => b.Image).FirstOrDefault();
                }

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
            Book book = db.Books.Find(id);
            var currentUser = User.Identity.GetUserId();
            var userID = book.OwnerId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (book == null)
            {
                return HttpNotFound();
            }
            //This code stops them from anyone but the owner from deleting the book - Ajay
            if (currentUser != userID)
            {
                Response.Write("<script>alert('Unathorized access! Returning to catalogue.');window.location.href='https://betterbooks20210320133241.azurewebsites.net/';</script>");
                //return RedirectToAction("Details/" + id);
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
            var request = book.RequestedByUsers.FirstOrDefault(bookRequest => bookRequest.UserId == userId);
            if (request != null)
            {
                book.RequestedByUsers.Remove(request);
            }
            else
            {
                book.RequestedByUsers.Add(new BookRequest
                {
                    UserId = userId,
                });

                // Initialize WebMail helper
                //WebMail.EnableSsl = true;
                //WebMail.SmtpServer = "smtp.gmail.com";
                //WebMail.SmtpPort = 587;
                //WebMail.UserName = "betterbookssite@gmail.com";
                //WebMail.Password = "q1q1q1q1q!";
                //WebMail.From = "betterbookssite@gmail.com";

                // Send email
                WebMail.Send(to: book.Owner.Email,
                    subject: "New book request",
                    body: "You have a new book request!"
                );
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
            //ViewData["book"] = id;
            //ViewData["book1"] = db.BookReviews.Find(id);
            ReviewVM model = new ReviewVM();
            BookReview review = new BookReview
            {
                BookId = id.GetValueOrDefault(),
            };
            model.BookReview = review;
            if (bookList != null)
            {
                model.BookReviews = bookList;
            }
            //if (bookList != null)
            //{
            //    return View(bookList);
            //}

            return View(model);
        }

        [HttpPost]
        public ActionResult Review(/*[Bind(Include = "Review")]*/ ReviewVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                model.BookReview.UserId = userId;
                //  model. BookReview.BookId = id;
                db.BookReviews.Add(model.BookReview);
                db.SaveChanges();
                // return RedirectToAction("Review",nameof { });
                return RedirectToAction(nameof(Review), new { id = model.BookReview.BookId });
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AcceptRequest(string userId, int bookId)
        {
            BookRequest request = db.BookRequests.Find(userId, bookId);
            if (request != null)
            {
                request.Status = "accepted";
            }
            db.SaveChanges();

            return RedirectToAction("RequestsToMe", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult RejectRequest(string userId, int bookId)
        {
            BookRequest request = db.BookRequests.Find(userId, bookId);
            if (request != null)
            {
                request.Status = "rejected";
            }
            db.SaveChanges();

            return RedirectToAction("RequestsToMe", "Manage");
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
