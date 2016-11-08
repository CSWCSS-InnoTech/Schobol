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
        public static StreamPlayer PlayAsync(Sounds Sound, double Volume = 1)
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
           return  Create(Resources.GetStream("Sounds." + Name), true, Volume);
        }
        public static StreamPlayer Play(Sounds Sound, double Volume = 1)
        { return Play(Sound, Volume);}
        private StreamPlayer() { }
        SoundPlayer _Player;
        string File;
        public static StreamPlayer Create(Stream Stream, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Stream, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Stream, bool Loop, double Volume)
        {
            File = Temp.TempFile;
            Temp.SaveStream(File, Stream);
            _Player = SoundPlayer.Create(File, Loop, Volume);
        }
        public void Play()
        { _Player.Play(); }
        public void Pause()
        { _Player.Pause(); }
        public void Stop()
        { _Player.Stop(); }
        public event System.EventHandler Complete
        { add { _Player.Complete += value; } remove { _Player.Complete -= value; } } 
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