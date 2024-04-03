using EngeneerLenRooAspNet.Models;
using EngeneerLenRooAspNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class LoggerController : Controller
    {

        [Route("loggers")]
        public IActionResult Index()
        {
            LogViewModel model = new LogViewModel()
            {
                Logs = ParseLogText($"logs/other/{DateTime.Now.ToString("yyyy-MM-dd")}.log")
            };
            return View(model);
        }

        private List<Log> ParseLogText(string pathFile)
        {
            if (!System.IO.File.Exists(pathFile))
            {
                return new List<Log>();
            }


            List<Log> logList = new List<Log>();
            using (var fs = new FileStream(pathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                string[] allText = sr.ReadToEnd().Split("StartLog");
                foreach(var logItem in allText)
                {
                    string[] splitLog = logItem.Replace("StartLog", "").Replace("EndLog", "").Split("%%%");
                    try
                    {
                        Log log = new Log()
                        {
                            DateTime = Convert.ToDateTime(splitLog[0]),
                            Type = splitLog[2],
                            Method = splitLog[3],
                            Text = splitLog[4],
                            Url = splitLog[5],
                            Action = splitLog[6]
                        };
                        logList.Add(log);
                    }
                    catch
                    {

                    }
                }
            }
            return logList.OrderByDescending(d => d.DateTime).ToList();
        }
    }
}
