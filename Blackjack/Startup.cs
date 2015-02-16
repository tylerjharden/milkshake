using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blackjack.Startup))]
namespace Blackjack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
