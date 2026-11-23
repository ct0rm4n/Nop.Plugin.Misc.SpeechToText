using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nop.Core.Infrastructure;
using Whisper.net;

namespace Nop.Plugin.Misc.SpeechToText.Services
{
    /// <summary>
    /// Transcrição offline e gratuita com Whisper.net (whisper.cpp). Registrado como singleton:
    /// cada modelo é carregado uma única vez; cada requisição cria seu próprio processor.
    /// </summary>
    public sealed class WhisperTranscriptionService : IWhisperTranscriptionService, IDisposable
    {
        private readonly INopFileProvider _fileProvider;
        private readonly ConcurrentDictionary<string, Lazy<WhisperFactory>> _factories = new(StringComparer.OrdinalIgnoreCase);
        // Whisper usa bastante CPU: processa 1 áudio por vez
        private static readonly SemaphoreSlim _gate = new(1, 1);

        public WhisperTranscriptionService(INopFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        private string MapPath(string modelPath)
        {
            if (string.IsNullOrWhiteSpace(modelPath))
                modelPath = SpeechToTextDefaults.DefaultWhisperModelPath;

            return modelPath.StartsWith("~/") ? _fileProvider.MapPath(modelPath) : modelPath;
        }

        public bool IsAvailable(string modelPath) => File.Exists(MapPath(modelPath));

        public async Task<string> TranscribeAsync(Stream wav, string modelPath, string language, CancellationToken cancellationToken = default)
        {
            var path = MapPath(modelPath);
            if (!File.Exists(path))
                throw new InvalidOperationException($"Whisper model not found at {path}");

            var factory = _factories.GetOrAdd(path,
                p => new Lazy<WhisperFactory>(() => WhisperFactory.FromPath(p), isThreadSafe: true)).Value;

            await _gate.WaitAsync(cancellationToken);
            try
            {
                var builder = factory.CreateBuilder();
                builder = string.IsNullOrWhiteSpace(language)
                    ? builder.WithLanguageDetection()
                    : builder.WithLanguage(language);

                await using var processor = builder.Build();

                var sb = new StringBuilder();
                await foreach (var segment in processor.ProcessAsync(wav, cancellationToken))
                    sb.Append(segment.Text);

                return sb.ToString().Trim();
            }
            finally
            {
                _gate.Release();
            }
        }

        public void Dispose()
        {
            foreach (var lazy in _factories.Values)
                if (lazy.IsValueCreated)
                    lazy.Value.Dispose();
        }
    }
}
