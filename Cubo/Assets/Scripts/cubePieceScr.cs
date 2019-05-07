using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubePieceScr : MonoBehaviour
{
    // Start is called before the first frame update
    /*[SerializeField] private GameObject UpPlane,
        DownPlane,
        FrontPlane,
        BackPlane,
        LeftPlane,
        RightPlane;*/
    [SerializeField] public List<GameObject> Planes = new List<GameObject>(); 
    public void SetColor(int x, int y, int z)
    {
        // esto se puede ver graficamente en unity
        if (y == 0) //Si la posicion del cubo en y es 0, es pq es el cubo que esta arriba
        {
            Planes[0].SetActive(true);
        }
        else
        {
            if (y == -2) // si la posicion del cubo en y es en -2 es pq es el cubo q esta en la base
            {
                Planes[1].SetActive(true);
            }
        }

        if (z == 0) //si la posicion del cubo en z es 0, es pq es el cubo que esta al lado isquierdo
        {
            Planes[2].SetActive(true);
        }
        else
        {
            if (z == 2) //si la posicion del cubo en z es 2, es pq es el cubo que esta al lado derecho
            {
                Planes[3].SetActive(true);
            }
        }

        if (x == 0) //Si la posicion del cubo en x es 0, es pq es el cubo q esta en la parte frontal
        {
            Planes[4].SetActive(true);
        }
        else
        {
            if (x == -2) //Si la posicion del cubo en x es 0, es pq es el cubo q esta en la parte trasera
            {
                Planes[5].SetActive(true);
            }
        }
    }
}
