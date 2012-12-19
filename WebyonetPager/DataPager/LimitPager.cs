using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Webyonet.DataPager.Static;
using Webyonet.DataPager.Pagers;
using Webyonet.DataPager.Mode;
using Webyonet.DataPager.Factory;

namespace Webyonet.DataPager
{
    [DefaultProperty("TotalData")]
    [ToolboxBitmap(typeof(ResourceFinder), "Webyonet.DataPager.Pager.bmp")]
    [ToolboxData("<{0}:LimitPager runat=server></{0}:LimitPager>")]
    public class LimitPager : Control
    {
        public int GetStartPeriod { get; set; }
        public int GetEndPeriod { get; set; }

        /// <summary>
        /// Private Variable
        /// </summary>
        #region Private Variable
        private int totalData = 0;
        private int pageCounter = 5;
        private int showData = 10;
        private int currentPage = 1;
        private bool anchor = false;
        private bool multiQuery = false;
        private string query = "Page";
        private string ReturnItem { get; set; }
        #endregion

        /// <summary>
        /// Front Properties
        /// </summary>
        #region Prev - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue("Prev Page")]
        [Description("Title of the element")]
        public string Prev
        {
            get { return SProperty.PrevText; }
            set { SProperty.PrevText = value; }
        }
        #endregion
        #region Next - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue("Next Page")]
        [Description("Title of the element")]
        public string Next
        {
            get { return SProperty.NextText; }
            set { SProperty.NextText = value; }
        }
        #endregion
        #region First - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue("First Page")]
        [Description("Title of the element")]
        public string First
        {
            get { return SProperty.FirstText; }
            set { SProperty.FirstText = value; }
        }
        #endregion
        #region Last - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue("Last Page")]
        [Description("Title of the element")]
        public string Last
        {
            get { return SProperty.LastText; }
            set { SProperty.LastText = value; }
        }
        #endregion
        #region Total Data - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue(0)]
        [Description("The total number of data")]
        public int TotalData
        {
            get { return totalData; }
            set
            {
                totalData = this.negativeValidate(value);
            }
        }
        #endregion
        #region Pager Counter - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue(5)]
        [Description("Show the pager size")]
        public int PageCounter
        {
            get { return pageCounter; }
            set
            {
                pageCounter = this.negativeValidate(value);
            }
        }
        #endregion
        #region Show Data - Property
        [Category("Designer Property")]
        [Browsable(true)]
        [DefaultValue(10)]
        [Description("Show the data size")]
        public int ShowData
        {
            get { return showData; }
            set 
            {
                showData = this.negativeValidate(value); 
            }
        }
        #endregion      
        #region Anchor - Property
        [Category("Designer Property")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }
        #endregion

        /// <summary>
        /// Code Behind Properties
        /// </summary>
        #region Current Page - Property
        [Category("Code Behind Property")]
        [Browsable(true)]
        [DefaultValue(1)]
        [Description("Current Page")]
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = this.negativeValidate(value);
            }
        }
        #endregion 
        #region Pager Method - Property
        [Category("Code Behind Property")]
        [DefaultValue("QueryString")]
        [Browsable(true)]
        public PagerMethod PagerMethod { get; set; }
        #endregion
        #region Url - Property
        [Category("Code Behind Property")]
        [DefaultValue(null)]
        [Browsable(true)]
        public string Url { get; set; }
        #endregion
        #region Query - Property
        [Category("Code Behind Property")]
        [DefaultValue("Page")]
        [Browsable(true)]
        public string Query 
        {
            get { return query; }
            set { query = value; } 
        }
        #endregion
        #region Multiple Query String - Property
        [Category("Code Behind Property")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool MultipleQueryString
        {
            get { return multiQuery; }
            set { multiQuery = value; }
        }
        #endregion


        /// <summary>
        /// Code Behind Methods
        /// </summary>
        #region Pager Execute - Public Method
        public void ExecutePager()
        {
            Pager pg = FactoryPager.GetFactoryPager(TotalData, PageCounter, ShowData, CurrentPage);

            if (MultipleQueryString == false && PagerMethod == PagerMethod.QueryString)
            {
                this.CreatePager(pg.GetPager(Url, PagerMethod, Query, MultipleQueryString));
            }
            else
            {
                if (MultipleQueryString)
                {
                    if (PagerMethod == PagerMethod.QueryString)
                    {
                        throw new Exception("Invalid Pager Method. Select to Pager Method Rewrite");
                    }
                    else
                    { 
                        //multi query
                    }
                }
                else 
                {
                    this.CreatePager(pg.GetPager(Url, PagerMethod, Query, MultipleQueryString));
                }
            }

            pg = null;
        }
        #endregion

        /// <summary>
        /// Private Methods
        /// </summary>
        #region Negative Control - Private Method
        private int negativeValidate(int val)
        {
            if (val < 0)
            {
                return 0;
            }
            else
            {
                return val;
            }

        }
        #endregion
        #region Create Pager - Private Method
        private void CreatePager(string data)
        {
            ReturnItem = "<div id=\"" + this.ID + "\" class=\"pagerFrame clearfix\">";
            ReturnItem += data;
            ReturnItem += "</div>";
        }
        #endregion
        #region Render
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(ReturnItem);
        }
        #endregion
    }
}
