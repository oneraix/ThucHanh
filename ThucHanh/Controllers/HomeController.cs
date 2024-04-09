using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using ThucHanh.Models;
using ThucHanh.Repositories;

namespace ThucHanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /*        public async Task<IActionResult> Index(string keyword)
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        // Nếu có từ khóa tìm kiếm, gọi action SearchResult để hiển thị kết quả
                        return await SearchResult(keyword);
                    }
                    else
                    {
                        // Nếu không có từ khóa tìm kiếm, hiển thị toàn bộ sản phẩm
                        var allProducts = await _productRepository.GetAllAsync();
                        return View(allProducts);
                    }
                }*/

        public async Task<IActionResult> Index(string keyword, string category)
        {
            // Lấy danh sách sản phẩm bất đồng bộ
            var products = await _productRepository.GetAllAsync();

            // Kiểm tra và áp dụng bộ lọc theo danh mục sản phẩm
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category); // Thay thế p.Category bằng thuộc tính tương ứng
            }

            // Kiểm tra và áp dụng bộ lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Name.Contains(keyword));
            }

            // Thực hiện truy vấn bất đồng bộ và trả về danh sách sản phẩm đã lọc
            return View(products.ToList());
        }



        public async Task<IActionResult> SearchResult(string keyword)
        {
            var products = await _productRepository.SearchAsync(keyword);
            ViewBag.Keyword = keyword; // Truyền từ khóa tìm kiếm để hiển thị trong view
            return View("Index", products); // Trả về view Index và truyền danh sách sản phẩm tìm được
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
