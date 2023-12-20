using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    public Massage[] massages;
    public Actor[] actors;

}

[System.Serializable]

public class Massage 
{
    public int actorId;
    public string massage;

}

[System.Serializable]

public class Actor 
{
    public string name;
    public Sprite sprite;

}
