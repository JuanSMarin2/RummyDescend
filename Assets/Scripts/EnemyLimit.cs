using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLimit : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_spriteRenderer.enabled = false;

    }

}
