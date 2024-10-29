﻿using GlideGear_backend.Models.Order_Model.Dtos;
using GlideGear_backend.Services.OrderSerices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Order-create")]
        public async Task<ActionResult> createOrder(long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest("enter a valid money ");
                }
                var orderId = await _orderService.RazorOrderCreate(price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payment")]
        public ActionResult Payment(PaymentDto razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("razorpay details must not null here");
                }
                var con = _orderService.RazorPayment(razorpay);
                return Ok(con);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("place-order")]
        public async Task<ActionResult> PlaceOrder(CreateOrderDto orderCreate)
        {
            try
            {

                int userId = GetUserIdFromClaims();
                var status = await _orderService.CreateOrder(userId, orderCreate);
                return Ok(status);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("getOrderDetails")]
        
        public async Task<ActionResult> GetOrderDetails()
        {
            try
            {
                int userId = GetUserIdFromClaims();
                return Ok(await _orderService.GetOrderDetails(userId));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


        }
        [HttpGet("get-order-details-admin")]
        
        public async Task<ActionResult> GetOrderDetailsAdmin()
        {
            try
            {
                return Ok(await _orderService.GetOrderDetailsAdmin());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


        }

        [HttpPut("update-order-status/{orderId}")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto updateOrder)
        {
            try
            {
                var statusUpdated = await _orderService.UpdateOrderStatus(orderId, updateOrder);

                if (statusUpdated)
                {
                    return Ok(); 
                }

                return NotFound(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpGet("totalProductsPurchased")]

        public async Task<ActionResult> TotalProducts()
        {
            try
            {
                return Ok(await _orderService.TotalProductsPurchased());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("totalRevenue")]

        public async Task<ActionResult> TotalRevenue()
        {
            try
            {
                return Ok(await _orderService.TotalRevenue());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        private int GetUserIdFromClaims()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new Exception("Invalid user ID.");
        }

    }
}
