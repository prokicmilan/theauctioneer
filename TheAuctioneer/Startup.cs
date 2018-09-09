using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheAuctioneer.Startup))]
namespace TheAuctioneer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
