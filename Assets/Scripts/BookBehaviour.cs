using System.Collections;
using UnityEngine;

public class BookBehaviour : MonoBehaviour
{
    private const string OpenTrigger = "OpenTrigger";
    private const string CloseTrigger = "CloseTrigger";

    [SerializeField] private GameObject defaultPdfReader;
    [SerializeField] private GameObject[] pdfReaders;
    [SerializeField] private Animator bookAnimator;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Transform cameraRig;

    public bool isBookOpen = false;
    public void OpenBook()
    {
        isBookOpen = true;
        SetBookRotation();
        TriggerBookAnimation(OpenTrigger);
        StartCoroutine(DelayedActivation(1.2f));
    }

    public void CloseBook()
    {
        isBookOpen = false;
        foreach (GameObject pdfReader in pdfReaders)
        {
            pdfReader.SetActive(false);
        }
        TriggerBookAnimation(CloseTrigger);
    }
    
    private void SetBookRotation()
    {
        float cameraYRotation = cameraRig.rotation.eulerAngles.y;
        StartCoroutine(LerpRotation(Quaternion.Euler(isBookOpen ? 180 : 0, cameraYRotation, isBookOpen ? 270 : 0))); // Modify this line
    }

    private void TriggerBookAnimation(string trigger)
    {
        bookAnimator.SetTrigger(trigger);
    }

    IEnumerator LerpRotation(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    IEnumerator DelayedActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        defaultPdfReader.SetActive(true);
    }
}