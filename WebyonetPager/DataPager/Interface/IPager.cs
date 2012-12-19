using System;

namespace Webyonet.DataPager.Interface
{
    interface IPager
    {
        void GetQueryStringPager(string url, string querystring);
        void GetRewritePager(string url, string querystring);
    }
}
