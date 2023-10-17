namespace Netzsch.Api.DataAccess;

public interface IRepository<in T>
{
    bool Insert(T entity);
    bool Update(T entity);
    int Delete(T entity);
}