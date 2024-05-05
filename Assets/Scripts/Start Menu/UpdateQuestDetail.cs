using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateQuestDetail : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI testDetail_ui;
    [SerializeField] private QuestDisplay questDisplay;

    void Start()
    {
        testDetail_ui = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        testDetail_ui.text = questDisplay.questList[questDisplay.currentQuestIndex].detail;
    }
}
