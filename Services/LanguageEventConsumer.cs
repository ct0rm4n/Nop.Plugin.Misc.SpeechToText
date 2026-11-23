using System.Threading.Tasks;
using Nop.Core.Domain.Localization;
using Nop.Core.Events;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.SpeechToText.Services
{
    /// <summary>
    /// Quando o lojista cria um idioma novo, já grava os textos do plugin nele
    /// </summary>
    public class LanguageEventConsumer : IConsumer<EntityInsertedEvent<Language>>
    {
        private readonly IPluginService _pluginService;
        private readonly LocaleResourceInstaller _localeInstaller;

        public LanguageEventConsumer(IPluginService pluginService, LocaleResourceInstaller localeInstaller)
        {
            _pluginService = pluginService;
            _localeInstaller = localeInstaller;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<Language> eventMessage)
        {
            var descriptor = await _pluginService.GetPluginDescriptorBySystemNameAsync<IWidgetPlugin>(
                SpeechToTextDefaults.SystemName, LoadPluginsMode.InstalledOnly);
            if (descriptor == null)
                return;

            await _localeInstaller.InstallForLanguageAsync(eventMessage.Entity);
        }
    }
}
