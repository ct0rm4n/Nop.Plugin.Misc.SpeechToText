using System.Threading.Tasks;
using Nop.Core.Domain.Localization;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.SpeechToText.Services
{
    /// <summary>
    /// Grava os textos do plugin em cada idioma da loja, usando a tradução do idioma quando existir
    /// </summary>
    public class LocaleResourceInstaller
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;

        public LocaleResourceInstaller(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        /// <summary>Código de 2 letras do idioma (Código SEO; se vazio, a cultura)</summary>
        public static string GetIsoCode(Language language)
        {
            return !string.IsNullOrWhiteSpace(language?.UniqueSeoCode)
                ? language.UniqueSeoCode
                : (language?.LanguageCulture ?? string.Empty).Split('-')[0];
        }

        public async Task InstallForAllLanguagesAsync()
        {
            foreach (var language in await _languageService.GetAllLanguagesAsync(showHidden: true))
                await InstallForLanguageAsync(language);
        }

        public Task InstallForLanguageAsync(Language language)
        {
            return _localizationService.AddOrUpdateLocaleResourceAsync(
                SpeechToTextLocales.For(GetIsoCode(language)), language.Id);
        }

        public Task UninstallAsync()
        {
            return _localizationService.DeleteLocaleResourcesAsync(SpeechToTextDefaults.LocalePrefix);
        }
    }
}
