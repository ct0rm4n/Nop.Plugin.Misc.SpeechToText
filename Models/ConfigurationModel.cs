using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SpeechToText.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SpeechToText.AutoSubmitSearch")]
        public bool AutoSubmitSearch { get; set; }
        public bool AutoSubmitSearch_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SpeechToText.EnableVoiceCommands")]
        public bool EnableVoiceCommands { get; set; }
        public bool EnableVoiceCommands_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SpeechToText.EnableServerFallback")]
        public bool EnableServerFallback { get; set; }
        public bool EnableServerFallback_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SpeechToText.WhisperModelPath")]
        public string WhisperModelPath { get; set; }
        public bool WhisperModelPath_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SpeechToText.MaxRecordingSeconds")]
        public int MaxRecordingSeconds { get; set; }
        public bool MaxRecordingSeconds_OverrideForStore { get; set; }

        public bool WhisperModelFound { get; set; }
    }
}
