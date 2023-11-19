using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseBoss : ScriptableObject
{
        public abstract void BossSpread(Vector2Int Bosslocation);
}
