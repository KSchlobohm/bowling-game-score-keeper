using System;

namespace TechTalk.SpecFlow
{
    // Shim for SpecFlow 3.9 generators that reference TagHelper.
    // Provides ContainsIgnoreTag used in generated feature code.
    internal static class TagHelper
    {
        public static bool ContainsIgnoreTag(string[] tags)
        {
            if (tags == null || tags.Length == 0)
                return false;

            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                if (string.Equals(tag, "ignore", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
