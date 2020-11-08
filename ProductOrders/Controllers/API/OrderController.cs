using ProductOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductOrders.Controllers
{
    public class OrderController : ApiController
    {
        // GET api/<controller>
        [Route("api/Order/All")]
        public IHttpActionResult GetOrders()
        {
            List<OrderDTO> orders = null;
            using (var ctx = new ecommerceEntities())
            {
                orders = ctx.Orders.Select(x => new OrderDTO()
                {
                    ID = x.ID,
                    ProductName = ctx.Products.Where(z => z.ID == x.ProductID).Select(z => z.Name).FirstOrDefault(),
                    ProductID = x.ProductID,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,
                    Date = x.Date

                }).ToList();
            }
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [Route("api/Order/GetOrderById")]
        public IHttpActionResult GetOrderById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var orderItem = ctx.Orders.Where(x => x.ID == id).FirstOrDefault();
                    var prodructItem = ctx.Products.Where(x => x.ID == orderItem.ProductID).FirstOrDefault();
                    if (orderItem != null)
                    {
                        OrderDTO order = new OrderDTO();
                        order.ID = orderItem.ID;
                        order.ProductID = orderItem.ProductID;
                        order.ProductName = prodructItem.Name;
                        order.Quantity = orderItem.Quantity;
                        order.TotalAmount = orderItem.TotalAmount;
                        return Ok(order);
                    }
                    else
                    {
                        return BadRequest("Invalid data");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data");
            }
        }


        [Route("api/Order/AddOrder")]
        public IHttpActionResult PostAddOrder([FromBody]int productId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var productItem = ctx.Products.Where(x => x.ID == productId).FirstOrDefault();
                    if (productItem != null && productItem.UnitInStock>0)
                    {
                        ctx.Orders.Add(new Order()
                        {
                            ProductID = productId,
                            Quantity = 1,
                            TotalAmount = 1 * productItem.Price,
                            Date = DateTime.Now
                        });
                        productItem.UnitInStock = productItem.UnitInStock - 1;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("The product is not available");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data");
            }
            return Ok();
        }

        [Route("api/Order/AddNewItem")]
        public IHttpActionResult PostNewOrder(OrderDTO order)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var orderItem = ctx.Orders.Where(x => x.ID == order.ID).FirstOrDefault();
                    var productItem = ctx.Products.Where(x => x.ID == order.ProductID).FirstOrDefault();
                    if (productItem != null)
                    {
                        if (order.Quantity <= productItem.UnitInStock)
                        {
                            ctx.Orders.Add(new Order()
                            {
                                ProductID = order.ProductID,
                                Quantity = order.Quantity,
                                TotalAmount = productItem.Price * order.Quantity,
                                Date = order.Date
                            });
                            productItem.UnitInStock = productItem.UnitInStock - order.Quantity;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("InSufficent count");
                        }
                    }
                    else
                    {
                        return BadRequest("The product is not available");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data");
            }
            return Ok();
        }

        [Route("api/Order/UpdateOrder")]
        public IHttpActionResult PutOrderById(int id, OrderDTO order)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var orderItem = ctx.Orders.Where(x => x.ID == order.ID).FirstOrDefault();
                    var productItem = ctx.Products.Where(x => x.ID == order.ProductID).FirstOrDefault();
                    if (productItem != null)
                    {
                        if (orderItem.Quantity != order.Quantity)
                        {

                            if (order.Quantity > orderItem.Quantity && productItem.UnitInStock >= (order.Quantity - orderItem.Quantity))
                            {
                                productItem.UnitInStock = productItem.UnitInStock - (order.Quantity - orderItem.Quantity);
                            }
                            else if (order.Quantity < orderItem.Quantity)
                            {
                                productItem.UnitInStock = productItem.UnitInStock - (order.Quantity - orderItem.Quantity);
                            }
                            else
                            {
                                return BadRequest("Insufficient Count");
                            }
                            orderItem.Quantity = order.Quantity;
                            orderItem.TotalAmount = productItem.Price * order.Quantity;
                            orderItem.Date = order.Date;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("No change in order count");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid data");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data");
            }
            return Ok();
        }

        [Route("api/Order/DeleteOrderById")]
        public IHttpActionResult DeleteProductById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var orderItem = ctx.Orders.Where(x => x.ID == id).FirstOrDefault();
                    if (orderItem != null)
                    {
                        ctx.Orders.Remove(orderItem);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("Invalid data");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid data");
            }
            return Ok();
        }
    }
}