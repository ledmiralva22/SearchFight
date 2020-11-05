using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ISearchEngine
    {
        string Name { get; }

        Task<long> Search(string searchRequest);
    }
}
