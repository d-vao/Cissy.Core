using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cissy.Tools.Templates
{
    public abstract class Template
    {
        StringBuilder sb = new StringBuilder();
        public void WriteLine(string str)
        {
            sb.Append(str);
            sb.Append("\n");
        }
        public abstract void TransformText();
        public string ToCode()
        {
            return sb.ToString();
        }
    }
}
