
    using System;

    public class EventHandle
    {
        public static Action OnPlayGame;
        public static Action<int> OnStarCountTimer;
        public static Action<int,int> OnCheckMatchStart;
        public static Action OnCheckMatchDone;
        public static Action OnTimeOut;
        public static Action<int> OnWinGame;
        public static Action OnNextLevel;
        public static Action OnMainMenu;
    }
