using Exernet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Exernet.Filters;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text.RegularExpressions;
using Exernet.Code;
using Exernet.Search;
using System.Net.Mail;
using System.IO;

namespace Exernet.Controllers
{
    [Culture]
    public class TaskController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        public ActionResult CreateTask(String input)
        {
            var md = new MarkdownDeep.Markdown();
            // Set options
            md.ExtraMode = true;
            md.SafeMode = false;
            string output = md.Transform(input);
            return View();
        }

        [HttpPost]
        public ActionResult CreateTask(ExernetTaskViewModel model, IEnumerable<HttpPostedFileBase> Images, string returnUrl)
        {
            ExernetTask task = new ExernetTask(); ;
            if (model.Id != 0)
            {
                task = db.Tasks.Where(a => a.Id.Equals(model.Id)).First();
                task.Tags.Clear();
                task.Answers.Clear();
                task.Videos.Clear();
            }
            task.Text = model.Text;
            task.Tags = GenerateTagsForTaskModel(model.Tags);
            task.Answers = GenerateAnswersForTaskModel(model.Answers);
            if (model.Videos != null)
            {
                task.Videos = GenerateVideosForTaskModel(model.Videos);
            }
            if((model.Expression !=null) && (model.To !=null) && (model.From !=null) && (model.Step !=null))
            {
                task.Chart = createGraphic(model);
            }
            task.Formulas = GenerateFormulasForTaskModel(model.Formulas);
            task.Title = model.Title;
            task.Category = model.Category;
            task.Block = true;
            task.UserId = User.Identity.GetUserId();
            task.UploadDate = DateTime.Now;
            if (model.Id == 0)
            {
                task.Images = UploadPicturesOnCloudinary(Images);
                db.Tasks.Add(task);
                db.Entry(task).State = EntityState.Added;
            }
            else
            {
                db.Entry(task).State = EntityState.Modified;
            }
            db.SaveChanges();

            return RedirectToAction("PostTask", new { id = GetId(model) });
        }

        private Chart createGraphic(ExernetTaskViewModel model)
        {
            var chart = new Chart();
            chart.Expression = model.Expression;
            chart.From = model.From;
            chart.To = model.To;
            chart.Step = model.Step;

            return chart;
 
        }
        private ICollection<Formula> GenerateFormulasForTaskModel(IEnumerable<string> formulasUrls)
        {
            List<Formula> formulas = new List<Formula>();
            if (formulasUrls != null)
            {
                foreach (var formula in formulasUrls)
                {
                    formulas.Add(new Formula() { FormulaURL = formula });
                }
                return formulas;
            }
            else return null;
        }

        private string[] parseForVideo(string[] listOfVideos)
        {
            string pattern = @".+?/?v=";
            string replacement1 = "//www.youtube.com/embed/";
            for (int i = 0; i < listOfVideos.Length; i++)
            {
                listOfVideos[i] = Regex.Replace(listOfVideos[i], pattern, replacement1);
            }

            return listOfVideos;
        }
        private ICollection<Video> GenerateVideosForTaskModel(string p)
        {
            if ((p != null) && p.Length > 0)
            {
                string[] listOfVideos = SplitString(p);
                listOfVideos = parseForVideo(listOfVideos);
                List<Video> Videos = new List<Video>();
                foreach (var str in listOfVideos)
                {
                    var Video = new Video();
                    Video.VideoURL = str;
                    Videos.Add(Video);
                }
                return Videos;
            }
            else
            {
                return null;
            }
        }

        private int GetId(ExernetTaskViewModel model)
        {
            int id;
            if (model.Id != 0)
            {
                id = model.Id;
            }
            else
            {
                id = db.Tasks.Local[0].Id;
            }
            return id;
        }

