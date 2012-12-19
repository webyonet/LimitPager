using System.Web.UI;
using System.Drawing;
using System.ComponentModel;

namespace Webyonet.DataPager
{
    [DefaultProperty("PagerControlID")]
    [ToolboxBitmap(typeof(ResourceFinder), "Webyonet.DataPager.Pager.bmp")]
    [ToolboxData("<{0}:LimitPagerAnchor runat=server></{0}:LimitPagerAnchor>")]
    public class LimitPagerAnchor : Control
    {
        private string pagerControlID;
        private int currentPage;

        [Category("Pager Property")]
        [DefaultValue("")]
        [Browsable(true)]
        public string PagerControlID
        {
            get { return pagerControlID; }
            set { pagerControlID = value; }
        }

        [Browsable(false)]
        public int CurrentPage
        {
            get 
            {
                if (currentPage <= 0)
                {
                    currentPage = 1;
                }
                return currentPage; 
            }
            set { currentPage = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(PagerControlID))
            {
                LimitPager pg = (LimitPager)Page.FindControl(PagerControlID);
                CurrentPage = pg.CurrentPage;
                writer.Write("<a id=\"" + this.ID + "\" name=\"" + pg.Query + "-" + CurrentPage + "\"></a>");
            }
            else 
            {
                writer.Write("<a id=\"" + this.ID + "\" name=\"\"></a>");
            }
        }
    }
}
