using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using TheHandyGuys.Models;

namespace TheHandyGuys.Controllers
{
    public class ChatsController : Controller
    {
        private TheHandyGuys_dbEntities db = new TheHandyGuys_dbEntities();

        private List<Chat> GetChatsByConversationId(int conversationId)
        {
            // Fetch and return the sorted list of chats
            return db.Chats
                     .Where(c => c.conv_id == conversationId)
                     .OrderBy(c => c.date_time) // Sort from oldest to newest
                     .ToList();
        }

        private List<Chat> GetLatestChatsPerConversation()
        {
            return db.Chats
                .GroupBy(chat => chat.conv_id)
                .Select(group => group.OrderByDescending(chat => chat.date_time).FirstOrDefault())
                .ToList();
        }

        private void MarkChatsAsRead(int conversationId, int otherId)
        {
            // Retrieve all chats for the given conversation ID
            var chats = db.Chats.Where(c => c.conv_id == conversationId && c.sender_id == otherId).ToList();

            // Update the isNew attribute to true for each chat
            foreach (var chat in chats)
            {
                chat.isNew = false;
                db.Entry(chat).State = EntityState.Modified;
            }

            // Save changes to the database
            db.SaveChanges();
        }


        // GET: Chats
        public ActionResult Index(int userId, int conversationId = 0)
        {
            if(conversationId != 0)
            {
                var chats = GetChatsByConversationId(conversationId);
                ViewBag.chats = chats;
                Conversation conversation = db.Conversations.Find(conversationId);
                if (conversation == null)
                {
                    return HttpNotFound();
                }
                ViewBag.conversation = conversation;
            }
            ViewBag.userId = userId;
            ViewBag.latestChats = GetLatestChatsPerConversation();
            return View();
        }

        // GET: Chats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            ViewBag.conv_id = new SelectList(db.Conversations, "conv_id", "conv_id");
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "conv_id,sender_id,message,date_time,isNew")] Chat chat)
        {
            var chats = GetChatsByConversationId(Convert.ToInt32(chat.conv_id));
            Conversation conversation = db.Conversations.Find(chat.conv_id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            int otherId;
            if (conversation.User1.user_id == chat.sender_id)
            {
                otherId = conversation.User.user_id;
            }
            else
            {
                otherId = conversation.User1.user_id;
            }

            if (chat.message != null)
            {
                if (ModelState.IsValid && !chat.message.Trim().IsEmpty())
                {
                    MarkChatsAsRead(Convert.ToInt32(chat.conv_id), otherId);

                    db.Chats.Add(chat);
                    db.SaveChanges();

                    ViewBag.message = "saved";
                    chats = GetChatsByConversationId(Convert.ToInt32(chat.conv_id));

                    ViewBag.chats = chats;
                    ViewBag.latestChats = GetLatestChatsPerConversation();
                    ViewBag.conversation = conversation;
                    return View();
                }
            }
            

            //ViewBag.conv_id = new SelectList(db.Conversations, "conv_id", "conv_id", chat.conv_id);
            ViewBag.message = "not saved";
            ViewBag.conversation = conversation;
            ViewBag.chats = chats;
            ViewBag.latestChats = GetLatestChatsPerConversation();
            ViewBag.userId = chat.sender_id;
            return View();
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            ViewBag.conv_id = new SelectList(db.Conversations, "conv_id", "conv_id", chat.conv_id);
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "message_id,conv_id,sender_id,message,date_time")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.conv_id = new SelectList(db.Conversations, "conv_id", "conv_id", chat.conv_id);
            return View(chat);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chats.Find(id);
            db.Chats.Remove(chat);
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
