using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DigitalRubyShared
{
    public class TouchControls : MonoBehaviour
    {
        //TAP VARIABLES
        private TapGestureRecognizer tap;
        private SwipeGestureRecognizer swipe;
        private LongPressGestureRecognizer longPress;
        //GAME VARIABLES
        public Camera cam;
        //===============================================================
        //HELPFUL STUFF
        private void DebugText(string text, params object[] format)
        {
            //bottomLabel.text = string.Format(text, format);
            Debug.Log(string.Format(text, format));
        }
        //===============================================================
        //INITIALIZATION
        private void Start()
        {
            CreateTap();
            CreateSwipe();
            CreateLongPress();
        }
        //GESTURE CREATION
        private void CreateTap()
        {
            tap = new TapGestureRecognizer();
            tap.StateUpdated += TapHandle;
            FingersScript.Instance.AddGesture(tap);
        }
        
        private void CreateSwipe()
        {
            swipe = new SwipeGestureRecognizer();
            swipe.Direction = SwipeGestureRecognizerDirection.Right;
            swipe.StateUpdated += SwipeHandle;
            FingersScript.Instance.AddGesture(swipe);
        }

        private void CreateLongPress()
        {
            longPress = new LongPressGestureRecognizer();
            longPress.MaximumNumberOfTouchesToTrack = 1;
            longPress.StateUpdated += LongPressHandle;
            FingersScript.Instance.AddGesture(longPress);
        }
        //===============================================================
        //GESTURE IMPLIMENTATION
        private void TapHandle(DigitalRubyShared.GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
                {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(new Vector3(gesture.FocusX, gesture.FocusY, 0.0f));
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Interactable")
                        {
                            hit.collider.gameObject.GetComponent<InteractableObstacle>().Anim();
                            return;
                        }
                    }
                    
                }
            }
        }
        private void SwipeHandle(DigitalRubyShared.GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                PlayerScript.Instance.Dash();

            }
        }
        private void LongPressHandle(DigitalRubyShared.GestureRecognizer gesture)
        {
            if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
            {
                if (gesture.State == GestureRecognizerState.Began)
                {
                    //DebugText("Long press began: {0}, {1}", gesture.FocusX, gesture.FocusY);
                    //BeginDrag(gesture.FocusX, gesture.FocusY);
                }
                else if (gesture.State == GestureRecognizerState.Executing)
                {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(new Vector3(gesture.FocusX, gesture.FocusY, 0.0f));
                    if (Physics.Raycast(ray, out hit))
                        PlayerScript.Instance.Walk(hit.point);
                }
                else if (gesture.State == GestureRecognizerState.Ended)
                {
                    //DebugText("Long press end: {0}, {1}, delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);
                    //EndDrag(longPressGesture.VelocityX, longPressGesture.VelocityY);
                }
            }
        }
        //===============================================================
    }
}
