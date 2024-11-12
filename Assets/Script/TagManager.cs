using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    // 定义所有可能的 personalTags
    private List<string> personalTags;
    private List<string> negativeTags;
    private List<string> preferenceTags;

    private void Start()
    {
        // 初始化标签库
        personalTags = new List<string>
        {
            "社交达人",
            "喜欢独处",
            "喜爱孩子",
            "丁克",
            "宠物爱好者",
            "家有宠物",
            "旅行家",
            "户外运动迷",
            "宅家",
            "烹饪高手",
            "园艺高手",
            "咖啡专家",
            "时尚达人",
            "摄影爱好者",
            "音乐爱好者",
            "美食主义",
            "浪漫派",
            "绝对理性",
            "喜剧人",
            "调停者",
            "数字敏感",
            "书虫",
            "派对人",
            "二次元",
            "追星控",
        };

        negativeTags = new List<string>
        {
            "吝啬",
            "刻薄",
            "爱说教",
            "情绪化",
            "中央空调",
            "善妒",
            "喜新厌旧",
            "装",
            "胆小鬼",
            "聒噪",
            "缺乏耐心",
            "过于自信",
            "拖延症",
            "自我中心",
            "回避依恋",
            "画饼专家",
        };

        preferenceTags = new List<string>
        {
            "颜控",
            "智性恋",
            "忠诚",
            "志趣相投",
            "完美身材",
            "体贴",
            "幽默",
            "有同情心",
            "创造力",
            "行动派",
            "独立坚定",
            "情绪稳定",
            "相互信任",
            "谦逊平和",
            "慷慨宽容",
        };
    }

    public string RollFirstPersonalTag(int appearance, int figure, int knowledge, int wealth)
    {
        List<string> availableTags = new List<string>(personalTags);

        // 根据条件添加特定标签
        if (knowledge >= 8 && knowledge <= 10 || knowledge >= 1 && knowledge <= 4){
            availableTags.Add("留学生");
            availableTags.Add("留学生");
        }

        if (figure >= 8 && figure <= 10){
            availableTags.Add("健身达人");
        }

        if (appearance >= 8 && appearance <= 10){
            availableTags.Add("网红");
        }

        // 随机选择一个标签
        if (availableTags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTags.Count);
            return availableTags[randomIndex];
        }

        return null; // 如果没有可用标签，返回 null
    }

    public string RollSecondPersonalTag(string firstTag, int appearance, int figure, int knowledge, int wealth)
    {
        List<string> availableTags = new List<string>(personalTags);

        if (knowledge >= 8 && knowledge <= 10 || knowledge >= 1 && knowledge <= 4){
            availableTags.Add("留学生");
            availableTags.Add("留学生");
        }

        if (figure >= 8 && figure <= 10){
            availableTags.Add("健身达人");
        }

        if (appearance >= 8 && appearance <= 10){
            availableTags.Add("网红");
        }

        // 去重
        if (availableTags.Contains(firstTag.ToString())){
            availableTags.Remove(firstTag.ToString());
        }

        // 删除逻辑对立标签
        switch (firstTag.ToString()){
            case "社交达人":
                availableTags.Remove("喜欢独处");
                break;
            case "喜欢独处":
                availableTags.Remove("社交达人");
                break;
            case "喜爱孩子":
                availableTags.Remove("丁克");
                break;
            case "丁克":
                availableTags.Remove("喜爱孩子");
                break;
            case "浪漫派":
            case "时尚达人":
            case "摄影爱好者":
            case "音乐爱好者":
                availableTags.Remove("绝对理性");
                break;
            case "绝对理性":
                availableTags.Remove("浪漫派");
                availableTags.Remove("时尚达人");
                availableTags.Remove("摄影爱好者");
                availableTags.Remove("音乐爱好者");
                break;
            case "旅行家":
            case "户外运动迷":
            case "健身达人":
                availableTags.Remove("宅家");
                break;
            case "宅家":
                availableTags.Remove("旅行家");
                availableTags.Remove("户外运动迷");
                availableTags.Remove("健身达人");
                break;
            case "二次元":
                availableTags.Remove("追星控");
                break;
            case "追星控":
                availableTags.Remove("二次元");
                break;
            case "留学生":
                availableTags.Add("烹饪高手");
                break;
            default:
                break;
        }
        // 随机选择一个标签
        if (availableTags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTags.Count);
            return availableTags[randomIndex];
        }

        return null; // 如果没有可用标签，返回 null
    }

    public string RollNegativeTag()
    {
        List<string> availableTags = new List<string>(negativeTags);

        // 随机选择一个标签
        if (availableTags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTags.Count);
            return availableTags[randomIndex];
        }

        return null; // 如果没有可用标签，返回 null
    }

    public string RollPreferenceTag()
    {
        List<string> availableTags = new List<string>(preferenceTags);

        // 随机选择一个标签
        if (preferenceTags.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTags.Count);
            return availableTags[randomIndex];
        }

        return null; // 如果没有可用标签，返回 null
    }
}
