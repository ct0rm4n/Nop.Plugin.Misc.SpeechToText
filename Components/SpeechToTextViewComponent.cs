using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SpeechToText.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.SpeechToText.Components
{
    public class SpeechToTextViewComponent : NopViewComponent
    {
        // Textos usados pelo JavaScript (chave no JS → recurso de idioma)
        private static readonly string[] _clientTexts =
        {
            "Button.Title", "Status.Listening", "Status.NotUnderstood", "Status.Transcribing",
            "Error.PermissionDenied", "Error.NoSpeech", "Error.Network", "Error.MicNotFound",
            "Error.Generic", "Error.TranscribeFailed"
        };

        private static readonly string[] _commands = { "Cart", "Wishlist", "Account", "Login", "Home" };

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWhisperTranscriptionService _whisper;

        public SpeechToTextViewComponent(ILocalizationService localizationService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IWhisperTranscriptionService whisper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
            _whisper = whisper;
        }

        private async Task<string> GetTextAsync(string key)
        {
            return await _localizationService.GetResourceAsync($"{SpeechToTextDefaults.LocalePrefix}.{key}");
        }

        private async Task<string[]> GetPhrasesAsync(string key)
        {
            return (await GetTextAsync(key))
                .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var settings = await _settingService.LoadSettingAsync<SpeechToTextSettings>(store.Id);
            var language = await _workContext.GetWorkingLanguageAsync();

            var texts = new Dictionary<string, string>();
            foreach (var key in _clientTexts)
                texts[key] = await GetTextAsync(key);

            var commands = new Dictionary<string, string[]>();
            if (settings.EnableVoiceCommands)
            {
                foreach (var key in _commands)
                    commands[key.ToLowerInvariant()] = await GetPhrasesAsync($"Commands.{key}");
            }

            var config = new
            {
                lang = string.IsNullOrWhiteSpace(language.LanguageCulture) ? "en-US" : language.LanguageCulture,
                autoSubmit = settings.AutoSubmitSearch,
                serverFallback = settings.EnableServerFallback && _whisper.IsAvailable(settings.WhisperModelPath),
                maxSeconds = Math.Clamp(settings.MaxRecordingSeconds, SpeechToTextDefaults.MinRecordingSeconds, SpeechToTextDefaults.MaxRecordingSecondsLimit),
                // Prefixos de busca, do mais longo para o mais curto ("buscar por" antes de "buscar")
                searchPrefixes = settings.EnableVoiceCommands
                    ? (await GetPhrasesAsync("Commands.Search")).OrderByDescending(p => p.Length).ToArray()
                    : Array.Empty<string>(),
                commands,
                texts
            };

            return View("~/Plugins/Misc.SpeechToText/Views/PublicInfo.cshtml", config);
        }
    }
}
