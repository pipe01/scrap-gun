using UnityEngine;
using System.Collections;

public class DoorActuator : MonoBehaviour {

    public GameObject player;
    public Sprite textureClosed, textureOpened;
    public float maxDistance = 1.0f;

    public bool Open
    {
        get;
        private set;
    }

    private SpriteRenderer spriteRenderer,
                           playerRenderer;
    private BoxCollider boxCollider;
    
	// Use this for initialization
	void Awake () {

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRenderer = player.GetComponent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider>();

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mpoint.z = 0;

        if (player && 
            Input.GetButtonDown("Use") &&
            spriteRenderer.bounds.Contains(mpoint) &&
            Vector3.Distance(player.transform.position, transform.position) < maxDistance &&
            !spriteRenderer.bounds.Intersects(playerRenderer.bounds))
        {
            ToggleOpen();
        }

	}

    public void ToggleOpen()
    {
        Open = !Open;

        spriteRenderer.sprite = Open ? textureOpened : textureClosed;

        boxCollider.isTrigger = Open;
    }
}
