using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MessageManager : MonoBehaviour
    {
        public GameObject messagePrefab;
        public GameObject permanentMessagePrefab;
        
        private Dictionary<GameObject, Action<Message>> _uninitializedMessages = new();
        private Dictionary<GameObject, string> _uninitializedPermanentMessages = new();

        private Dictionary<Vector2Int, PermanentMessage> _permanentMessages = new();

        public void InvokeMessage(Vector2 position, string message, bool positive)
        {
            _uninitializedMessages.Add(
                Instantiate(messagePrefab, position, Quaternion.identity),
                m => m.SetText(message, positive));
        }

        public void InvokeMessage(Vector2 position, string message)
        {
            _uninitializedMessages.Add(
                Instantiate(messagePrefab, position, Quaternion.identity),
                m => m.SetText(message));
        }

        public void InvokePermanentMessage(Vector2Int position, string message)
        {
            if (_permanentMessages.TryGetValue(position, out PermanentMessage m))
            {
                m.Text = message;
            }
            else
            {
                _uninitializedPermanentMessages.Add(
                    Instantiate(permanentMessagePrefab, new Vector3(position.x, position.y), Quaternion.identity),
                    message);
            }
        }

        public void HidePermanentMessage(Vector2Int pos)
        {
            if (_permanentMessages.TryGetValue(pos, out PermanentMessage message))
            {
                message.FadeIn = false;
            }
        }

        private void Update()
        {
            List<GameObject> initialized = new();
            foreach (KeyValuePair<GameObject, Action<Message>> entry in _uninitializedMessages)
            {
                Debug.Log("Trying to get Message component!!!");
                Message messageComponent = entry.Key.GetComponent<Message>();
                if (messageComponent != null)
                {
                    entry.Value.Invoke(messageComponent);
                    initialized.Add(entry.Key);
                }
            }
            
            foreach (GameObject g in initialized)
            {
                _uninitializedMessages.Remove(g);
            }
            initialized.Clear();

            foreach (KeyValuePair<GameObject, string> entry in _uninitializedPermanentMessages)
            {
                Debug.Log("Trying to get Message component!!!");
                PermanentMessage messageComponent = entry.Key.GetComponent<PermanentMessage>();
                if (messageComponent != null)
                {
                    messageComponent.Text = entry.Value;
                    messageComponent.OnDestroyPeramanentMessage += OnDestroyPermanentMessage;
                    _permanentMessages.Add(messageComponent.StartPos, messageComponent);
                    initialized.Add(entry.Key);
                }
            }

            foreach (GameObject g in initialized)
            {
                _uninitializedPermanentMessages.Remove(g);
            }
        }

        private void OnDestroyPermanentMessage(PermanentMessage message)
        {
            message.OnDestroyPeramanentMessage -= OnDestroyPermanentMessage;
            _permanentMessages.Remove(message.StartPos);
        }
    }
}