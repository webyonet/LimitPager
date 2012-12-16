using System.Text;
using System.Text.RegularExpressions;
using Webyonet.DataPager.Generator;
using Webyonet.DataPager.Core;
using Webyonet.DataPager.Static;
using Webyonet.DataPager.Interface;
using Webyonet.DataPager.Mode;

namespace Webyonet.DataPager.Pagers
{
    class Pager : PagerCore, IPager
    {
        private StringBuilder returnIt = new StringBuilder();
        CreateElement Element = CreateElement.GetElement();
        Regex Rgx = new Regex(@"\?");
        
        public Pager(int totalData, int pageCounter, int showdata, int currentPage)
        {
            PageCounter = pageCounter;
            ShowData = showdata;
            TotalData = totalData;
            CurrentPage = currentPage;
            GetStartPer = CurrentPage;
            GetEndPer = CurrentPage;
            Anchor = false;
        }

        public string GetPager(string url, PagerMethod page_method, string query_string, bool rewrite_multi_query_string)
        {
            IPager pager = this;

            MultiQueryString = rewrite_multi_query_string;
            if (PagerMethod.QueryString == page_method)
                pager.GetQueryStringPager(url, query_string);
            else
                pager.GetRewritePager(url, query_string);

            return returnIt.ToString();
        }

        public string GetPager(string url, PagerMethod page_method, string query_string, bool rewrite_multi_query_string, string other_query, string dummy_text)
        {
            IPager pager = this;

            MultiQueryString = rewrite_multi_query_string;
            DummyText = dummy_text;
            OtherQuery = other_query;
            if (PagerMethod.QueryString == page_method)
                pager.GetQueryStringPager(url, query_string);
            else
                pager.GetRewritePager(url, query_string);

            return returnIt.ToString();
        }

        protected override string CreateUrl(string url, string querystring, int pageID)
        {
            if (MultiQueryString)
            {
                if (Anchor)
                    return url + "/" + OtherQuery + "/" + pageID + "/" + DummyText + "-#" + querystring + "-" + pageID;
                else
                    return url + "/" + OtherQuery + "/" + pageID + "/" + DummyText + "-" + querystring + "-" + pageID;
            }
            else
            {
                if (Anchor)
                    return url + "/" + pageID + "/#" + querystring + "-" + pageID;
                else
                    return url + "/" + pageID + "/" + querystring + "-" + pageID;
            }
        }
        protected override string TestAndCreateUrl(string url, string querystring, int pageID)
        {
            if (Rgx.IsMatch(url))
            {
                newUrl = url + "&" + querystring + "=" + pageID;
            }
            else 
            {
                newUrl = url + "?" + querystring + "=" + pageID;
            }
            return newUrl;
        }

