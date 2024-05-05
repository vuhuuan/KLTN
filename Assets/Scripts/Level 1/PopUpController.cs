using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopUpController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private KeyCode triggerKey = KeyCode.Q;
    private PlayableDirector popUpDirector;
    [SerializeField] private float cooldownTime = 2f;

    private bool isPopUpRunning = false;
    private float lastPopUpTime = 0f;

    private void Start()
    {
        popUpDirector = GetComponent<PlayableDirector>();
    }
    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && !isPopUpRunning && Time.time - lastPopUpTime >= cooldownTime)
        {
            PlayPopUpEffect();
        }
    }

    void PlayPopUpEffect()
    {
        if (popUpDirector != null)
        {
            isPopUpRunning = true;

            popUpDirector.Stop();
            popUpDirector.time = 0;
            popUpDirector.Play();

            lastPopUpTime = Time.time + (float)popUpDirector.duration;

            StartCoroutine(WaitForPopUpToEnd());
        }
        else
        {
            Debug.LogWarning("PopUpDirector is not assigned!");
        }
    }

    IEnumerator WaitForPopUpToEnd()
    {
        yield return new WaitForSeconds((float)popUpDirector.duration);

        isPopUpRunning = false;
    }
}
