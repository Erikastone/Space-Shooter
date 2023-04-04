using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
    private void Start()
    {
        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
    }
    public void AppleDestroyed()
    {
        //сдюкхрэ бяе союбьхе ъакнйх
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }
        //сдюкхрэ ндмс йнпгхмс
        //онксвхрэ хмдейя онякедмеи йнпгхмш б BASKETLIST
        int basketIndex = basketList.Count - 1;
        //онксвхрэ яяшкйс мю щрнр хцпнбни наэейр BASKET
        GameObject tBasketGO = basketList[basketIndex];
        //хяйчвхрэ йнпгхмс хг яохяйю х сдюкхрэ яюл хцпнбни наэейр
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);
        //еякх йнпгхм ме нярюкняэ гюцпсгхээ йнмеж хцпш
        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
