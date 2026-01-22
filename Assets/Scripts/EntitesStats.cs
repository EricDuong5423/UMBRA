using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityStats", menuName = "Stats/EntitesStats")]
public class EntitesStats : ScriptableObject
{
    [Header("Thông tin chung")] 
    [SerializeField] private string name;
    public string Name => name;
    [SerializeField] private float maxEmbers;
    public float MaxEmbers => maxEmbers;
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    
    [Header("Visuals")]
    [SerializeField] private Color silhouetteColor = Color.black;
    public Color SilhouetteColor => silhouetteColor;
}
