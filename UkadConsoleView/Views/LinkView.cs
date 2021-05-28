using System.Collections.Generic;
using System.Linq;
using UKAD.Logic.Crawlers;
using UKAD.Logic.Filters;
using UKAD.Logic.Models;
using UKAD.Logic.Services;

namespace UKADConsoleView.Views
{
    public class LinkView
    {
        private readonly ResultWritter ConsoleWritter;

        public LinkView(ResultWritter consoleWritter)
        {
            ConsoleWritter = consoleWritter;
        }

        public virtual void StartWork()
        {
            ConsoleWritter.WriteLine("Please enter a url");
            var inputUrl = ConsoleWritter.ReadLine();

            var linkProcessing = new LinkProcessing();
            var linkFilter = new LinkFilter();
            var requestService = new RequestService();

            if (linkFilter.IsCorrectLink(inputUrl) == false)
            {
                ConsoleWritter.WriteLine("Bad url");
                return;
            }

            ConsoleWritter.WriteLine("Program is working, please don't close it.");

            var viewLinks    = new ViewCrawler   (requestService, linkProcessing, linkFilter).GetViewLinks(inputUrl);
            var sitemapLinks = new SitemapCrawler(requestService, linkProcessing, linkFilter).GetSitemapLinks(inputUrl);

            var allLinks = viewLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                   .Concat(sitemapLinks)
                                   .ToList();

            PrintWithCaption(sitemapLinks.Except(viewLinks, (x, y) => x.Url == y.Url), "\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            PrintWithCaption(viewLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url), "\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            PrintWithTime(allLinks.OrderBy(p=>p.TimeResponse));
            PrintCounts(viewLinks.Count(), sitemapLinks.Count(), allLinks.Count());
        }


        public virtual void PrintCounts(int viewCount, int sitemapCount, int allCount)
        {
            ConsoleWritter.WriteLine($"All links found {allCount} \n");
            ConsoleWritter.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            ConsoleWritter.WriteLine($"Urls(html documents) found after crawling a website: {viewCount} \n");
        }
    
        public virtual void PrintWithCaption(IEnumerable<Link> list,string caption)
        {
            ConsoleWritter.WriteLine("\n\n\n");
            ConsoleWritter.WriteLine(caption);
            PrintList(list);
        }

        public virtual void PrintList(IEnumerable<Link> links)
        {
            WriteRaw('_');
            int i = 1;
            foreach (var link in links)
            {
                ConsoleWritter.WriteLine("\n");
                ConsoleWritter.WriteLine($" {i}) " + link.Url);
                WriteRaw('_');
                i++;
            }
        }

        public virtual void WriteRaw(char symbol)
        {
            for (int i = 0; i < ConsoleWritter.GetOutputWidth(); i++)
                ConsoleWritter.Write(symbol.ToString());
        }

        public virtual void PrintWithTime(IEnumerable<Link> links)
        {
            int i = 1;
            WriteRaw('_');
            ConsoleWritter.Write("|  Url");

            ConsoleWritter.ChangeCursorPositonX(ConsoleWritter.GetOutputWidth() - 16);

            ConsoleWritter.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > ConsoleWritter.GetOutputWidth() - 25)
                    link.Url = SeparateWithWidth(link.Url, (ConsoleWritter.GetOutputWidth() - 25));

                ConsoleWritter.WriteLine("\n|  ");
                ConsoleWritter.Write($"{i}) " + link.Url);

                ConsoleWritter.ChangeCursorPositonX(ConsoleWritter.GetOutputWidth() - 15);

                ConsoleWritter.WriteLine(" | " + link.TimeResponse + "ms  |");
                i++;
                WriteRaw('_');
            }
        }
        public virtual string DivideWithWidth(string input, int maxWidth)
        {
            int insertSymbols = input.Length / maxWidth;

            for (int i = 1; i <= insertSymbols; i++)
                input = input.Insert(maxWidth * i, "\n   ");


            return input;
        }
    }
}
