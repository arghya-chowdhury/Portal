using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PortalApp.Models;
using PortalApp.ViewModels;

namespace PortalApp.Controllers
{
    public class HomeController : Controller
    {
        CloudStorageAccount _storage;
        IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            var con = _configuration?.GetConnectionString("DefaultConnection");
            if(!string.IsNullOrEmpty(con))
            {
                _storage = CloudStorageAccount.Parse(con);
            }
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactPost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Contact([Bind]ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                if(_storage!=null)
                {
                    try
                    {
                        var model = new ContactModel()
                        {
                            PartitionKey = DateTime.Now.Year.ToString(),
                            RowKey = Guid.NewGuid().ToString(),
                            Name = contact?.Name?.Length > 80 ? contact.Name.Substring(0, 80) : contact.Name,
                            Email = contact?.Email?.Length > 80 ? contact.Email.Substring(0, 80) : contact.Email,
                            Message = contact?.Message?.Length > 400 ? contact.Message.Substring(0, 400) : contact.Message,
                            Longitude = contact.Longitude,
                            Latitude = contact.Latitude
                        };

                        var client = _storage.CreateCloudTableClient();
                        var table = client.GetTableReference("contacts");
                        var tableOp = TableOperation.Insert(model);
                        var tableResult = await table.ExecuteAsync(tableOp);
                        if(tableResult.HttpStatusCode==(int)HttpStatusCode.NoContent)
                        {
                            return RedirectToAction("ContactPost");
                        }
                        else
                        {
                            ModelState.AddModelError("Summary", $"Sorry! Unable to post [HTTP {tableResult.HttpStatusCode}]. Please use email instead.");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("Summary", $"Sorry! Error. Please use email instead.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Summary", "Unable to connect with data store. Please use email instead.");
                }
            }
            return View(contact);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
