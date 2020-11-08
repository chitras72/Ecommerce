using Newtonsoft.Json;
using ProductOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ProductOrders.Controllers.API
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            IEnumerable<OrderDTO> orders = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");

                var responseTask = client.GetAsync("api/Order/All");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<OrderDTO>>();
                    readTask.Wait();
                    orders = readTask.Result;
                }
                else
                {
                    orders = Enumerable.Empty<OrderDTO>();
                    ModelState.AddModelError(string.Empty, "No Data Available");
                }
            }
            return View(orders);
        }
        public ActionResult Edit(int id)
        {
            OrderDTO order = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");

                var responseTask = client.GetAsync("api/Order/GetOrderById?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<OrderDTO>();
                    readTask.Wait();
                    order = readTask.Result;
                }
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(OrderDTO order)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");
                var putTask = client.PutAsJsonAsync<OrderDTO>("api/Order/UpdateOrder?id="+order.ID, order);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errormessage = result.Content.ReadAsStringAsync().Result;
                    dynamic json = JsonConvert.DeserializeObject(errormessage);
                    ModelState.AddModelError(string.Empty, json.Message.Value);
                }
            }
            return View(order);
        }

        public ActionResult Delete(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51953/api");
                var deleteTask = client.DeleteAsync("api/Order/DeleteOrderById?id=" + Id.ToString());
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

                var postTask = client.PostAsJsonAsync<int>("api/Order/AddOrder",Id);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Main");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    Error err= new Error() { StatusCode = Convert.ToInt16( result.StatusCode), ErrorMessage = result.Content.ReadAsStringAsync().Result };
                    return RedirectToAction("Error", "Main", new { ErrorMessage = err });
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Data Format");
            return RedirectToAction("Index", "Main");
        }
    }
}