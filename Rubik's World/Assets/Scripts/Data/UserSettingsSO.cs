using UnityEngine;

[CreateAssetMenu(fileName = "UserSettings", menuName = "Game/User Settings")]
public class UserSettingsSO : SingletonScriptableObject<UserSettingsSO>
{
    public float mouseSensitivity = 1f;
}