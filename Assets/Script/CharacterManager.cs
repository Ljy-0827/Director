using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterInteractiveAttributes
{
    public float affection; // 好感度
    public float familiarity; // 熟悉度

    public CharacterInteractiveAttributes(float affection, float familiarity)
    {
        this.affection = affection;
        this.familiarity = familiarity;
    }
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<Character> characterList = new List<Character>();
    // 六个一维数组，存储每个角色的属性
    public CharacterInteractiveAttributes[][] characterInteractiveAttributeList = new CharacterInteractiveAttributes[6][];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保持
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < 6; i++)
        {
            characterInteractiveAttributeList[i] = new CharacterInteractiveAttributes[6];
            for (int j = 0; j < 6; j++)
            {
                characterInteractiveAttributeList[i][j] = new CharacterInteractiveAttributes(0f, 0f);
            }
        }
    }

    public void AddCharacter(Character character)
    {
        if (characterList.Count < 6) // 确保最多存储6个角色
        {
            characterList.Add(character);
        }
    }

    public Character GetCharacter(int index)
    {
        if (index >= 0 && index < characterList.Count)
        {
            return characterList[index];
        }
        return null;
    }

    //设置角色i对角色j的好感度
    public void SetCharacterAffection(int i, int j, float affection)
    {
        if (i >= 0 && i < 6 && j >= 0 && j < 6)
        {
            characterInteractiveAttributeList[i][j].affection = affection;
        }
    }

    //设置角色i对角色j的熟悉度
    public void SetCharacterFamiliarity(int i, int j, float familiarity)
    {
        if (i >= 0 && i < 6 && j >= 0 && j < 6)
        {
            characterInteractiveAttributeList[i][j].familiarity = familiarity;
        }
    }

    public void PrintAllAffections()
    {
        for (int i = 0; i < characterInteractiveAttributeList.Length; i++)
        {
            for (int j = 0; j < characterInteractiveAttributeList[i].Length; j++)
            {
                float affectionValue = characterInteractiveAttributeList[i][j].affection;
                Debug.Log($"角色{i}对角色{j}的好感度: {affectionValue}");
            }
        }
    }

}
