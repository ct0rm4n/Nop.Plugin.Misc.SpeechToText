using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.SpeechToText.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(
                name: SpeechToTextDefaults.TranscribeRouteName,
                pattern: "speech-to-text/transcribe",
                defaults: new { controller = "SpeechToText", action = "Transcribe" });
        }

        public int Priority => 0;
    }
}
