using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{

        public Camera cam;
        [SerializeField]
        private Vector2 parallaxEffect;
        private Transform cameraTransform;
        private Vector3 lastCameraPosition;
        private float textureUnitSizeX;

        void Start()
        {
            cameraTransform = cam.transform;
            lastCameraPosition = cameraTransform.position;
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }

        void LateUpdate()
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y);
            lastCameraPosition = cameraTransform.position;


        }
    
}
