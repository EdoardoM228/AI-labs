using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость движения игрока

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Получаем компонент Rigidbody2D
    }

    void Update()
    {
        // Получаем ввод от пользователя (W/S/A/D или стрелки)
        moveInput.x = Input.GetAxis("Horizontal");  // Влево/вправо
        moveInput.y = Input.GetAxis("Vertical");    // Вверх/вниз
    }

    void FixedUpdate()
    {
        // Применяем перемещение к Rigidbody2D
        rb.velocity = moveInput * moveSpeed;
    }
}