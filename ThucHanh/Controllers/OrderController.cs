﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThucHanh.Models;
using ThucHanh.Repositories;

namespace ThucHanh.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderRepository _orderRepository;

		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		// Action hiển thị danh sách đơn hàng
		public IActionResult Index()
		{
			// Code để lấy danh sách đơn hàng và hiển thị
			var orders = _orderRepository.GetAll();
			return View(orders);
		}
		public IActionResult Create()
		{
			// Hiển thị form để người dùng nhập thông tin tạo đơn hàng
			return View();
		}

		[HttpPost]
		public IActionResult Create(Order order)
		{
			if (ModelState.IsValid)
			{
				// Lấy ID của khách hàng từ thông tin đăng nhập
				order.UserId = User.Identity.Name;

				// Lưu đơn hàng vào cơ sở dữ liệu hoặc thực hiện các bước khác để tạo đơn hàng

				return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách đơn hàng sau khi tạo thành công
			}
			// Nếu thông tin không hợp lệ, hiển thị lại form với thông báo lỗi
			return View(order);
		}
		// Action hủy đơn hàng chỉ dành cho khách hàng
		[Authorize(Roles = "Customer")]
		public IActionResult Cancel(int orderId)
		{
			// Code để hủy đơn hàng của khách hàng
			var order = _orderRepository.GetById(orderId);
			if (order == null)
			{
				return NotFound();
			}

			// Kiểm tra xem đơn hàng có thuộc về khách hàng hiện tại hay không
			if (order.UserId != User.Identity.Name)
			{
				return Forbid(); // Trả về 403 Forbidden nếu không phải khách hàng sở hữu đơn hàng
			}

			// Xử lý hủy đơn hàng
			_orderRepository.CancelOrder(orderId);
			return RedirectToAction(nameof(Index));
		}

		// Action chỉ cho phép Admin xem chi tiết đơn hàng
		[Authorize(Roles = "Admin")]
		public IActionResult Details(int orderId)
		{
			// Code để hiển thị chi tiết đơn hàng
			var order = _orderRepository.GetById(orderId);
			if (order == null)
			{
				return NotFound();
			}
			return View(order);
		}

		// Action chỉ cho phép Admin xóa đơn hàng
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int orderId)
		{
			// Code để xóa đơn hàng
			var order = _orderRepository.GetById(orderId);
			if (order == null)
			{
				return NotFound();
			}

			_orderRepository.DeleteOrder(orderId);
			return RedirectToAction(nameof(Index));
		}


	}
}

