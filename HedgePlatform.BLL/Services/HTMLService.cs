using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using HedgePlatform.BLL.Infr;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace HedgePlatform.BLL.Services
{
    public class HTMLService : IHTMLService
    {        
        public string GenerateHTMLRequest(ResidentDTO resident)
        {
            var document = GetHTMLDocument("\\wwwroot\\html\\pdfrequest.html");
            return HTMLResidentRequestBuilder(resident,document).DocumentNode.OuterHtml;
        }

        public string GenerateVoteStat(IEnumerable<VoteResultDTO> voteResults)
        {
            var document = GetHTMLDocument("\\wwwroot\\html\\pdfvotestat.html");
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

        private static HtmlDocument HTMLResidentRequestBuilder(ResidentDTO resident, HtmlDocument document)
        {
            HtmlNode fio = document.GetElementbyId("FIO");
            var newNode = HtmlNode.CreateNode(resident.FIO);
            fio.InnerHtml = resident.FIO;

            document.GetElementbyId("Phone").InnerHtml = resident.Phone.Number;

            string address = resident.Flat.House.City + ", " + resident.Flat.House.Street + " д. " + resident.Flat.House.Home;
            if (resident.Flat.House.Corpus != null && resident.Flat.House.Corpus != "") 
                address += " корп. " + resident.Flat.House.Corpus;
            address += " кв. " + resident.Flat.House.Home;
            document.GetElementbyId("Address").InnerHtml = address;

            return document;
        }
    }
}
