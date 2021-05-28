/*using System.Threading.Tasks;
using UKAD.Filters;
using UKAD.Interfaces;
using UKAD.Interfaces.View;
using UKAD.Interfaces.Repository;

namespace UKADConsoleView.Controllers
{
    public class LinkController
    {
        private ILinkService LinkService { get; set; }
        private ILinkView LinkView { get; set; }
        private ILinkRepository LinkRepository { get; set; }
        private LinkFilter LinkFilter { get; set; }
        public LinkController(ILinkService linkService, ILinkRepository linkRepository, ILinkView linkView)
        {
            LinkService = linkService;
            LinkRepository = linkRepository;
            LinkView = linkView;
            LinkFilter = new LinkFilter();
        }

        /// <summary>
        /// It starting parse url and start Menu method
        /// Needed time for work
        /// </summary>
        public bool StartWork()
        {
            var basicUrl = LinkView.ReadUrl();

            if (LinkFilter.IsCorrectLink(basicUrl))
            {
                basicUrl = LinkFilter.ToSingleStyle(basicUrl);
                LinkService.SetUpBaseUrl(basicUrl);
                LinkView.PrintProcessingMessage();
                LinkService.AnalyzeAllSiteAsync().Wait();
                LinkRepository.Sort(p => p.TimeDuration);
                LinkView.PrintAllInformation(LinkRepository);
            }
            else
            {
                LinkView.PrintErrorMessage("You entered a bad url");
                return false;
            }

            return false;
        }
    }
}
*/