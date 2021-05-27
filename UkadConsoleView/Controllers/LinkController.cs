using System.Threading.Tasks;
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
        public LinkController(ILinkService linkService,ILinkRepository linkRepository, ILinkView linkView)
        {
            LinkService = linkService;
            LinkRepository = linkRepository;
            LinkView = linkView;
            LinkFilter = new LinkFilter();
        }

        /// <summary>
        /// Is calling url and validate it
        /// If url valid, started parsing site
        /// </summary>
        private async Task<bool> AddAllLinksAsync()
        {
            var input = LinkView.ReadUrl();

            if (LinkFilter.IsCorrectLink(input))
            {
                input = LinkFilter.ToSingleStyle(input);
                LinkService.SetUpBaseUrl(input);
                LinkView.PrintProcessingMessage();

                await LinkService.AnalyzeSiteForUrlAsync();

                return true;
            }
            return false;
        }

        /// <summary>
        /// It starting parse url and start Menu method
        /// Needed time for work
        /// </summary>
        public bool StartWork()
        {
            if (AddAllLinksAsync().Result == false)
            {
                LinkView.PrintErrorMessage("You entered a bad url");
                return false;
            }

            LinkRepository.Sort(p => p.TimeDuration);
            LinkView.PrintAllInformation(LinkRepository);

            return false;
        }
    }
}
