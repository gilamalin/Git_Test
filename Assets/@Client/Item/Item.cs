using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float healAmount = 20f;

    private Action<Item> onCollected = null;
    private bool isCollected = false;

    public void Init(Action<Item> onCollected)
    {
        this.onCollected = onCollected;
        isCollected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected)
        {
            return;
        }

        if (other.TryGetComponent<Player>(out Player player))
        {
            isCollected = true;
            player.Heal(healAmount);
            onCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
