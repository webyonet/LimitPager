using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webyonet.DataPager.Pagers;

namespace Webyonet.DataPager.Factory
{
    class FactoryPager
    {
        public static Pager GetFactoryPager(int totaldata, int pagecounter, int showdata, int currentpage)
        {
            return new Pager(totaldata, pagecounter, showdata, currentpage);
        }
    }
}
