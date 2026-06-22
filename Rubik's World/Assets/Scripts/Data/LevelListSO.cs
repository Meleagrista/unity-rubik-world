using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Game/Level List")]
public class LevelListSO : ScriptableObject
{
    public List<string> levels;
}