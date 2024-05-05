using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryTelling : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUI;
    public string[] storyLines;
    public float displayDuration = 2f;

    private bool canMoveNextLine = true;

    public Animator animator;

    private int currentIndex = 0;

    public bool tellingIsDone;

    private void Start()
    {
        textMeshProUI.text = storyLines[currentIndex];
        animator = textMeshProUI.GetComponent<Animator>();

        animator.ResetTrigger("FadeIn");
        animator.SetTrigger("FadeIn");

        tellingIsDone = false;
    }

    private void Update()
    {
        if (currentIndex < storyLines.Length && canMoveNextLine && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Button Click");
            StartCoroutine(WaitForNextLine());
        }
    }
    IEnumerator WaitForNextLine()
    {
        canMoveNextLine = false;
        animator.ResetTrigger("FadeOut");
        animator.SetTrigger("FadeOut");
        
        currentIndex = currentIndex + 1;

        if (currentIndex < storyLines.Length)
        {
            yield return new WaitForSeconds(1f);
            textMeshProUI.text = storyLines[currentIndex];

            animator.ResetTrigger("FadeIn");
            animator.SetTrigger("FadeIn");

            yield return new WaitForSeconds(1f);

            canMoveNextLine = true;
        } else
        {
            animator.ResetTrigger("FadeOut");
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            tellingIsDone = true;
            gameObject.SetActive(false);
        }
    }
}
