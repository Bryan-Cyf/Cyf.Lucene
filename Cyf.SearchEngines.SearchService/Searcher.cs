using Cyf.SearchEngines.LuceneService;
using Cyf.SearchEngines.LuceneService.DataService;
using Cyf.SearchEngines.LuceneService.Model;
using Cyf.SearchEngines.SearchService.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyf.SearchEngines.SearchService
{
    /// <summary>
    /// WCF Service
    /// </summary>
    public class Searcher : ISearcher
    {
        public string QueryCommodityPage(int pageIndex, int pageSize, string keyword, List<int> categoryIdList, string priceFilter, string priceOrderBy)
        {
            ISearcherAOP searcher = SearcherAOPFactory.CreateInstance();
            return searcher.QueryCommodityPage(pageIndex, pageSize, keyword, categoryIdList, priceFilter, priceOrderBy);
        }
    }
}
