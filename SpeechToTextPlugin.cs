using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Misc.SpeechToText.Components;
using Nop.Plugin.Misc.SpeechToText.Services;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.SpeechToText
{
    /// <summary>
    /// Plugin de widget: injeta o botão de microfone no rodapé de todas as páginas públicas
    /// </summary>
    public class SpeechToTextPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly LocaleResourceInstaller _localeInstaller;

        public SpeechToTextPlugin(ISettingService settingService, IWebHelper webHelper, LocaleResourceInstaller localeInstaller)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _localeInstaller = localeInstaller;
        }

        public bool HideInWidgetList => false;

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                PublicWidgetZones.BodyEndHtmlTagBefore
            });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(SpeechToTextViewComponent);
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MiscSpeechToText/Configure";
        }

        public override async Task InstallAsync()
        {
            await _settingService.SaveSettingAsync(new SpeechToTextSettings());
            await _localeInstaller.InstallForAllLanguagesAsync();

            await base.InstallAsync();
        }

        /// <summary>
        /// Chamado pelo nop quando a versão do plugin.json muda: mantém os textos em dia
        /// em lojas que já tinham o plugin instalado
        /// </summary>
        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            await _localeInstaller.InstallForAllLanguagesAsync();

            await base.UpdateAsync(currentVersion, targetVersion);
        }

        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<SpeechToTextSettings>();
            await _localeInstaller.UninstallAsync();

            await base.UninstallAsync();
        }
    }
}
