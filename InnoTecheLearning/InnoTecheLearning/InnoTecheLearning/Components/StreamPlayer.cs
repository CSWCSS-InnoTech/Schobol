using System.IO;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;

namespace InnoTecheLearning
{
    public class StreamPlayer : ISoundPlayer, System.IDisposable
    {
        public enum Sounds : byte
        {Violin_G,
        Violin_D,
        Violin_A,
        Violin_E,
        Cello_C,
        Cello_G,
        Cello_D,
        Cello_A}
        public static async Task<StreamPlayer> Play(Sounds Sound, double Volume = 1)
        {
            string Name = "";
            switch (Sound)
            {
                case Sounds.Violin_G:
                    Name = "ViolinG.wav";
                    break;
                case Sounds.Violin_D:
                    Name = "ViolinD.wav";
                    break;
                case Sounds.Violin_A:
                    Name = "ViolinA.wav";
                    break;
                case Sounds.Violin_E:
                    Name = "ViolinE.wav";
                    break;
                case Sounds.Cello_C:
                    Name = "CelloCC.wav";
                    break;
                case Sounds.Cello_G:
                    Name = "CelloGG.wav";
                    break;
                case Sounds.Cello_D:
                    Name = "CelloD.wav";
                    break;
                case Sounds.Cello_A:
                    Name = "CelloA.wav";
                    break;
                default:
                    break;
            }
            var Return = await Create(Resources.GetStream("Sounds." + Name), true, Volume);
            Alert(App.Current.MainPage, "After StreamPlayer.Create");
            await Return.Play();
            Alert(App.Current.MainPage, "After StreamPlayer.Play");
            return Return;
        }

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
            File = Temp.TempFile;
            Temp.SaveStream(File, Stream);
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                Temp.Delete(File);
                // TODO: set large fields to null.
                _Player = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~StreamPlayer() {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
         }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}