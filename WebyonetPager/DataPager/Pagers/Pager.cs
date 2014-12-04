using System.Text;
using System.Text.RegularExpressions;
using Webyonet.DataPager.Generator;
using Webyonet.DataPager.Core;
using Webyonet.DataPager.Static;
using Webyonet.DataPager.Interface;
using Webyonet.DataPager.Mode;

namespace Webyonet.DataPager
{
    class Pager : PagerCore, IPager
    {
        private StringBuilder returnIt = new StringBuilder();
        CreateElement Element = CreateElement.GetElement();
        readonly Regex Rgx = new Regex(@"\?");
        
        public Pager(int totalData, int pageCounter, int showdata, int currentPage, bool anchor)
        {
            PageCounter = pageCounter;
            ShowData = showdata;
            TotalData = totalData;
            CurrentPage = currentPage;
            GetStartPer = CurrentPage;
            GetEndPer = CurrentPage;
            Anchor = anchor;
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

        public string GetMultiPager(string url, PagerMethod page_method, string query_string, bool rewrite_multi_query_string, string other_query, string dummy_text)
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

        protected override string CreateRewriteUrl(string url, string querystring, int pageID)
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
        protected override string CreateQueryStringUrl(string url, string querystring, int pageID)
        {
            if (Rgx.IsMatch(url))
            {
                if (Anchor)
                    newUrl = url + "&" + querystring + "=" + pageID + "#" + querystring + "-" + pageID;
                else
                    newUrl = url + "&" + querystring + "=" + pageID;
            }
            else
            {
                if (Anchor)
                    newUrl = url + "?" + querystring + "=" + pageID + "#" + querystring + "-" + pageID;
                else
                    newUrl = url + "?" + querystring + "=" + pageID;
            }
            return newUrl;
        }

        void IPager.GetQueryStringPager(string url, string querystring)
        {
            returnIt.Clear();
            if (TotalData > 0)
            {
                var group = CurrentPage / PageCounter;
                var groupIndex = CurrentPage % PageCounter;

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
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.CreateQueryStringUrl(url, querystring, (CurrentPage - 1))));
                    }

                    for (var i = 1; i <= PageCounter; i++)
                    {
                        returnIt.Append(CurrentPage == i
                            ? Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active, i.ToString(), null)
                            : Element.ElementGenerator(CreateElement.ElementType.Link, null, i.ToString(),
                                CreateQueryStringUrl(url, querystring, i)));
                        if (i == TotalData)
                            break;
                    }

