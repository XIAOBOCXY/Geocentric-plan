using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorLightingControl : MonoBehaviour
{
    //�Ƿ���
    private bool isLit = false;
    private Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    //����
    public void FadeInDoor(Door door)
    {
        //����һ���µĲ���
        Material material = new Material(GameResources.Instance.variableLitShader);

        //���û����
        if (!isLit)
        {
            //��ȡ���е�sprite renderer,��Ϊ������������
            SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();
            //ѭ���������е�spriterender 
            foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
            {
                //����Э��
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }

            isLit = true;
        }
    }

    //���� Э��
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

    //��ҽ����ŵ�trigger,������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FadeInDoor(door);
    }
}
