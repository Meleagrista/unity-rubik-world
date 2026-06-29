using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Game/Level List")]
public class LevelListSO : SingletonScriptableObject<LevelListSO>
{
    public List<string> levels;
}