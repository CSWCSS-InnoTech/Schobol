using System.IO;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;

namespace InnoTecheLearning
{
    class StreamPlayer : ISoundPlayer
    {
        private StreamPlayer() { }
        SoundPlayer _Player;
        public async static Task<StreamPlayer> Create(Stream Stream, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
            await Return.Init(Stream, Loop, Volume);
            return Return;
        }
        protected async Task Init(Stream Stream, bool Loop, double Volume)
        {
            var FilePath = IO.TempFile;
            IO.SaveBytes(FilePath, ToBytes(Stream));
            _Player = await SoundPlayer.Create(FilePath, Loop, Volume);
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
    }
}