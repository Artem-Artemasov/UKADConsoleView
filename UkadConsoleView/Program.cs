using System;
using UKADConsoleView.Controllers;
using UKADConsoleView.Views;
using UKADLocalStorage.Repository;
using UKAD.Services;

namespace UkadConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            LinkRepository linkRepository = new LinkRepository();
            LinkService linkService = new LinkService(linkRepository);
            ConsoleWritter resultWritter = new ConsoleWritter();
            LinkView linkView = new LinkView(resultWritter);
            LinkController linkController = new LinkController(linkService, linkRepository, linkView);

            linkController.StartWork();
        }
    }
}
