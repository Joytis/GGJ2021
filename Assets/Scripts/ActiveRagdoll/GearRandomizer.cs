using UnityEngine;
using System.Linq;

/// <summary> Default behaviour of an Active Ragdoll </summary>
public class GearRandomizer : MonoBehaviour 
{
    [SerializeField] GameObject[] _headGear = default;
    [SerializeField] Vector2Int _gearRange = default;

    void Start()
    {
        var randomGear = _headGear
            .OrderBy(x => Random.value)
            .Take(Random.Range(_gearRange.x, _gearRange.y));

        foreach(var prefab in randomGear)
        {
            Instantiate(prefab, transform);
        }
    }
}
