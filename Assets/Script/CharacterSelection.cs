using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 引用TextMeshPro命名空间
using System.Linq;
using System.IO;

public class CharacterSelection : MonoBehaviour
{
    public Image femaleImage;
    public Image maleImage;
    public TextMeshProUGUI appearanceNumber;
    public Image appearanceFillImage;
    public TextMeshProUGUI figureNumber;
    public Image figureFillImage;
    public TextMeshProUGUI knowledgeNumber;
    public Image knowledgeFillImage;
    public TextMeshProUGUI wealthNumber;
    public Image wealthFillImage;

    public TextMeshProUGUI[] smallNameTexts = new TextMeshProUGUI[6];
    public TextMeshProUGUI[] smallAppearanceTexts = new TextMeshProUGUI[6];
    public TextMeshProUGUI[] smallFigureTexts = new TextMeshProUGUI[6];
    public TextMeshProUGUI[] smallKnowledgeTexts = new TextMeshProUGUI[6];
    public TextMeshProUGUI[] smallWealthTexts = new TextMeshProUGUI[6];
    public Image[] iconAppearance = new Image[6];
    public Image[] iconFigure = new Image[6];
    public Image[] iconKnowledge = new Image[6];
    public Image[] iconWealth = new Image[6];

    public TagManager tagManager; // 引用TagManager
    public TextMeshProUGUI personalTag1Text;
    public TextMeshProUGUI personalTag2Text;
    public TextMeshProUGUI preferenceTagText;

    public TMP_InputField characterNameInputField;  // 输入框
    public TextMeshProUGUI placeholderText;  // 占位符文本
    public Button editNameButton;
    public Button ackNameButton;

    private string gender;
    private int appearance;
    private int figure;
    private int knowledge;
    private int wealth;
    private string characterName;  // 存储角色名称
    private int explosion;
    private string personalTag1;
    private string personalTag2;
    private string negativeTag;
    private string preferenceTag;
    private CalculationManager calculationManager;

    //颜值和身材只会出现3-10
    int[] appearanceFigureValues = {3, 4, 5, 6, 7, 8, 9, 10};
    float[] appearanceFigureWeights = {0.1f, 0.1f, 0.2f, 0.2f, 0.2f, 0.1f, 0.05f, 0.05f}; 

    //知识和财富1-10都可能会有
    int[] knowledgeWealthValues = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    float[] knowledgeWealthWeights = {0.03f, 0.05f, 0.1f, 0.15f, 0.17f, 0.17f, 0.15f, 0.1f, 0.05f, 0.03f}; 

    public float maxSliderWidth = 466f;  // 滑条的最大宽度
    private int maxValue = 10;  // 属性的最大数值

