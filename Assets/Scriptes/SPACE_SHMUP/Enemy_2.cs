using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    //определяют насколько будет ярко выражен синусоидный характер движения
    public float sinEccetricity = 0.6f;
    public float lifeTime = 10;
    [Header("Set Dynamically: Enemy_2")]
    //Enemy_2 использцет линейную интерполяцию между двумя точками
    //изменяя результат по синусоиде
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;
    private void Start()
    {
        //выбрать случайную точку на евой границе экрана
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        //выбрать случайную точку на правой границе
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        //случайно поменять начальную и конечную точку местами
        if (Random.value > 0.5f)
        {
            //изменение знака .х каждой точки
            //переносит ее на другой кран экрана
            p0.x *= -1;
            p1.x *= -1;
        }
        //записать текущее втремя
        birthTime = Time.time;
    }
    public override void Move()
    {
        //кривые базье вычисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;
        //Если u>1, значит, корабль существует доьше чем lifeTime
        if (u > 1)
        {
            //этот экземпяр завершил свой жизненный цикл
            Destroy(this.gameObject);
            return;
        }
        //скоректировать u добавлением значения кривой, изменяющейся по синусоиде
        u = u + sinEccetricity * (Mathf.Sin(u * Mathf.PI * 2));
        //интерполировать местопоожение между двумя точками
        pos = (1 - u) * p0 + u * p1;
    }
}
