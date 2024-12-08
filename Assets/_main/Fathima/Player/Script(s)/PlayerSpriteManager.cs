using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite baseSprite;
    public Sprite dashingSprite;
    public Sprite wallClimbingSprite;
    public Sprite fallingSprite;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        UpdateSpriteBasedOnAction();
    }

    private void UpdateSpriteBasedOnAction()
    {
        if (playerMovement.isDashing)
        {
            SetSprite(dashingSprite);
        }
        else if (!playerMovement.IsGrounded() && playerMovement.rb.velocity.y < 0)
        {
            SetSprite(fallingSprite);
        }
        else if (playerMovement.IsWalled() && !playerMovement.IsGrounded() && playerMovement.horizontal != 0)
        {
            SetSprite(wallClimbingSprite);
        }
        else
        {
            SetSprite(baseSprite);
        }
    }

    private void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer.sprite != newSprite)
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}
