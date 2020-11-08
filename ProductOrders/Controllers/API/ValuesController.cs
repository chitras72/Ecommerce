using ProductOrders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductOrders.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/Product/GetAll")]
        public IHttpActionResult GETAllProducts()
        {
            List<Product> products = null;
            using (var ctx = new ecommerceEntities())
            {
                products = ctx.Products.ToList();
            }
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Route("api/Product/All")]
        public IHttpActionResult GETProducts()
        {
            List<ProductDTO> products = null;
            using (var ctx = new ecommerceEntities())
            {
                products = ctx.Products.Select(x => new ProductDTO()
                {
                    ID=x.ID,
                    Name = x.Name,
                    Price = x.Price,
                    Description=x.Description,
                   UnitInStock = x.UnitInStock

                }).ToList();
            }
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Route("api/Product/GetProductById")]
        public IHttpActionResult GetProduct(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var prodructItem = ctx.Products.Where(x => x.ID == id).FirstOrDefault();
                    if (prodructItem != null)
                    {
                        ProductDTO product = new ProductDTO();
                        product.ID = prodructItem.ID;
                        product.Name = prodructItem.Name;
                        product.Price = prodructItem.Price;
                        product.Description = prodructItem.Description;
                        product.UnitInStock = prodructItem.UnitInStock;
                        return Ok(product);
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

        [Route("api/Product/AddNewProduct")]
        public IHttpActionResult PostAddNewProduct(ProductDTO product)
        {
            if (product==null)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    ctx.Products.Add(new Product()
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        UnitInStock = product.UnitInStock
                    });
                    ctx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Invalid data");
            }
            return Ok();
        }
      
        [Route("api/Product/UpdateProduct")]
        public IHttpActionResult PutProductById(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var prodructItem = ctx.Products.Where(x => x.ID == product.ID).FirstOrDefault();
                    if (prodructItem != null)
                    {
                        prodructItem.Name = product.Name;
                        prodructItem.Price = product.Price;
                        prodructItem.Description = product.Description;
                        prodructItem.UnitInStock = product.UnitInStock;
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

        [Route("api/Product/DeleteProductById")]
        public IHttpActionResult DeleteProductById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            try
            {
                using (var ctx = new ecommerceEntities())
                {
                    var prodructItem = ctx.Products.Where(x => x.ID == id).FirstOrDefault();
                    if (prodructItem != null)
                    {
                        ctx.Products.Remove(prodructItem);
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
