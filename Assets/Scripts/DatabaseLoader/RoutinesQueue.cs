using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


    public class RoutinesQueue : MonoBehaviour
    {
        public static Queue<IEnumerator> coroutineQueue = new();
        public bool coroutinesRunning = false;

        private void Update()
        {
            if (!coroutinesRunning)
            {
                StartCoroutine(Consecutive(coroutineQueue, (isRunning) => coroutinesRunning = isRunning));
            }
        }

        private IEnumerator Consecutive(Queue<IEnumerator> coroutines, Action<bool> SetIsRunning)
        {
            SetIsRunning(true);

            while (coroutines.TryDequeue(out IEnumerator coroutine))
            {
                yield return StartCoroutine(coroutine);
            }

            SetIsRunning(false);
        }
    }