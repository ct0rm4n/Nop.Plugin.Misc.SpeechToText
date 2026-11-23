using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SpeechToText.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.SpeechToText.Controllers
{
    /// <summary>
    /// Endpoint de fallback: recebe WAV 16 kHz mono do navegador e devolve o texto (Whisper local)
    /// no idioma atual da loja
    /// </summary>
    public class SpeechToTextController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWhisperTranscriptionService _whisper;

        public SpeechToTextController(ILocalizationService localizationService,
            ILogger logger,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IWhisperTranscriptionService whisper)
        {
            _localizationService = localizationService;
            _logger = logger;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
            _whisper = whisper;
        }

        private async Task<IActionResult> ErrorAsync(int statusCode, string resourceKey)
        {
            var message = await _localizationService.GetResourceAsync($"{SpeechToTextDefaults.LocalePrefix}.{resourceKey}");
            return StatusCode(statusCode, new { error = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(SpeechToTextDefaults.MaxAudioBytes + 64_000)]
        public async Task<IActionResult> Transcribe(IFormFile audio)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var settings = await _settingService.LoadSettingAsync<SpeechToTextSettings>(store.Id);

            if (!settings.EnableServerFallback || !_whisper.IsAvailable(settings.WhisperModelPath))
                return await ErrorAsync(StatusCodes.Status503ServiceUnavailable, "Error.ServerUnavailable");

            if (audio == null || audio.Length == 0)
                return await ErrorAsync(StatusCodes.Status400BadRequest, "Error.NoAudio");

            if (audio.Length > SpeechToTextDefaults.MaxAudioBytes)
                return await ErrorAsync(StatusCodes.Status400BadRequest, "Error.AudioTooLong");

            try
            {
                var language = LocaleResourceInstaller.GetIsoCode(await _workContext.GetWorkingLanguageAsync());

                await using var stream = audio.OpenReadStream();
                var text = await _whisper.TranscribeAsync(stream, settings.WhisperModelPath, language, HttpContext.RequestAborted);
                return Json(new { text });
            }
            catch (Exception ex)
            {
                await _logger.ErrorAsync("SpeechToText: transcription failed", ex);
                return await ErrorAsync(StatusCodes.Status500InternalServerError, "Error.TranscribeFailed");
            }
        }
    }
}
