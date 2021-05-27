using System.Collections.Generic;
using System.Linq;
using UKAD.Interfaces.View;
using UKAD.Models;
using UKAD.Repository;

namespace UKADConsoleView.Views
{
    public class LinkView : ILinkView
    {
        private IResultWritter ResultWritter { get; set; }

        public LinkView(IResultWritter writter)
        {
            ResultWritter = writter;
        }

        public string ReadUrl()
        {
            ResultWritter.WriteLine("Please enter a basic url");
            return ResultWritter.ReadLine();
        }

        public bool PrintAllInformation(LinkRepository linkRepository)
        {
            ResultWritter.WriteLine("\n\n\n");
            ResultWritter.WriteLine("\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            PrintList(linkRepository.GetLinksAsync(p=>p.LocationUrl == Enums.LocationUrl.InSiteMap).Result);

            ResultWritter.WriteLine("\n\n\n");
            ResultWritter.WriteLine("\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            PrintList(linkRepository.GetLinksAsync(p => p.LocationUrl == Enums.LocationUrl.InView).Result);

            ResultWritter.WriteLine("\n\n\n");
            ResultWritter.WriteLine("\t Timing");
            PrintWithTime(linkRepository.GetLinksAsync().Result);
            ResultWritter.WriteLine("\n\n\n");

            PrintCounts(linkRepository);

            return true;
        }

        public bool PrintProcessingMessage()
        {
            ResultWritter.WriteLine("Program is working, please don't close it.");
            return true;
        }

        public bool PrintErrorMessage(string errorMessage)
        {
            ResultWritter.WriteLine(errorMessage);
            return true;
        }

        /// <summary>
        /// Caclulate links and print it to console
        /// </summary>
        public bool PrintCounts(LinkRepository linkRepository)
        {
            int allCount = linkRepository.GetLinksAsync().Result.Count();
            int sitemapCount = allCount - linkRepository.GetLinksAsync(p => p.LocationUrl == Enums.LocationUrl.InView).Result.Count();
            int viewCount = allCount - linkRepository.GetLinksAsync(p => p.LocationUrl == Enums.LocationUrl.InSiteMap).Result.Count();

            ResultWritter.WriteLine($"All founded urls - {allCount} \n");
            ResultWritter.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            ResultWritter.WriteLine($"Urls(html documents) found after crawling a website: {viewCount} \n");

            return true;
        }

        /// <summary>
        /// Print input list to console
        /// </summary>
        public bool PrintList(IEnumerable<Link> links)
        {
            WriteRaw('_');
            int i = 1;
            foreach (var link in links)
            {
                ResultWritter.WriteLine("\n");
                ResultWritter.WriteLine($" {i}) " + link.Url);
                WriteRaw('_');
                i++;
            }

            return true;
        }

        /// <summary>
        /// Print to console all object from list with url and time
        /// </summary>
        public bool PrintWithTime(IEnumerable<Link> links)
        {
            int i = 1;
            WriteRaw('_');
            ResultWritter.Write("|  Url");

            ResultWritter.ChangeCursorPositonX(ResultWritter.GetOutputWidth() - 16);

            ResultWritter.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > ResultWritter.GetOutputWidth() - 25)
                {
                    link.Url = InsertNewLine(link.Url, (ResultWritter.GetOutputWidth() - 25));
                }

                ResultWritter.WriteLine("\n|  ");
                ResultWritter.Write($"{i}) " + link.Url);

                ResultWritter.ChangeCursorPositonX(ResultWritter.GetOutputWidth() - 15);

                ResultWritter.WriteLine(" | " + link.TimeDuration + "ms  |" );
                i++;
                WriteRaw('_');
            }

            return true;
        }

        /// <summary>
        /// Split input string on the many, insert a \n beetwen and return as one string 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string InsertNewLine(string input,int maxWidth)
        {
            int insertSymbols = input.Length / maxWidth;
            
            for (int i = 1; i <= insertSymbols; i++)
            {
                input = input.Insert(maxWidth * i,"\n   ");
            }

            return input;
        }

        /// <summary>
        /// Print line with input symbol. Width = console.BufferWidth at the call moment
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool WriteRaw(char symbol)
        {
            for(int i = 0; i < ResultWritter.GetOutputWidth(); i++)
            {
                ResultWritter.Write(symbol.ToString());
            }
            return true;
        }
    }
}
