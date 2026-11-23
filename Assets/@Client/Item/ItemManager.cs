using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab = null;

    [SerializeField]
    private Renderer floorRenderer = null;

    [SerializeField]
    private float spawnInterval = 5f;

    [SerializeField]
    private int maxItemCount = 3;

    [SerializeField]
    private float edgePadding = 2f;

    [SerializeField]
    private float spawnHeight = 0.5f;

    private List<Item> itemList = null;

    private void Awake()
    {
        itemList = new List<Item>();
    }

    public void Init()
    {
        StartCoroutine(CoSpawnItems());
    }

    private IEnumerator CoSpawnItems()
    {
        while (true)
        {
            if (itemList.Count < maxItemCount)
            {
                SpawnItem();
                yield return new WaitForSeconds(spawnInterval);
            }
            yield return null;
        }
    }

    private void SpawnItem()
    {
        if (itemPrefab == null || floorRenderer == null)
        {
            Debug.LogWarning("[ItemManager] itemPrefab 또는 floorRenderer가 할당되지 않았습니다.");
            return;
        }

        Bounds bounds = floorRenderer.bounds;
        float minX = bounds.min.x + edgePadding;
        float maxX = bounds.max.x - edgePadding;
        float minZ = bounds.min.z + edgePadding;
        float maxZ = bounds.max.z - edgePadding;

        if (minX > maxX)
        {
            minX = maxX = bounds.center.x;
        }
        if (minZ > maxZ)
        {
            minZ = maxZ = bounds.center.z;
        }

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float spawnY = bounds.max.y + spawnHeight;

        Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

        GameObject go = Instantiate(
            itemPrefab,
            spawnPosition,
            Quaternion.identity,
            this.transform);

        Item itemComponent = go.GetComponent<Item>();
        if (itemComponent != null)
        {
            itemComponent.Init(OnItemCollected);
            itemList.Add(itemComponent);
        }
    }

    private void OnItemCollected(Item item)
    {
        itemList.Remove(item);
    }
}
