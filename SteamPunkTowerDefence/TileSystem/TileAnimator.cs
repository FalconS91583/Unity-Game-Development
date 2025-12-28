using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    [SerializeField] private float defaultMoveDuration = .1f;

    [Header("Build Slot Movement")]
    [SerializeField] private float buildSlotYOffset = .25f;

    [Header("Grid Animation Details")]
    [SerializeField] private float tileMoveDuration = .1f;
    [SerializeField] private float tileDelay = .1f;
    [SerializeField] private float yOffset = 5;

    [Space]
    [SerializeField] private List<GameObject> mainMenuObjects = new List<GameObject>();
    [SerializeField] private GridBuilder mainSceneGrid;
    private Coroutine currentActiveCo;
    private bool isGridMoving;

    [Header("Grid Disolve Details")]
    [SerializeField] private Material disolveMat;
    [SerializeField] private float disolveDuration = 1.2f;
    [SerializeField] private List<Transform> dissolvingObjects = new List<Transform>();


    private void Start()
    {
        if (GameManager.instance.IsTestingLevel())
            return;

        CollectMainSceneObjects();
        ShowGrid(mainSceneGrid, true);
    }

    public void ShowMainGrid(bool showMainGrid)
    {
        ShowGrid(mainSceneGrid, showMainGrid);
    }

    public void ShowGrid(GridBuilder gridToMove, bool showGrid)
    {
        List<GameObject> objectsToMove = GetObjectsToMove(gridToMove, showGrid);

        if(gridToMove.IsOnFirstLoad())
            ApplyOffset(objectsToMove, new Vector3(0, -yOffset, 0));

        float offset = showGrid ? yOffset : -yOffset;

        gridToMove.MakeTilesNonInteractable(true);
       currentActiveCo = StartCoroutine(MoveGridCo(objectsToMove, offset, showGrid));
    }

    private IEnumerator MoveGridCo(List<GameObject> objectsToMove, float yOffset, bool showGrid)
    {
        isGridMoving = true;

        for (int i = 0; i < objectsToMove.Count; i++)
        {
            yield return new WaitForSeconds(tileDelay);

            if (objectsToMove[i] == null)
                continue;

            Transform tile = objectsToMove[i].transform;

            Vector3 targetPosition = tile.position + new Vector3(0,yOffset, 0);

            DissolveTiles(showGrid, tile);

            MoveTile(tile, targetPosition,showGrid,tileMoveDuration);
        }

        while(dissolvingObjects.Count > 0)
        {
            yield return null;
        }

        foreach (var tile in objectsToMove)
            tile.GetComponent<TileSlot>()?.MakeNonInteractable(false);

        isGridMoving = false;
    }

    public void MoveTile(Transform objectToMove, Vector3 targetPosition,bool showGrid, float? newDuration = null)
    {
        float moveDelay = showGrid ? 0 : .8f;

        float duration = newDuration ?? defaultMoveDuration;
        StartCoroutine(MoveTileCo(objectToMove, targetPosition,moveDelay, duration));
    }

    public IEnumerator MoveTileCo(Transform objectToMove, Vector3 targetPosition,float dealy = 0, float? newDuration = null)
    {
        yield return new WaitForSeconds(dealy);

        float time = 0;
        Vector3 startPosition = objectToMove.position;
        float duration = newDuration ?? defaultMoveDuration;

        while (time < duration)
        {
            if (objectToMove == null)
                break;

            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        if(objectToMove != null)
            objectToMove.position = targetPosition;
    }

    public void DissolveTiles(bool showTiles, Transform tile)
    {
        MeshRenderer[] meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();

        if(tile.GetComponent<TileSlot>() != null)
        {
            foreach (MeshRenderer mesh in meshRenderers)
            {
                StartCoroutine(DisolveTileCo(mesh, disolveDuration, showTiles));
            }
        }
    }

    private IEnumerator DisolveTileCo(MeshRenderer meshRenderer, float duration, bool showTile)
    {
        TextMeshPro textMeshPro = meshRenderer.GetComponent<TextMeshPro>();

        if(textMeshPro != null)
        {
            textMeshPro.enabled = showTile;
            yield break;
        }

        dissolvingObjects.Add(meshRenderer.transform);

        float startValue = showTile ? 1 : 0;
        float targetValue = showTile ? 0 : 1;

        //Cache values
        Material OriginalMaterial = meshRenderer.material;

        meshRenderer.material = new Material(disolveMat);

        Material disolveMaterialInstance = meshRenderer.material;

        disolveMaterialInstance.SetColor("_BaseColor", OriginalMaterial.GetColor("_BaseColor"));
        disolveMaterialInstance.SetFloat("_Metallic", OriginalMaterial.GetFloat("_Metallic"));
        disolveMaterialInstance.SetFloat("_Smoothness", OriginalMaterial.GetFloat("_Smoothness"));
        disolveMaterialInstance.SetFloat("_Dissolve", startValue);

        float time = 0;

        while(time < duration)
        {
            float dissolveValue = Mathf.Lerp(startValue, targetValue, time / duration);

            disolveMaterialInstance.SetFloat("_Dissolve", dissolveValue);

            time += Time.deltaTime;
            yield return null;
        }

        meshRenderer.material = OriginalMaterial;

        if(meshRenderer != null) 
            dissolvingObjects.Remove(meshRenderer.transform);
    }

    private void ApplyOffset(List<GameObject> objectsToMove, Vector3 offset)
    { 
        foreach (var obj in objectsToMove)
        {
            obj.transform.position += offset;
        }

    }

    public void EnableMainMenuGrid(bool enable)
    {
        ShowGrid(mainSceneGrid, enable);
        mainSceneGrid.GetComponent<NavMeshSurface>().enabled = enable;
    }

    public void EnableMainSceneObjects(bool enable)
    {
        foreach (var obj in mainMenuObjects)
        {
            obj.SetActive(enable);
        }
    }

    private void CollectMainSceneObjects()
    {
        mainMenuObjects.AddRange(mainSceneGrid.GetTileSetup());
        mainMenuObjects.AddRange(GetExtraObjects());
    }


    private List<GameObject> GetObjectsToMove(GridBuilder gridToMove, bool startWithTiles)
    {
        List<GameObject> objectsToMove = new List<GameObject>();
        List<GameObject> extraObjects = GetExtraObjects();

        if (startWithTiles)
        {
            objectsToMove.AddRange(gridToMove.GetTileSetup());
            objectsToMove.AddRange(extraObjects);
        }
        else
        {
            objectsToMove.AddRange(extraObjects);
            objectsToMove.AddRange(gridToMove.GetTileSetup());
        }

        return objectsToMove;
    }

    private List<GameObject> GetExtraObjects()
    {
        List<GameObject> extraObjects = new List<GameObject>();

        extraObjects.AddRange(FindObjectsOfType<EnemyPortal>().Select(component => component.gameObject));
        extraObjects.AddRange(FindObjectsOfType<Castle>().Select(component => component.gameObject));

        return extraObjects;
    }

    public Coroutine GetCurrentActiveCo() => currentActiveCo;
    public float GetBuildOffset() => buildSlotYOffset;
    public float GetTravelDuration() => defaultMoveDuration;
    public bool IsGridMoving() => isGridMoving;
}
