using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //===========ФУНКЦИИ ДЛЯ РАБОТЫ С МАТЕРИАЛАМИ==============
    //возвращает список всех материалов в данном игровом обьекте и его дочерних
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }
        return (mats.ToArray());/*В заключение список mats преобразуется в массив и возвращается вызывающему
коду.*/
    }
}
