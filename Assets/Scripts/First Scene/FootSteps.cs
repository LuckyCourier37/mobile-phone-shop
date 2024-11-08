using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource; // ссылка на аудио-источник
    [SerializeField]private AudioClip footstepSounds; // массив звуков шагов
    public float stepInterval = 0.5f; // интервал между шагами
    private float stepTimer = 0f;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void FixedUpdate()
    {
        backToPlane();

        if (IsMoving()) // проверка, двигается ли персонаж
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval && !audioSource.isPlaying)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = stepInterval; // сброс таймера при остановке движения
            audioSource.Stop();

        }

    }

    private bool IsMoving()
    {
        // замените это условие на проверку состояния вашего персонажа
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void PlayFootstep()
    {
        
        audioSource.clip = footstepSounds;
        audioSource.Play();
    }

    private void backToPlane()
    {
        if(transform.position.y < - 10f)
        {
            transform.position = initialPos;
        }
    }
}
