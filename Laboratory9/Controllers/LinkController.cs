using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Laboratory9.Entities;
using Laboratory9.Models;

namespace Laboratory9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private LinkContext context = new LinkContext();
        [HttpGet]
        public IEnumerable<NewsModel> Get(string username, string password)
        {
            UserInfo sender = context.Users.SingleOrDefault(usr => usr.Username == username);
            if (sender != null && Crypto.VerifyHashedPassword(sender.PasswordHash, password))
            {
                IQueryable<News> allNews = from news in context.News
                                                 orderby news.ID
                                                 select news;
                return allNews.ToList().ConvertAll(msg => NewsModel.FromEntity(msg));
            }
            else
            {
                return null;
            }
        }
    }
}
