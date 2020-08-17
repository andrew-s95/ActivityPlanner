using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ActivityPlanner.Controllers
{
    public class HomeController : Controller
    {
        private int? InSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }

        private MyContext DbContext;
        public HomeController(MyContext context)
        {
            DbContext = context;
        }

        [HttpGet("")]
        public IActionResult RegisterPage()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newuser)
        {
            if(ModelState.IsValid)
            {
                var DbUser = DbContext.Users.FirstOrDefault(u => u.Email == newuser.Email);
                if(DbUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("RegisterPage");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                DbContext.Users.Add(newuser);
                DbContext.SaveChanges();
                InSession = newuser.UserId;
                return Redirect("Dashboard");
            }
            return View("RegisterPage");
        }

        [HttpGet("loginpage")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var DbUser = DbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if(DbUser == null)
                {
                    ModelState.AddModelError("Email", "Invalid Login Info");
                    return View("LoginPage");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, DbUser.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Login Info");
                    return View("LoginPage");
                }
                InSession = DbUser.UserId;
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("LoginPage");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("LoginPage");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard(int activityId)
        {
            if(InSession == null)
                return Redirect("LoginPage");

            List<Funtime> AllActivities = DbContext.Funtimes
                .Include(a => a.Associations)
                .OrderByDescending(a => a.Date)
                .ToList();

            List<Funtime> Coordinators = DbContext.Funtimes
                .Include(p => p.Planner)
                .ToList();

            ViewBag.planner = Coordinators;
            ViewBag.user = InSession;
            return View(AllActivities);
        }

        [HttpGet("addactivitypage")]
        public IActionResult AddActivityPage()
        {
            return View();
        }

        [HttpPost("addactivity")]
        public IActionResult AddActivity(Funtime newactivity)
        {
            if(InSession == null)
                return RedirectToAction("LoginPage");
            if(ModelState.IsValid)
            {
                newactivity.UserId = (int)InSession;
                DbContext.Funtimes.Add(newactivity);
                DbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("AddActivityPage");
        }

        [HttpGet("delete")]
        public IActionResult Delete(int activityId)
        {
            if(InSession == null)
                return RedirectToAction("LoginPage");
            
            Funtime DeleteThis = DbContext.Funtimes.FirstOrDefault(a => a.FuntimeId == activityId);

            if(DeleteThis == null)
                return RedirectToAction("Dashboard");
            if(DeleteThis.UserId != InSession)
                return RedirectToAction("Dashboard");

            DbContext.Funtimes.Remove(DeleteThis);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("{activityId}")]
        public IActionResult ActivityInfoPage(int activityId)
        {
            var SelectActivity = DbContext.Funtimes
                .Include(a => a.Associations)
                .ThenInclude(p => p.Participant)
                .FirstOrDefault(a => a.FuntimeId == activityId);
            
            List<Funtime> AllActivities = DbContext.Funtimes
                .Include(a => a.Associations)
                .OrderByDescending(a => a.Date)
                .ToList();
                
                ViewBag.activities = AllActivities;
                ViewBag.user = InSession;
                return View(SelectActivity);
        }

        [HttpGet("join/{activityId}")]
        public IActionResult Join(int activityId)
        {
            if(InSession == null)
                return RedirectToAction("LoginPage");

            Association JoinThis = new Association()
            {
                FuntimeId = activityId,
                UserId = (int)InSession
            };
            DbContext.Associations.Add(JoinThis);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{activityId}")]
        public IActionResult Leave(int activityId)
        {
            if(InSession == null)
                return RedirectToAction("LoginPage");

            Association LeaveThis = DbContext.Associations.FirstOrDefault(l => l.FuntimeId == activityId && l.UserId == InSession);

            if(LeaveThis == null)
                return RedirectToAction("Dashboard");
            
            DbContext.Associations.Remove(LeaveThis);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
