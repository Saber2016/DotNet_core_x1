using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MVCSample01.Models;

namespace MVCSample01.Controllers
{
    public class ContentController:Controller
    {

        public IActionResult Index()
        {
            var contents = new List<Content>();
            for(int i=0;i<10;i++)
            {
                contents.Add(new Models.Content
                {
                    Id = i,
                    title = $"{i}的标题",
                    content = $"{i}的内容",
                    status = 1,
                    add_time = DateTime.Now.AddDays(-i)
                });
            }
            return View(new ContentViewModel { Contents = contents });
        }

    }
}
