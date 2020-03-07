using System.Linq;
using Microsoft.Msagl.Drawing;

namespace Expert_System_21.MyExtensions
{
    public static class MyExtensions
    {
        public static string DeleteComment(this string input)
        {
            int sharp = input.IndexOf('#');
            if (sharp != -1)
            {
                input = input.Substring(0, input.IndexOf('#'));
            }
            return input;
        }
        
        public static string DeleteTabs(this string input)
        {
            return input.Replace(" ", "").Replace("\t", "");
        }
        
        public static string PreProcess(this string input)
        {
            return input.DeleteTabs().DeleteComment();
        }

        public static string ReplaceOneOf(this string input, string oldValues, string newValue)
        {
            return oldValues.Aggregate(input, (current, oldValue) => current.Replace(oldValue.ToString(), newValue));
        }

        public static NodeAttr Copy(this NodeAttr attr, NodeAttr newAttr)
        {
            attr.Shape = newAttr.Shape;
            attr.Color = newAttr.Color;
            attr.FillColor = newAttr.FillColor;
            return attr;
        }
    }
}