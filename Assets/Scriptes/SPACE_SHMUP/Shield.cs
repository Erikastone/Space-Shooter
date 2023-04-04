using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;
    [Header("Set Dynamically")]
    public int levelShown = 0;

    private Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        //прочитать текущую мощность защитного поля из обьекта одиночки hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);/*Значение Hero. S. shield Level округляется вниз до ближайшего целого, и результат
присваивается currLevel. Округление shieldLevel гарантирует, что
переход к новому смещению в текстуре по оси X произойдет точно к границе
между сегментами, а не между двумя границами.*/
        //если они отличается от levelShown
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            //скоректировать смещение в текстуре, чтобы отобразить поле с другой мощностью
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //поворачивать поле в каждом кадре с постоянной скоростью
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
