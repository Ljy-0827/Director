using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public int appearance;
    public int figure;
    public int knowledge;
    public int wealth;
    public float popularity;
    public float risk;
    public string gender;
    public string personalTag1;
    public string personalTag2;
    public string negativeTag;
    public string preferenceTag;

    public Character(string name, string gender, int appearance, int figure, int knowledge, int wealth, float popularity, float risk, string personalTag1, string personalTag2, string negativeTag, string preferenceTag)
    {
        this.name = name;
        this.gender = gender;
        this.appearance = appearance;
        this.figure = figure;
        this.knowledge = knowledge;
        this.wealth = wealth;
        this.popularity = popularity;
        this.risk = risk;
        this.personalTag1 = personalTag1;
        this.personalTag2 = personalTag2;
        this.negativeTag = negativeTag;
        this.preferenceTag = preferenceTag;
    }
}