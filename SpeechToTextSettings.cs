using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.SpeechToText
{
    /// <summary>
    /// Configurações do plugin (por loja). Os valores iniciais valem como padrão
    /// quando a configuração ainda não foi salva no banco.
    /// </summary>
    public class SpeechToTextSettings : ISettings
    {
        /// <summary>Ao terminar de falar, executa a pesquisa automaticamente (senão só preenche o campo)</summary>
        public bool AutoSubmitSearch { get; set; } = true;

        /// <summary>Reconhece comandos de navegação ("carrinho", "minha conta"...)</summary>
        public bool EnableVoiceCommands { get; set; } = true;

        /// <summary>Usa o Whisper no servidor quando o navegador não suporta reconhecimento de voz</summary>
        public bool EnableServerFallback { get; set; } = true;

        /// <summary>Caminho do modelo Whisper (ggml-*.bin)</summary>
        public string WhisperModelPath { get; set; } = SpeechToTextDefaults.DefaultWhisperModelPath;

        /// <summary>Tempo máximo de gravação no modo servidor (segundos)</summary>
        public int MaxRecordingSeconds { get; set; } = 10;
    }
}
