﻿using System;

namespace Webyonet.DataPager.Static
{
    public static class SProperty
    {
        private static string last = "Last Page";
        private static string first = "First Page";
        private static string next = "Next Page";
        private static string prev = "Prev Page";

        public static string FirstText
        {
            get { return first; }
            set { first = value; }
        }
        public static string LastText
        {
            get { return last; }
            set { last = value; }
        }
        public static string PrevText
        {
            get { return prev; }
            set { prev = value; }
        }
        public static string NextText
        {
            get { return next; }
            set { next = value; }
        }
    }
}
