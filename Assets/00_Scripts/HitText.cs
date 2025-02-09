using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitText : MonoBehaviour
{
    private Vector3 target;
    private Camera cam;
    public TextMeshProUGUI txt;

    [SerializeField] private GameObject criticalObj;
    private float upRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double damage, bool monster = false, bool isCritical = false)
    {
        pos.x += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f, 0.1f);
        
        target = pos;
        txt.text = damage.ToCurrencyString();

        txt.color = monster ? Color.red : Color.white;
        
        transform.SetParent(BaseCanvas.Instance.HolderLayer(1));

        Color color = Color.yellow;
        
        criticalObj.SetActive(isCritical);
        
        txt.colorGradient = isCritical ? new VertexGradient(color, color, Color.white, Color.white) : new VertexGradient(Color.white, Color.white, Color.white, Color.white);
        
        BaseManager.Instance.ReturnToPool(2.0f, gameObject, "HitText");
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.x, target.y + upRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos);
        if (upRange <= 0.3f)
        {
            upRange += Time.deltaTime;
        }
    }

    private void ReturnText()
    {
         
    }
}