    private int currentCharacterIndex = 0; // 记录当前生成的角色数量
    private const int totalCharacters = 6; // 需要生成6个角色

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            smallNameTexts[i].gameObject.SetActive(false);
            smallAppearanceTexts[i].gameObject.SetActive(false);
            smallFigureTexts[i].gameObject.SetActive(false);
            smallKnowledgeTexts[i].gameObject.SetActive(false);
            smallWealthTexts[i].gameObject.SetActive(false);
            iconAppearance[i].gameObject.SetActive(false);
            iconFigure[i].gameObject.SetActive(false);
            iconKnowledge[i].gameObject.SetActive(false);
            iconWealth[i].gameObject.SetActive(false);
        }
        // 初始化输入框状态为不可编辑
        characterNameInputField.interactable = false;
        placeholderText.text = "默认名字";  // 设置默认占位符文本
        // 获取 CalculationManager 的实例
        calculationManager = FindObjectOfType<CalculationManager>();
        RollAttributes();
    }
    
    public int WeightedRandom(int[] values, float[] weights)
    {
        float totalWeight = 0;

        // 计算权重总和
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < values.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return values[i];
            }
        }
        return values[0]; // 默认返回最小值
    }


    // 点击重新摇点时调用
    public void RollAttributes()
    {
        // ----- 生成性别 -----
        int femaleCount = CharacterManager.instance.characterList.Count(c => c.gender == "female");
        int maleCount = CharacterManager.instance.characterList.Count(c => c.gender == "male");
        if (femaleCount >= 3){
            gender = "male";
        }else if (maleCount >= 3){
            gender = "female";
        }else{
            gender = Random.Range(0, 2) == 0 ? "female" : "male";
        }

        characterName = RollName(gender);
        placeholderText.text = characterName;

        // ----- 四个属性 加权随机值 3-10/1-10 -----
        appearance = WeightedRandom(appearanceFigureValues, appearanceFigureWeights);
        figure = WeightedRandom(appearanceFigureValues, appearanceFigureWeights);
        knowledge = WeightedRandom(knowledgeWealthValues, knowledgeWealthWeights);
        wealth = WeightedRandom(knowledgeWealthValues, knowledgeWealthWeights);

        // ----- 更新档案板上UI显示 -----
        appearanceNumber.text = appearance.ToString();
        figureNumber.text = figure.ToString();
        knowledgeNumber.text = knowledge.ToString();
        wealthNumber.text = wealth.ToString();
        UpdateSlider(appearanceFillImage, appearance);
        UpdateSlider(figureFillImage, figure);
        UpdateSlider(knowledgeFillImage, knowledge);
        UpdateSlider(wealthFillImage, wealth);
        if (gender == "male"){
            maleImage.gameObject.SetActive(true);
            femaleImage.gameObject.SetActive(false);
        }else{
            maleImage.gameObject.SetActive(false);
            femaleImage.gameObject.SetActive(true);
        }

        // ----- 个人标签 personalTag1和2 -----
        personalTag1 = tagManager.RollFirstPersonalTag(appearance, figure, knowledge, wealth);
        personalTag1Text.text = personalTag1;
        personalTag2 = tagManager.RollSecondPersonalTag(personalTag1, appearance, figure, knowledge, wealth);
        personalTag2Text.text = personalTag2;

        // ----- 偏好标签 preferenceTag -----
        preferenceTag = tagManager.RollPreferenceTag();
        preferenceTagText.text = preferenceTag;

        // ----- 负向标签（初始不公开给玩家）negativeTag -----
        negativeTag = tagManager.RollNegativeTag();
    }

    private void UpdateSlider(Image fillImage, int value)
    {
        // 计算新的宽度，基于当前数值占最大数值的比例
        float newWidth = (value / (float)maxValue) * maxSliderWidth;

        // 设置Image的RectTransform的宽度
        RectTransform fillRectTransform = fillImage.rectTransform;
        fillRectTransform.sizeDelta = new Vector2(newWidth, fillRectTransform.sizeDelta.y);
    }

    // 启用输入框的编辑功能
    public void EnableNameEdit()
    {
        characterNameInputField.interactable = true;
    }

    // 确认输入的角色名称并锁定输入框
    public void AcknowledgeName()
    {
        if (!string.IsNullOrEmpty(characterNameInputField.text))
        {
            characterName = characterNameInputField.text;  // 存储角色名称
            characterNameInputField.interactable = false;  // 锁定输入框
            Debug.Log($"角色名称已确认: {characterName}");
        }
        else
        {
            Debug.Log("角色名称不能为空！");
        }
    }

    public void ConfirmAllCharacter()
    {
        if(CharacterManager.instance.characterList.Count == 6){
            Debug.Log("全部角色已经确认");
            calculationManager.CalculateAllAffections();
        }
    }


    // 点击确定按钮时调用
    public void ConfirmCharacter()
    {
        if (currentCharacterIndex >= totalCharacters)
        {
            Debug.Log("所有角色已创建");
            return;
        }

        // string characterName = "test";
        // 调用 CalculateInitCharacterPopularity 来计算角色的初始喜爱度
        float popularity = calculationManager.CalculateInitCharacterPopularity(appearance, figure, knowledge, wealth);

        float risk = calculationManager.CalculateExplosionRisk();

        // 创建一个角色并存储到CharacterManager
        Character newCharacter = new Character(characterName, gender, appearance, figure, knowledge, wealth, popularity, risk, personalTag1, personalTag2, negativeTag, preferenceTag);
        CharacterManager.instance.AddCharacter(newCharacter);

        PrintCharacterList();

        if (currentCharacterIndex < totalCharacters)
        {
            RollAttributes(); // 自动roll下一次点
        }
        else
        {
            // 角色选择完成，切换场景或执行其他操作
            Debug.Log("所有角色已完成选择，可以进入下一场景");
            // 例如：SceneManager.LoadScene("NextSceneName");
        }

        if (currentCharacterIndex >= 0 && currentCharacterIndex < 6)
        {
            // 获取当前角色
            Character character = CharacterManager.instance.characterList[currentCharacterIndex];

            // 填充对应的 TMP 组件
            smallNameTexts[currentCharacterIndex].text = character.name;
            smallAppearanceTexts[currentCharacterIndex].text = character.appearance.ToString();
            smallFigureTexts[currentCharacterIndex].text = character.figure.ToString();
            smallKnowledgeTexts[currentCharacterIndex].text = character.knowledge.ToString();
            smallWealthTexts[currentCharacterIndex].text = character.wealth.ToString();

            // 将这些组件设为可见
            smallNameTexts[currentCharacterIndex].gameObject.SetActive(true);
            smallAppearanceTexts[currentCharacterIndex].gameObject.SetActive(true);
            smallFigureTexts[currentCharacterIndex].gameObject.SetActive(true);
            smallKnowledgeTexts[currentCharacterIndex].gameObject.SetActive(true);
            smallWealthTexts[currentCharacterIndex].gameObject.SetActive(true);
            iconAppearance[currentCharacterIndex].gameObject.SetActive(true);
            iconFigure[currentCharacterIndex].gameObject.SetActive(true);
            iconKnowledge[currentCharacterIndex].gameObject.SetActive(true);
            iconWealth[currentCharacterIndex].gameObject.SetActive(true);
        }
        currentCharacterIndex++; // 当前角色数+1
    }

    public string RollName(string gender)
    {
        string fileName = gender == "female" ? "femaleName.txt" : "maleName.txt";
        string filePath = Path.Combine(Application.dataPath, "TxtResources", fileName);
        
        if (File.Exists(filePath))
        {
            string[] names = File.ReadAllLines(filePath);
            if (names.Length > 0)
            {
                int randomIndex = Random.Range(0, names.Length);
                return names[randomIndex];
            }
        }

        return "DefaultName"; // 如果没有名字，返回默认名字
    }

    private void PrintCharacterList()
    {
        Debug.Log("当前角色列表：");
        foreach (Character character in CharacterManager.instance.characterList)
        {
            Debug.Log($"角色名称: {character.name}, " +
                      $"性别: {character.gender}, " + 
                      $"颜值: {character.appearance}, " +
                      $"身材: {character.figure}, " +
                      $"知识: {character.knowledge}, " +
                      $"财富: {character.wealth}, " +
                      $"个人喜爱度: {character.popularity}, " +
                      $"爆雷值: {character.risk}");
        }
    }
}
