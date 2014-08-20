using Exernet.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using Exernet.Filters;

namespace Exernet.Controllers
{
    [Authorize]
    [Culture]
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public UserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /User/Details/id
        public ActionResult Details(string id)
        {
            if (id == null) return RedirectToAction("Index", "Home");
            var user = UserManager.FindByName(id);
            if (user == null) return RedirectToAction("Index", "Home");
            var taskList = GetFewTasksForPartialView(id, 1, 5);
            var userInfo = new ShowUserViewModel()
            {
                CommentsQuantity = user.Comments.Count,
                Email = user.Email,
                Rating = user.Rating,
                ResolvedTasksQuantity = user.Solutions.Count(x => x.Correct),
                Solutions = user.Solutions,
                SolutionsQuantity = user.Solutions.Count,
                Tasks = taskList,
                TasksQuantity = user.Tasks.Count,
                ProfileFotoURL=user.ProfileFotoURL,
                UserName=user.UserName
            };
            return View(userInfo);
        }

        // POST: /User/Details/id
        [HttpPost]
        public ActionResult Details(string id, HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
            {
                return RedirectToAction("Details", new { id = id });
            }

            Cloudinary cloudinary = new Cloudinary(new Account(
            "goodcloud",
            "836668373272998",
            "HJ2Q7oe53Ru7muxKcpVj4ZdqVPQ"));

            var imageToDelete = UserManager.FindByName(id).ProfileFotoURL;
            var public_id = Path.GetFileNameWithoutExtension(imageToDelete);
            DelResParams deleteParams = new DelResParams()
            {
                PublicIds = new System.Collections.Generic.List<String>() { String.Format(@"Exernet/ProfilePictures/{0}",public_id) },
                Invalidate = true
            };
            cloudinary.DeleteResources(deleteParams);
            db.SaveChanges();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileUpload.FileName, fileUpload.InputStream),
                Folder="Exernet/ProfilePictures"
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            var uplPath = uploadResult.Uri.AbsoluteUri;

            db.Users.Find(User.Identity.GetUserId()).ProfileFotoURL = uplPath;
            //db.Entry().State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Details", new {id = id });
        }

        //
        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var user = UserManager.FindByName(id);
                UserManager.Delete(user);
                return new JsonResult() { Data = id };
            }
            catch
            {
                return null;
            }
        }

        public ActionResult ViewListOfTasks(IEnumerable<ExernetTask> Model) {
            var list = Model.ToList();
            return PartialView("~/Views/Task/_ShowTaskOnly.cshtml",list);
        }

        public List<ExernetTask> GetFewTasksForPartialView(string UserName, int BlockNumber, int BlockSize)
        {
            var user = UserManager.FindByName(UserName);
            int startIndex = (BlockNumber - 1) * BlockSize;
            return (from p in user.Tasks.OrderByDescending(obj => obj.UploadDate) select p).Skip(startIndex).Take(BlockSize).ToList();
        }

        [HttpPost]
        public ActionResult InfiniteScroll(string UserName, int BlockNumber)
        {
            int BlockSize = 5;
            var tasks = GetFewTasksForPartialView(UserName, BlockNumber, BlockSize);
            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = tasks.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("~/Views/Task/_ShowTaskOnly.cshtml", tasks);
            return Json(jsonModel);
        }

        private string RenderPartialViewToString(string viewName, List<ExernetTask> model)
        {
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult =
                ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext
                (ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult ShowUsers() 
        {
            if (!User.IsInRole("Administrator")) return null;
            var users = UserManager.Users.ToArray() ;
            foreach (var user in users) 
            {
                user.IsAdmin = UserManager.IsInRole(user.Id,"Administrator");
            }
            return PartialView("~/Views/User/_AllUsersShort.cshtml",
                users);
        }
    }
}
