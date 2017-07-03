using System;

namespace NetSteps.Silverlight
{
    public interface IVideoPlayback
    {
        Uri Source { get; set; }
        bool AutoPlay { get; set; }

        void Play();
        void Pause();
        void Stop();
    }
}
