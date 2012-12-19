using System;

namespace Webyonet.DataPager.Factory
{
    class FactoryPager
    {
        public static Pager GetFactoryPager(int totaldata, int pagecounter, int showdata, int currentpage, bool anchor)
        {
            return new Pager(totaldata, pagecounter, showdata, currentpage, anchor);
        }
    }
}
