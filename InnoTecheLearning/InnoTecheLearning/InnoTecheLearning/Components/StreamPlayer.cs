using System.IO;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;

namespace InnoTecheLearning
{
    public class StreamPlayer : ISoundPlayer
    {
        private StreamPlayer() { }
        SoundPlayer _Player;
        string File;
        public async static Task<StreamPlayer> Create(Stream Stream, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
            await Return.Init(Stream, Loop, Volume);
            return Return;
        }
        protected async Task Init(Stream Stream, bool Loop, double Volume)
        {
            File = IO.TempFile;
            IO.SaveStream(File, Stream);
            _Player = await SoundPlayer.Create(File, Loop, Volume);
        }
        public async Task Play()
        {
            await  _Player.Play();
        }
        public async Task Pause()
        {
            await _Player.Pause();
        }
        public async Task Stop()
        {
            await _Player.Stop();
        }
        ~StreamPlayer()
        { _Player = null;
          IO.Delete(File); }
    }
}