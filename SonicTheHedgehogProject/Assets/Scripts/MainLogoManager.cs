using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogoManager : MonoBehaviour
{
    [System.Serializable]
    public class CanvasStep
    {
        public GameObject canvas;
        public float waitTillNext = 1f;
    }

    public List<CanvasStep> steps = new List<CanvasStep>();

    void Start()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // disable all first
        foreach (var step in steps)
            if (step.canvas != null)
                step.canvas.SetActive(false);

        // now enable one at a time
        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].canvas != null)
                steps[i].canvas.SetActive(true);

            yield return new WaitForSeconds(steps[i].waitTillNext);
        }
    }
}
