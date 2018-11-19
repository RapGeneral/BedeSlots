using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.DataContext.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
		void Add(T entity);

        Task AddAsync(T entity);

		void Update(T entity);
		void Save();
		Task SaveAsync();

	}

}
