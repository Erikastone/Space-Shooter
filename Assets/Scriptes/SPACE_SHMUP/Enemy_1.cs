using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector")]
    //число секунд полного цикла синусоиды
    public float waveFrequency = 2;
    //ширина синусоиды в метра
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0;//начальное значение координаты Х
    private float birthTime;
    private void Start()
    {
        //установить начальную координату Х 
        x0 = pos.x;
        birthTime = Time.time;
    }
    public override void Move()
    {
        //так как pos это свойство, нельзя напрямую изменить pos.x
        //поэтому получим pos в виде Vector3 доступного для изменения
        Vector3 tempPos = pos;
        //значение theta изменяется с течением времени
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;
        //повернуть немного относительно оси Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        //обрабатывает движение вниз
        base.Move();/*base.Move() вызывает метод Move() суперкласса Enemy. В данном случае
метод Move() в подклассе Enemy_l отвечает за горизонтальное перемещение
по синусоиде, а метод Move() в суперклассе Enemy отвечает за перемещение
по вертикали.*/
        //print(bndCheck.isOnScreen);
    }
}
