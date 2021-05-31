using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Models;


namespace LinkFounder.ConsoleView
{
    public class LinkView
    {
        private readonly ResultWritter _ConsoleWritter;
        private readonly HtmlCrawler _HtmlCrawler;
        private readonly SitemapCrawler _SitemapCrawler;

        public LinkView(ResultWritter consoleWritter, HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler)
        {
            _ConsoleWritter = consoleWritter;
            _HtmlCrawler = htmlCrawler;
            _SitemapCrawler = sitemapCrawler;
        }

        public virtual void StartWork()
        {
            _ConsoleWritter.WriteLine("Please enter a url");
            var inputUrl = _ConsoleWritter.ReadLine();

            _ConsoleWritter.WriteLine("Program is working, please don't close it.");

            var htmlLinks = _HtmlCrawler.GetLinks(inputUrl);
            var sitemapLinks = _SitemapCrawler.GetLinks(inputUrl);

            if (htmlLinks.Count() == 0 || sitemapLinks.Count() == 0)
            {
                _ConsoleWritter.WriteLine("Sorry, but you write a bad url");
                return;
            }

            PrintAllInformation(htmlLinks, sitemapLinks);

        }

        public virtual void PrintAllInformation(IEnumerable<Link> htmlLinks,IEnumerable<Link> sitemapLinks)
        {
            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                   .Concat(sitemapLinks)
                                   .ToList();

            PrintCaption("\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            PrintList(sitemapLinks.Except(htmlLinks, (x, y) => x.Url == y.Url));

            PrintCaption("\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            PrintList(htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url));


            PrintWithTime(allLinks.OrderBy(p => p.TimeResponse));

            PrintCounts(htmlLinks.Count(), sitemapLinks.Count(), allLinks.Count());
        }
        public virtual void PrintCounts(int htmlCount, int sitemapCount, int allCount)
        {
            _ConsoleWritter.WriteLine($"All links found {allCount} \n");
            _ConsoleWritter.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            _ConsoleWritter.WriteLine($"Urls(html documents) found after crawling a website: {htmlCount} \n");
        }

        public virtual void PrintCaption(string caption)
        {
            _ConsoleWritter.WriteLine("\n\n\n");
            _ConsoleWritter.WriteLine(caption);
            WriteRaw('_');
        }

        public virtual void PrintList(IEnumerable<Link> links)
        {
            int index = 1;
            WriteRaw('_');
            foreach (var link in links)
            {
                _ConsoleWritter.WriteLine("\n");
                _ConsoleWritter.WriteLine($" {index}) " + link.Url);
                WriteRaw('_');
                index++;
            }
        }

        private void WriteRaw(char symbol)
        {
            for (int i = 0; i < _ConsoleWritter.GetOutputWidth(); i++)
                _ConsoleWritter.Write(symbol.ToString());
        }

        public virtual void PrintWithTime(IEnumerable<Link> links)
        {
            int index = 1;
            WriteRaw('_');
            _ConsoleWritter.Write("|  Url");

            _ConsoleWritter.ChangeCursorPositonX(_ConsoleWritter.GetOutputWidth() - 16);

            _ConsoleWritter.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > _ConsoleWritter.GetOutputWidth() - 25)
                    link.Url = SliceWithWidth(link.Url, (_ConsoleWritter.GetOutputWidth() - 25));

                _ConsoleWritter.WriteLine("\n|  ");
                _ConsoleWritter.Write($"{index}) " + link.Url);

                _ConsoleWritter.ChangeCursorPositonX(_ConsoleWritter.GetOutputWidth() - 15);

                _ConsoleWritter.WriteLine(" | " + link.TimeResponse + "ms  |");
                index++;
                WriteRaw('_');
            }
        }
        public virtual string SliceWithWidth(string input, int maxWidth)
        {
            if (maxWidth < 0)
            {
                return input;
            }

            int insertSymbols = input.Length / maxWidth;

            for (int i = 1; i <= insertSymbols; i++)
                input = input.Insert(maxWidth * i, "\n   ");


            return input;
        }
    }
}
