using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour
{
    public GameObject levelUp;
    public bool levellingUp;

    private void Awake()
    {
        levelUp = transform.Find("Level Up Image").gameObject;
        levelUp.SetActive(false);

    }

    void Update()
    {
        if (PersistantGameManager.Instance.levellingUp == true)
        {
            StartCoroutine(ShowLevelUp());
            PersistantGameManager.Instance.levellingUp = false;
        }

    }

    public IEnumerator ShowLevelUp()
    {
        levelUp.SetActive(true);
        Image image = levelUp.GetComponent<Image>();
        image.color = new Color32(255, 255, 255, 0);
        for (int i = 0; i <= 255; i += 10)
        {

            image.color = new Color32(255, 255, 255, (byte)i);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1);
        for (int i = 255; i >= 0; i -= 10)
        {

            image.color = new Color32(255, 255, 255, (byte)i);
            yield return null;
        }
        levelUp.SetActive(false);
    }
}
