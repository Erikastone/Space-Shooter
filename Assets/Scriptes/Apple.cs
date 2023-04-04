using UnityEngine;

public class Apple : MonoBehaviour
{/*«Классы
», статические переменные совместно используются всеми экземплярами
класса, поэтому все экземпляры Apple будут иметь одно и то же значение
bottomY. Если bottomY изменится в одном экземпляре, она одновременно изменится
во всех экземплярах. Также важно заметить, что статические поля,
такие как bottomY, не отображаются в инспекторе.
    */
    [Header("In inspector")]
    public static float bottomY = -20f;
    private void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);
            //ПОЛУЧИТЬ ССЫКУ НА КОМПОНЕНТ APPLEPICKER ГАВНОЙ КАМЕРЫ
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            //ВЫЗВАТЬ ОБОБЩЕННЫЙ МЕТОД APPLEDESTROYES ИЗ APSRIPT
            apScript.AppleDestroyed();
        }
    }
}
