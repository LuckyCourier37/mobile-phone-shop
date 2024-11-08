using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource; // ������ �� �����-��������
    [SerializeField]private AudioClip footstepSounds; // ������ ������ �����
    public float stepInterval = 0.5f; // �������� ����� ������
    private float stepTimer = 0f;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void FixedUpdate()
    {
        backToPlane();

        if (IsMoving()) // ��������, ��������� �� ��������
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
            stepTimer = stepInterval; // ����� ������� ��� ��������� ��������
            audioSource.Stop();

        }

    }

    private bool IsMoving()
    {
        // �������� ��� ������� �� �������� ��������� ������ ���������
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
