using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HedgePlatform.BLL.Services
{
    public class HTMLService : IHTMLService
    {
        private readonly ILogger _logger = Log.CreateLogger<HTMLService>();

        public string GenerateVoteStat(IEnumerable<VoteResultDTO> voteResults)
        {
            var document = GetHTMLDocument("pdfvotestat.html");
            HtmlNode table = document.GetElementbyId("inserttable");
            table.InnerHtml = HTMLVoteStatBuilder(voteResults);

            return document.DocumentNode.OuterHtml;
        }

        private static HtmlDocument GetHTMLDocument(string name)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.Load(name);
                return document;
            }
            catch (Exception ex)
            {
                throw new ValidationException("HTML template load error", ex.Message);
            }
        }

        private static string HTMLVoteStatBuilder(IEnumerable<VoteResultDTO> voteResults)
        {
            string html = string.Empty;
            foreach (var voteResult in voteResults)
            {
                html += "<tr><td>";
                html += voteResult.VoteOption.Vote.Header;
                html += "</td><td>";
                html += voteResult.VoteOption.Vote.Content;
                html += "</td><td>";
                html += voteResult.DateVote;
                html += "</td></tr>";
            }
            return html;
        }
    }
}
