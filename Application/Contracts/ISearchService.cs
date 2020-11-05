using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ISearchService
    {
        Task<SearchResults> RunProcess(IEnumerable<string> searchWords);
    }
}
