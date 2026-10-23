using System;

namespace tplaba3
{
    public interface ITranscriptionRepository
    {
        bool StartTranscription(int sessionId, string audioFormat);
        TranscriptionSession GetSessionById(int id);
    }
}