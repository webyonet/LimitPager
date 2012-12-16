using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webyonet.DataPager.Interface
{
    interface IPager
    {
        void GetQueryStringPager(string url, string querystring);
        void GetRewritePager(string url, string querystring);
    }
}
