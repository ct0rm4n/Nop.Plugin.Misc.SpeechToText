using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Misc.SpeechToText
{
    /// <summary>
    /// Textos do plugin por idioma. O inglês é a base: qualquer idioma da loja sem tradução
    /// aqui recebe o texto em inglês, e o lojista pode editar tudo em
    /// Admin → Configuração → Idiomas → Recursos de string (filtro "Plugins.Misc.SpeechToText").
    /// Para adicionar um idioma, crie um novo bloco no dicionário <see cref="_translations"/>
    /// usando o código ISO de 2 letras (o mesmo do campo "Código SEO" do idioma).
    /// </summary>
    public static class SpeechToTextLocales
    {
        // Comandos de voz: frases separadas por "|". A comparação ignora maiúsculas, acentos e pontuação.
        // "Commands.Search" são prefixos: "buscar tênis" → pesquisa "tênis".

        private static readonly Dictionary<string, Dictionary<string, string>> _translations = new(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new()
            {
                ["Instructions"] = "<p>Adds a microphone button next to the store search box. The customer speaks and the text is typed into the search field.</p><ul><li><b>Browser (default):</b> Web Speech API in Chrome, Edge and Safari, free and without keys. Requires HTTPS.</li><li><b>Server (optional fallback):</b> Whisper.net runs offline on your server for browsers without speech recognition (e.g. Firefox). Download a free model (ggml-base.bin) from <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> and save it in the path below.</li></ul><p>The recognition language follows the customer's current store language. Texts and voice commands can be translated in <i>Configuration → Languages → String resources</i> (search for <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Search automatically",
                ["AutoSubmitSearch.Hint"] = "When enabled, the search runs as soon as the customer stops speaking. When disabled, the spoken text is only typed into the search box and the customer submits it.",
                ["EnableVoiceCommands"] = "Enable voice commands",
                ["EnableVoiceCommands.Hint"] = "Recognize navigation commands such as \"cart\", \"wishlist\", \"my account\". The phrases of each language can be edited in the string resources.",
                ["EnableServerFallback"] = "Enable server fallback (Whisper)",
                ["EnableServerFallback.Hint"] = "Transcribe audio on the server with Whisper.net when the browser has no speech recognition. Only works if the model file exists.",
                ["WhisperModelPath"] = "Whisper model path",
                ["WhisperModelPath.Hint"] = "Path of the ggml model file. Use ~/ for paths relative to the site root, e.g. ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Model found.",
                ["WhisperModelPath.NotFound"] = "Model not found: the server fallback is inactive.",
                ["MaxRecordingSeconds"] = "Maximum recording time (seconds)",
                ["MaxRecordingSeconds.Hint"] = "Maximum length of the audio recorded in server mode.",
                ["MaxRecordingSeconds.Range"] = "Enter a value between {0} and {1}.",

                ["Button.Title"] = "Search by voice",
                ["Status.Listening"] = "Listening… speak now",
                ["Status.NotUnderstood"] = "Sorry, I didn't catch that. Please try again.",
                ["Status.Transcribing"] = "Transcribing…",
                ["Error.PermissionDenied"] = "Microphone permission denied.",
                ["Error.NoSpeech"] = "No speech detected.",
                ["Error.Network"] = "Could not connect to the browser's speech service.",
                ["Error.MicNotFound"] = "No microphone found.",
                ["Error.Generic"] = "Error: {0}",
                ["Error.TranscribeFailed"] = "Could not transcribe the audio.",
                ["Error.ServerUnavailable"] = "Server transcription is unavailable.",
                ["Error.NoAudio"] = "No audio received.",
                ["Error.AudioTooLong"] = "Audio is too long.",

                ["Commands.Search"] = "search for|search|find|look for|show me",
                ["Commands.Cart"] = "cart|shopping cart|open cart|go to cart|my cart",
                ["Commands.Wishlist"] = "wishlist|wish list|open wishlist|my wishlist",
                ["Commands.Account"] = "my account|account|open my account",
                ["Commands.Login"] = "login|log in|sign in",
                ["Commands.Home"] = "home|home page|go home|start page"
            },

            ["pt"] = new()
            {
                ["Instructions"] = "<p>Adiciona um botão de microfone ao lado da busca da loja. O cliente fala e o texto é digitado no campo de pesquisa.</p><ul><li><b>Navegador (padrão):</b> Web Speech API no Chrome, Edge e Safari, gratuita e sem chaves. Exige HTTPS.</li><li><b>Servidor (fallback opcional):</b> o Whisper.net roda offline no seu servidor para navegadores sem reconhecimento de voz (ex.: Firefox). Baixe um modelo gratuito (ggml-base.bin) no <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> e salve no caminho abaixo.</li></ul><p>O idioma do reconhecimento segue o idioma atual da loja escolhido pelo cliente. Textos e comandos de voz podem ser traduzidos em <i>Configuração → Idiomas → Recursos de string</i> (pesquise por <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Pesquisar automaticamente",
                ["AutoSubmitSearch.Hint"] = "Ativado: a pesquisa é executada assim que o cliente termina de falar. Desativado: o texto falado só é digitado no campo de busca e o cliente confirma a pesquisa.",
                ["EnableVoiceCommands"] = "Ativar comandos de voz",
                ["EnableVoiceCommands.Hint"] = "Reconhece comandos de navegação como \"carrinho\", \"lista de desejos\", \"minha conta\". As frases de cada idioma podem ser editadas nos recursos de string.",
                ["EnableServerFallback"] = "Ativar fallback no servidor (Whisper)",
                ["EnableServerFallback.Hint"] = "Transcreve o áudio no servidor com Whisper.net quando o navegador não tem reconhecimento de voz. Só funciona se o arquivo do modelo existir.",
                ["WhisperModelPath"] = "Caminho do modelo Whisper",
                ["WhisperModelPath.Hint"] = "Caminho do arquivo de modelo ggml. Use ~/ para caminhos relativos à raiz do site, ex.: ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Modelo encontrado.",
                ["WhisperModelPath.NotFound"] = "Modelo não encontrado: o fallback no servidor está inativo.",
                ["MaxRecordingSeconds"] = "Tempo máximo de gravação (segundos)",
                ["MaxRecordingSeconds.Hint"] = "Duração máxima do áudio gravado no modo servidor.",
                ["MaxRecordingSeconds.Range"] = "Informe um valor entre {0} e {1}.",

                ["Button.Title"] = "Pesquisar por voz",
                ["Status.Listening"] = "Ouvindo… pode falar",
                ["Status.NotUnderstood"] = "Não entendi. Tente novamente.",
                ["Status.Transcribing"] = "Transcrevendo…",
                ["Error.PermissionDenied"] = "Permissão de microfone negada.",
                ["Error.NoSpeech"] = "Nenhuma fala detectada.",
                ["Error.Network"] = "Sem conexão com o serviço de voz do navegador.",
                ["Error.MicNotFound"] = "Microfone não encontrado.",
                ["Error.Generic"] = "Erro: {0}",
                ["Error.TranscribeFailed"] = "Falha ao transcrever o áudio.",
                ["Error.ServerUnavailable"] = "Transcrição no servidor indisponível.",
                ["Error.NoAudio"] = "Nenhum áudio recebido.",
                ["Error.AudioTooLong"] = "Áudio muito longo.",

                ["Commands.Search"] = "buscar por|pesquisar por|procurar por|buscar|pesquisar|procurar|encontrar",
                ["Commands.Cart"] = "carrinho|abrir carrinho|abrir o carrinho|ir para o carrinho|ver carrinho|ver o carrinho|meu carrinho",
                ["Commands.Wishlist"] = "lista de desejos|abrir lista de desejos|ir para a lista de desejos|ver a lista de desejos",
                ["Commands.Account"] = "minha conta|abrir minha conta|ir para minha conta",
                ["Commands.Login"] = "entrar|login|fazer login",
                ["Commands.Home"] = "inicio|pagina inicial|home|voltar ao inicio"
            },

            ["es"] = new()
            {
                ["Instructions"] = "<p>Añade un botón de micrófono junto al buscador de la tienda. El cliente habla y el texto se escribe en el campo de búsqueda.</p><ul><li><b>Navegador (predeterminado):</b> Web Speech API en Chrome, Edge y Safari, gratuita y sin claves. Requiere HTTPS.</li><li><b>Servidor (alternativa opcional):</b> Whisper.net se ejecuta sin conexión en su servidor para navegadores sin reconocimiento de voz (p. ej. Firefox). Descargue un modelo gratuito (ggml-base.bin) de <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> y guárdelo en la ruta indicada abajo.</li></ul><p>El idioma de reconocimiento sigue el idioma actual de la tienda. Los textos y comandos de voz se pueden traducir en <i>Configuración → Idiomas → Recursos de cadena</i> (busque <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Buscar automáticamente",
                ["AutoSubmitSearch.Hint"] = "Activado: la búsqueda se ejecuta en cuanto el cliente deja de hablar. Desactivado: el texto solo se escribe en el buscador y el cliente confirma la búsqueda.",
                ["EnableVoiceCommands"] = "Activar comandos de voz",
                ["EnableVoiceCommands.Hint"] = "Reconoce comandos de navegación como \"carrito\", \"lista de deseos\", \"mi cuenta\". Las frases de cada idioma se pueden editar en los recursos de cadena.",
                ["EnableServerFallback"] = "Activar alternativa en el servidor (Whisper)",
                ["EnableServerFallback.Hint"] = "Transcribe el audio en el servidor con Whisper.net cuando el navegador no tiene reconocimiento de voz. Solo funciona si existe el archivo del modelo.",
                ["WhisperModelPath"] = "Ruta del modelo Whisper",
                ["WhisperModelPath.Hint"] = "Ruta del archivo de modelo ggml. Use ~/ para rutas relativas a la raíz del sitio, p. ej. ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Modelo encontrado.",
                ["WhisperModelPath.NotFound"] = "Modelo no encontrado: la alternativa en el servidor está inactiva.",
                ["MaxRecordingSeconds"] = "Tiempo máximo de grabación (segundos)",
                ["MaxRecordingSeconds.Hint"] = "Duración máxima del audio grabado en modo servidor.",
                ["MaxRecordingSeconds.Range"] = "Introduzca un valor entre {0} y {1}.",

                ["Button.Title"] = "Buscar por voz",
                ["Status.Listening"] = "Escuchando… hable ahora",
                ["Status.NotUnderstood"] = "No le he entendido. Inténtelo de nuevo.",
                ["Status.Transcribing"] = "Transcribiendo…",
                ["Error.PermissionDenied"] = "Permiso de micrófono denegado.",
                ["Error.NoSpeech"] = "No se detectó voz.",
                ["Error.Network"] = "No se pudo conectar con el servicio de voz del navegador.",
                ["Error.MicNotFound"] = "No se encontró ningún micrófono.",
                ["Error.Generic"] = "Error: {0}",
                ["Error.TranscribeFailed"] = "No se pudo transcribir el audio.",
                ["Error.ServerUnavailable"] = "La transcripción en el servidor no está disponible.",
                ["Error.NoAudio"] = "No se recibió audio.",
                ["Error.AudioTooLong"] = "El audio es demasiado largo.",

                ["Commands.Search"] = "buscar|busca|encontrar|mostrar",
                ["Commands.Cart"] = "carrito|abrir carrito|ir al carrito|ver carrito|mi carrito",
                ["Commands.Wishlist"] = "lista de deseos|abrir lista de deseos|mi lista de deseos",
                ["Commands.Account"] = "mi cuenta|abrir mi cuenta|cuenta",
                ["Commands.Login"] = "entrar|iniciar sesion|login",
                ["Commands.Home"] = "inicio|pagina de inicio|volver al inicio|home"
            },

            ["fr"] = new()
            {
                ["Instructions"] = "<p>Ajoute un bouton micro à côté de la recherche de la boutique. Le client parle et le texte est saisi dans le champ de recherche.</p><ul><li><b>Navigateur (par défaut) :</b> Web Speech API dans Chrome, Edge et Safari, gratuite et sans clé. HTTPS requis.</li><li><b>Serveur (secours optionnel) :</b> Whisper.net fonctionne hors ligne sur votre serveur pour les navigateurs sans reconnaissance vocale (ex. Firefox). Téléchargez un modèle gratuit (ggml-base.bin) sur <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> et enregistrez-le dans le chemin ci-dessous.</li></ul><p>La langue de reconnaissance suit la langue actuelle de la boutique. Les textes et commandes vocales peuvent être traduits dans <i>Configuration → Langues → Ressources de chaîne</i> (recherchez <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Rechercher automatiquement",
                ["AutoSubmitSearch.Hint"] = "Activé : la recherche est lancée dès que le client a fini de parler. Désactivé : le texte est seulement saisi dans le champ de recherche et le client valide.",
                ["EnableVoiceCommands"] = "Activer les commandes vocales",
                ["EnableVoiceCommands.Hint"] = "Reconnaît des commandes de navigation comme \"panier\", \"liste de souhaits\", \"mon compte\". Les phrases de chaque langue sont modifiables dans les ressources de chaîne.",
                ["EnableServerFallback"] = "Activer le secours serveur (Whisper)",
                ["EnableServerFallback.Hint"] = "Transcrit l'audio sur le serveur avec Whisper.net lorsque le navigateur n'a pas de reconnaissance vocale. Fonctionne uniquement si le fichier du modèle existe.",
                ["WhisperModelPath"] = "Chemin du modèle Whisper",
                ["WhisperModelPath.Hint"] = "Chemin du fichier de modèle ggml. Utilisez ~/ pour un chemin relatif à la racine du site, ex. ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Modèle trouvé.",
                ["WhisperModelPath.NotFound"] = "Modèle introuvable : le secours serveur est inactif.",
                ["MaxRecordingSeconds"] = "Durée maximale d'enregistrement (secondes)",
                ["MaxRecordingSeconds.Hint"] = "Durée maximale de l'audio enregistré en mode serveur.",
                ["MaxRecordingSeconds.Range"] = "Saisissez une valeur entre {0} et {1}.",

                ["Button.Title"] = "Recherche vocale",
                ["Status.Listening"] = "Écoute… parlez maintenant",
                ["Status.NotUnderstood"] = "Je n'ai pas compris. Veuillez réessayer.",
                ["Status.Transcribing"] = "Transcription…",
                ["Error.PermissionDenied"] = "Accès au micro refusé.",
                ["Error.NoSpeech"] = "Aucune parole détectée.",
                ["Error.Network"] = "Impossible de joindre le service vocal du navigateur.",
                ["Error.MicNotFound"] = "Aucun micro trouvé.",
                ["Error.Generic"] = "Erreur : {0}",
                ["Error.TranscribeFailed"] = "Impossible de transcrire l'audio.",
                ["Error.ServerUnavailable"] = "La transcription serveur est indisponible.",
                ["Error.NoAudio"] = "Aucun audio reçu.",
                ["Error.AudioTooLong"] = "L'audio est trop long.",

                ["Commands.Search"] = "rechercher|chercher|trouver|montre moi",
                ["Commands.Cart"] = "panier|mon panier|ouvrir le panier|aller au panier",
                ["Commands.Wishlist"] = "liste de souhaits|ma liste de souhaits|ouvrir la liste de souhaits",
                ["Commands.Account"] = "mon compte|ouvrir mon compte|compte",
                ["Commands.Login"] = "connexion|se connecter|login",
                ["Commands.Home"] = "accueil|page d'accueil|retour a l'accueil"
            },

            ["de"] = new()
            {
                ["Instructions"] = "<p>Fügt neben der Shop-Suche eine Mikrofon-Schaltfläche hinzu. Der Kunde spricht und der Text wird in das Suchfeld eingetragen.</p><ul><li><b>Browser (Standard):</b> Web Speech API in Chrome, Edge und Safari, kostenlos und ohne Schlüssel. Erfordert HTTPS.</li><li><b>Server (optionaler Fallback):</b> Whisper.net läuft offline auf Ihrem Server für Browser ohne Spracherkennung (z. B. Firefox). Laden Sie ein kostenloses Modell (ggml-base.bin) von <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> herunter und speichern Sie es unter dem unten angegebenen Pfad.</li></ul><p>Die Erkennungssprache folgt der aktuellen Shop-Sprache. Texte und Sprachbefehle können unter <i>Konfiguration → Sprachen → Zeichenfolgen-Ressourcen</i> übersetzt werden (suchen Sie nach <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Automatisch suchen",
                ["AutoSubmitSearch.Hint"] = "Aktiviert: Die Suche startet, sobald der Kunde aufhört zu sprechen. Deaktiviert: Der Text wird nur in das Suchfeld eingetragen und der Kunde startet die Suche selbst.",
                ["EnableVoiceCommands"] = "Sprachbefehle aktivieren",
                ["EnableVoiceCommands.Hint"] = "Erkennt Navigationsbefehle wie \"Warenkorb\", \"Wunschliste\", \"mein Konto\". Die Phrasen jeder Sprache können in den Zeichenfolgen-Ressourcen bearbeitet werden.",
                ["EnableServerFallback"] = "Server-Fallback aktivieren (Whisper)",
                ["EnableServerFallback.Hint"] = "Transkribiert das Audio auf dem Server mit Whisper.net, wenn der Browser keine Spracherkennung hat. Funktioniert nur, wenn die Modelldatei vorhanden ist.",
                ["WhisperModelPath"] = "Pfad des Whisper-Modells",
                ["WhisperModelPath.Hint"] = "Pfad der ggml-Modelldatei. Verwenden Sie ~/ für Pfade relativ zum Stammverzeichnis, z. B. ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Modell gefunden.",
                ["WhisperModelPath.NotFound"] = "Modell nicht gefunden: Der Server-Fallback ist inaktiv.",
                ["MaxRecordingSeconds"] = "Maximale Aufnahmedauer (Sekunden)",
                ["MaxRecordingSeconds.Hint"] = "Maximale Länge des im Servermodus aufgenommenen Audios.",
                ["MaxRecordingSeconds.Range"] = "Geben Sie einen Wert zwischen {0} und {1} ein.",

                ["Button.Title"] = "Sprachsuche",
                ["Status.Listening"] = "Ich höre zu… bitte sprechen",
                ["Status.NotUnderstood"] = "Das habe ich nicht verstanden. Bitte erneut versuchen.",
                ["Status.Transcribing"] = "Wird transkribiert…",
                ["Error.PermissionDenied"] = "Mikrofonzugriff verweigert.",
                ["Error.NoSpeech"] = "Keine Sprache erkannt.",
                ["Error.Network"] = "Keine Verbindung zum Sprachdienst des Browsers.",
                ["Error.MicNotFound"] = "Kein Mikrofon gefunden.",
                ["Error.Generic"] = "Fehler: {0}",
                ["Error.TranscribeFailed"] = "Das Audio konnte nicht transkribiert werden.",
                ["Error.ServerUnavailable"] = "Die Server-Transkription ist nicht verfügbar.",
                ["Error.NoAudio"] = "Kein Audio empfangen.",
                ["Error.AudioTooLong"] = "Das Audio ist zu lang.",

                ["Commands.Search"] = "suche nach|suche|suchen|finde|zeige mir",
                ["Commands.Cart"] = "warenkorb|mein warenkorb|warenkorb offnen|zum warenkorb",
                ["Commands.Wishlist"] = "wunschliste|meine wunschliste|wunschliste offnen",
                ["Commands.Account"] = "mein konto|konto|konto offnen",
                ["Commands.Login"] = "anmelden|einloggen|login",
                ["Commands.Home"] = "startseite|zur startseite|home"
            },

            ["it"] = new()
            {
                ["Instructions"] = "<p>Aggiunge un pulsante microfono accanto alla ricerca del negozio. Il cliente parla e il testo viene inserito nel campo di ricerca.</p><ul><li><b>Browser (predefinito):</b> Web Speech API in Chrome, Edge e Safari, gratuita e senza chiavi. Richiede HTTPS.</li><li><b>Server (alternativa opzionale):</b> Whisper.net funziona offline sul tuo server per i browser senza riconoscimento vocale (es. Firefox). Scarica un modello gratuito (ggml-base.bin) da <a href=\"https://huggingface.co/ggerganov/whisper.cpp/tree/main\" target=\"_blank\">Hugging Face</a> e salvalo nel percorso indicato sotto.</li></ul><p>La lingua di riconoscimento segue la lingua corrente del negozio. Testi e comandi vocali possono essere tradotti in <i>Configurazione → Lingue → Risorse stringa</i> (cerca <code>Plugins.Misc.SpeechToText</code>).</p>",
                ["AutoSubmitSearch"] = "Cerca automaticamente",
                ["AutoSubmitSearch.Hint"] = "Attivo: la ricerca parte appena il cliente smette di parlare. Disattivo: il testo viene solo inserito nel campo di ricerca e il cliente conferma.",
                ["EnableVoiceCommands"] = "Attiva comandi vocali",
                ["EnableVoiceCommands.Hint"] = "Riconosce comandi di navigazione come \"carrello\", \"lista dei desideri\", \"il mio account\". Le frasi di ogni lingua sono modificabili nelle risorse stringa.",
                ["EnableServerFallback"] = "Attiva alternativa sul server (Whisper)",
                ["EnableServerFallback.Hint"] = "Trascrive l'audio sul server con Whisper.net quando il browser non ha il riconoscimento vocale. Funziona solo se il file del modello esiste.",
                ["WhisperModelPath"] = "Percorso del modello Whisper",
                ["WhisperModelPath.Hint"] = "Percorso del file del modello ggml. Usa ~/ per percorsi relativi alla radice del sito, es. ~/App_Data/whisper/ggml-base.bin.",
                ["WhisperModelPath.Found"] = "Modello trovato.",
                ["WhisperModelPath.NotFound"] = "Modello non trovato: l'alternativa sul server è inattiva.",
                ["MaxRecordingSeconds"] = "Durata massima di registrazione (secondi)",
                ["MaxRecordingSeconds.Hint"] = "Durata massima dell'audio registrato in modalità server.",
                ["MaxRecordingSeconds.Range"] = "Inserisci un valore tra {0} e {1}.",

                ["Button.Title"] = "Ricerca vocale",
                ["Status.Listening"] = "In ascolto… parla ora",
                ["Status.NotUnderstood"] = "Non ho capito. Riprova.",
                ["Status.Transcribing"] = "Trascrizione in corso…",
                ["Error.PermissionDenied"] = "Permesso del microfono negato.",
                ["Error.NoSpeech"] = "Nessuna voce rilevata.",
                ["Error.Network"] = "Impossibile connettersi al servizio vocale del browser.",
                ["Error.MicNotFound"] = "Nessun microfono trovato.",
                ["Error.Generic"] = "Errore: {0}",
                ["Error.TranscribeFailed"] = "Impossibile trascrivere l'audio.",
                ["Error.ServerUnavailable"] = "La trascrizione sul server non è disponibile.",
                ["Error.NoAudio"] = "Nessun audio ricevuto.",
                ["Error.AudioTooLong"] = "L'audio è troppo lungo.",

                ["Commands.Search"] = "cerca|cercare|trova|mostrami",
                ["Commands.Cart"] = "carrello|il mio carrello|apri carrello|vai al carrello",
                ["Commands.Wishlist"] = "lista dei desideri|la mia lista dei desideri|apri lista dei desideri",
                ["Commands.Account"] = "il mio account|account|apri il mio account",
                ["Commands.Login"] = "accedi|login|entra",
                ["Commands.Home"] = "home|pagina iniziale|torna alla home"
            }
        };

        /// <summary>
        /// Recursos completos para um idioma (chaves já com o prefixo do plugin).
        /// O que não estiver traduzido cai no inglês.
        /// </summary>
        /// <param name="isoCode">Código de 2 letras (ex.: "pt") ou cultura (ex.: "pt-BR")</param>
        public static IDictionary<string, string> For(string isoCode)
        {
            var code = (isoCode ?? string.Empty).Split('-', '_')[0];
            _translations.TryGetValue(code, out var translated);

            return _translations["en"].ToDictionary(
                kv => $"{SpeechToTextDefaults.LocalePrefix}.{kv.Key}",
                kv => translated != null && translated.TryGetValue(kv.Key, out var value) ? value : kv.Value);
        }
    }
}
