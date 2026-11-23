/*
 * Speech to Text para nopCommerce 4.60
 * 1) Web Speech API do navegador (Chrome, Edge, Safari) — gratuito, sem chave
 * 2) Fallback: grava WAV 16 kHz mono e envia ao servidor (Whisper.net, offline)
 *
 * Idioma, textos, comandos e comportamento vêm da configuração do plugin
 * (JSON no atributo data-config de #stt-config, gerado pelo servidor no idioma atual da loja).
 * O nop agrupa este arquivo no rodapé, depois da div #stt-config, então ela já existe aqui.
 */
(function () {
  'use strict';

  var cfgEl = document.getElementById('stt-config');
  if (!cfgEl) return;

  var server;
  try { server = JSON.parse(cfgEl.dataset.config || '{}'); } catch (e) { return; }

  var cfg = {
    lang: server.lang || document.documentElement.lang || 'en-US',
    autoSubmit: server.autoSubmit !== false,
    serverFallback: !!server.serverFallback,
    maxSeconds: server.maxSeconds || 10,
    searchPrefixes: server.searchPrefixes || [],
    commands: server.commands || {},
    texts: server.texts || {},
    transcribeUrl: cfgEl.dataset.transcribeUrl,
    searchUrl: cfgEl.dataset.searchUrl || '/search',
    urls: {
      cart: cfgEl.dataset.cartUrl,
      wishlist: cfgEl.dataset.wishlistUrl,
      account: cfgEl.dataset.accountUrl,
      login: cfgEl.dataset.loginUrl,
      home: cfgEl.dataset.homeUrl
    }
  };
  var tokenInput = cfgEl.querySelector('input[name="__RequestVerificationToken"]');

  function t(key, arg) {
    var s = cfg.texts[key] || key;
    return arg === undefined ? s : s.replace('{0}', arg);
  }

  var SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
  var canRecord = !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia &&
                     (window.AudioContext || window.webkitAudioContext));
  var mode = SpeechRecognition ? 'browser' : (cfg.serverFallback && canRecord ? 'server' : null);
  if (!mode) return; // nenhum método disponível: não mostra o botão

  // ---------- UI ----------
  var searchInput = document.getElementById('small-searchterms') ||
                    document.querySelector('form[action*="search"] input[name="q"]');

  var btn = document.createElement('button');
  btn.type = 'button';
  btn.className = 'stt-mic-button';
  btn.title = t('Button.Title');
  btn.setAttribute('aria-label', t('Button.Title'));
  btn.innerHTML = '<svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">' +
    '<path fill="currentColor" d="M12 14a3 3 0 0 0 3-3V5a3 3 0 0 0-6 0v6a3 3 0 0 0 3 3zm5-3a5 5 0 0 1-10 0H5a7 7 0 0 0 6 6.92V21h2v-3.08A7 7 0 0 0 19 11h-2z"/></svg>';

  var status = document.createElement('div');
  status.className = 'stt-status';
  status.setAttribute('role', 'status');
  status.setAttribute('aria-live', 'polite');

  if (searchInput && searchInput.parentNode) {
    searchInput.parentNode.insertBefore(btn, searchInput.nextSibling);
  } else {
    btn.classList.add('stt-floating');
    document.body.appendChild(btn);
  }
  document.body.appendChild(status);

  var statusTimer;
  function showStatus(text, persist) {
    status.textContent = text;
    status.classList.add('stt-visible');
    clearTimeout(statusTimer);
    if (!persist) statusTimer = setTimeout(function () { status.classList.remove('stt-visible'); }, 3500);
  }
  function setListening(on) {
    btn.classList.toggle('stt-listening', on);
    if (on) showStatus(t('Status.Listening'), true);
  }

  // ---------- Comandos de voz ----------
  // Minúsculo, sem acentos e sem pontuação — a comparação funciona em qualquer idioma latino
  function normalize(s) {
    return (s || '').toLowerCase().normalize('NFD').replace(/[̀-ͯ]/g, '')
      .replace(/[.,!?;:¿¡"'’]/g, ' ').replace(/\s+/g, ' ').trim();
  }
  function go(url) { if (url) window.location.href = url; }
  function search(q) {
    var sep = cfg.searchUrl.indexOf('?') >= 0 ? '&' : '?';
    window.location.href = cfg.searchUrl + sep + 'q=' + encodeURIComponent(q);
  }
  function fillSearch(q) {
    if (!searchInput) return;
    searchInput.value = q;
    searchInput.dispatchEvent(new Event('input', { bubbles: true }));
  }

  // Frases normalizadas uma única vez
  var navCommands = Object.keys(cfg.commands).map(function (key) {
    return { key: key, phrases: (cfg.commands[key] || []).map(normalize) };
  });
  var searchPrefixes = cfg.searchPrefixes.map(normalize).filter(Boolean);

  function findNavCommand(n) {
    for (var i = 0; i < navCommands.length; i++)
      if (navCommands[i].phrases.indexOf(n) >= 0) return navCommands[i].key;
    return null;
  }

  // "buscar tênis preto" → "tênis preto" (mantém o texto original, com acentos)
  function stripSearchPrefix(raw, n) {
    for (var i = 0; i < searchPrefixes.length; i++) {
      var p = searchPrefixes[i];
      if (n === p) return '';
      if (n.indexOf(p + ' ') === 0)
        return raw.trim().split(/\s+/).slice(p.split(' ').length).join(' ');
    }
    return raw;
  }

  function handleText(raw) {
    raw = (raw || '').trim();
    if (!raw) { showStatus(t('Status.NotUnderstood')); return; }

    showStatus('“' + raw + '”');

    // Evento para usar o texto em qualquer outro lugar da loja:
    // document.addEventListener('stt:result', e => { console.log(e.detail.text); e.preventDefault(); });
    var evt = new CustomEvent('stt:result', { detail: { text: raw, autoSubmit: cfg.autoSubmit }, cancelable: true });
    if (!document.dispatchEvent(evt)) return; // alguém chamou preventDefault()

    var n = normalize(raw);

    var nav = findNavCommand(n);
    if (nav) { go(cfg.urls[nav]); return; }

    var q = stripSearchPrefix(raw, n);
    if (!q) { showStatus(t('Status.NotUnderstood')); return; }

    fillSearch(q);

    // Pesquisa automática (padrão) ou só preenche o campo para o cliente confirmar
    if (cfg.autoSubmit || !searchInput) {
      search(q);
    } else {
      searchInput.focus();
      try { searchInput.setSelectionRange(q.length, q.length); } catch (e) { /* tipo de input sem seleção */ }
    }
  }

  // ---------- Modo 1: Web Speech API ----------
  var recognition, listening = false;
  if (mode === 'browser') {
    recognition = new SpeechRecognition();
    recognition.lang = cfg.lang;
    recognition.interimResults = true;
    recognition.continuous = false;
    recognition.maxAlternatives = 1;

    var finalText = '';
    recognition.onstart = function () { listening = true; finalText = ''; setListening(true); };
    recognition.onresult = function (e) {
      var interim = '';
      for (var i = e.resultIndex; i < e.results.length; i++) {
        if (e.results[i].isFinal) finalText += e.results[i][0].transcript;
        else interim += e.results[i][0].transcript;
      }
      fillSearch(finalText + interim);
      showStatus(finalText + interim, true);
    };
    recognition.onerror = function (e) {
      var msg = {
        'not-allowed': t('Error.PermissionDenied'),
        'no-speech': t('Error.NoSpeech'),
        'network': t('Error.Network'),
        'audio-capture': t('Error.MicNotFound')
      }[e.error] || t('Error.Generic', e.error);
      showStatus(msg);
      // Se o serviço do navegador falhar e houver Whisper no servidor, troca de modo
      if ((e.error === 'network' || e.error === 'service-not-allowed') && cfg.serverFallback && canRecord) {
        mode = 'server';
      }
    };
    recognition.onend = function () {
      listening = false;
      setListening(false);
      if (finalText) handleText(finalText);
    };
  }

  // ---------- Modo 2: grava WAV e envia ao servidor (Whisper) ----------
  var rec = null;

  function startServerRecording() {
    navigator.mediaDevices.getUserMedia({ audio: { channelCount: 1, echoCancellation: true, noiseSuppression: true } })
      .then(function (stream) {
        var Ctx = window.AudioContext || window.webkitAudioContext;
        var ctx = new Ctx();
        var source = ctx.createMediaStreamSource(stream);
        var proc = ctx.createScriptProcessor(4096, 1, 1);
        var chunks = [];
        proc.onaudioprocess = function (e) { chunks.push(new Float32Array(e.inputBuffer.getChannelData(0))); };
        source.connect(proc);
        proc.connect(ctx.destination);

        rec = { stream: stream, ctx: ctx, source: source, proc: proc, chunks: chunks,
                timer: setTimeout(stopServerRecording, cfg.maxSeconds * 1000) };
        setListening(true);
      })
      .catch(function () { showStatus(t('Error.PermissionDenied')); });
  }

  function stopServerRecording() {
    if (!rec) return;
    var r = rec; rec = null;
    clearTimeout(r.timer);
    r.proc.disconnect(); r.source.disconnect();
    r.stream.getTracks().forEach(function (tr) { tr.stop(); });
    var sampleRate = r.ctx.sampleRate;
    r.ctx.close();
    setListening(false);

    var wav = encodeWav(downsample(merge(r.chunks), sampleRate, 16000), 16000);
    showStatus(t('Status.Transcribing'), true);

    var fd = new FormData();
    fd.append('audio', wav, 'voice.wav');
    if (tokenInput) fd.append('__RequestVerificationToken', tokenInput.value);

    fetch(cfg.transcribeUrl, { method: 'POST', body: fd, credentials: 'same-origin' })
      .then(function (res) {
        return res.json().catch(function () { return {}; })
          .then(function (j) { return { ok: res.ok, j: j }; });
      })
      .then(function (r) {
        if (!r.ok) throw new Error(r.j && r.j.error || t('Error.TranscribeFailed'));
        handleText(r.j.text);
      })
      .catch(function (err) { showStatus(err.message || t('Error.TranscribeFailed')); });
  }

  function merge(chunks) {
    var len = chunks.reduce(function (s, c) { return s + c.length; }, 0);
    var out = new Float32Array(len), off = 0;
    chunks.forEach(function (c) { out.set(c, off); off += c.length; });
    return out;
  }

  function downsample(buf, from, to) {
    if (from === to) return buf;
    var ratio = from / to, len = Math.floor(buf.length / ratio), out = new Float32Array(len);
    for (var i = 0; i < len; i++) {
      var start = Math.floor(i * ratio), end = Math.min(Math.floor((i + 1) * ratio), buf.length), sum = 0;
      for (var j = start; j < end; j++) sum += buf[j];
      out[i] = sum / Math.max(1, end - start);
    }
    return out;
  }

  // WAV PCM 16 bits mono — formato exigido pelo Whisper
  function encodeWav(samples, rate) {
    var buffer = new ArrayBuffer(44 + samples.length * 2), v = new DataView(buffer);
    function str(o, s) { for (var i = 0; i < s.length; i++) v.setUint8(o + i, s.charCodeAt(i)); }
    str(0, 'RIFF'); v.setUint32(4, 36 + samples.length * 2, true); str(8, 'WAVE');
    str(12, 'fmt '); v.setUint32(16, 16, true); v.setUint16(20, 1, true); v.setUint16(22, 1, true);
    v.setUint32(24, rate, true); v.setUint32(28, rate * 2, true); v.setUint16(32, 2, true); v.setUint16(34, 16, true);
    str(36, 'data'); v.setUint32(40, samples.length * 2, true);
    for (var i = 0, o = 44; i < samples.length; i++, o += 2) {
      var s = Math.max(-1, Math.min(1, samples[i]));
      v.setInt16(o, s < 0 ? s * 0x8000 : s * 0x7FFF, true);
    }
    return new Blob([v], { type: 'audio/wav' });
  }

  // ---------- Clique no microfone ----------
  btn.addEventListener('click', function () {
    if (mode === 'browser') {
      if (listening) recognition.stop();
      else { try { recognition.start(); } catch (e) { /* já iniciado */ } }
    } else {
      if (rec) stopServerRecording(); else startServerRecording();
    }
  });
})();
