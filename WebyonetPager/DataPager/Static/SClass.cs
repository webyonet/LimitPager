using System;

namespace Webyonet.DataPager.Static
{
    public static class SClass
    {
        private static string next = "next";
        private static string prev = "prev";
        private static string last = "last";
        private static string first = "first";
        private static string disabled = "disabled";
        private static string active = "active";

        public static string Prev
        {
            get { return SClass.prev; }
        }
        public static string Next
        {
            get { return SClass.next; }
        }
        public static string Last
        {
            get { return SClass.last; }
        }
        public static string First
        {
            get { return SClass.first; }
        }
        public static string Disabled
        {
            get { return SClass.disabled; }
        }
        public static string Active
        {
            get { return SClass.active; }
        }

        public static string Join(string firstClass, string lastClass)
        {
            return firstClass + " " + lastClass;
        }
    }
}
