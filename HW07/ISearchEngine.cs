using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW07;

interface ISearchEngine
{
    public Task<string> SearchAsync(string engineUrl, string keyWords);
}