        void IPager.GetQueryStringPager(string url, string querystring)
        {
            returnIt.Clear();
            if (TotalData > 0)
            {
                int group = CurrentPage / PageCounter;
                int groupIndex = CurrentPage % PageCounter;

                if ((CurrentPage == 0 || group == 0) || (groupIndex == 0 && group == 1))
                {
                    if (CurrentPage == 1)
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.First, SClass.Disabled), SProperty.FirstText, null));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Prev, SClass.Disabled), SProperty.PrevText, null));
                    }
                    else
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.First, SClass.Disabled), SProperty.FirstText, null));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.TestAndCreateUrl(url, querystring, (CurrentPage - 1))));
                    }

                    for (int i = 1; i <= PageCounter; i++)
                    {
                        if (CurrentPage == i)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active, i.ToString(), null));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, i.ToString(), this.TestAndCreateUrl(url, querystring, i)));
                        }
                        if (i == TotalData)
                            break;
                    }

                    if (PageCounter < TotalData)
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.TestAndCreateUrl(url, querystring, (PageCounter + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.TestAndCreateUrl(url, querystring, (CurrentPage + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, this.TestAndCreateUrl(url, querystring, TotalData)));
                    }
                    else
                    {
                        if (CurrentPage == TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Next, SClass.Disabled), SProperty.NextText, null));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.TestAndCreateUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                }
                else
                {
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.First, SProperty.FirstText, this.TestAndCreateUrl(url, querystring, 1)));
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.TestAndCreateUrl(url, querystring, (CurrentPage - 1))));

                    int pageGroup = CurrentPage / PageCounter;
                    int pageGroupsort = CurrentPage % PageCounter;

                    if (pageGroupsort == 0)
                    {
                        pageGroup -= 1;
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.TestAndCreateUrl(url, querystring, ((pageGroup) * PageCounter))));
                        pageGroup += 1;

                        pageGroupsort = PageCounter;
                        if (pageGroup != 0 || pageGroup != 1)
                            pageGroup = pageGroup - 1;
                    }
                    else
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.TestAndCreateUrl(url, querystring, ((pageGroup) * PageCounter))));
                    }

                    int startingPoint = pageGroup * PageCounter;
                    int lastGroup = TotalData / PageCounter;
                    int endOf;

                    if (pageGroup != lastGroup)
                        endOf = PageCounter;
                    else
                        endOf = TotalData - (pageGroup * PageCounter);

                    for (int j = 1; j <= endOf; j++)
                    {
                        if (j == pageGroupsort)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active, (startingPoint + j).ToString(), null));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, (startingPoint + j).ToString(), this.TestAndCreateUrl(url, querystring, (startingPoint + j))));
                        }
                    }

                    int lastOne = (pageGroup + 1) * PageCounter + 1;

                    if (pageGroup == 0)
                    {
                        lastOne = PageCounter + 1;
                    }
                    if (CurrentPage != TotalData && pageGroup != lastGroup)
                    {
                        if ((CurrentPage + PageCounter) <= TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.TestAndCreateUrl(url, querystring, lastOne)));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.TestAndCreateUrl(url, querystring, TotalData)));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, this.TestAndCreateUrl(url, querystring, TotalData)));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.TestAndCreateUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                    else
                    {
                        if (CurrentPage == TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Next, SClass.Disabled), SProperty.NextText, null));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.TestAndCreateUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                }
            }
        }

        void IPager.GetRewritePager(string url, string querystring)
        {
            returnIt.Clear();
            if (TotalData > 0)
            {
                int group = CurrentPage / PageCounter;
                int groupIndex = CurrentPage % PageCounter;

                if ((CurrentPage == 0 || group == 0) || (groupIndex == 0 && group == 1))
                {
                    if (CurrentPage == 1)
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.First, SClass.Disabled), SProperty.FirstText, null));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Prev, SClass.Disabled), SProperty.PrevText, null));
                        //returnIt.Append("<span title='" + SProperty.FirstText + "' class='first disabled'>" + SProperty.FirstText + "</span><span title='" + SProperty.PrevText + "' class='prev disabled'>" + SProperty.PrevText + "</span>");
                    }
                    else
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.First, SClass.Disabled), SProperty.FirstText, null));
                        //returnIt.Append("<span title='" + SProperty.FirstText + "' class='first disabled'>" + SProperty.FirstText + "</span>");
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.CreateUrl(url, querystring, (CurrentPage - 1))));
                        //returnIt.Append("<a title='" + SProperty.PrevText + "' class='prev' href='" + this.CreateUrl(url, querystring, (CurrentPage - 1)) + "'>" + SProperty.PrevText + "</a>");
                    }

                    for (int i = 1; i <= PageCounter; i++)
                    {
                        // burak yazdı
                        if (CurrentPage == i)
                        {
                            returnIt.Append("<span class ='active' title='" + i + "' >" + i + "</span>");
                        }
                        else
                            returnIt.Append("<a title='" + i + "' href='" + this.CreateUrl(url, querystring, i) + "'>" + i + "</a>");
                        if (i == TotalData)
                            break;
                    }

                    if (PageCounter < TotalData)
                    {
                        returnIt.Append("<a title='...' href='" + this.CreateUrl(url, querystring, (PageCounter + 1)) + "'>...</a>");
                        returnIt.Append("<a title='" + SProperty.NextText + "' class='next' href='" + this.CreateUrl(url, querystring, (CurrentPage + 1)) + "'>" + SProperty.NextText + "</a>");
                        returnIt.Append("<a title='" + SProperty.LastText + "' class='last' href='" + this.CreateUrl(url, querystring, TotalData) + "'>" + SProperty.LastText + "</a>");
                    }
                    else
                    {
                        if (CurrentPage == TotalData)
                            returnIt.Append("<span title='" + SProperty.NextText + "' class='next disabled'>" + SProperty.NextText + "</span><span title='" + SProperty.LastText + "' class='last disabled'>" + SProperty.LastText + "</span>");
                        else
                        {
                            returnIt.Append("<a title='" + SProperty.NextText + "' class='next' href='" + this.CreateUrl(url, querystring, (CurrentPage + 1)) + "'>" + SProperty.NextText + "</a>");
                            returnIt.Append("<span title='" + SProperty.LastText + "' class='last disabled'>" + SProperty.LastText + "</span>");
                        }
                    }
                }
                else
                {
                    returnIt.Append("<a title='" + SProperty.FirstText + "' class='first' href='" + this.CreateUrl(url, querystring, 1) + "'>" + SProperty.FirstText + "</a>");
                    returnIt.Append("<a title='" + SProperty.PrevText + "' class='prev' href='" + this.CreateUrl(url, querystring, (CurrentPage - 1)) + "'>" + SProperty.PrevText + "</a>");

                    int pageGroup = CurrentPage / PageCounter;
                    int pageGroupsort = CurrentPage % PageCounter;

                    if (pageGroupsort == 0)
                    {
                        pageGroup -= 1;
                        returnIt.Append("<a title='...' href='" + this.CreateUrl(url, querystring, ((pageGroup) * PageCounter)) + "'> ... </a>");
                        pageGroup += 1;
                    }
                    else
                        returnIt.Append("<a title='...' href='" + this.CreateUrl(url, querystring, ((pageGroup) * PageCounter)) + "'> ... </a>");

                    if (pageGroupsort == 0)
                    {
                        pageGroupsort = PageCounter;
                        if (pageGroup != 0 || pageGroup != 1)
                            pageGroup = pageGroup - 1;
                    }

                    int startingPoint = pageGroup * PageCounter;
                    int lastGroup = TotalData / PageCounter;
                    int endOf;

                    if (pageGroup != lastGroup)
                        endOf = PageCounter;
                    else
                        endOf = TotalData - (pageGroup * PageCounter);

                    for (int j = 1; j <= endOf; j++)
                    {
                        if (j == pageGroupsort)
                            returnIt.Append("<span title='" + (startingPoint + j) + "' class='active'>" + (startingPoint + j) + "</span>");
                        else
                            returnIt.Append("<a title='" + (startingPoint + j) + "' href='" + this.CreateUrl(url, querystring, (startingPoint + j)) + "'>" + (startingPoint + j) + "</a>");
                    }

                    int lastOne = (pageGroup + 1) * PageCounter + 1;

                    if (pageGroup == 0)
                    {
                        lastOne = PageCounter + 1;
                    }
                    if (CurrentPage != TotalData && pageGroup != lastGroup)
                    {
                        if ((CurrentPage + PageCounter) <= TotalData)
                        {
                            returnIt.Append("<a title='...' href='" + this.CreateUrl(url, querystring, lastOne) + "'>...</a>");
                            returnIt.Append("<a title='" + SProperty.NextText + "' class='next' href='" + this.CreateUrl(url, querystring, (CurrentPage + 1)) + "'>" + SProperty.NextText + "</a>");
                            returnIt.Append("<a title='" + SProperty.LastText + "' class='last' href='" + this.CreateUrl(url, querystring, TotalData) + "'>" + SProperty.LastText + "</a>");
                        }
                        else
                        {
                            returnIt.Append("<a title='" + SProperty.NextText + "' class='next' href='" + this.CreateUrl(url, querystring, (CurrentPage + 1)) + "'>" + SProperty.NextText + "</a>");
                            returnIt.Append("<span title='" + SProperty.LastText + "' class='last disabled'>" + SProperty.LastText + "</span>");
                        }
                    }
                    else
                    {
                        if (CurrentPage == TotalData)
                            returnIt.Append("<span title='" + SProperty.NextText + "' class='next disabled'>" + SProperty.NextText + "</span><span title='" + SProperty.LastText + "' class='last disabled'>" + SProperty.LastText + "</span>");
                        else
                        {
                            returnIt.Append("<a title='" + SProperty.NextText + "' class='next' href='" + this.CreateUrl(url, querystring, (CurrentPage + 1)) + "'>" + SProperty.NextText + "</a>");
                            returnIt.Append("<span title='" + SProperty.LastText + "' class='last disabled'>" + SProperty.LastText + "</span>");
                        }
                    }
                }
            }
        }
    }
}
