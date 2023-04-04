using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;/*numSpheresMin / numSpheresMax — минимальное и максимальное (в действительности
на 1 больше фактического) количество создаваемых экземпляров
CloudSphere.
    */
    public int numSpheresMax = 10;
    public Vector3 sphereOFfsetScale = new Vector3(5, 2, 1);/*sphereOff setScale — максимальное расстояние (положительное или отрицательное)
CloudSphere от центра Cloud вдоль каждой оси
    */
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);/*sphereScaleRangeX / Y / Z — диапазон масштаба для каждой оси. По
умолчанию создаются экземпляры CloudSphere, ширина которых больше
высоты.*/
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;/*scaleYMin — в конце функции Start () масштаб по оси Y каждого экземпляра
CloudSphere уменьшается в зависимости от удаленности от центра
вдоль оси X. Благодаря этому толщина облаков будет уменьшаться к краям.
scaleYMin — это наименьший возможный масштаб по оси Y (чтобы
избежать появления слишком тонких облаков).*/

    private List<GameObject> spheres;
    private void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            //ВЫБРАТЬ СЛУЧАЙНОЕ МЕСТОПОЛОЖЕНИЕ
            /*Выбирается случайная точка внутри единичной сферы (то есть точка где-то
            внутри сферы с радиусом, равным единице, и с центром в начале координат:
            [ 0, 0, 0 ] ). Каждая координата (X, Y, Z) этой точки затем умножается на соответствующее
            значение sphereOff setScale.*/
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOFfsetScale.x;
            offset.y *= sphereOFfsetScale.y;
            offset.z *= sphereOFfsetScale.z;
            spTrans.localPosition = offset;/*Свойству CloudSphere.transform.localposition присваивается смещение
offset, transform, position всегда выражается в мировых координатах, тогда
как transform. localPosition — в координатах относительно центра родителя
(в данном случае объекта Cloud).*/

            //ВЫБРАТЬ СЛУЧАЙНЫЙ МАСШТАБ
            /*Случайно выбирается масштаб для каждой оси в отдельности. Вектор
            sphereScaleRange хранит в поле х минимальное значение, а в поле у — максимальное.*/
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //СКОРЕКТИРОВАТЬ МАСШТАБ У ПО РАССТОЯНИЮ Х ОТ ЦЕНТРА
            /*После выбора масштаба по осям изменяется масштаб по оси Y, в зависимости
            от смещения CloudSphere от центра Cloud вдоль оси X. Чем дальше сфера от
            центра облака, тем меньше масштаб по оси Y.*/
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOFfsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            /*Значение scale присваивается свойству localscale экземпляра CloudSphere.
              Масштаб всегда определяется относительно родительского компонента
              Transform, поэтому нет поля transform. scale. Только localscale и lossyScale.
              Свойство lossyScale, доступное только для чтения, пытается возвращать
              масштаб в мировых координатах, при этом вы должны понимать, что это
              всего лишь оценка.*/
            spTrans.localScale = scale;
        }
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
        */
    }
    private void Restart()
    {
        //УДАЛИТЬ СТАРЫЕ СФЕРЫ, СОСТАВЛЯЮЩИЕ ОБЛАКО
        /*Метод Restart() уничтожает все дочерние сферы CloudSphere и вызывает
        Start(), чтобы сгенерировать новое облако*/
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
