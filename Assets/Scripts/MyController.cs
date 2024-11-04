using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyController : MonoBehaviour
{
    // Write code inside the MeldValid function. You don't need to touch any other part of the script

    private bool MeldValid(List<int> cards)
    {
        // START CODING HERE
        return false;
    }

    #region Initialization
    [SerializeField] private Sprite[] suitSprites;

    public static MyController instance;

    private Transform cards, meld, loading;
    private Transform canvas;
    private List<int> selectedCards = new();
    private int currentCase = 0;
    private List<int>[] caseCards = {
        new(){ 0, 1, 2, 13, 26, 34, 51, 21, 19, 14, 32, 5, 44, 35, 12  },
        new(){ 15, 16, 17, 18, 19, 20, 12, 25,  29, 3, 35, 12 , 38, 51, 50 },
        new(){ 12, 12, 35, 28, 51, 36, 39, 21,  15, 44, 41, 49 , 10, 12, 41 },
        new(){ 0, 1, 2, 13, 26, 34, 200, 21, 19, 14, 32, 5, 44, 35, 12  },
        new(){ 15, 16, 17, 18, 19, 20, 12, 25,  29, 200, 35, 200 , 38, 51, 50 },
    };
    private static int[] answers = { 2, 19, 3, 20, 144 };
    private int[] myAnswers = {-1, -1, -1, -1, -1 };

    private void Awake()
    {
        canvas = new List<GameObject>(SceneManager.GetActiveScene().GetRootGameObjects()).Find(x => x.name == "Canvas").GetComponent<Canvas>().transform;
        instance = this;
    }

    private void Start()
    {
        loading = canvas.Find("UI").Find("loading");
        StartCoroutine(AnalyzeMelds());
    }

    private IEnumerator AnalyzeMelds()
    {
        var loadingRoutine = StartCoroutine(Loading());
        for (int i = 0; i < answers.Length; i++) { myAnswers[i] = GetAllMelds(caseCards[i]); yield return new WaitForEndOfFrame(); }
        StopCoroutine(loadingRoutine);
        loading.gameObject.SetActive(false);
        ChangeCase(0);
        canvas.Find("UI").Find("case").gameObject.SetActive(true);
    }
    private IEnumerator Loading()
    {
        loading.gameObject.SetActive(true);
        DateTime startTime = DateTime.UtcNow;

        while (true)
        {
            int elapsed = (int)(DateTime.UtcNow.Subtract(startTime).TotalMilliseconds);
            loading.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Analyzing" +  new string('.', 1+(elapsed/1000)%3)+ " (" + (elapsed/1000).ToString("n0")+"s)";
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region UI
    public void ChangeCase(int inc)
    {
        currentCase += inc;
        UpdateGraphics(caseCards[currentCase]);
        int melds = myAnswers[currentCase];
        canvas.Find("UI").Find("case").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Case: " + (currentCase+1) + " of " + answers.Length;
        canvas.Find("UI").Find("case").GetChild(1).GetComponent<TextMeshProUGUI>().text = "Found: " + (melds == -1 ? "-" : melds) + "/" + answers[currentCase];
        canvas.Find("UI").Find("case").GetChild(1).GetComponent<TextMeshProUGUI>().color = ColorFromHex(melds == -1 ? "#FFFFFF" : (melds == answers[currentCase] ? "#A6FFC2" : "#FFA6A6"));
        canvas.Find("UI").Find("prev").gameObject.SetActive(currentCase > 0);
        canvas.Find("UI").Find("next").gameObject.SetActive(currentCase < (answers.Length - 1));
        canvas.Find("UI").Find("case").gameObject.SetActive(true);
    }
    #endregion

    #region Graphics
    public void UpdateGraphics(List<int> myCards)
    {
        if (cards == null) { cards = canvas.Find("UI").Find("cards"); meld = canvas.Find("UI").Find("meld"); }

        foreach(Transform t in cards.GetChild(0)) t.gameObject.SetActive(false);

        myCards = myCards.ToList();
        myCards.Sort(SortCards);

        selectedCards.Clear();

        for (int i = 0; i < myCards.Count; i++)
        {
            int id = myCards[i];
            var card = cards.GetChild(0).GetChild(i);
            card.gameObject.SetActive(true);
            card.name = id.ToString();
            card.Find("num").GetComponent<TextMeshProUGUI>().text = id == 200 ? "J" : GetCardName(id)[1] == 'T' ? "10" : GetCardName(id)[1].ToString();
            card.Find("num").GetComponent<TextMeshProUGUI>().color = card.Find("sym").GetComponent<Image>().color = id == 200 ? ColorFromHex("#408040") : (id / 13) % 2 == 0 ? Color.black : ColorFromHex("#C03030");
            card.Find("sym").GetComponent<Image>().sprite = suitSprites[id == 200 ? 4 : (id / 13)];
            card.GetComponent<Card>().SetSelected(false);
        }
    }

    public void DeselectAll()
    {
        foreach (Transform t in cards.GetChild(0)) t.GetComponent<Card>().SetSelected(false);
        selectedCards.Clear();
        meld.GetComponent<Animator>().SetTrigger("hideIm");
    }

    public bool CardSelected(int card, bool sel)
    {
        bool res = false;

        if (sel)
        { 
            if (selectedCards.Count < 5) 
            { 
                selectedCards.Add(card); res = true; 
            } 
        }
        else selectedCards.Remove(card);

        if (selectedCards.Count >= 3 && MeldValid(selectedCards))
        {
            if (meld.localScale.x < 0.05f) meld.GetComponent<Animator>().SetTrigger("show");
        }
        else
        {
            meld.GetComponent<Animator>().SetTrigger("hideIm");
        }

        return res;
    }
    #endregion

    #region Utility
    private static Color ColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex + (hex.Length > 6 ? "" : "FF"), out Color result);
        return result;
    }

    public static string GetCardName(int id)
    {
        string symbol = GetSymbol(id / 13);
        string number = GetNumber(id) switch
        {
            9 => "T",
            10 => "J",
            11 => "Q",
            12 => "K",
            13 => "A",
            _ => "" + (GetNumber(id) + 1),
        };

        string nam = symbol + number;

        return nam;
    }

    private static string GetSymbol(int clr)
    {
        return clr switch
        {
            0 => "♣",
            1 => "♦",
            2 => "♠",
            3 => "♥",
            _ => "_",
        };
    }

    public static int GetNumber(int card)
    {
        int num = card % 13;
        if (num == 0) num = 13;

        return num;
    }
    static int SortCards(int id2, int id1)
    {
        if (id1 % 13 == 0 || id2 % 13 == 0)
            if (Mathf.Floor(id1 / 13.0f) == Mathf.Floor(id2 / 13.0f))
                return ((id1 % 13 == 0) ? 100 : 0).CompareTo(((id2 % 13 == 0) ? 100 : 0));
        return id1.CompareTo(id2);
    }

    private int GetAllMelds(List<int> cards)
    {
        int res = 0;

        List<int> nums = new() { 0, 0, 0 };

        for (int idx1 = 0; idx1 < cards.Count-2; idx1++)
        {
            nums[0] = cards[idx1];
            for (int idx2 = idx1 + 1; idx2 < cards.Count-1; idx2++)
            {
                if (idx1 == idx2) continue;
                nums[1] = cards[idx2];
                for (int idx3 = idx2 + 1; idx3 < cards.Count; idx3++)
                {
                    if (idx2 == idx3) continue;
                    nums[2] = cards[idx3];
                    res += MeldValid(nums) ? 1 : 0;
                }
            }
        }

        nums = new() { 0, 0, 0, 0 };

        for (int idx1 = 0; idx1 < cards.Count-3; idx1++)
        {
            nums[0] = cards[idx1];
            for (int idx2 = idx1 + 1; idx2 < cards.Count-2; idx2++)
            {
                if (idx1 == idx2) continue;
                nums[1] = cards[idx2];
                for (int idx3 = idx2+1; idx3 < cards.Count-1; idx3++)
                {
                    if (idx2 == idx3) continue;
                    nums[2] = cards[idx3];
                    for (int idx4 = idx3 + 1; idx4 < cards.Count; idx4++)
                    {
                        if (idx3 == idx4) continue;
                        nums[3] = cards[idx4];
                        res += MeldValid(nums) ? 1 : 0;
                    }
                }
            }
        }

        nums = new() { 0, 0, 0, 0, 0 };

        for (int idx1 = 0; idx1 < cards.Count-4; idx1++)
        {
            nums[0] = cards[idx1];
            for (int idx2 = idx1 + 1; idx2 < cards.Count-3; idx2++)
            {
                if (idx1 == idx2) continue;
                nums[1] = cards[idx2];
                for (int idx3 = idx2 + 1; idx3 < cards.Count-2; idx3++)
                {
                    if (idx2 == idx3) continue;
                    nums[2] = cards[idx3];
                    for (int idx4 = idx3 + 1; idx4 < cards.Count-1; idx4++)
                    {
                        if (idx3 == idx4) continue;
                        nums[3] = cards[idx4];
                        for (int idx5 = idx4 + 1; idx5 < cards.Count; idx5++)
                        {
                            if (idx4 == idx5) continue;
                            nums[4] = cards[idx5];
                            res += MeldValid(nums) ? 1 : 0;
                        }
                    }
                }
            }
        }

        return res;
    }
    #endregion
}
