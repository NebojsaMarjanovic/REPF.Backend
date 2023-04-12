namespace REPF.Backend.Utilities
{
    public enum ResultState
    {
        Faulted,
        Success
    }

    public readonly struct Result<T>
    {
        internal readonly ResultState State;
        internal readonly T? Value;

        internal Exception Exception { get; }

        public Result(T value)
        {
            State = ResultState.Success;
            Value = value;
            Exception = new Exception();
        }

        public Result(Exception e)
        {
            State = ResultState.Faulted;
            Exception = e;
            Value = default;
        }

        public bool IsFaulted =>
            State == ResultState.Faulted;

        public bool IsSuccess =>
            State == ResultState.Success;

        public R Match<R>(Func<T, R> Succ, Func<Exception, R> Fail) =>
            IsFaulted
                ? Fail(Exception)
                : Succ(Value!);

    }
}
