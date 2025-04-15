using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using static Unity.VisualScripting.Member;


namespace NodeCanvas.Tasks.Actions {

    public class PlayAudioClipTask : ActionTask
    {
        public AudioSource source;
        public AudioClip clip;
        [SliderField(0, 1)]
        public float volume = 0.5f;
        public Transform transform;

        protected override void OnExecute()
        {
            source.volume = volume;
            AudioSource.PlayClipAtPoint(clip, transform.position);
            EndAction(true);
        }
    }
}