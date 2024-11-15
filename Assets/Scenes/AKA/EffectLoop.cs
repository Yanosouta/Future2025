using UnityEngine;
using Effekseer;

public class EffectLoop : MonoBehaviour
{
    // Effekseer Emitter をアタッチした GameObject をアサイン
    public EffekseerEmitter effect1;
    public EffekseerEmitter effect2;

    // 再生間隔（秒）
    public float interval = 2.0f;

    private float timer = 0.0f;
    private bool isPlayingEffect1 = true;

    void Update()
    {
        timer += Time.deltaTime;

        // 一定時間ごとにエフェクトを切り替える
        if (timer >= interval)
        {
            timer = 0.0f;

            if (isPlayingEffect1)
            {
                PlayEffect(effect2);
                //StopEffect(effect1);
            }
            else
            {
                PlayEffect(effect1);
                //StopEffect(effect2);
            }

            isPlayingEffect1 = !isPlayingEffect1;
        }
    }

    // エフェクトを再生
    void PlayEffect(EffekseerEmitter emitter)
    {
        if (emitter != null)
        {
            emitter.Play();
        }
    }

    // エフェクトを停止
    void StopEffect(EffekseerEmitter emitter)
    {
        if (emitter != null)
        {
            emitter.Stop();
        }
    }
}
