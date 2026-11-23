namespace Nop.Plugin.Misc.SpeechToText
{
    public static class SpeechToTextDefaults
    {
        public const string SystemName = "Misc.SpeechToText";
        public const string TranscribeRouteName = "Plugin.Misc.SpeechToText.Transcribe";

        /// <summary>Prefixo de todos os recursos de idioma do plugin</summary>
        public const string LocalePrefix = "Plugins.Misc.SpeechToText";

        /// <summary>Caminho padrão do modelo Whisper (baixe gratuitamente no Hugging Face)</summary>
        public const string DefaultWhisperModelPath = "~/App_Data/whisper/ggml-base.bin";

        /// <summary>Limites do tempo de gravação no modo servidor (segundos)</summary>
        public const int MinRecordingSeconds = 3;
        public const int MaxRecordingSecondsLimit = 30;

        /// <summary>Tamanho máximo do áudio enviado ao servidor (bytes) ≈ 30 s em WAV 16 kHz mono</summary>
        public const long MaxAudioBytes = 1_000_000;
    }
}
