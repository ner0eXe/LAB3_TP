using System;
using System.Collections.Generic;

namespace tplaba3
{
    public class TranscriptionController : ITranscriptionRepository
    {
        private List<TranscriptionSession> _sessions = new List<TranscriptionSession>();

        // Флаг для имитации отсутствия звука в видеопотоке (для тестирования альтернативного потока)
        public bool SimulateNoAudioError { get; set; } = false;

        public TranscriptionController()
        {
            // Тестовые данные сессий с IP-камер
            _sessions.Add(new TranscriptionSession(1, "Конференц-зал №1 (RTSP)", new DateTime(2026, 5, 1), 1800));
            _sessions.Add(new TranscriptionSession(2, "Переговорная №2 (RTSP)", new DateTime(2026, 4, 15), 2400));
            _sessions.Add(new TranscriptionSession(3, "Аудитория 305 (Файл MP4)", new DateTime(2026, 5, 10), 3600));
        }

        /// <summary>
        /// МОДУЛЬ №1: Запуск транскрипции видеопотока
        /// </summary>
        /// <param name="sessionId">ID сессии</param>
        /// <param name="audioFormat">Целевой аудиокодек (FLAC/OGG/WAV)</param>
        /// <returns>true - успех, false - ошибка</returns>
        public bool StartTranscription(int sessionId, string audioFormat)
        {
            // Находим сессию по ID
            var session = _sessions.Find(s => s.Id == sessionId);
            if (session == null || string.IsNullOrWhiteSpace(audioFormat))
                return false;

            // Извлекаем аудио и отправляем в API (вызов Модуля №2)
            if (!ExtractAndProcessAudio())
                return false;

            session.AudioFormat = audioFormat;
            session.Status = "Transcribed";
            return true;
        }

        /// <summary>
        /// МОДУЛЬ №2: Извлечение аудиодорожки из видеопотока и отправка в облачный API
        /// АЛЬТЕРНАТИВНЫЙ ПОТОК: при SimulateNoAudioError = true возвращает false
        /// </summary>
        private bool ExtractAndProcessAudio()
        {
            if (SimulateNoAudioError)
            {
                // АЛЬТЕРНАТИВНЫЙ ПОТОК: в видеопотоке отсутствует звуковая дорожка
                return false;
            }
            // ОСНОВНОЙ ПОТОК: звук успешно извлечен и распознан
            return true;
        }

        // Вспомогательный метод для получения сессии по ID (для тестов)
        public TranscriptionSession GetSessionById(int id)
        {
            return _sessions.Find(s => s.Id == id);
        }
    }
}