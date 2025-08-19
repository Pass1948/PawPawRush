using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "New Achievement")]
public class AchievementsData : ScriptableObject
{
    public List<ToastUIData> toastUIInfo = new List<ToastUIData>();
}
