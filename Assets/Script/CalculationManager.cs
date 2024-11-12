using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CalculationManager : MonoBehaviour
{
    // 权重参数设置，可以根据需要调整
    private float alpha = 0.4f;  // appearance 权重
    private float beta = 0.2f;   // figure 权重
    private float gamma = 0.2f;  // knowledge 权重
    private float delta = 0.2f;  // wealth 权重
    private float lambda = 0.2f; // appearance 和 figure 的交互权重

    private Dictionary<string, (string[] addTags, string[] subTags)> personalTagAffectionRules = new Dictionary<string, (string[], string[])>()
    {
        { "社交达人", (new string[] {"派对人"}, new string[] {"喜欢独处"}) },
        { "喜欢独处", (new string[] {"书虫"}, new string[] {"派对人", "社交达人"}) },
        { "喜爱孩子", (new string[] {}, new string[] {"丁克"}) },
        { "丁克", (new string[] {}, new string[] {"喜爱孩子"}) },
        { "家有宠物", (new string[] {"宠物爱好者"}, new string[] {}) },
        { "宠物爱好者", (new string[] {"家有宠物"}, new string[] {}) },
        { "旅行家", (new string[] {"户外运动迷"}, new string[] {"宅家"}) },
        { "户外运动迷", (new string[] {"旅行家"}, new string[] {"宅家"}) },
        { "烹饪高手", (new string[] {"美食主义"}, new string[] {}) },
        { "宅家", (new string[] {}, new string[] {"户外运动迷", "旅行家", "健身达人"}) },
        { "咖啡专家", (new string[] {"美食主义"}, new string[] {}) },
        { "派对人", (new string[] {"社交达人"}, new string[] {"喜欢独处"}) },
        { "时尚达人", (new string[] {"浪漫派"}, new string[] {"绝对理性"}) },
        { "摄影爱好者", (new string[] {"浪漫派"}, new string[] {"绝对理性"}) },
        { "音乐爱好者", (new string[] {"浪漫派"}, new string[] {"绝对理性"}) },
        { "美食主义", (new string[] {"烹饪高手","咖啡专家"}, new string[] {}) },
        { "浪漫派", (new string[] {"时尚达人","摄影爱好者","音乐爱好者"}, new string[] {"绝对理性"}) },
        { "绝对理性", (new string[] {"数字敏感"}, new string[] {"浪漫派"}) },
        { "留学生", (new string[] {"美食主义"}, new string[] {}) },
        { "健身达人", (new string[] {"户外运动迷"}, new string[] {"宅家"}) },
        { "二次元", (new string[] {}, new string[] {"追星控"}) },
        { "追星控", (new string[] {}, new string[] {"二次元"}) },
    };

    private Dictionary<string, string> personalTagCategories = new Dictionary<string, string>
    {
        { "旅行家", "户外" },
        { "户外运动迷", "户外" },
        { "宠物爱好者", "宠物" },
        { "家有宠物", "宠物" },
        { "烹饪高手", "生活" },
        { "园艺高手", "生活" },
        { "咖啡专家", "生活" },
        { "时尚达人", "艺术" },
        { "摄影爱好者", "艺术" },
        { "音乐爱好者", "艺术" }
    };

    // 计算Sigmoid函数
    private float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

    // 计算角色初始喜爱度的函数
    public float CalculateInitCharacterPopularity(int appearance, int figure, int knowledge, int wealth)
    {
        // 计算基础属性加权和及交互项
        float popularity = alpha * appearance + 
                      beta * figure + 
                      gamma * knowledge + 
                      delta * wealth + 
                      lambda * (appearance * figure) / 10f;

        return popularity;
    }

    public float CalculateExplosionRisk()
    {
        float explosionRisk = Random.Range(0f, 40f);

        return explosionRisk; // 返回爆雷值（0%到40%之间）
    }
    
    //A对B的初始好感度
    public float CalculateInitAffection(Character roleA, Character roleB){
        float baseAffection = 30f;
        string[] roleATags = { roleA.personalTag1, roleA.personalTag2 };  // roleA 的标签数组
        string[] roleBTags = { roleB.personalTag1, roleB.personalTag2 };  // roleB 的标签数组

        // 遍历 roleA 的每个标签
        foreach (string tag in roleATags)
        {
            if (personalTagAffectionRules.ContainsKey(tag))
            {
                // 获取该标签的加分和减分项
                var (addTags, subTags) = personalTagAffectionRules[tag];
                // 检查roleB是否有加分标签
                foreach (string addTag in addTags)
                {
                    if (System.Array.Exists(roleBTags, t => t == addTag))
                    {
                        baseAffection += Random.Range(5f, 10f);
                        Debug.Log($"{roleA.name}对{roleB.name}因为{roleA.name}有{tag}且{roleB.name}有{addTag}加分");
                    }
                }
                // 检查roleB是否有减分标签
                foreach (string subTag in subTags)
                {
                    if (System.Array.Exists(roleBTags, t => t == subTag))
                    {
                        baseAffection -= Random.Range(5f, 10f);
                        Debug.Log($"{roleA.name}对{roleB.name}因为{roleA.name}有{tag}且{roleB.name}有{subTag}减分");
                    }
                }
            }
        }

        if(roleA.preferenceTag == "颜控"){
            if(roleB.appearance >= 8 && roleB.appearance <= 10){
                baseAffection += Random.Range(5f, 10f);
            }
            if(roleB.appearance >= 1 && roleB.appearance <= 4){
                baseAffection -= Random.Range(5f, 10f);
            }
        }
        if(roleA.preferenceTag == "智性恋"){
            if(roleB.knowledge >= 8 && roleB.knowledge <= 10){
                baseAffection += Random.Range(5f, 10f);
            }
            if(roleB.knowledge >= 1 && roleB.knowledge <= 4){
                baseAffection -= Random.Range(5f, 10f);
            }
        }
        if(roleA.preferenceTag == "完美身材"){
            if(roleB.figure >= 8 && roleB.figure <= 10){
                baseAffection += Random.Range(5f, 10f);
            }
            if(roleB.figure >= 1 && roleB.figure <= 4){
                baseAffection -= Random.Range(5f, 10f);
            }
        }
        if(roleA.preferenceTag == "幽默" && roleBTags.Contains("喜剧人")){       
            baseAffection += Random.Range(5f, 10f);
        }
        if(roleA.preferenceTag == "有同情心" && roleBTags.Contains("调停者")){       
            baseAffection += Random.Range(5f, 10f);
        }
        if(roleA.preferenceTag == "创造力"){
            if(roleBTags.Contains("摄影爱好者") || roleBTags.Contains("音乐爱好者") || roleBTags.Contains("时尚达人") || roleBTags.Contains("浪漫派")){
                baseAffection += Random.Range(5f, 10f);
            }
            if(roleBTags.Contains("绝对理性")){
                baseAffection -= Random.Range(5f, 10f);
            }
        }

        // 志趣相投的判断
        if (roleA.preferenceTag == "志趣相投")
        {
            // 使用 HashSet 来存储匹配的类别
            HashSet<string> matchedCategories = new HashSet<string>();

            // 遍历 roleA 的标签并记录类别
            foreach (string tagA in roleATags)
            {
                if (personalTagCategories.TryGetValue(tagA, out string categoryA))
                {
                    matchedCategories.Add(categoryA);
                }
            }

            // 遍历 roleB 的标签并检查匹配类别
            foreach (string tagB in roleBTags)
            {
                if (personalTagCategories.TryGetValue(tagB, out string categoryB))
                {
                    // 如果类别匹配，则加分
                    if (matchedCategories.Contains(categoryB))
                    {
                        baseAffection += Random.Range(5f, 10f);
                        Debug.Log($"{roleA.name} 与 {roleB.name} 由于共同的兴趣类别加分: {categoryB}");
                    }
                }
            }
        }
        return baseAffection;
    }
    public void CalculateAllAffections()
    {
        int totalCharacters = CharacterManager.instance.characterList.Count;

        for (int i = 0; i < totalCharacters; i++)
        {
            for (int j = 0; j < totalCharacters; j++)
            {
                if (i != j)
                {
                    Character roleA = CharacterManager.instance.GetCharacter(i);
                    Character roleB = CharacterManager.instance.GetCharacter(j);

                    // 调用 CalculateInitAffection 计算好感度
                    float affection = CalculateInitAffection(roleA, roleB);

                    // 将好感度存入 CharacterManager 的 affection 数组中
                    CharacterManager.instance.characterInteractiveAttributeList[i][j].affection = affection;
                }
            }
        }
        CharacterManager.instance.PrintAllAffections();
        Debug.Log("所有角色之间的好感度已计算完成");
    }
}