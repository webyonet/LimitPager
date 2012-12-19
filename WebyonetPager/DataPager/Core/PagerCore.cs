using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webyonet.DataPager.Core
{
    abstract class PagerCore
    {
        private int getstartper;
        private int getendper;
        private int currentpage;
        private int totaldata;

        protected int PageCounter { get; set; }
        protected int ShowData { get; set; }
        protected string DummyText { get; set; }
        protected string OtherQuery { get; set; }
        protected string newUrl { get; set; }
        protected bool MultiQueryString { get; set; }
        protected int TotalData
        {
            get { return totaldata; }
            set
            {
                totaldata = (int)Math.Ceiling((float)value / (float)ShowData);
            }
        }
        protected int CurrentPage
        {
            get { return currentpage; }
            set
            {
                currentpage = value == 0 ? 1 : value;
            }
        }

        public bool Anchor { get; set; }
        public int GetStartPer
        {
            get
            {
                return getstartper;
            }
            set
            {
                getstartper = value == 1 ? 1 : (ShowData * (value - 1)) + 1;
            }
        }
        public int GetEndPer
        {
            get { return getendper; }
            set
            {
                getendper = value == 1 ? ShowData : ShowData * (value);
            }
        }

        protected abstract string CreateRewriteUrl(string url, string querystring, int pageID);
        protected abstract string CreateQueryStringUrl(string url, string querystring, int pageID);
    }
}
