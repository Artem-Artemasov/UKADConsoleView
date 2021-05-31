using LinkFounder.Logic.Services;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Crawlers;
using LinkFounder.ConsoleView;

namespace LinkFounder.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            var RequestService = new RequestService();
            var LinkParser = new LinkParser();
            var LinkConverter = new LinkConverter();
            var LinkValidator = new LinkValidator();

            var HtmlCrawler = new HtmlCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);
            var SitemapCrawler = new SitemapCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);

            var ConsoleView = new ResultWritter();

            var LinkView = new LinkView(ConsoleView, HtmlCrawler, SitemapCrawler);
            LinkView.StartWork();

        }
    }
}
