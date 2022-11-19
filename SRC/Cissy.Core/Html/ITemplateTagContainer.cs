using System;
namespace Cissy.Html
{
    public interface ITemplateTagContainer
    {
        void AddTag(TemplateTag templateTag);
        void AddTag(string Tag);
        void AddTag(string Tag, string Value);
        void ClearTags();
    }
}
