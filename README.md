# Speech to Text (voice search) — nopCommerce 4.60

> Versão em português: [LEIA-ME.md](LEIA-ME.md)

Adds a microphone button next to the store search box. The customer speaks, the text appears in the search field and the search runs, in the language the customer selected in the store.

## Features
- **Voice search** that either runs the search automatically or just fills in the search box (configurable).
- **Multi-language**: recognition follows the current store language. Built-in texts in English, Portuguese, Spanish, French, German and Italian; any other language gets English and can be translated from the admin area.
- **Voice commands** for navigation (cart, wishlist, my account, login, home page), editable per language.
- **Free, no API keys**:
  1. **Browser (default):** Web Speech API — Chrome, Edge and Safari. Requires HTTPS (or localhost). In Chrome the audio is processed by Google's speech service.
  2. **Server (optional fallback):** Whisper.net (MIT) runs offline on your server for browsers without speech recognition (e.g. Firefox).
- **Multi-store**: every option can be set per store.

## Installation
1. Copy the folder to `src/Plugins/Nop.Plugin.Misc.SpeechToText`.
2. In Visual Studio: right-click the solution's *Plugins* folder → **Add → Existing Project** → select the `.csproj`.
3. Build (Debug or Release) **with the site stopped**. The output goes to `Presentation/Nop.Web/Plugins/Misc.SpeechToText`.
4. Admin → **Configuration → Local plugins** → install **Speech to Text** and restart the application.
5. Admin → **Configuration → Widgets** → activate **Speech to Text** and click **Configure**.

If you received the plugin already compiled, copy the `Misc.SpeechToText` folder to `Presentation/Nop.Web/Plugins` (or `Plugins` in a published site) and follow steps 4 and 5.

### Upgrading from a previous version
Replace the files and restart. When the version in `plugin.json` changes, nopCommerce runs the plugin update, which adds/updates the texts in every language — no reinstall needed.

## Configuration
| Option | Default | Description |
|---|---|---|
| Search automatically | On | On: the search runs as soon as the customer stops speaking. Off: the text is only typed into the search box (focused) and the customer submits it. |
| Enable voice commands | On | Recognizes the navigation commands. Off: everything spoken becomes a search. |
| Enable server fallback (Whisper) | On | Uses Whisper when the browser has no speech recognition. Only works if the model file exists. |
| Whisper model path | `~/App_Data/whisper/ggml-base.bin` | Path of the `ggml-*.bin` file. The page shows whether the model was found. |
| Maximum recording time | 10 s | Maximum audio length in server mode (3 to 30 s). |

Navigation commands always open their page, even when automatic search is off.

## (Optional) Enable Whisper on the server
Download a free model and save it in the configured path (default `Presentation/Nop.Web/App_Data/whisper/ggml-base.bin`):
https://huggingface.co/ggerganov/whisper.cpp/tree/main

| Model | Size | Notes |
|---|---|---|
| ggml-tiny.bin  | ~75 MB  | fastest, less accurate |
| ggml-base.bin  | ~142 MB | good balance (default) |
| ggml-small.bin | ~466 MB | more accurate, slower |

Use the multilingual models (no `.en` in the name) to recognize languages other than English. The `Whisper.net.Runtime` package already ships the native libraries (Windows, Linux, macOS). The server processes one audio at a time.

## Languages and translations
- **Recognition language:** the browser uses the store language *culture* (e.g. `en-US`); Whisper uses the *SEO code* (e.g. `en`). Check these fields in **Configuration → Languages**.
- **Edit texts:** **Configuration → Languages → (language) → String resources**, search for `Plugins.Misc.SpeechToText`.
- **New languages:** when you create a language in the store, the plugin adds its texts to it automatically (in English if there is no built-in translation).
- **Add a built-in translation:** add a block with the 2-letter code in `SpeechToTextLocales.cs`.

## Voice commands
Phrases are stored in the string resources, separated by `|`. Matching ignores case, accents and punctuation, and the whole phrase must be spoken.

| Resource | Example (en) | Action |
|---|---|---|
| `Commands.Search` | `search for\|search\|find` | prefix: "search red shoes" searches for "red shoes" |
| `Commands.Cart` | `cart\|open cart` | shopping cart |
| `Commands.Wishlist` | `wishlist\|wish list` | wishlist |
| `Commands.Account` | `my account` | customer account |
| `Commands.Login` | `login\|sign in` | login |
| `Commands.Home` | `home\|home page` | home page |
| — | any other phrase | product search with the spoken text |

## Using the text elsewhere (JavaScript)
```js
document.addEventListener('stt:result', function (e) {
  console.log('Spoken text:', e.detail.text, 'Auto search:', e.detail.autoSubmit);
  // e.preventDefault(); // cancels the default action (search/navigation)
});
```

## Requirements and notes
- nopCommerce 4.60 (.NET 7).
- `Whisper.net` is pinned to **1.7.2**: newer versions require `Microsoft.Bcl.AsyncInterfaces 9.0`, which conflicts with version 6.0 loaded by nopCommerce 4.60. Do not upgrade the package while the store runs on .NET 7.
- The button is placed next to the `#small-searchterms` field (default theme). If the theme has no such field, the button floats in the bottom-right corner.
- If no method is available (browser without Web Speech and no Whisper on the server), the button is not shown.

## Uninstall
Uninstalling removes the plugin settings and all `Plugins.Misc.SpeechToText.*` texts. The Whisper model in `App_Data` is not deleted.
