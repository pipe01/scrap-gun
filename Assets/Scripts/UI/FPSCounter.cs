﻿/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* **************************************************************************
 * CLASS: FPS COUNTER
 * *************************************************************************/
[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    /* Public Variables */
    public float frequency = 0.5f;

    /* **********************************************************************
	 * PROPERTIES
	 * *********************************************************************/
    public int FramesPerSec { get; protected set; }

    private Text text;

    /* **********************************************************************
	 * EVENT HANDLERS
	 * *********************************************************************/
    /*
	 * EVENT: Start
	 */
    private void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(FPS());
    }

    /*
	 * EVENT: FPS
	 */
    private IEnumerator FPS()
    {
        for (;;)
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            text.text = FramesPerSec.ToString() + " fps";
        }
    }
}