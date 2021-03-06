// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Dfm
{
    using System;
    using System.Text.RegularExpressions;

    using Microsoft.DocAsCode.MarkdownLite;

    public class DfmVideoBlockRule : IMarkdownRule
    {
        private static readonly Regex _videoRegex = new Regex(@"^ *\[\!Video +(?<link>https?\:\/\/.+?) *\] *(\n|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));
        public virtual string Name => "DfmVideoBlock";
        public virtual Regex VideoRegex => _videoRegex;

        public IMarkdownToken TryMatch(IMarkdownParser parser, IMarkdownParsingContext context)
        {
            if (!parser.Context.Variables.ContainsKey(MarkdownBlockContext.IsBlockQuote) || !(bool)parser.Context.Variables[MarkdownBlockContext.IsBlockQuote])
            {
                return null;
            }

            var match = VideoRegex.Match(context.CurrentMarkdown);
            if (match.Length == 0)
            {
                return null;
            }
            var sourceInfo = context.Consume(match.Length);

            // [!Video https://]
            var link = match.Groups["link"].Value;
            return new DfmVideoBlockToken(this, parser.Context, link, sourceInfo);
        }
    }
}
