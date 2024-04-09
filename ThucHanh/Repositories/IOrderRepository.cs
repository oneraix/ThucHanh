using ThucHanh.Models;

namespace ThucHanh.Repositories
{
	public interface IOrderRepository
	{
		IEnumerable<Order> GetAll();
		Order GetById(int id);
		void CancelOrder(int id);
		void DeleteOrder(int id);
	}
}
