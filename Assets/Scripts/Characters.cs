using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character", order = 1)]
public class Characters : ScriptableObject
{
    [SerializeField] Sprite image;
    [SerializeField] string myName;

    public Sprite Image { get { return image; } }
    public string Name { get { return myName; } }
}
