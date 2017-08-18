using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
#if !WINDOWS_UWP
        [Serializable]
#endif
        [StructLayout(LayoutKind.Sequential, Size = 1), ComVisible(true)]
        public struct Unit : IEquatable<Unit>
        {
            public static readonly Unit Default = new Unit();
            public static ValueTask<Unit> CompletedTask { get => new ValueTask<Unit>(Default); }

            public static Unit Invoke(Action a) { a?.Invoke(); return Default; }
            public static Unit Invoke<T>(Func<T> a) { a?.Invoke(); return Default; }
            public static ValueTask<Unit> InvokeAsync(Action a) => 
                new ValueTask<Unit>(Task.Run(() => { a?.Invoke(); return Default; }));
            public static ValueTask<Unit> InvokeAsync<T>(Func<T> a) => Await(Task.Run(a));
            public static ValueTask<Unit> InvokeAsync(Action a, CancellationToken c) =>
                new ValueTask<Unit>(Task.Run(() => { a?.Invoke(); return Default; }, c));
            public static ValueTask<Unit> InvokeAsync<T>(Func<T> a, CancellationToken c) => Await(Task.Run(a, c));

            public static async ValueTask<Unit> Await(Task t) { await t; return Default; }
            public static async ValueTask<Unit> Await(Func<Task> f) { await f?.Invoke(); return Default; }
            public static ValueTask<Unit> Await(IAsyncResult iar) => InvokeAsync(iar.AsyncWaitHandle.WaitOne);
            public static ValueTask<Unit> Await(Func<IAsyncResult> fiar) => Await(fiar.Invoke());
#if WINDOWS_UWP
            public static async ValueTask<Unit> Await(Windows.Foundation.IAsyncAction iaa)
            { await iaa; return Default; }
            public static ValueTask<Unit> Await(Func<Windows.Foundation.IAsyncAction> fiaa) =>
                Await(fiaa.Invoke());
            public static async ValueTask<Unit> Await<TProgress>
                (Windows.Foundation.IAsyncActionWithProgress<TProgress> iaawp)
            { await iaawp; return Default; }
            public static ValueTask<Unit> Await<TProgress>
                (Func<Windows.Foundation.IAsyncActionWithProgress<TProgress>> fiaawp) => 
                Await(fiaawp.Invoke());
#endif

            public static bool operator ==(Unit a, Unit b) => true;
            public static bool operator !=(Unit a, Unit b) => false;
            public bool Equals(Unit other) => true;
            public override bool Equals(object obj) => obj is Unit;
            public override int GetHashCode() => 0;
        }
    }
}