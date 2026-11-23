# Speech to Text (pesquisa por voz) — nopCommerce 4.60

> English version: [README.md](README.md)

Botão de microfone ao lado da busca da loja. O cliente fala, o texto aparece no campo de pesquisa e a busca é executada, no idioma que o cliente escolheu na loja.

## Recursos
- **Pesquisa por voz** com execução automática ou apenas preenchendo o campo (configurável).
- **Multi-idioma**: o reconhecimento segue o idioma atual da loja. Textos incluídos em inglês, português, espanhol, francês, alemão e italiano; qualquer outro idioma recebe o inglês e pode ser traduzido pelo admin.
- **Comandos de voz** de navegação (carrinho, lista de desejos, minha conta, login, página inicial), editáveis por idioma.
- **Gratuito, sem chaves de API**:
  1. **Navegador (padrão):** Web Speech API — Chrome, Edge e Safari. Exige HTTPS (ou localhost). No Chrome o áudio é processado pelo serviço do Google.
  2. **Servidor (fallback opcional):** Whisper.net (MIT) roda offline no seu servidor, para navegadores sem reconhecimento de voz (ex.: Firefox).
- **Multi-loja**: todas as opções podem ser definidas por loja.

## Instalação
1. Copie a pasta para `src/Plugins/Nop.Plugin.Misc.SpeechToText`.
2. No Visual Studio: botão direito na pasta *Plugins* da solução → **Add → Existing Project** → selecione o `.csproj`.
3. Compile (Debug ou Release) **com o site parado**. A saída vai para `Presentation/Nop.Web/Plugins/Misc.SpeechToText`.
4. Admin → **Configuração → Plugins locais** → instale **Speech to Text** e reinicie a aplicação.
5. Admin → **Configuração → Widgets** → ative **Speech to Text** e clique em **Configurar**.

### Atualização de versão anterior
Basta substituir os arquivos e reiniciar. Quando a versão do `plugin.json` muda, o nopCommerce chama a atualização do plugin, que grava/atualiza os textos em todos os idiomas — não é preciso reinstalar.

## Configuração
| Opção | Padrão | Descrição |
|---|---|---|
| Pesquisar automaticamente | Ligado | Ligado: a busca é executada assim que o cliente para de falar. Desligado: o texto só é digitado no campo de busca (com foco) e o cliente confirma. |
| Ativar comandos de voz | Ligado | Reconhece os comandos de navegação. Desligado: tudo o que for falado vira pesquisa. |
| Ativar fallback no servidor (Whisper) | Ligado | Usa o Whisper quando o navegador não suporta reconhecimento de voz. Só funciona se o modelo existir. |
| Caminho do modelo Whisper | `~/App_Data/whisper/ggml-base.bin` | Caminho do arquivo `ggml-*.bin`. A tela mostra se o modelo foi encontrado. |
| Tempo máximo de gravação | 10 s | Duração máxima do áudio no modo servidor (3 a 30 s). |

Os comandos de navegação sempre abrem a página correspondente, mesmo com a pesquisa automática desligada.

## (Opcional) Ativar o Whisper no servidor
Baixe gratuitamente um modelo e salve no caminho configurado (padrão `Presentation/Nop.Web/App_Data/whisper/ggml-base.bin`):
https://huggingface.co/ggerganov/whisper.cpp/tree/main

| Modelo | Tamanho | Observação |
|---|---|---|
| ggml-tiny.bin  | ~75 MB  | mais rápido, menos preciso |
| ggml-base.bin  | ~142 MB | bom equilíbrio (padrão) |
| ggml-small.bin | ~466 MB | mais preciso, mais lento |

Use os modelos multilíngues (sem `.en` no nome) para reconhecer idiomas além do inglês. O pacote `Whisper.net.Runtime` já traz as bibliotecas nativas (Windows, Linux, macOS). O servidor processa um áudio por vez.

## Idiomas e traduções
- **Idioma do reconhecimento:** o navegador usa a *cultura* do idioma da loja (ex.: `pt-BR`); o Whisper usa o *Código SEO* (ex.: `pt`). Confira esses campos em **Configuração → Idiomas**.
- **Editar textos:** **Configuração → Idiomas → (idioma) → Recursos de string**, pesquise por `Plugins.Misc.SpeechToText`.
- **Novos idiomas:** ao criar um idioma na loja, o plugin grava seus textos nele automaticamente (em inglês, se não houver tradução embutida).
- **Adicionar uma tradução embutida:** inclua um bloco com o código de 2 letras em `SpeechToTextLocales.cs`.

## Comandos de voz
As frases ficam nos recursos de string, separadas por `|`. A comparação ignora maiúsculas, acentos e pontuação, e a frase precisa ser falada inteira.

| Recurso | Exemplo (pt) | Ação |
|---|---|---|
| `Commands.Search` | `buscar por\|pesquisar\|procurar` | prefixo: "buscar tênis preto" pesquisa "tênis preto" |
| `Commands.Cart` | `carrinho\|abrir carrinho` | carrinho |
| `Commands.Wishlist` | `lista de desejos` | lista de desejos |
| `Commands.Account` | `minha conta` | conta do cliente |
| `Commands.Login` | `entrar\|login` | login |
| `Commands.Home` | `inicio\|pagina inicial` | página inicial |
| — | qualquer outra frase | pesquisa de produtos com o texto falado |

## Usar o texto em outro lugar (JavaScript)
```js
document.addEventListener('stt:result', function (e) {
  console.log('Texto falado:', e.detail.text, 'Pesquisa automática:', e.detail.autoSubmit);
  // e.preventDefault(); // impede a ação padrão (busca/navegação)
});
```

## Requisitos e observações
- nopCommerce 4.60 (.NET 7).
- O `Whisper.net` está fixo na **1.7.2**: versões mais novas exigem `Microsoft.Bcl.AsyncInterfaces 9.0`, que conflita com a 6.0 carregada pelo nopCommerce 4.60. Não atualize o pacote enquanto a loja estiver em .NET 7.
- O botão é inserido ao lado do campo `#small-searchterms` (tema padrão). Se o tema não tiver esse campo, o botão aparece flutuando no canto inferior direito.
- Se nenhum método estiver disponível (navegador sem Web Speech e sem Whisper no servidor), o botão não é exibido.

## Desinstalação
Ao desinstalar, o plugin remove suas configurações e todos os textos `Plugins.Misc.SpeechToText.*`. O modelo Whisper em `App_Data` não é apagado.
