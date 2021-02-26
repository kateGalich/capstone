using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BetterBooks.Startup))]
namespace BetterBooks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
