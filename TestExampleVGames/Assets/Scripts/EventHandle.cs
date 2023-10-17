
    using System;

    public class EventHandle
    {
        public static Action OnPlayGame;
        public static Action<int> OnCheckMatchStart;
        public static Action OnCheckMatchDone;
    }
