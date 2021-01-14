﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Linq.Expressions;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.SurfacePulse
{
    /// <summary>
    /// Script for triggering the pulse shader effect on hand mesh.
    /// </summary>
    [AddComponentMenu("Scripts/MRTK/SDK/HandPulseLogic")]
    [RequireComponent(typeof(SurfacePulse))]
    public class HandPulseLogic : MonoBehaviour, IMixedRealityPointerHandler
    {
        [SerializeField]
        [Tooltip("The SurfacePulse component. The SurfacePulse component modifies properties of the pulse shader.")]
        private SurfacePulse pulse;

        /// <summary>
        /// The SurfacePulse component.  The SurfacePulse component modifies
        /// properties of the pulse shader. 
        /// </summary>
        public SurfacePulse Pulse
        {
            get => pulse;
            set => pulse = value;
        }

        [SerializeField]
        [Tooltip("Trigger the pulse shader visual if the palm of the left or right hand is facing the camera.")]
        private bool pulseOnLookAtPalms = false;

        /// <summary>
        /// Trigger the pulse shader visual if the palm of the left or right hand is facing the camera.
        /// </summary>
        public bool PulseOnLookAtPalms
        {
            get => pulseOnLookAtPalms;
            set => pulseOnLookAtPalms = value;
        }

        [SerializeField]
        [Tooltip("Trigger the pulse shader visual if the hand is in the pinch/select gesture. ")]
        private bool pulseOnPinch = false;

        /// <summary>
        /// Trigger the pulse shader visual if the hand is in the pinch/select gesture. 
        /// </summary>
        public bool PulseOnPinch
        {
            get => pulseOnPinch;
            set => pulseOnPinch = value;
        }

        [SerializeField]
        [Tooltip("")]
        private bool pulseOnHandDetected = true;

        /// <summary>
        /// Trigger the pulse shader visual once if the left or right hand enters the frame and is tracked.
        /// </summary>
        public bool PulseOnHandDetected
        {
            get => pulseOnHandDetected;
            set => pulseOnHandDetected = value;
        }

        [SerializeField]
        [Tooltip("The length of time required for the palm to face the camera before the pulse shader visual is triggered. ")]
        private float palmFacingTime = 0.25f;

        /// <summary>
        /// The length of time required for the palm to face the camera before the pulse shader visual is triggered. 
        /// </summary>
        public float PalmFacingTime
        {
            get => palmFacingTime;
            set => palmFacingTime = value;
        }

        [SerializeField]
        [Tooltip("The local origin of the palms.")]
        private Vector3 pulseOriginPalms = new Vector3(0.5f, 0.5f, 0);

        /// <summary>
        /// The local origin of the palms.
        /// </summary>
        public Vector3 PulseOriginPalms
        {
            get => pulseOriginPalms;
            set => pulseOriginPalms = value;
        }

        [SerializeField]
        [Tooltip("The local origin of the pulse shader visual for the finger tips. ")]
        private Vector3 pulseOriginFingertips = new Vector3(0, 1f, 0);

        /// <summary>
        /// The local origin of the pulse shader visual for the finger tips. 
        /// </summary>
        public Vector3 PulseOriginFingertips
        {
            get => pulseOriginFingertips;
            set => pulseOriginFingertips = value;
        }

        private bool pulseTriggeredLeft = false;
        private bool pulseTriggeredRight = false;

        private float palmFacingTimer = 0;

        private void Start()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);

            if (Pulse == null)
            {
                Pulse = GetComponent<SurfacePulse>();
            }
        }

        private void OnDestroy()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
        }

        void Update()
        {
            if (PulseOnLookAtPalms)
            {
                if (IsAPalmFacingCamera())
                {
                    if (palmFacingTimer >= 0)
                    {
                        palmFacingTimer += Time.deltaTime;
                        if (palmFacingTimer > PalmFacingTime)
                        {
                            PulsePalms();
                            palmFacingTimer = -1;
                        }
                    }
                }
                else
                {
                    palmFacingTimer = 0;
                }
            }

            if (PulseOnHandDetected)
            {
                // If joints are detected then an articulated hand is present
                bool isLeftHandDetected = HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out MixedRealityPose leftPose);
                bool isRightHandDetected = HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out MixedRealityPose rightPose);

                TriggerPulseOnHandDetected(isLeftHandDetected, isRightHandDetected);
            }
        }

        private void TriggerPulseOnHandDetected(bool isLeftHandDetected, bool isRightHandDetected)
        {
            // Left Hand
            if (isLeftHandDetected && !pulseTriggeredLeft)
            {
                pulseTriggeredLeft = true;
                PulsePalms();
            }
            else if (!isLeftHandDetected && pulseTriggeredLeft)
            {
                pulseTriggeredLeft = false;
            }

            // Right Hand
            if (isRightHandDetected && !pulseTriggeredRight)
            {
                pulseTriggeredRight = true;
                PulsePalms();
            }
            else if (!isRightHandDetected && pulseTriggeredRight)
            {
                pulseTriggeredRight = false;
            }
        }

        /// <summary>
        /// Set the local origin of the pulse shader visual to the palms and trigger the animation. 
        /// </summary>
        public void PulsePalms()
        {
            Pulse.SetLocalOrigin(PulseOriginPalms);
            Pulse.PulseOnce();
        }

        /// <summary>
        /// Set the local origin of the pulse shader visual to the finger tips and trigger the animation. 
        /// </summary>
        public void PulseFingerTips()
        {
            Pulse.SetLocalOrigin(PulseOriginFingertips);
            Pulse.PulseOnce();
        }

        // Check if the palm is facing the camera
        private static bool IsAPalmFacingCamera()
        {
            foreach (IMixedRealityController c in CoreServices.InputSystem.DetectedControllers)
            {
                if (c.ControllerHandedness.IsMatch(Handedness.Both))
                {
                    MixedRealityPose palmPose;
                    var jointedHand = c as IMixedRealityHand;

                    if ((jointedHand != null) && jointedHand.TryGetJoint(TrackedHandJoint.Palm, out palmPose))
                    {
                        if (Vector3.Dot(palmPose.Up, CameraCache.Main.transform.forward) > 0.0f)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #region Pointer Handler

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            if (PulseOnPinch)
            {
                PulseFingerTips();
            }
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
        public void OnPointerUp(MixedRealityPointerEventData eventData) { }
        public void OnPointerClicked(MixedRealityPointerEventData eventData) { }

        #endregion

    }
}
