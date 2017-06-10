using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates the type of the async method builder that should be used by a language compiler to
    /// build the attributed type when used as the return type of an async method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | 
        AttributeTargets.Delegate | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class AsyncMethodBuilderAttribute : Attribute
    {
        /// <summary>Initializes the <see cref="AsyncMethodBuilderAttribute"/>.</summary>
        /// <param name="builderType">The <see cref="Type"/> of the associated builder.</param>
        public AsyncMethodBuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }

        /// <summary>Gets the <see cref="Type"/> of the associated builder.</summary>
        public Type BuilderType { get; }
    }

    /// <summary>Represents a builder for asynchronous methods that returns a <see cref="ValueTask{TResult}"/>.</summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct AsyncValueTaskMethodBuilder<TResult>
    {
        /// <summary>The <see cref="AsyncTaskMethodBuilder{TResult}"/> to which most operations are delegated.</summary>
        private AsyncTaskMethodBuilder<TResult> _methodBuilder;
        /// <summary>The result for this builder, if it's completed before any awaits occur.</summary>
        private TResult _result;
        /// <summary>true if <see cref="_result"/> contains the synchronous result for the async method; otherwise, false.</summary>
        private bool _haveResult;
        /// <summary>true if the builder should be used for setting/getting the result; otherwise, false.</summary>
        private bool _useBuilder;

        /// <summary>Creates an instance of the <see cref="AsyncValueTaskMethodBuilder{TResult}"/> struct.</summary>
        /// <returns>The initialized instance.</returns>
        public static AsyncValueTaskMethodBuilder<TResult> Create() =>
            new AsyncValueTaskMethodBuilder<TResult>() { _methodBuilder = AsyncTaskMethodBuilder<TResult>.Create() };

        /// <summary>Begins running the builder with the associated state machine.</summary>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="stateMachine">The state machine instance, passed by reference.</param>
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.Start(ref stateMachine); // will provide the right ExecutionContext semantics
        }

        /// <summary>Associates the builder with the specified state machine.</summary>
        /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine) => _methodBuilder.SetStateMachine(stateMachine);

        /// <summary>Marks the task as successfully completed.</summary>
        /// <param name="result">The result to use to complete the task.</param>
        public void SetResult(TResult result)
        {
            if (_useBuilder)
            {
                _methodBuilder.SetResult(result);
            }
            else
            {
                _result = result;
                _haveResult = true;
            }
        }

        /// <summary>Marks the task as failed and binds the specified exception to the task.</summary>
        /// <param name="exception">The exception to bind to the task.</param>
        public void SetException(Exception exception) => _methodBuilder.SetException(exception);

        /// <summary>Gets the task for this builder.</summary>
        public ValueTask<TResult> Task
        {
            get
            {
                if (_haveResult)
                {
                    return new ValueTask<TResult>(_result);
                }
                else
                {
                    _useBuilder = true;
                    return new ValueTask<TResult>(_methodBuilder.Task);
                }
            }
        }

        /// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes.</summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">the awaiter</param>
        /// <param name="stateMachine">The state machine.</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        /// <summary>Schedules the state machine to proceed to the next action when the specified awaiter completes.</summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">the awaiter</param>
        /// <param name="stateMachine">The state machine.</param>
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            _useBuilder = true;
            _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }
    }

    /// <summary>Provides an awaitable type that enables configured awaits on a <see cref="ValueTask{TResult}"/>.</summary>
    /// <typeparam name="TResult">The type of the result produced.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct ConfiguredValueTaskAwaitable<TResult>
    {
        /// <summary>The wrapped <see cref="ValueTask{TResult}"/>.</summary>
        private readonly ValueTask<TResult> _value;
        /// <summary>true to attempt to marshal the continuation back to the original context captured; otherwise, false.</summary>
        private readonly bool _continueOnCapturedContext;

        /// <summary>Initializes the awaitable.</summary>
        /// <param name="value">The wrapped <see cref="ValueTask{TResult}"/>.</param>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original synchronization context captured; otherwise, false.
        /// </param>
        internal ConfiguredValueTaskAwaitable(ValueTask<TResult> value, bool continueOnCapturedContext)
        {
            _value = value;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>Returns an awaiter for this <see cref="ConfiguredValueTaskAwaitable{TResult}"/> instance.</summary>
        public ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return new ConfiguredValueTaskAwaiter(_value, _continueOnCapturedContext);
        }

        /// <summary>Provides an awaiter for a <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
        [StructLayout(LayoutKind.Auto)]
        public struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion
        {
            /// <summary>The value being awaited.</summary>
            private readonly ValueTask<TResult> _value;
            /// <summary>The value to pass to ConfigureAwait.</summary>
            private readonly bool _continueOnCapturedContext;

            /// <summary>Initializes the awaiter.</summary>
            /// <param name="value">The value to be awaited.</param>
            /// <param name="continueOnCapturedContext">The value to pass to ConfigureAwait.</param>
            internal ConfiguredValueTaskAwaiter(ValueTask<TResult> value, bool continueOnCapturedContext)
            {
                _value = value;
                _continueOnCapturedContext = continueOnCapturedContext;
            }

            /// <summary>Gets whether the <see cref="ConfiguredValueTaskAwaitable{TResult}"/> has completed.</summary>
            public bool IsCompleted { get { return _value.IsCompleted; } }

            /// <summary>Gets the result of the ValueTask.</summary>
            public TResult GetResult()
            {
                return _value._task == null ?
                    _value._result :
                    _value._task.GetAwaiter().GetResult();
            }

            /// <summary>Schedules the continuation action for the <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
            public void OnCompleted(Action continuation)
            {
                _value.AsTask().ConfigureAwait(_continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
            }

            /// <summary>Schedules the continuation action for the <see cref="ConfiguredValueTaskAwaitable{TResult}"/>.</summary>
            public void UnsafeOnCompleted(Action continuation)
            {
                _value.AsTask().ConfigureAwait(_continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
            }
        }
    }

    /// <summary>Provides an awaiter for a <see cref="ValueTask{TResult}"/>.</summary>
    public struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion
    {
        /// <summary>The value being awaited.</summary>
        private readonly ValueTask<TResult> _value;

        /// <summary>Initializes the awaiter.</summary>
        /// <param name="value">The value to be awaited.</param>
        internal ValueTaskAwaiter(ValueTask<TResult> value) { _value = value; }

        /// <summary>Gets whether the <see cref="ValueTask{TResult}"/> has completed.</summary>
        public bool IsCompleted { get { return _value.IsCompleted; } }

        /// <summary>Gets the result of the ValueTask.</summary>
        public TResult GetResult()
        {
            return _value._task == null ?
                _value._result :
                _value._task.GetAwaiter().GetResult();
        }

        /// <summary>Schedules the continuation action for this ValueTask.</summary>
        public void OnCompleted(Action continuation)
        {
            _value.AsTask().ConfigureAwait(continueOnCapturedContext: true).GetAwaiter().OnCompleted(continuation);
        }

        /// <summary>Schedules the continuation action for this ValueTask.</summary>
        public void UnsafeOnCompleted(Action continuation)
        {
            _value.AsTask().ConfigureAwait(continueOnCapturedContext: true).GetAwaiter().UnsafeOnCompleted(continuation);
        }
    }
}

namespace System.Threading.Tasks
{
    /// <summary>
    /// Provides a value type that wraps a <see cref="Task{TResult}"/> and a <typeparamref name="TResult"/>,
    /// only one of which is used.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <remarks>
    /// <para>
    /// Methods may return an instance of this value type when it's likely that the result of their
    /// operations will be available synchronously and when the method is expected to be invoked so
    /// frequently that the cost of allocating a new <see cref="Task{TResult}"/> for each call will
    /// be prohibitive.
    /// </para>
    /// <para>
    /// There are tradeoffs to using a <see cref="ValueTask{TResult}"/> instead of a <see cref="Task{TResult}"/>.
    /// For example, while a <see cref="ValueTask{TResult}"/> can help avoid an allocation in the case where the 
    /// successful result is available synchronously, it also contains two fields whereas a <see cref="Task{TResult}"/>
    /// as a reference type is a single field.  This means that a method call ends up returning two fields worth of
    /// data instead of one, which is more data to copy.  It also means that if a method that returns one of these
    /// is awaited within an async method, the state machine for that async method will be larger due to needing
    /// to store the struct that's two fields instead of a single reference.
    /// </para>
    /// <para>
    /// Further, for uses other than consuming the result of an asynchronous operation via await, 
    /// <see cref="ValueTask{TResult}"/> can lead to a more convoluted programming model, which can in turn actually 
    /// lead to more allocations.  For example, consider a method that could return either a <see cref="Task{TResult}"/> 
    /// with a cached task as a common result or a <see cref="ValueTask{TResult}"/>.  If the consumer of the result 
    /// wants to use it as a <see cref="Task{TResult}"/>, such as to use with in methods like Task.WhenAll and Task.WhenAny, 
    /// the <see cref="ValueTask{TResult}"/> would first need to be converted into a <see cref="Task{TResult}"/> using 
    /// <see cref="ValueTask{TResult}.AsTask"/>, which leads to an allocation that would have been avoided if a cached 
    /// <see cref="Task{TResult}"/> had been used in the first place.
    /// </para>
    /// <para>
    /// As such, the default choice for any asynchronous method should be to return a <see cref="Task"/> or 
    /// <see cref="Task{TResult}"/>. Only if performance analysis proves it worthwhile should a <see cref="ValueTask{TResult}"/> 
    /// be used instead of <see cref="Task{TResult}"/>.  There is no non-generic version of <see cref="ValueTask{TResult}"/> 
    /// as the Task.CompletedTask property may be used to hand back a successfully completed singleton in the case where
    /// a <see cref="Task"/>-returning method completes synchronously and successfully.
    /// </para>
    /// </remarks>
    [AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
    [StructLayout(LayoutKind.Auto)]
    public struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
    {
        /// <summary>The task to be used if the operation completed asynchronously or if it completed synchronously but non-successfully.</summary>
        internal readonly Task<TResult> _task;
        /// <summary>The result to be used if the operation completed successfully synchronously.</summary>
        internal readonly TResult _result;

        /// <summary>Initialize the <see cref="ValueTask{TResult}"/> with the result of the successful operation.</summary>
        /// <param name="result">The result.</param>
        public ValueTask(TResult result)
        {
            _task = null;
            _result = result;
        }

        /// <summary>
        /// Initialize the <see cref="ValueTask{TResult}"/> with a <see cref="Task{TResult}"/> that represents the operation.
        /// </summary>
        /// <param name="task">The task.</param>
        public ValueTask(Task<TResult> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _result = default(TResult);
        }

        /// <summary>
        /// Initialize the <see cref="ValueTask{TResult}"/> with an async <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="func">The function.</param>
        public ValueTask(Func<TResult> func)
        {
            _task = Task.Run(func ?? throw new ArgumentNullException(nameof(func)));
            _result = default(TResult);
        }

        /// <summary>
        /// Initialize the <see cref="ValueTask{TResult}"/> with an async <see cref="Func{Task{TResult}}"/>.
        /// </summary>
        /// <param name="taskfunc">The function.</param>
        public ValueTask(Func<Task<TResult>> taskfunc)
        {
            _task = Task.Run(taskfunc ?? throw new ArgumentNullException(nameof(taskfunc)));
            _result = default(TResult);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        public override int GetHashCode()
        {
            return
                _task != null ? _task.GetHashCode() :
                _result != null ? _result.GetHashCode() :
                0;
        }

        /// <summary>Returns a value indicating whether this value is equal to a specified <see cref="object"/>.</summary>
        public override bool Equals(object obj)
        {
            return
                obj is ValueTask<TResult> &&
                Equals((ValueTask<TResult>)obj);
        }

        /// <summary>Returns a value indicating whether this value is equal to a specified <see cref="ValueTask{TResult}"/> value.</summary>
        public bool Equals(ValueTask<TResult> other)
        {
            return _task != null || other._task != null ?
                _task == other._task :
                EqualityComparer<TResult>.Default.Equals(_result, other._result);
        }

        /// <summary>Returns a value indicating whether two <see cref="ValueTask{TResult}"/> values are equal.</summary>
        public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns a value indicating whether two <see cref="ValueTask{TResult}"/> values are not equal.</summary>
        public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Gets a <see cref="Task{TResult}"/> object to represent this ValueTask.  It will
        /// either return the wrapped task object if one exists, or it'll manufacture a new
        /// task object to represent the result.
        /// </summary>
        public Task<TResult> AsTask()
        {
            // Return the task if we were constructed from one, otherwise manufacture one.  We don't
            // cache the generated task into _task as it would end up changing both equality comparison
            // and the hash code we generate in GetHashCode.
            return _task ?? Task.FromResult(_result);
        }

        /// <summary>Gets whether the <see cref="ValueTask{TResult}"/> represents a completed operation.</summary>
        public bool IsCompleted { get { return _task == null || _task.IsCompleted; } }

        /// <summary>Gets whether the <see cref="ValueTask{TResult}"/> represents a successfully completed operation.</summary>
        public bool IsCompletedSuccessfully { get { return _task == null || _task.Status == TaskStatus.RanToCompletion; } }

        /// <summary>Gets whether the <see cref="ValueTask{TResult}"/> represents a failed operation.</summary>
        public bool IsFaulted { get { return _task != null && _task.IsFaulted; } }

        /// <summary>Gets whether the <see cref="ValueTask{TResult}"/> represents a canceled operation.</summary>
        public bool IsCanceled { get { return _task != null && _task.IsCanceled; } }

        /// <summary>Gets the result.</summary>
        public TResult Result { get { return _task == null ? _result : _task.GetAwaiter().GetResult(); } }

        /// <summary>Gets the id of this task, or -1 if task not running.</summary>
        public int Id { get => _task?.Id ?? -1; }



        /// <summary>Gets an awaiter for this value.</summary>
        public ValueTaskAwaiter<TResult> GetAwaiter()
        {
            return new ValueTaskAwaiter<TResult>(this);
        }

        /// <summary>Configures an awaiter for this value.</summary>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the captured context; otherwise, false.
        /// </param>
        public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
        {
            return new ConfiguredValueTaskAwaitable<TResult>(this, continueOnCapturedContext: continueOnCapturedContext);
        }

        /// <summary>Gets a string-representation of this <see cref="ValueTask{TResult}"/>.</summary>
        public override string ToString()
        {
            if (_task != null)
            {
                return _task.Status == TaskStatus.RanToCompletion && _task.Result != null ?
                    _task.Result.ToString() :
                    string.Empty;
            }
            else
            {
                return _result != null ?
                    _result.ToString() :
                    string.Empty;
            }
        }

        // TODO: Remove CreateAsyncMethodBuilder once the C# compiler relies on the AsyncBuilder attribute.

        /// <summary>Creates a method builder for use with an async method.</summary>
        /// <returns>The created builder.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)] // intended only for compiler consumption
        public static AsyncValueTaskMethodBuilder<TResult> CreateAsyncMethodBuilder() => AsyncValueTaskMethodBuilder<TResult>.Create();
    }
}