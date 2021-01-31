using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using System;
using Cinemachine;

public enum GameState { PLAYING, WON, LOST }

public delegate void OnStateChangeHandler();

public class GameStateManager : MonoBehaviour
{
    // NOTE(clark): I usually use a serrialized dictionarly, but didn't want to import it. 
    [Serializable]
    public class FriendMap
    {
        public FriendBehavior friend;
    }

    [SerializeField] List<FriendMap> _friends = default;
    [SerializeField] GameObject _explosionPrefab = default;
    [SerializeField] PlayableDirector _youWonScreen = default;
    [SerializeField] ActiveRagdoll.GripModule _playerGripModule = default;
    [SerializeField] CinemachineImpulseSource _impulse = default;
    [SerializeField] AudioSource _gottemSound = default;
    [SerializeField] VolumePulseOnEvent _pulser = default;
    
    private static GameStateManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState state { get; private set; } = GameState.PLAYING;

    // public int totalFriends = 3;
    // public int friendsFound { get; private set; } = 0;

    // NOTE(clark): collection moved here. Instance superfluous. 
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameStateManager>();
            }
            return instance;
        }
    }

    IEnumerator DoFriendExplosion(FriendMap friend)
    {
        _gottemSound.Play();
        yield return new WaitForSeconds(1f);

        // Blow up friend and make some particles. 
        var friendTransform = friend.friend.PhysicalTransform;
        var explosion = Instantiate(_explosionPrefab, friendTransform.position, friendTransform.rotation);
        // Blow up the explosion GO after a couple seconds
        Destroy(explosion, 6.0f);
        // blow up the friend. 
        // Ensure that a player has ungripped a friend before blowing them up. 
        _playerGripModule.ForceUngrip();
        Destroy(friend.friend.gameObject);

        // Shake the screen. Play a sound. Do some other stuff. 
        _impulse.GenerateImpulse();
        _pulser.Pulse();
    }

    IEnumerator DoEndGameSequence()
    {
        yield return new WaitForSeconds(1.0f);
        _youWonScreen.Play();
    }

    public void FriendFound(FriendBehavior friend)
    {
        // Just throw exceptions if we can't find it. 
        var foundFriend = _friends.Find(x => x.friend == friend);
        // Sometimes this is called twice. Handle that. Yikes.
        if(foundFriend == null) return;
        
        // Do UI code and sequence here. 
        _friends.Remove(foundFriend);
        StartCoroutine(DoFriendExplosion(foundFriend));
        // Remove friend from tracking. 

        // friendsFound += 1;
        if (_friends.Count == 0)
        {
            // NOTE(clark): This state code is way more clean, but unfortunately I've got a timeline that quits the game for 
            //              temporal and timing reasons. What you have is more in-line with what a game state machine should look like!!
            //              I'm moving. So fast right now. 
            // state = GameState.WON;
            StartCoroutine(DoEndGameSequence());
        }
    }

    // NOTE(clark): Our physics is a mess, so we have to do jank shit. 
    public void CollectFriend(Collider other)
    {
        var friend = other.GetComponentInParent<FriendBehavior>();
        if(friend == null) throw new InvalidOperationException();
        FriendFound(friend);
    }

    // NOTE(clark): Currently sitting on Ex game object. What you have is a much more clean way to do it. 
    // public void TouchedEx()
    // {
    //   if (state == GameState.PLAYING)
    //   {
    //     state = GameState.LOST;
    //     OnStateChange();
    //   }
    // }
}