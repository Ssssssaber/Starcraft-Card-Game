using System;
using UnityEngine;
using UnityEngine.Events;


    public class AttackCursor: MonoBehaviour
    {
        public GameObject Canvas;
        // public static UnityEvent<bool> OnCursorToggle = new UnityEvent<bool>();
        BoardAwareness _awareness = BoardAwareness.Instance;
        public bool Displayed
        {
            get
            {
                return _displayed;
            }
            set
            {
                _displayed = value;
                gameObject.SetActive(value);
            }
        }

        private bool _displayed = false;
        
        private void Start()
        {
            _awareness = BoardAwareness.Instance;
            Canvas = _awareness.MainCanvas.gameObject;
            // OnCursorToggle.AddListener(ToggleCursor);
        }

        private void Update()
        {
            if (Displayed)
            {
                // transform.position = Input.mousePosition;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                transform.SetParent(Canvas.transform);
            }
        }

        public void ToggleCursor(bool canAttack)
        {
            Displayed = canAttack;
        }
    }