                    if (PageCounter < TotalData)
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.CreateQueryStringUrl(url, querystring, (PageCounter + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateQueryStringUrl(url, querystring, (CurrentPage + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, this.CreateQueryStringUrl(url, querystring, TotalData)));
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
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateQueryStringUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                }
                else
                {
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.First, SProperty.FirstText, this.CreateQueryStringUrl(url, querystring, 1)));
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.CreateQueryStringUrl(url, querystring, (CurrentPage - 1))));

                    var pageGroup = CurrentPage / PageCounter;
                    var pageGroupsort = CurrentPage % PageCounter;

                    if (pageGroupsort == 0)
                    {
                        pageGroup -= 1;
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.CreateQueryStringUrl(url, querystring, ((pageGroup) * PageCounter))));
                        pageGroup += 1;

                        pageGroupsort = PageCounter;
                        if (pageGroup != 0 || pageGroup != 1)
                            pageGroup = pageGroup - 1;
                    }
                    else
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.CreateQueryStringUrl(url, querystring, ((pageGroup) * PageCounter))));
                    }

                    var startingPoint = pageGroup * PageCounter;
                    var lastGroup = TotalData / PageCounter;
                    int endOf;

                    if (pageGroup != lastGroup)
                        endOf = PageCounter;
                    else
                        endOf = TotalData - (pageGroup * PageCounter);

                    for (int j = 1; j <= endOf; j++)
                    {
                        returnIt.Append(j == pageGroupsort
                            ? Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active,
                                (startingPoint + j).ToString(), null)
                            : Element.ElementGenerator(CreateElement.ElementType.Link, null,
                                (startingPoint + j).ToString(),
                                CreateQueryStringUrl(url, querystring, (startingPoint + j))));
                    }

                    var lastOne = (pageGroup + 1) * PageCounter + 1;

                    if (pageGroup == 0)
                    {
                        lastOne = PageCounter + 1;
                    }
                    if (CurrentPage != TotalData && pageGroup != lastGroup)
                    {
                        if ((CurrentPage + PageCounter) <= TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.CreateQueryStringUrl(url, querystring, lastOne)));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateQueryStringUrl(url, querystring, TotalData)));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, this.CreateQueryStringUrl(url, querystring, TotalData)));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateQueryStringUrl(url, querystring, (CurrentPage + 1))));
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
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateQueryStringUrl(url, querystring, (CurrentPage + 1))));
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
                var group = CurrentPage / PageCounter;
                var groupIndex = CurrentPage % PageCounter;

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
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, this.CreateRewriteUrl(url, querystring, (CurrentPage - 1))));
                    }

                    for (var i = 1; i <= PageCounter; i++)
                    {
                        returnIt.Append(CurrentPage == i
                            ? Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active, i.ToString(), null)
                            : Element.ElementGenerator(CreateElement.ElementType.Link, null, i.ToString(),
                                CreateRewriteUrl(url, querystring, i)));
                        if (i == TotalData)
                            break;
                    }

                    if (PageCounter < TotalData)
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", this.CreateRewriteUrl(url, querystring, (PageCounter + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, this.CreateRewriteUrl(url, querystring, TotalData)));
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
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                }
                else
                {
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.First, SProperty.FirstText,CreateRewriteUrl(url, querystring, 1)));
                    returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Prev, SProperty.PrevText, CreateRewriteUrl(url, querystring, (CurrentPage - 1))));

                    var pageGroup = CurrentPage / PageCounter;
                    var pageGroupsort = CurrentPage % PageCounter;

                    if (pageGroupsort == 0)
                    {
                        pageGroup -= 1;

                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", CreateRewriteUrl(url, querystring, ((pageGroup) * PageCounter))));
                        pageGroup += 1;
                    }
                    else
                    {
                        returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, "...", CreateRewriteUrl(url, querystring, ((pageGroup) * PageCounter))));
                    }
                    if (pageGroupsort == 0)
                    {
                        pageGroupsort = PageCounter;
                        if (pageGroup != 0 || pageGroup != 1)
                            pageGroup = pageGroup - 1;
                    }

                    var startingPoint = pageGroup * PageCounter;
                    var lastGroup = TotalData / PageCounter;
                    int endOf;

                    if (pageGroup != lastGroup)
                        endOf = PageCounter;
                    else
                        endOf = TotalData - (pageGroup * PageCounter);

                    for (var j = 1; j <= endOf; j++)
                    {
                        returnIt.Append(j == pageGroupsort
                            ? Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Active,
                                (startingPoint + j).ToString(), null)
                            : Element.ElementGenerator(CreateElement.ElementType.Link, null,
                                (startingPoint + j).ToString(),
                                CreateRewriteUrl(url, querystring, (startingPoint + j))));
                    }

                    var lastOne = (pageGroup + 1) * PageCounter + 1;

                    if (pageGroup == 0)
                    {
                        lastOne = PageCounter + 1;
                    }
                    if (CurrentPage != TotalData && pageGroup != lastGroup)
                    {
                        if ((CurrentPage + PageCounter) <= TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, null, SProperty.NextText, CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Last, SProperty.LastText, CreateRewriteUrl(url, querystring, TotalData)));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, this.CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                    else
                    {
                        if (CurrentPage == TotalData)
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Next, SClass.Disabled), SProperty.NextText, null));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText,null));
                        }
                        else
                        {
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Link, SClass.Next, SProperty.NextText, CreateRewriteUrl(url, querystring, (CurrentPage + 1))));
                            returnIt.Append(Element.ElementGenerator(CreateElement.ElementType.Text, SClass.Join(SClass.Last, SClass.Disabled), SProperty.LastText, null));
                        }
                    }
                }
            }
        }
    }
}
