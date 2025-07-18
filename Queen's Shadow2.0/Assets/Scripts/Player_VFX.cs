using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchointerval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCo;

    public void CreateEffectOf(GameObject effect, Transform targer)
    {
        Instantiate(effect, targer.position, Quaternion.identity);
    }

    public void DoImageEchoEffect(float duration)
    {
        if(imageEchoCo != null) 
            StopCoroutine(imageEchoCo);

        imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }

    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float timeTracker = 0;

        while (timeTracker < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchointerval);
            timeTracker = timeTracker + imageEchointerval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
