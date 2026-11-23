using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SpeechToText.Models;
using Nop.Plugin.Misc.SpeechToText.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.SpeechToText.Controllers
{
    /// <summary>
    /// Página de configuração do plugin no admin
    /// </summary>
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class MiscSpeechToTextController : BasePluginController
    {
        private const string ViewPath = "~/Plugins/Misc.SpeechToText/Views/Configure.cshtml";

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWhisperTranscriptionService _whisper;

        public MiscSpeechToTextController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWhisperTranscriptionService whisper)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _whisper = whisper;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<SpeechToTextSettings>(storeScope);

            var model = new ConfigurationModel
            {
                ActiveStoreScopeConfiguration = storeScope,
                AutoSubmitSearch = settings.AutoSubmitSearch,
                EnableVoiceCommands = settings.EnableVoiceCommands,
                EnableServerFallback = settings.EnableServerFallback,
                WhisperModelPath = settings.WhisperModelPath,
                MaxRecordingSeconds = settings.MaxRecordingSeconds,
                WhisperModelFound = _whisper.IsAvailable(settings.WhisperModelPath)
            };

            if (storeScope > 0)
            {
                model.AutoSubmitSearch_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.AutoSubmitSearch, storeScope);
                model.EnableVoiceCommands_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.EnableVoiceCommands, storeScope);
                model.EnableServerFallback_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.EnableServerFallback, storeScope);
                model.WhisperModelPath_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.WhisperModelPath, storeScope);
                model.MaxRecordingSeconds_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.MaxRecordingSeconds, storeScope);
            }

            return View(ViewPath, model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (model.MaxRecordingSeconds < SpeechToTextDefaults.MinRecordingSeconds ||
                model.MaxRecordingSeconds > SpeechToTextDefaults.MaxRecordingSecondsLimit)
            {
                ModelState.AddModelError(nameof(model.MaxRecordingSeconds), string.Format(
                    await _localizationService.GetResourceAsync("Plugins.Misc.SpeechToText.MaxRecordingSeconds.Range"),
                    SpeechToTextDefaults.MinRecordingSeconds, SpeechToTextDefaults.MaxRecordingSecondsLimit));
            }

            if (!ModelState.IsValid)
            {
                model.WhisperModelFound = _whisper.IsAvailable(model.WhisperModelPath);
                return View(ViewPath, model);
            }

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<SpeechToTextSettings>(storeScope);

            settings.AutoSubmitSearch = model.AutoSubmitSearch;
            settings.EnableVoiceCommands = model.EnableVoiceCommands;
            settings.EnableServerFallback = model.EnableServerFallback;
            settings.WhisperModelPath = string.IsNullOrWhiteSpace(model.WhisperModelPath)
                ? SpeechToTextDefaults.DefaultWhisperModelPath
                : model.WhisperModelPath.Trim();
            settings.MaxRecordingSeconds = model.MaxRecordingSeconds;

            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.AutoSubmitSearch, model.AutoSubmitSearch_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.EnableVoiceCommands, model.EnableVoiceCommands_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.EnableServerFallback, model.EnableServerFallback_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.WhisperModelPath, model.WhisperModelPath_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.MaxRecordingSeconds, model.MaxRecordingSeconds_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }
    }
}
