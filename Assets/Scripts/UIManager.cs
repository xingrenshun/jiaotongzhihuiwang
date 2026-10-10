using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Button btn_RemoveCollisionItem;
    public TextMeshProUGUI txt_RemoveCollisionItemCount;
    public Button btn_RoadBlockItem;
    public TextMeshProUGUI txt_RoadBlockItemCount;
    public Button btn_PoliceCarItem;
    public TextMeshProUGUI txt_PoliceCarItemCount;
    public Button btn_SpeedUpItem;
    public TextMeshProUGUI txt_SpeedUpItemCount;
    // 高亮色和默认色
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    // 存所有道具按钮，方便统一操作
    private List<Button> allItemButtons;
    // 当前选中的按钮
    private Button selectedButton;
    public Image[] healthImages;
    public TextMeshProUGUI txt_Score;
    public TextMeshProUGUI txt_Score_over;
    public TextMeshProUGUI txt_Heat;

    public GameObject panelMain;
    public Button btn_StartGame;
    public Button btn_Rank;
    public GameObject panelGameOver;
    public GameObject gameInItems;
    public Button btn_RestartGame;
    public Button btn_BackMain;
    public Button btn_ContinueGame;
    public GameObject panelPauseGame;
    public Button btn_PauseGame;
    public GameObject panel_Help;
    public Button btn_Help;
    public Camera MainSceneCamera;
    public Button btn_Rank_Close;
    public GameObject panel_Rank;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // 把按钮集中到列表
        allItemButtons = new List<Button> {
            btn_RemoveCollisionItem,
            btn_RoadBlockItem,
            btn_PoliceCarItem,
            btn_SpeedUpItem
        };
        // 注册点击
        btn_RemoveCollisionItem.onClick.AddListener(() => OnItemButtonClick(btn_RemoveCollisionItem, ItemType.RemoveCollision));
        btn_RoadBlockItem.onClick.AddListener(() => OnItemButtonClick(btn_RoadBlockItem, ItemType.RoadBlock));
        btn_PoliceCarItem.onClick.AddListener(() => OnItemButtonClick(btn_PoliceCarItem, ItemType.PoliceCar));
        btn_SpeedUpItem.onClick.AddListener(() => OnItemButtonClick(btn_SpeedUpItem, ItemType.SpeedUp));
        btn_ContinueGame.onClick.AddListener(btn_ContinueGame_Onclick);
        btn_BackMain.onClick.AddListener(btn_BackMain_Onclick);
        btn_RestartGame.onClick.AddListener(btn_RestartGame_Onclick);
        btn_PauseGame.onClick.AddListener(btn_PauseGame_Onclick);
        btn_StartGame.onClick.AddListener(btn_StartGame_Onclick);
        btn_Help.onClick.AddListener(btn_Help_Onclick);
        btn_Rank.onClick.AddListener(btn_Rank_Onclick);
        btn_Rank_Close.onClick.AddListener(btn_Rank_Close_Onclick);
        ResetAllButtonColors();
    }

    private void OnItemButtonClick(Button btn, ItemType type)
    {
        AudioManager.Instance.PlayMouseClick();
        // 如果再次点击的是已经选中的按钮，则视为取消选择
        if (selectedButton == btn)
        {
            ItemManager.Instance.ClearAllState();
            ClearSelection();
            return;
        }
        // 检查数量
        if (ItemManager.Instance.itemCounts[type] == 0)
        {
            Debug.Log($"道具 {type} 已耗尽，播放广告获得1个");
        }
        else
        {
            // 切换高亮
            HighlightButton(btn);
            if (type == ItemType.SpeedUp)
            {
                ItemManager.Instance.SpeedUpState = true;
            }
            if (type == ItemType.RemoveCollision)
            {
                ItemManager.Instance.RemoveCollisionState = true;
            }
            if (type == ItemType.RoadBlock)
            {
                ItemManager.Instance.UseItem(ItemType.RoadBlock, null);
                ClearSelection();
            }
            if (type == ItemType.PoliceCar)
            {
                ItemManager.Instance.UseItem(ItemType.PoliceCar, null);
                ClearSelection();
            }
        }
    }
    public void ResetAllButtonColors()
    {
        foreach (var b in allItemButtons)
        {
            var img = b.GetComponent<Image>();
            img.color = normalColor;
        }
    }
    private void HighlightButton(Button btn)
    {
        ResetAllButtonColors();
        var img = btn.GetComponent<Image>();
        img.color = highlightColor;
        selectedButton = btn;
    }
    public void ClearSelection()
    {
        ResetAllButtonColors();
        selectedButton = null;
    }
    public void UpdateUICount()
    {
        txt_RemoveCollisionItemCount.text = ItemManager.Instance.itemCounts[ItemType.RemoveCollision].ToString();
        txt_RoadBlockItemCount.text = ItemManager.Instance.itemCounts[ItemType.RoadBlock].ToString();
        txt_PoliceCarItemCount.text = ItemManager.Instance.itemCounts[ItemType.PoliceCar].ToString();
        txt_SpeedUpItemCount.text = ItemManager.Instance.itemCounts[ItemType.SpeedUp].ToString();
        txt_Score.text = "得分：" + GameManager.Instance.currentScore.ToString();
        // 将健康值从图像显示改为文本显示
        txt_Heat.text = GameManager.Instance.currentHealth.ToString();
    }

    private void btn_PauseGame_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        panelPauseGame.SetActive(true);
        Time.timeScale = 0;
    }
    private void btn_ContinueGame_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        panelPauseGame.SetActive(false);
        Time.timeScale = 1;
    }
    private void btn_BackMain_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        gameInItems.SetActive(false); // 隐藏游戏界面
        panelMain.SetActive(true); // 显示主菜单界面
        panelGameOver.SetActive(false); // 隐藏游戏结束界面
        MainSceneCamera.gameObject.SetActive(true); // 显示主场景摄像机
    }
    private void btn_RestartGame_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        GameManager.Instance.StartGame();
        panelGameOver.SetActive(false); // 隐藏游戏结束界面
    }
    private void btn_StartGame_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        panelMain.SetActive(false); // 隐藏主菜单界面
        gameInItems.SetActive(true); // 显示游戏界面
        GameManager.Instance.StartGame();
        MainSceneCamera.gameObject.SetActive(false); // 隐藏主场景摄像机
    }
    private void btn_Help_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        if (panel_Help.activeSelf)
            panel_Help.SetActive(false);
        else
            panel_Help.SetActive(true);
    }
    private void btn_Rank_Close_Onclick()
    {
        AudioManager.Instance.PlayMouseClick();
        RankManager.Instance.HideRank();
        panel_Rank.SetActive(false);
    }
    private void btn_Rank_Onclick()
    {
        // panel_Rank.SetActive(true);
        AudioManager.Instance.PlayMouseClick();
        // RankManager.Instance.ShowRank();
    }

    // 增加生命值的方法
    public void AddHealth(float amount)
    {
        // 强制设置为1，确保广告看完后增加1点生命值
        float actualAmount = 1f;
        Debug.Log("AddHealth方法被调用，原始amount值为: " + amount + "，实际使用值为: " + actualAmount);

        // 判断是否在编辑器环境下，方便测试
#if UNITY_EDITOR
        Debug.Log("编辑器环境下，直接增加生命值，跳过广告");
        Debug.Log("增加生命值前，当前生命值为: " + GameManager.Instance.currentHealth);
        GameManager.Instance.UpdateHealth(actualAmount);
        Debug.Log("增加生命值后，当前生命值为: " + GameManager.Instance.currentHealth);
        // 更新UI显示
        UpdateUICount();
        Debug.Log("UI已更新，当前生命值显示为: " + UIManager.Instance.txt_Heat.text);
#else
        AdManager.Instance.ShowRewardedVideoAd(onSuccess: () =>
        {
            Debug.Log("广告调用成功，准备增加生命值: " + actualAmount);
            Debug.Log("增加生命值前，当前生命值为: " + GameManager.Instance.currentHealth);
            GameManager.Instance.UpdateHealth(actualAmount);
            Debug.Log("增加生命值后，当前生命值为: " + GameManager.Instance.currentHealth);
            // 更新UI显示
            UpdateUICount();
            Debug.Log("UI已更新，当前生命值显示为: " + UIManager.Instance.txt_Heat.text);

        }, onFail: (error) =>
        {
            Debug.LogError($"广告播放失败，无法使用技能: {error}");
        });
#endif
    }
}
