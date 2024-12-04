using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheHandyGuys.Models;

namespace TheHandyGuys.Controllers
{
    public class ReviewsController : Controller
    {
        private TheHandyGuys_dbEntities db = new TheHandyGuys_dbEntities();

        // GET: Reviews
        public ActionResult Index(int skillId = 1)
        {
            if (skillId > 0)
            {
                ViewBag.reviews = db.Reviews.Where(r => r.Skill_id == skillId).ToList();
                ViewBag.skill = db.Skills.Find(skillId);
                ViewBag.rating = db.Ratings.Where(r => r.Skill_id == skillId).FirstOrDefault();
            }

            return View();
        }



        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create(int skillId, int userId)
        {
            Skill skill = db.Skills.Find(skillId);
            ViewBag.Skill = skill;
            User user = db.Users.Find(userId);
            ViewBag.user = user;

            if (user != null && skill != null)
            {
                Review review = db.Reviews.Where(r => r.user_id == userId && r.Skill_id == skillId).FirstOrDefault();
                ViewBag.review = review;
            }

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,Skill_id,review1,rating,date")] Review review)
        {
            Skill skill = db.Skills.Find(review.Skill_id);
            User user = db.Users.Find(review.user_id);
            if (ModelState.IsValid)
            {
                Rating rating = db.Ratings.Where(ra => ra.Skill_id == review.Skill_id).FirstOrDefault();
                Review checkReview = db.Reviews.Where(r => r.user_id == review.user_id).FirstOrDefault();
                if (checkReview == null)
                {
                    //If there isn't already a rating create a new one 
                    if(rating == null)
                    {
                        Rating newRating = new Rating();
                        newRating.Skill_id = review.Skill_id;
                        newRating.n_of_ratings = 1;
                        newRating.cumulated_rating = (decimal?) review.rating;
                        newRating.avg_rating = review.rating;
                        db.Ratings.Add(newRating);
                    }
                    //Else update the existing one
                    else
                    {
                        rating.n_of_ratings++;
                        rating.cumulated_rating += (decimal?)review.rating;
                        double avg = (double)(rating.cumulated_rating / rating.n_of_ratings);
                        rating.avg_rating = (double?)Math.Round(avg, 1);
                        db.Entry(rating).State = EntityState.Modified;
                    }
                    db.Reviews.Add(review);
                    ViewBag.successMessage = "Review saved successfully";
                }
                else
                {
                    
                    if (rating != null)
                    {
                        //Update the cumulated rating 
                        rating.cumulated_rating -= (decimal?) checkReview.rating;
                        rating.cumulated_rating += (decimal?) review.rating;
                        //Update the average rating 
                        double avg = (double)(rating.cumulated_rating / rating.n_of_ratings);
                        rating.avg_rating = (double?)Math.Round(avg, 1);
                        db.Entry(rating).State = EntityState.Modified;

                        //Update the already existing review.
                        checkReview.review1 = review.review1;
                        checkReview.rating = review.rating;
                        checkReview.date = review.date;

                        db.Entry(checkReview).State = EntityState.Modified;
                        ViewBag.successMessage = "Review updated successfully";
                    }

                    
                }
                //Save the changes 
                db.SaveChanges();

                ViewBag.Skill = skill;
                ViewBag.user = user;
                return View();
            }

            ViewBag.Skill = skill;
            ViewBag.user = user;
            return View();
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.Skill_id = new SelectList(db.Skills, "Skill_id", "Category", review.Skill_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "first_name", review.user_id);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,Skill_id,review1,rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Skill_id = new SelectList(db.Skills, "Skill_id", "Category", review.Skill_id);
            ViewBag.user_id = new SelectList(db.Users, "user_id", "first_name", review.user_id);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
