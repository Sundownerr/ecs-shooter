using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class StaticData
    {
        public GameObject DdolWrapper;

        [Space]
        public PlayerUIProvider PlayerUi;
        public GameOverUiProvider GameOverUi;
        public StartMenuUiProvider StartMenuUi;
        public PlayerScoreUiProvider ScoreUi;
    }
}