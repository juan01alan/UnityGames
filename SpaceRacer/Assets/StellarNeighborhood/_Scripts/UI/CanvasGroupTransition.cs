using System.Collections;
using UnityEngine;

namespace StellarNeighborhood
{
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupTransition : MonoBehaviour
{
    [SerializeField] private bool startOpaque = false;
    [SerializeField] private bool switchOnStart = false;    
    [Range(0f, 1f)]
    [SerializeField] private float transparentAlpha = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float opaqueAlpha = 1f;
    [Range(0.05f, 10f)]
    [SerializeField] private float transitionTime = 1f;
    public float TransitionTime { get { return transitionTime; } }
    [SerializeField] private float waitToTransitionTime = 1f;
    public float WaitToTransitionTime { get { return waitToTransitionTime; } }

    private bool isTransitioning;
    private bool isOpaque;
    private float alphaCache;

    private CanvasGroup group;    

    private void Start()
    {
        group = GetComponent<CanvasGroup>();

        isTransitioning = false;
        if (startOpaque)
        {
            isOpaque = true;
            alphaCache = opaqueAlpha;
        }
        else
        {
            isOpaque = false;
            alphaCache = transparentAlpha;
        }

        group.alpha = alphaCache;   
        
        if(switchOnStart)
        {
            WaitToTransitionToggle(waitToTransitionTime);
        }
    }    

    public void TransitionToOpaque()
    {
        isOpaque = true;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition());
    }

    public void TransitionToTransparent()
    {
        isOpaque = false;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition());
    }

    public void TransitionToggle()
    {
        isOpaque = !isOpaque;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition());
    }

    public void TransitionToOpaque(float transitionT)
    {
        isOpaque = true;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition(transitionT));
    }

    public void TransitionToTransparent(float transitionT)
    {
        isOpaque = false;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition(transitionT));
    }

    public void TransitionToggle(float transitionT)
    {
        isOpaque = !isOpaque;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(Transition(transitionT));
    }

    public void WaitToTransitionToOpaque(float waitTime)
    {
        isOpaque = true;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransition(waitTime));
    }

    public void WaitToTransitionToTransparent(float waitTime)
    {
        isOpaque = false;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransition(waitTime));
    }

    public void WaitToTransitionToggle(float waitTime)
    {
        isOpaque = !isOpaque;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransition(waitTime));
    }

    public void WaitToTransitionToOpaqueSetTransitionTime(float transitionT)
    {
        isOpaque = true;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransitionSetTransitionTime(transitionT));
    }

    public void WaitToTransitionToTransparentSetTransitionTime(float transitionT)
    {
        isOpaque = false;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransitionSetTransitionTime(transitionT));
    }

    public void WaitToTransitionToggleSetTransitionTime(float transitionT)
    {
        isOpaque = !isOpaque;
        StopAllCoroutines();
        isTransitioning = true;
        StartCoroutine(WaitToTransitionSetTransitionTime(transitionT));
    }

    private IEnumerator Transition()
    {
        if (opaqueAlpha < transparentAlpha)
        {
            Debug.Log("Opaque and Transparent values are Switched");
            opaqueAlpha = transparentAlpha;
        }

        float step = opaqueAlpha - transparentAlpha;

        if (transitionTime <= 0f)
        {
            transitionTime = 0.05f;
        }

        float transitionStep = step / transitionTime;

        while (isTransitioning)
        {
            if (isOpaque)
            {
                alphaCache += Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);

                if (group)
                {
                    group.alpha = alphaCache;
                }
            }
            else
            {
                alphaCache -= Time.unscaledDeltaTime * transitionStep;

                if (alphaCache <= transparentAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                if (group)
                {
                    group.alpha = alphaCache;
                }
            }

            yield return null;
        }
    }

    private IEnumerator Transition(float transTime)
    {
        if (opaqueAlpha < transparentAlpha)
        {
            Debug.Log("Opaque and Transparent values are Switched");
            opaqueAlpha = transparentAlpha;
        }

        float step = opaqueAlpha - transparentAlpha;

        if (transTime <= 0f)
        {
            transTime = 0.05f;
        }

        float transitionStep = step / transTime;

        while (isTransitioning)
        {
            if (isOpaque)
            {
                alphaCache += Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);

                if (group)
                {
                    group.alpha = alphaCache;
                }
            }
            else
            {
                alphaCache -= Time.unscaledDeltaTime * transitionStep;

                if (alphaCache <= transparentAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                if (group)
                {
                    group.alpha = alphaCache;
                }
            }

            yield return null;
        }
    }

    private IEnumerator WaitToTransition(float waitTime)
    {       
        yield return new WaitForSecondsRealtime(waitTime);

        if (opaqueAlpha < transparentAlpha)
        {
            Debug.Log("Opaque and Transparent values are Switched");
            opaqueAlpha = transparentAlpha;
        }

        float step = opaqueAlpha - transparentAlpha;

        if (transitionTime <= 0f)
        {
            transitionTime = 0.05f;
        }

        float transitionStep = step / transitionTime;

        while (isTransitioning)
        {
            if (isOpaque)
            {
                alphaCache += Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                group.alpha = alphaCache;
            }
            else
            {
                alphaCache -= Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                group.alpha = alphaCache;
            }

            yield return null;
        }
    }

    private IEnumerator WaitToTransitionSetTransitionTime(float transTime)
    {
        yield return new WaitForSecondsRealtime(waitToTransitionTime);

        if (opaqueAlpha < transparentAlpha)
        {
            Debug.Log("Opaque and Transparent values are Switched");
            opaqueAlpha = transparentAlpha;
        }

        float step = opaqueAlpha - transparentAlpha;

        if (transTime <= 0f)
        {
            transTime = 0.05f;
        }

        float transitionStep = step / transTime;

        while (isTransitioning)
        {
            if (isOpaque)
            {
                alphaCache += Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                group.alpha = alphaCache;
            }
            else
            {
                alphaCache -= Time.unscaledDeltaTime * transitionStep;

                if (alphaCache >= opaqueAlpha)
                {
                    isTransitioning = false;
                }

                alphaCache = Mathf.Clamp(alphaCache, transparentAlpha, opaqueAlpha);
                group.alpha = alphaCache;
            }

            yield return null;
        }
    }
}
}