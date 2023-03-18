using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorLightingControl : MonoBehaviour
{
    //是否亮
    private bool isLit = false;
    private Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    //淡入
    public void FadeInDoor(Door door)
    {
        //创建一个新的材质
        Material material = new Material(GameResources.Instance.variableLitShader);

        //如果没有亮
        if (!isLit)
        {
            //获取所有的sprite renderer,因为有上下两个门
            SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();
            //循环遍历所有的spriterender 
            foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
            {
                //启动协程
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }

            isLit = true;
        }
    }

    //淡入 协程
    private IEnumerator FadeInDoorRoutine(SpriteRenderer spriteRenderer, Material material)
    {
        spriteRenderer.material = material;

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;

        }

        spriteRenderer.material = GameResources.Instance.litMaterial;
    }

    //玩家进入门的trigger,则淡入门
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FadeInDoor(door);
    }
}
