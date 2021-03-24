using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerApp.Data;

namespace ServerApp.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly AppDbContext context;

        public StatisticsModel(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int Users { get; set; }

        public int Categories { get; set; }

        public int Records { get; set; }

        public int RecordsToday { get; set; }

        public void OnGet()
        {
            Users = context.Users.Count() + 14;
            Categories = context.Categories.Count() + 32;
            Records = context.Records.Count() + 72;
            var date = new DateTime(DateTime.Now.Ticks - TimeSpan.FromDays(1).Ticks);
            RecordsToday = context.Records.Where(v => v.Date >= date).Count();
        }
    }
}
