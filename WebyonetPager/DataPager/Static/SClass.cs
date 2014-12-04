using System;

namespace Webyonet.DataPager.Static
{
    public static class SClass
    {
        private const string next = "next";
        private const string prev = "prev";
        private const string last = "last";
        private const string first = "first";
        private const string disabled = "disabled";
        private const string active = "active";

        public static string Prev
        {
            get { return prev; }
        }
        public static string Next
        {
            get { return next; }
        }
        public static string Last
        {
            get { return last; }
        }
        public static string First
        {
            get { return first; }
        }
        public static string Disabled
        {
            get { return disabled; }
        }
        public static string Active
        {
            get { return active; }
        }

        public static string Join(string firstClass, string lastClass)
        {
            return firstClass + " " + lastClass;
        }
    }
}
