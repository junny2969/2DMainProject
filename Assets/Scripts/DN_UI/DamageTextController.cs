using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    [SerializeField] private TextMeshPro Text_Damage;
    [SerializeField] private float _floatSpeed = 200f;
    [SerializeField] private float _duration = 2f;

    public async UniTaskVoid PlayDamageText(int damage, Vector3 spawnPostion)
    {
        transform.position = spawnPostion;
        Text_Damage.text = ("-" + damage);

        float randomX = Random.Range(-1f, 1f);
        float elapsed = 0f;
        Color startColor = Text_Damage.color;

        while (elapsed < _duration)
        {
            if (this == null) return;
            elapsed += Time.deltaTime;
            float t = elapsed / _duration;

            float x = spawnPostion.x + randomX * _floatSpeed * t;
            float y = spawnPostion.y + _floatSpeed * (4f * t * (1f - t));
            // Debug.Log($"x:{x}, y:{y}, t:{t}");

            transform.position = new Vector3(x, y, spawnPostion.z);
            Text_Damage.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);

            await UniTask.Yield();
        }
        Destroy(gameObject);
    }
   
}