        private ICollection<Tag> GenerateTagsForTaskModel(string tags)
        {
            string[] listOfTags = SplitString(tags);

            var listOfTagsWithoutDuplicate = listOfTags.Distinct();

            List<Tag> Tags = new List<Tag>();

            foreach (string str in listOfTagsWithoutDuplicate)
            {
                var tag = db.Tags.FirstOrDefault(obj => obj.Text == str);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Text = str;
                }
                Tags.Add(tag);
            }
            return Tags;
        }

        private string[] SplitString(string term)
        {
            string[] str = term.Split(new string[] { ", ", ",", "; ", ";" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < str.Length; i++)
            {
                str[i] = str[i].ToString().Trim();
            }
            return str;
        }


        private ICollection<Answer> GenerateAnswersForTaskModel(string term)
        {
            string[] listOfTags = SplitString(term);

            List<Answer> answers = new List<Answer>();

            foreach (string str in listOfTags)
            {
                Answer answer = new Answer();
                answer.Text = str;
                answers.Add(answer);
            }
            return answers;
        }

        public ActionResult PostTask(int id)
        {
            var task = new ExernetTask();
            task = db.Tasks.Find(id);
            return View(task);
        }

        public ActionResult EditTask(int id)
        {
            var model = db.Tasks.Find(id);
            ExernetTaskViewModel task = new ExernetTaskViewModel();

            task.Text = model.Text;
            task.Tags = GenerateStringTagsForTaskModel(model.Tags);
            task.Answers = GenerateStringAnswersForTaskModel(model.Answers);
            task.Videos = GenerateStringVideosForTaskModel(model.Videos);
            task.Title = model.Title;
            task.Category = model.Category;
            task.Id = id;
            return View(task);
        }

        private static string GenerateStringVideosForTaskModel(ICollection<Video> collection)
        {
            string stringOfVideos = string.Join("; ", collection.Select(obj => obj.VideoURL));
            return stringOfVideos;
        }

        private static string GenerateStringAnswersForTaskModel(ICollection<Answer> collection)
        {
            string stringOfAnswers = string.Join("; ", collection.Select(obj => obj.Text));
            return stringOfAnswers;
        }

        public static string GenerateStringTagsForTaskModel(ICollection<Tag> collection)
        {
            string stringOfTags = string.Join(", ", collection.Select(obj => obj.Text));

            return stringOfTags;
        }

        public static string GenerateStringCommentsForTaskModel(ICollection<Comment> collection)
        {
            string stringOfTags = string.Join(", ", collection.Select(obj => obj.Text));

            return stringOfTags;
        }

        public JsonResult TagSearch(string term)
        {

            var tags = db.Tags.Select(obj => obj.Text);

            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        private List<Tag> GetTag(string searchString)
        {
            var tags = db.Tags.Where(a => a.Text.Contains(searchString)).ToList();
            return tags;

        }

        public ActionResult SetLike(int id, bool likeState)
        {
            var like = db.Tasks.Find(id).Likes.FirstOrDefault(obj => obj.User.UserName.Equals(User.Identity.Name));
            if (like == null)
            {
                like = new Like();
                like.Type = likeState;
                like.UserId = User.Identity.GetUserId();
                db.Tasks.Find(id).Likes.Add(like);
                db.Entry(db.Tasks.Find(id).Likes.FirstOrDefault(obj => obj.UserId.Equals(User.Identity.GetUserId()))).State = EntityState.Added;

            }
            else
            {
                if (like.Type == !likeState)
                {
                    like.Type = !like.Type;
                    foreach (var item in db.Tasks.Find(id).Likes)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
                else
                {
                    db.Tasks.Find(id).Likes.Remove(like);
                    foreach (var item in db.Tasks.Find(id).Likes)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }

            }
            db.SaveChanges();
            CountRating(id);
            return PartialView(db.Tasks.Find(id));
        }

        public ActionResult GetTag()
        {
            var listOfTags = db.Tags.OrderByDescending(obj => obj.Tasks.Count).Take(25);
            return PartialView(listOfTags);
        }

        private List<Image> UploadPicturesOnCloudinary(IEnumerable<HttpPostedFileBase> pictures)
        {
            if (pictures == null) return null;
            List<Image> PictureUrls = new List<Image>();
            Cloudinary cloudinary = new Cloudinary(new Account(
           "goodcloud",
           "836668373272998",
           "HJ2Q7oe53Ru7muxKcpVj4ZdqVPQ"));
            foreach (var pic in pictures)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(pic.FileName, pic.InputStream),
                    Folder = "Exernet/TaskPictures"
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                var uplPath = uploadResult.Uri.AbsoluteUri;
                PictureUrls.Add(new Image() { ImageURL = uplPath });
            }
            return PictureUrls;
        }
        public ActionResult SolveTask(int id, string solveTry)
        {
            var solution = db.Tasks.Find(id).Solutions.FirstOrDefault(obj => obj.User.UserName.Equals(User.Identity.Name));
            var answer = db.Tasks.Find(id).Answers.FirstOrDefault(obj => obj.Text.Equals(solveTry));
            if (solution == null)
            {
                solution = new Solution();
                solution.UploadDate = DateTime.Now;
                solution.Correct = false;
                solution.UserId = User.Identity.GetUserId();
                db.Tasks.Find(id).Solutions.Add(solution);
                db.Entry(db.Tasks.Find(id).Solutions.FirstOrDefault(obj => obj.UserId.Equals(User.Identity.GetUserId()))).State = EntityState.Added;
                db.SaveChanges();
                solution = db.Tasks.Find(id).Solutions.FirstOrDefault(obj => obj.UserId.Equals(User.Identity.GetUserId()));
            }
            if (answer != null)
            {
                solution.Correct = true;
                db.Entry(db.Tasks.Find(id).Solutions.FirstOrDefault(obj => obj.UserId.Equals(User.Identity.GetUserId()))).State = EntityState.Modified;
                db.SaveChanges();
                CountRating();
            }
            return PartialView(solution);
        }

        public void CountrPopularity(int id)
        {
            var task = db.Tasks.Find(id);
            task.Popularity = task.Likes + task.Solutions.Count();
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void CountRating()
        {
            var users = db.Users.OrderByDescending(obj => obj.Solutions.Count);
            int i = 1;
            foreach(var user in users)
            {
                user.Rating = i++;
                db.Entry(user).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public ActionResult CheckSolve(int id)
        {
            var solution = db.Tasks.Find(id).Solutions.FirstOrDefault(obj => obj.User.UserName.Equals(User.Identity.Name));

            if (solution != null && solution.Correct)
            {
                return PartialView("Solved");
            }
            else
            {
                return PartialView("Unsolved", id);
            }

        }

        public ActionResult LeaveComment(int id, string commentText)
        {
            var comment = new Comment();
            comment.Date = DateTime.Now;
            comment.Text = commentText;
            comment.UserId = User.Identity.GetUserId();
            db.Tasks.Find(id).Comments.Add(comment);


            db.Entry(db.Tasks.Find(id).Comments.FirstOrDefault(obj => obj.UserId.Equals(User.Identity.GetUserId()) && obj.Date.Equals(comment.Date))).State = EntityState.Added;
            db.SaveChanges();
            var c = db.Comments.Include("User").FirstOrDefault(obj => obj.Id == comment.Id);

            
            return PartialView("Comment", c);
        }

        public ActionResult SaveFormula(string formulaURL) 
        {
            return PartialView("Formula");
        }

        public List<ExernetTask> GetFewTasksForPartialView(int BlockNumber, int BlockSize)
        {
            int startIndex = (BlockNumber - 1) * BlockSize;
            return null;
        }

        public ActionResult FullTextSearching(string searchText) 
        {
            LuceneSearch.ClearLuceneIndex();
            LuceneSearch.AddUpdateLuceneIndex(db.Tasks.Where(obj => obj.Id > 0));

            var result = LuceneSearch.Search(searchText);
            var results = new List<ExernetTask>();
            foreach (var task in result)
            {
                results.Add(db.Tasks.First(obj => obj.Id == task.Id));
            }
            results = results.OrderByDescending(obj => obj.UploadDate).ToList();

            ViewBag.SearchText = searchText;
            return View(results);
        }

        public ActionResult ShowTags(string tag)
        {
            var results = db.Tasks.OrderByDescending(x => x.UploadDate).Where(obj => obj.Tags.FirstOrDefault(t => t.Text.Equals(tag)) != null);

            ViewBag.TagSearch = tag;
            return View(results.ToList());
        }

        [HttpPost]
        public JsonResult DeleteTask(string id)
        {
            var tsk = db.Tasks.Find(Int32.Parse(id));
            if (tsk != null)
            {
                ClearTask(tsk);
                db.Tasks.Remove(tsk);
                db.Entry(tsk).State = EntityState.Deleted;
                db.SaveChanges();
                return new JsonResult() { Data = id };
            }
            else return null;
        }
        [HttpPost]
        public JsonResult DeleteComment(string id)
        {
            var coment = db.Comments.Find(Int32.Parse(id));
            if (coment != null)
            {
                db.Comments.Remove(coment);
                db.Entry(coment).State = EntityState.Deleted;
                db.SaveChanges();
                return new JsonResult() { Data = id };
            }
            else return null;
        }

        private void ClearTask(ExernetTask tsk)
        {
            tsk.Answers.Clear();
            tsk.Comments.Clear();
            tsk.Formulas.Clear();
            tsk.Images.Clear();
            tsk.Likes.Clear();
            tsk.Solutions.Clear();
            tsk.Tags.Clear();
            tsk.Videos.Clear();
        }

        [HttpPost]
        public ActionResult EmailSend(String mailBody, int id)
        {
                MailMessage mail = new MailMessage();
                mail.To.Add(db.Tasks.Find(id).User.Email);
                mail.From = new MailAddress("noreply@gmail.com",User.Identity.GetUserName());
                mail.Subject = "Problem with your task" + db.Tasks.Find(id).Title;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.live.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential
                ("Nokuap@outlook.com", "298746773Pauk");// Enter seders User name and password
                smtp.Send(mail);
                //return View("Index", _objModelMail);
                return RedirectToAction("PostTask", new { id = id});
        }


        public ActionResult BlockTask(int id)
        {
            var task = db.Tasks.Find(id);
            task.Block = false;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PostTask", new { id = id});
        }

        public ActionResult Graphic()
        {
            return View();
        }

        public ActionResult ViewListOfComments(IEnumerable<Comment> Model)
        {
            return PartialView("_ListOfComments", Model.OrderByDescending(obj=> obj.Date).Take(5));
        }

        public IEnumerable<Comment> GetFewCommentsForPartialView(string TaskId, int BlockNumber, int BlockSize)
        {
            var task = db.Tasks.Find(Int32.Parse(TaskId));
            int startIndex = (BlockNumber - 1) * BlockSize;
            return (from p in task.Comments.OrderByDescending(obj => obj.Date) select p).Skip(startIndex).Take(BlockSize).ToList();
        }

        [HttpPost]
        public ActionResult InfiniteComments(string TaskId, int BlockNumber)
        {
            int BlockSize = 5;
            var comments = GetFewCommentsForPartialView(TaskId, BlockNumber, BlockSize);
            return PartialView("_ListOfComments", comments);
        }

        public ActionResult ViewTasksOrderedByRating() 
        {
            return PartialView("_ShowTaskOnly", db.Tasks.OrderByDescending(obj => obj.Solutions.Count).ToList());
        }
        public ActionResult ViewTasksOrderedByUploadDate()
        {
            return PartialView("_ShowTaskOnly", db.Tasks.OrderByDescending(obj => obj.UploadDate).ToList());
        }
    }
}