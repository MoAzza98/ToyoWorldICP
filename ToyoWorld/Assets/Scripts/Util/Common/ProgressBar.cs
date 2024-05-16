using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameObject progress;

    public void SetProgress(float val)
    {
        progress.transform.localScale = new Vector2(val, 1f);
    }

    bool isUpdatingProgress = false;
    public IEnumerator SetProgressSmooth(float val, float updateDuration=0.5f)
    {
        isUpdatingProgress = true;

        float startVal = progress.transform.localScale.x;
        float finalVal = val;

        float timer = 0f;
        while (timer <= updateDuration)
        {
            float currVal = Mathf.Lerp(startVal, finalVal, timer / updateDuration);
            progress.transform.localScale = new Vector2(currVal, 1f);

            timer += Time.deltaTime;
            yield return null;
        }

        progress.transform.localScale = new Vector2(finalVal, 1f);

        isUpdatingProgress = false;
    }

    public IEnumerator WaitForUpdate()
    {
        yield return new WaitUntil(() => isUpdatingProgress == false);
    }
}
