using System;

namespace Webyonet.DataPager.Generator
{
    class CreateElement
    {
        private static CreateElement element;
        private string ReturnElement { get; set; }
        private CreateElement() { }

        public static CreateElement GetElement()
        {
            return element ?? (element = new CreateElement());
        }

        public string ElementGenerator(ElementType type, string ElementClass, string Text, string Link)
        {
            ReturnElement = type == ElementType.Link ? LinkElementGenerator(ElementClass, Text, Link) : TextElementGenerator(ElementClass, Text);
            return ReturnElement;
        }

        private string TextElementGenerator(string ElementClass, string Text)
        {
            return "<span title='" + Text + "' class='" + ElementClass + "'>" + Text + "</span>";
        }
        private string LinkElementGenerator(string ElementClass, string Text, string Link)
        {
            if (!string.IsNullOrEmpty(ElementClass))
            {
                return "<a title='" + Text + "' class='" + ElementClass + "' href='" + Link + "'>" + Text + "</a>";
            }
            else
            {
                return "<a title='" + Text + "' href='" + Link + "'>" + Text + "</a>";
            }
        }

        public enum ElementType 
        { 
            Link,
            Text
        }
    }
}
