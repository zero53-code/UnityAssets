using System;
using UnityEngine;

namespace Zero53.Gas.Magnitudes
{
    [Serializable]
    public class LevelMagnitude : Magnitude
    {
        public float level;
        public AnimationCurve levelCurve;

        public override float value => baseValue * levelCurve.Evaluate(level);
    }
}