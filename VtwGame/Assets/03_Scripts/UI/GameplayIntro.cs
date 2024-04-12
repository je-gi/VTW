using UnityEngine;
using TMPro;
using System.Collections;

public class GameplayIntro : MonoBehaviour
{
    public GameObject overlayCanvas;
    public TextMeshProUGUI displayText;
    public float delay = 0.05f;
    public string promptText = "Press 'F' to continue";
    public string promptTextColor = "#FF4D06";
    public int promptTextSize = 28;

    private string fullText = 
    "Luxpera, once a radiant land, now lies cloaked in darkness.\n\n" +
    "Lux, the last lightbearer, emerges to revive the fading world. Guided by faint lights, Lux embarks on a quest to uncover the Radiant Core, a legendary beacon believed to restore balance.\n\n" +
    "Step forth, Lux. Your journey to reclaim the light begins now.";


    private void Start()
    {
        if (PlayerPrefs.GetInt("IsRestarting", 0) == 1)
        {
            PlayerPrefs.SetInt("IsRestarting", 0);
            overlayCanvas.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowText());
            Time.timeScale = 0;
        }
    }

    IEnumerator ShowText()
    {
        displayText.text = "";
        foreach (char letter in fullText)
        {
            displayText.text += letter;
            yield return new WaitForSecondsRealtime(delay);
        }
        yield return new WaitForSecondsRealtime(1);

        displayText.text += "\n\n\n\n<align=right><size=" + promptTextSize + "><color=" + promptTextColor + ">" + promptText + "</color></size></align>";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && displayText.text.Contains(promptText))
        {
            overlayCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
