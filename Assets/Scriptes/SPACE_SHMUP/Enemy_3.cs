using UnityEngine;

public class Enemy_3 : Enemy
{
    //траектория движения Enemy_3 вычисляется путем линейной
    //интерполяции кривой Безье по более чем двум точкам
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;
    [Header("Set Dynamically")]
    public Vector3[] points;
    public float birthTime;
    private void Start()
    {
        points = new Vector3[3];
        //начальная позиция уже определена в Main.SpawnEnemy()
        points[0] = pos;
        //установить xMin, xMax так же, как это делает Main.Sp...
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;
        Vector3 v;
        //случайно выбрать среднюю точку ниже нижней границы экрана
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;
        //случайно выбрать конечную точку выше верхней границы экрана
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;
        birthTime = Time.time;
    }
    public override void Move()
    {
        //кривые Безье вчисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //Интерполировать кривую Безье по 3-и точкам
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + p12;
    }
}
