using NUnit.Framework;
using System;
using tplaba3;

namespace TestProject2
{
    // ========= ТЕСТОВЫЙ ДВОЙНИК =========
    public class MockTranscriptionController : ITranscriptionRepository
    {
        private bool _simulateError;
        private TranscriptionSession _mockSession;

        public MockTranscriptionController(bool simulateError = false)
        {
            _simulateError = simulateError;
            _mockSession = new TranscriptionSession(1, "Mock IP-Камера", new DateTime(2026, 5, 1), 1200);
        }

        public bool StartTranscription(int sessionId, string audioFormat)
        {
            if (_simulateError)
                return false;

            _mockSession.AudioFormat = audioFormat;
            _mockSession.Status = "Transcribed";
            return true;
        }

        public TranscriptionSession GetSessionById(int id)
        {
            return _mockSession;
        }
    }

    // ========= ОСНОВНЫЕ ТЕСТЫ =========
    public class Tests
    {
        private TranscriptionController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new TranscriptionController();
        }

        [Test]
        public void StartTranscription_ValidSession_Success_ReturnsTrue()
        {
            _controller.SimulateNoAudioError = false;
            bool result = _controller.StartTranscription(1, "FLAC");
            Assert.That(result, Is.True);
        }

        [Test]
        public void StartTranscription_NoAudioError_ReturnsFalse()
        {
            _controller.SimulateNoAudioError = true;
            bool result = _controller.StartTranscription(1, "FLAC");
            Assert.That(result, Is.False);
        }
        [TestCase(1, "FLAC", true)]
        [TestCase(2, "OGG", true)]
        [TestCase(3, "WAV", true)]
        [TestCase(999, "FLAC", false)]
        public void StartTranscription_MultipleCases_ReturnsExpected(
            int sessionId, string audioFormat, bool expectedResult)
        {
            _controller.SimulateNoAudioError = false;
            bool result = _controller.StartTranscription(sessionId, audioFormat);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MockTranscriptionController_Success_ReturnsTrue()
        {
            ITranscriptionRepository mockRepo = new MockTranscriptionController(simulateError: false);
            bool result = mockRepo.StartTranscription(1, "FLAC");
            Assert.That(result, Is.True);
        }

        [Test]
        public void MockTranscriptionController_Error_ReturnsFalse()
        {
            ITranscriptionRepository mockRepo = new MockTranscriptionController(simulateError: true);
            bool result = mockRepo.StartTranscription(1, "FLAC");
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateSession_InvalidId_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new TranscriptionSession(-1, "Тест Камера", DateTime.Now, 600));
        }

        [Test]
        public void CreateSession_FutureDate_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new TranscriptionSession(1, "Тест Камера", DateTime.Now.AddDays(5), 600));
        }

        [Test]
        public void CreateSession_InvalidDuration_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                new TranscriptionSession(1, "Тест Камера", DateTime.Now, -10));
        }
    }
}