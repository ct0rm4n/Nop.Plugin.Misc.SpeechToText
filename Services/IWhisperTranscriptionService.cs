using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SpeechToText.Services
{
    public interface IWhisperTranscriptionService
    {
        /// <summary>True se o arquivo do modelo existe no servidor</summary>
        /// <param name="modelPath">Caminho virtual (~/...) ou absoluto do modelo ggml</param>
        bool IsAvailable(string modelPath);

        /// <summary>Transcreve um WAV PCM 16 kHz mono</summary>
        /// <param name="language">Código ISO de 2 letras; null/vazio = detecção automática</param>
        Task<string> TranscribeAsync(Stream wav, string modelPath, string language, CancellationToken cancellationToken = default);
    }
}
