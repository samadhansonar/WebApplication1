using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class TestModel : PageModel
    {
        //public void OnGet()
        //{
        //}

        //=======================================
        // Created By   -- Samdhan Soanr
        // Date         --09-05-2023        
        //  Description: create simple page with cheking input and download file .   
        //=======================================  

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            if (IsBlocked())
            {
                return Content("You are blocked for the next 5 minutes.");
            }

            return Page();
        }

        private bool IsBlocked()
        {
            var lastInvalidTimestamp = _httpContextAccessor.HttpContext.Request.Cookies["LastInvalidTimestamp"];

            if (lastInvalidTimestamp == null)
            {
                return false;
            }

            var timestamp = long.Parse(lastInvalidTimestamp);

            return (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp) < 300;
        }

        public async Task<IActionResult> OnPostAsync(string inputText)
        {
            var isValid = true;
            var filteredWords = new[] { "samadhan", "deepak", "amit" }; // Add your own predefined list of words here

            foreach (var filteredWord in filteredWords)
            {
                if (inputText.Contains(filteredWord))
                {
                    isValid = false;
                    break;
                }
            }

            var urlRegex = new System.Text.RegularExpressions.Regex(@"^(http|https)://[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");
            if (!urlRegex.IsMatch(inputText))
            {
                isValid = false;
            }

            var mobileRegex = new System.Text.RegularExpressions.Regex(@"^[1-9]{1}[0-9]{9}$");
            if (!mobileRegex.IsMatch(inputText))
            {
                isValid = false;
            }

            if (isValid)
            {
            
                var fileBytes = await System.IO.File.ReadAllBytesAsync("C: /Users/itservices.abe/source/repos/WebApplication1/wwwroot/MyFiles/TestTextFile.txt");
                return File(fileBytes, "application/octet-stream", "filename.extension");
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("LastInvalidTimestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
                return Content("Invalid input.");
            }
        }

    }
}

        
