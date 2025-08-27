using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;

public class RespawnManager : MonoSingleton<RespawnManager>
{
    public Vector2 CurrentSpawnPoint { get; private set; }

    public void SetSpawnPoint(Vector2 newPoint)
    {
        CurrentSpawnPoint = newPoint;
    }

    public void SetSpawnPoint()
    {
        var player = FindFirstObjectByType<PlayerHarmable>();
        CurrentSpawnPoint = player.transform.position;
    }

    public void RespawnPlayer()
    {
        
        Timing.RunCoroutine(RespawnPlayerCoroutine(), Segment.RealtimeUpdate);
    }

    private IEnumerator<float> RespawnPlayerCoroutine()
    {
        Time.timeScale = 0f;
        CameraShake.Instance.OnShake(0.3f, 1f);
        yield return Timing.WaitForSeconds(0.3f);
        const float dur = 0.25f;
        FullScreenFade.Instance.FadeIn(dur);
        yield return Timing.WaitForSeconds(dur);
        HandleRespawn();
        Time.timeScale = 1f;
        FullScreenFade.Instance.FadeOut(dur);
        
    }

    private void HandleRespawn()
    {
        var player = FindFirstObjectByType<PlayerHarmable>();
        player.transform.position = CurrentSpawnPoint;

        var resetables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IResetable>();
        foreach(var x in resetables) x?.OnReset();
    }
}
