using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Laboratory9.Entities;

namespace Laboratory9.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Заголовок")]
        [StringLength(100)]
        public string Title { get; set; }
        [DisplayName("Текст новости")]
        [StringLength(300)]
        public string Text { get; set; }
        public UserInfo Author { get; set; }

        public DateTime Time { get; set; }

        public static NewsModel FromEntity(News news)
        {
            return new NewsModel()
            {
                Title = news.Title,
                Text = news.Text,
                Author = news.Author,
                Time = news.Time
            };
        }
    }

}
