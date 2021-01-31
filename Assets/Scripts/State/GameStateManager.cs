using UnityEngine;
using System.Collections;

public enum GameState { PLAYING, WON, LOST }

public delegate void OnStateChangeHandler();

public class GameStateManager : MonoBehaviour
{
  protected GameStateManager() { }
  private static GameStateManager instance = null;
  public event OnStateChangeHandler OnStateChange;
  public GameState state { get; private set; } = GameState.PLAYING;

  public int totalFriends = 3;
  public int friendsFound { get; private set; } = 0;

  public static GameStateManager Instance
  {
    get
    {
      if (GameStateManager.instance == null)
      {
        GameStateManager.instance = FindObjectOfType(typeof(GameStateManager)) as GameStateManager;
      }
      return GameStateManager.instance;
    }

  }

  void Awake()
  {
    instance = this;
    DontDestroyOnLoad(this);
  }

  public void FriendFound()
  {
    friendsFound += 1;
    if (friendsFound >= totalFriends)
    {
      state = GameState.WON;
      OnStateChange();
    }
  }

  public void TouchedEx()
  {
    if (state == GameState.PLAYING)
    {
      state = GameState.LOST;
      OnStateChange();
    }
  }

  public void OnApplicationQuit()
  {
    GameStateManager.instance = null;
  }

}