using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMenuPattern : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public float scrollSpeed = 50f;
    [SerializeField] private  RectTransform[] elements; // Asignar ambos elementos en el Inspector

    private float elementHeight;
    private Vector3 initialPosition;

    void Start()
    {
        elementHeight = elements[0].rect.height;
        initialPosition = elements[0].anchoredPosition;

        // Posicionar el segundo elemento debajo del primero
        elements[1].anchoredPosition = new Vector2(
            initialPosition.x,
            initialPosition.y - elementHeight
        );
    }

    void Update()
    {
        foreach (RectTransform element in elements)
        {
            // Mover cada elemento hacia arriba
            element.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            // Comprobar si el elemento ha salido completamente de la pantalla
            if (element.anchoredPosition.y > elementHeight)
            {
                // Reposicionar debajo del otro elemento
                Vector2 newPos = element.anchoredPosition;
                newPos.y = -elementHeight;
                element.anchoredPosition = newPos;
            }
        }
    }
}
