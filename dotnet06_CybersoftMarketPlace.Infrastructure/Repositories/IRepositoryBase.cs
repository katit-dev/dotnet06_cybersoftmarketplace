using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IRepositoryBase<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(dynamic id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(dynamic id);

    Task<T> SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<List<T>> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

}

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly CybersoftMarketPlaceContext _context;
    private readonly DbSet<T> _dbSet;

    public RepositoryBase(CybersoftMarketPlaceContext context)
    {
        _context = context;

        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(dynamic id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(dynamic id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    //Bổ sung thêm 2 phương thức lấy theo điều kiện : lấy 1 và lấy nhiều

    public async Task<T> SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }


}