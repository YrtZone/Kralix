using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer spriteRenderer;



    // Guarda a última direção de movimento para animação quando parado.
    private Vector2 lastDirection = new Vector2(0, -1);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // A lógica de animação agora precisa ser mais inteligente para 4 direções.
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // A lógica de movimento é a mesma, pois o 'movement' já foi processado no OnMove.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        //AQUI ESTÁ A LÓGICA DAS 4 DIREÇÕES
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Se o movimento horizontal é mais forte, zera o vertical.
            movement.x = input.x;
            movement.y = 0;
        }
        else
        {
            // Se o movimento vertical é mais forte (ou igual), zera o horizontal.
            movement.x = 0;
            movement.y = input.y;
        }

        // Normaliza o resultado para garantir velocidade constante.
        movement.Normalize();

        // Se houver algum movimento, atualiza a última direção.
        if (movement.sqrMagnitude > 0.1f)
        {
            lastDirection = movement;
        }
    }

    
    private void UpdateAnimation()
    {
        // Se estamos nos movendo, usamos a direção atual.
        // Se estamos parados, usamos a última direção em que nos movemos.
        Vector2 directionToAnimate = (movement.sqrMagnitude > 0.1f) ? movement : lastDirection;

        // Passa os valores para o Animator.
        animator.SetFloat("moveX", directionToAnimate.x);
        animator.SetFloat("moveY", directionToAnimate.y);

        // Inverte o sprite para movimento horizontal.
        if (directionToAnimate.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (directionToAnimate.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}