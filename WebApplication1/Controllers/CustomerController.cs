using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:1010/api/customer");
        private readonly HttpClient _client;

        public CustomerController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult index()
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                customerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerViewModel>>(data);
                //CustomerViewModel customerList = JsonSerializer.Deserialize<CustomerViewModel>(data);
               

            }


            return View(customerList);
        }

        public IActionResult addUser()
        {
            return View();
        }
        public IActionResult createUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult createUser(CustomerViewModel customer)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1010/api/customer");
                var postJob = client.PostAsJsonAsync<CustomerViewModel>("customer", customer);
                postJob.Wait();

                var postResult = postJob.Result;
                if (postResult.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");

            return View(customer);
        }
    }
}
