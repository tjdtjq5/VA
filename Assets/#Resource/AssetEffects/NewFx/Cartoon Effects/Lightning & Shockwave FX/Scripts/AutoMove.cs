using UnityEngine;

namespace CartoonEffects
{
    public class AutoMove : MonoBehaviour
    {
        public float speed = 5f; // �ƶ��ٶ�
        public Vector3 direction = Vector3.forward; // �ƶ�����

        void Update()
        {
            // ���������λ��
            Vector3 movement = direction.normalized * speed * Time.deltaTime;

            // ��λ��Ӧ�õ������λ��
            transform.Translate(movement);
        }
    }
}
