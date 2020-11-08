using Newtonsoft.Json;
using ProductOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ProductOrders.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            var products = GetProduct();
            if (products ==null || products.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, "No Data Available");
            }
            return View(products);
        }
        private IEnumerable<ProductDTO> GetProduct()
        {
            IEnumerable<ProductDTO> products = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");

                var responseTask = client.GetAsync("api/Product/All");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ProductDTO>>();
                    readTask.Wait();
                    products = readTask.Result;
                }
                else
                {
                    products = Enumerable.Empty<ProductDTO>();
                }
                return products;
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductDTO product)
        {
            using(var client= new HttpClient())
            {
                client.BaseAddress= new Uri("http://localhost:51953/api");

                var postTask = client.PostAsJsonAsync<ProductDTO>("api/Product/AddNewProduct", product);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Data Format");
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            ProductDTO product = null;

            using(var client=new HttpClient())
            {
                client.BaseAddress= new Uri("http://localhost:51953/api");

                var responseTask = client.GetAsync("api/Product/GetProductById?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProductDTO>();
                    readTask.Wait();
                    product = readTask.Result;
                }
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(ProductDTO product)
        {
            using(var client= new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");
                var putTask = client.PutAsJsonAsync<ProductDTO>("api/Product/UpdateProduct", product);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        public ActionResult Delete(int Id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress= new Uri("http://localhost:51953/api");
                var deleteTask = client.DeleteAsync("api/Product/DeleteProductById?id=" + Id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult AddToOrder(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");

                var postTask = client.PostAsJsonAsync<int>("api/Order/AddOrder", Id);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Main");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errormessage = result.Content.ReadAsStringAsync().Result;
                    dynamic json =JsonConvert.DeserializeObject(errormessage);
                    ModelState.AddModelError(string.Empty, json.Message.Value);
                    return View("Product",GetProduct());
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Data");
            return View(GetProduct());
        }

        public PartialViewResult Error(Error ErrorMessage)
        {
            return PartialView(ErrorMessage);
        }
    }
}