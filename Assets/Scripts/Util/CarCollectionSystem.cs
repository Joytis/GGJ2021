using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarCollectionSystem : MonoBehaviour
{
    public struct FriendMap
    {
        public GameObject friend;
    }

    [SerializeField] List<FriendMap> _friends = default;


    public void CollectFriend(Collision collision)
    {
        var friendMap = _friends.Find(x => x.friend == collision.gameObject);
    }

    IEnumerator DoEndGame()
    {
        yield return null;
    }

    IEnumerator DoFriendExplosion(GameObject obj)
    {
        yield return null;
    }
}