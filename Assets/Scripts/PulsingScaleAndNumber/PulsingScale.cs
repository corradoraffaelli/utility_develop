using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PulsingScale : MonoBehaviour {

    public enum TimeType
    {
        Normal,
        Unscaled
    }

    public TimeType timeType;

    public float totalTime = 1.0f;
    public float risingTime = 0.2f;
    public float standardScale = 1.0f;
    public float bigScale = 1.2f;

    public string finalString = "GO!";

    float beginTime = 0.0f;

    float actualScale;

    float decreasingTime = 0.8f;

    int number = -1;
    
    bool rising = false;

    TextMesh textMesh;
    Text uiText;

    Renderer renderer;

    float timeFromBeginning = 0.0f;
    float timeFromBeginningOld = 0.0f;

    bool singlePulse = false;

    public bool hideOnStart = false;
    bool hideEnd = false;

    void Awake ()
    {
        textMesh = GetComponent<TextMesh>();
        renderer = GetComponent<Renderer>();
        uiText = GetComponent<Text>();
        if (hideOnStart)
            Show(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            BeginTimer(4);
        if (Input.GetKeyUp(KeyCode.C))
            BeginSinglePulse(10, true);

        if (number >= 0 || singlePulse)
        {
            if (timeType == TimeType.Normal)
            {
                timeFromBeginning = (Time.time - beginTime) % totalTime;
            }
            else if (timeType == TimeType.Unscaled)
            {
                timeFromBeginning = (Time.unscaledTime - beginTime) % totalTime;
            }

            
            if (timeFromBeginningOld > timeFromBeginning)
            {
                number--;
                if ((number < 0 || singlePulse) && hideEnd)
                {
                    Show(false);
                }
                else if (!singlePulse)
                {
                    ChangeText();
                    rising = true;
                }

                if (singlePulse)
                {
                    singlePulse = false;
                    number = -1;
                }
            }
            
            if (rising)
            {
                if (timeFromBeginning <= risingTime)
                {
                    actualScale = standardScale + (((bigScale - standardScale) * timeFromBeginning) / risingTime);
                }
                else
                {
                    rising = false;
                    actualScale = bigScale;
                }

            }
            else
            {
                float decreasingTimeFromBeginning = timeFromBeginning - risingTime;

                actualScale = bigScale - (((bigScale - standardScale) * decreasingTimeFromBeginning) / decreasingTime);
            }

            try
            {
                transform.localScale = new Vector3(actualScale, actualScale, actualScale);
            }
            catch
            {
                Debug.Log(actualScale);
            }

            timeFromBeginningOld = timeFromBeginning;
        }
        
    }

    void ChangeText()
    {
        if (textMesh != null)
        {
            if (number > 0)
                textMesh.text = number.ToString();
            else if (number == 0)
                textMesh.text = finalString;
        }

        if (uiText != null)
        {
            if (number > 0)
                uiText.text = number.ToString();
            else if (number == 0)
                uiText.text = finalString;
        }
    }

    void Show(bool show = true)
    {
        if (renderer != null)
        {
            renderer.enabled = show;
        }

        if (uiText != null && !show)
            uiText.text = "";
    }

    public void BeginSinglePulse(int numberToShow, bool hideAtEnd = false)
    {
        singlePulse = true;

        if (risingTime > totalTime)
            risingTime = totalTime;

        decreasingTime = totalTime - risingTime;

        number = numberToShow;

        actualScale = transform.localScale.x;

        rising = true;

        if (timeType == TimeType.Normal)
            beginTime = Time.time;
        else if (timeType == TimeType.Unscaled)
            beginTime = Time.unscaledTime;

        Show(true);

        timeFromBeginning = 0.0f;
        timeFromBeginningOld = 0.0f;

        ChangeText();

        hideEnd = hideAtEnd;
    }

    public void BeginTimer(int pulseNumber, bool hideAtEnd = true)
    {
        if (risingTime > totalTime)
            risingTime = totalTime;

        decreasingTime = totalTime - risingTime;

        number = pulseNumber;

        actualScale = transform.localScale.x;

        rising = true;

        if (timeType == TimeType.Normal)
            beginTime = Time.time;
        else if (timeType == TimeType.Unscaled)
            beginTime = Time.unscaledTime;

        Show(true);

        timeFromBeginning = 0.0f;
        timeFromBeginningOld = 0.0f;

        ChangeText();

        hideEnd = hideAtEnd;
    }
}
