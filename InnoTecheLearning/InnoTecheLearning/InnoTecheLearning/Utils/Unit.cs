using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
#if !WINDOWS_UWP
        [Serializable]
#endif
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        [ComVisible(true)]
        public struct Unit : IEquatable<Unit>
        {
            public static readonly Unit Default = new Unit();
            public static Unit Eval(Action a) { a?.Invoke(); return Default; }
            public static async ValueTask<Unit> Await(Task t) { await t; return Default; }
            public static async ValueTask<Unit> Await(Func<Task> f) { await f?.Invoke(); return Default; }
            public static ValueTask<Unit> Await(Action a) => 
                new ValueTask<Unit>(Task.Run(() => { a?.Invoke(); return Default; }));

            public static bool operator ==(Unit a, Unit b) => true;
            public static bool operator !=(Unit a, Unit b) => false;
            public bool Equals(Unit other) => true;
            public override bool Equals(object obj) => obj is Unit;
            public override int GetHashCode() => 0;
        }
    }
}