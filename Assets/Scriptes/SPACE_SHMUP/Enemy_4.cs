
using UnityEngine;
/// <summary>
/// создается за верхней границей, выбирает случайную точку на экране
/// и перемещается к ней. Добравшись до места выбирает др сл точку
/// и продолжает двигатся пока игрок не уничтожит его
/// 
/// Part еще один сереализуемый класс подобно WeaponDefinition
/// предназначенный для хранения данных
/// </summary>
[System.Serializable]
public class Part
{
    public string name;
    public float health;//степень стойкойти
    public string[] protectedBy;//др части, защичающие эту
    //кеширование ускоряет получение необходимых данных
    [HideInInspector]
    public GameObject go;//игровой обьект части
    [HideInInspector]
    public Material mat;//для отображения повреждений
}
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector")]
    public Part[] parts;//массив частей составляющих корабль
    private Vector3 p0, p1;//точки для интерполяции
    private float timeStart;
    private float duration = 4;//продолжительность перемещения
    private void Start()
    {
        p0 = p1 = pos;
        InitMovement();
        //записать в кэш игровой объект и материал каждой части в parts
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }
    void InitMovement()
    {
        p0 = p1;
        //выбрать новую точку p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        //сбросить время
        timeStart = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);//применить плавное замедление
        pos = (1 - u) * p0 + u * p1;
    }
    //эти 2 функции выполняют поиск  части в массиве parts n
    //по имени или по ссылке на игровой объект
    Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }
    //возвращают true если данная часть уничтожена
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            //если ссылка на часть не была передана
            return (true);
        }
        return (prt.health <= 0);
    }
    //окрашивает в красный только одну часть а не весь корабль
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDurantion;
        showingDanage = true;
    }
    //переопределяет метод из энеми
    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //если корабль за границей экрана не повреждать его
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //поразить вражеский корабль
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //проверить защищена ли еще эта часть корабля
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        //если хотябы одна из защищающих частей еще не разрушена..
                        if (!Destroyed(s))
                        {
                            //не наносить повреждений этой части
                            Destroy(other);
                            return;
                        }
                    }
                }
                //эта часть не защищена нанести ей повреждение
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                //показать эффект попадания в часть
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    //вместо разрушения всего корабля
                    //деактивировать уничтоженную часть
                    prtHit.go.SetActive(false);
                }
                //проверить был ли корабль полностью разрушен
                bool allDestroyed = true;
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))//если какаято часть еще существует записать в
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)//если корабль разрушен полностью
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }
}
