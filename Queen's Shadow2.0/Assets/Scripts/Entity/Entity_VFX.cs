using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamgeVFXDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCo;

    [Header("On Doint Damge VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject critHitVFX;

    [Header("Element Colors")]
    [SerializeField] private Color chillVFX = Color.cyan;
    private Color defaultHitVfxColor;
    [Space]
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color lightningVFX = Color.yellow;

    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchointerval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCo;


    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        defaultHitVfxColor = hitVFXColor;
    }
    public void DoImageEchoEffect(float duration)
    {
        StopImageEchoEffect();

        imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }

    public void StopImageEchoEffect()
    {
        if (imageEchoCo != null)
            StopCoroutine(imageEchoCo);
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
        Vector3 position = entity.anim.transform.position;
        float scale = entity.anim.transform.localScale.x;

        GameObject imageEcho = Instantiate(imageEchoPrefab, position, transform.rotation);

        imageEcho.transform.localScale = new Vector3 (scale, scale, scale);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }


    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if(element == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCo(chillVFX, duration));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCo(burnVFX, duration));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVFXCo(lightningVFX, duration));
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVFXCo(Color color, float duration)
    {
        float tickInterval = 0.25f;
        float timer = 0;

        Color lighColor = color * 1.2f;
        Color darkColor = color * 0.8f;

        bool toggle = false;

        while (timer < duration)
        {
            sr.color = toggle ? lighColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timer = timer + tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, ElementType element)
    {
        GameObject hitPrefab = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

       // if(isCrit == false)
           // vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);

        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                return chillVFX;
            case ElementType.Fire:
                return burnVFX;
            case ElementType.Lightning:
                return lightningVFX;

            default:
                return Color.white;
        }
    }
    public void PlayOnDamageVFX()
    {
        if(onDamageVFXCo != null)
            StopCoroutine(onDamageVFXCo);

       onDamageVFXCo = StartCoroutine(OnDamgeVFXCo()); 
    }

    private IEnumerator OnDamgeVFXCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamgeVFXDuration);
        sr.material = originalMaterial;
    }

}
