using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Mover : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    
    [Header("Настройки передвижения")]
    [SerializeField] private float _moveSpeed = 5f; // Настраивай скорость бега здесь
    [SerializeField] private float _jumpForce = 7f; // Настраивай высоту прыжка здесь
    [SerializeField] private int _maxJumps = 2;     // Ограничение: 1 для обычного, 2 для двойного

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private int _jumpCount = 0; // Счетчик текущих прыжков

    // Названия параметров в вашем Animator
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _inputReader.JumpButtonPressed += Jump;
        _inputReader.MovementButtonPressed += Move;
    }

    private void OnDisable()
    {
        _inputReader.JumpButtonPressed -= Jump;
        _inputReader.MovementButtonPressed -= Move;
    }

    private void Update()
    {
        // Если вертикальная скорость близка к нулю (персонаж коснулся земли/перестал лететь вверх), выключаем анимацию
        if (Mathf.Abs(_rigidbody2D.velocity.y) < 0.05f)
        {
            _animator.SetBool(IsJumping, false);
            _jumpCount = 0; // Сбрасываем счетчик прыжков при приземлении
        }
    }

    private void Jump()
    {
        // Проверяем, не превышен ли лимит прыжков
        if (_jumpCount < _maxJumps)
        {
            // Сохраняем текущую скорость по X (чтобы не останавливаться в воздухе), меняем только Y
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _jumpCount++;
            
            // Включаем параметр IsJumping в Animator
            _animator.SetBool(IsJumping, true);
        }
    }

    private void Move(float movementValue)
    {
        // Теперь бег умножается на _moveSpeed
        Vector2 movement = Vector2.right * (movementValue * _moveSpeed * Time.deltaTime);
        transform.Translate(movement);

        // Если movementValue не равен 0 — персонаж бежит
        bool isMoving = Mathf.Abs(movementValue) > 0.01f;
        _animator.SetBool(IsRunning, isMoving);

        // Разворот спрайта влево/вправо в зависимости от направления
        if (_spriteRenderer != null && isMoving)
        {
            _spriteRenderer.flipX = movementValue < 0;
        }
    }
}