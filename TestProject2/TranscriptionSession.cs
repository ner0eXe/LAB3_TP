using System;

namespace tplaba3
{
    public class TranscriptionSession
    {
        public int Id { get; set; }
        public string CameraName { get; set; }
        public DateTime RecordDate { get; set; }
        public int DurationSeconds { get; set; }
        public string AudioFormat { get; set; }
        public string Status { get; set; }

        public TranscriptionSession(int id, string cameraName, DateTime recordDate, int durationSeconds)
        {
            // Проверка ID (не может быть меньше или равно 0)
            if (id <= 0)
                throw new ArgumentException("Ошибка: ID сессии не может быть меньше или равен 0");
            // Проверка Имени камеры/потока (не может быть пустым или null)
            if (string.IsNullOrWhiteSpace(cameraName))
                throw new ArgumentException("Ошибка: Название IP-камеры не может быть пустым");
            // Проверка Даты записи (не может быть в будущем)
            if (recordDate > DateTime.Now)
                throw new ArgumentException("Ошибка: Дата записи не может быть в будущем");
            // Проверка Длительности потока (должна быть больше 0)
            if (durationSeconds <= 0)
                throw new ArgumentException("Ошибка: Длительность потока должна быть больше 0");

            // Если все проверки пройдены - создаем объект
            Id = id;
            CameraName = cameraName;
            RecordDate = recordDate;
            DurationSeconds = durationSeconds;
            AudioFormat = "PCM";
            Status = "Ready";
        }
    }
}