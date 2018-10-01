﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Core.Attributes;
using Microsoft.MixedReality.Toolkit.Core.Definitions.BoundarySystem;
using Microsoft.MixedReality.Toolkit.Core.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Core.Definitions.Diagnostics;
using Microsoft.MixedReality.Toolkit.Core.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Definitions.SpatialAwarenessSystem;
using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using Microsoft.MixedReality.Toolkit.Core.Interfaces;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.BoundarySystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.Diagnostics;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.InputSystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.SpatialAwarenessSystem;
using Microsoft.MixedReality.Toolkit.Core.Interfaces.TeleportSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Core.Definitions
{
    /// <summary>
    /// Configuration profile settings for the Mixed Reality Toolkit.
    /// </summary>
    [CreateAssetMenu(menuName = "Mixed Reality Toolkit/Mixed Reality Configuration Profile", fileName = "MixedRealityConfigurationProfile", order = (int)CreateProfileMenuItemIndices.Configuration)]
    public class MixedRealityConfigurationProfile : BaseMixedRealityProfile, ISerializationCallbackReceiver
    {
        #region Manager Registry properties

        [SerializeField]
        private SystemType[] initialManagerTypes = null;

        /// <summary>
        /// Dictionary list of active managers used by the Mixed Reality Manager at runtime
        /// </summary>
        public Dictionary<Type, IMixedRealityManager> ActiveManagers { get; } = new Dictionary<Type, IMixedRealityManager>();

        #endregion Manager Registry properties

        #region Mixed Reality Manager configurable properties

        [SerializeField]
        [Tooltip("The scale of the Mixed Reality experience.")]
        private ExperienceScale targetExperienceScale = ExperienceScale.Room;

        /// <summary>
        /// The desired the scale of the experience.
        /// </summary>
        public ExperienceScale TargetExperienceScale
        {
            get { return targetExperienceScale; }
            set { targetExperienceScale = value; }
        }

        [SerializeField]
        [Tooltip("Enable the Camera Profile on Startup.")]
        private bool enableCameraProfile = false;

        /// <summary>
        /// Enable and configure the Camera Profile for the Mixed Reality Toolkit
        /// </summary>
        public bool IsCameraProfileEnabled
        {
            get
            {
                return CameraProfile != null && enableCameraProfile;
            }

            private set { enableCameraProfile = value; }
        }

        [SerializeField]
        [Tooltip("Camera profile.")]
        private MixedRealityCameraProfile cameraProfile;

        /// <summary>
        /// Profile for customizing your camera and quality settings based on if your 
        /// head mounted display (HMD) is a transparent device or an occluded device.
        /// </summary>
        public MixedRealityCameraProfile CameraProfile
        {
            get { return cameraProfile; }
            private set { cameraProfile = value; }
        }

        [SerializeField]
        [Tooltip("Enable the Input System on Startup.")]
        private bool enableInputSystem = false;

        /// <summary>
        /// Enable and configure the Input System component for the Mixed Reality Toolkit
        /// </summary>
        public bool IsInputSystemEnabled
        {
            get
            {
                return inputSystemProfile != null && inputSystemType != null && inputSystemType.Type != null && enableInputSystem;
            }
            private set { enableInputSystem = value; }
        }

        [SerializeField]
        [Tooltip("Input System profile for setting wiring up events and actions to input devices.")]
        private MixedRealityInputSystemProfile inputSystemProfile;

        /// <summary>
        /// Input System profile for setting wiring up events and actions to input devices.
        /// </summary>
        public MixedRealityInputSystemProfile InputSystemProfile
        {
            get { return inputSystemProfile; }
            private set { inputSystemProfile = value; }
        }

        [SerializeField]
        [Tooltip("Input System Class to instantiate at runtime.")]
        [Implements(typeof(IMixedRealityInputSystem), TypeGrouping.ByNamespaceFlat)]
        private SystemType inputSystemType;

        /// <summary>
        /// Input System Script File to instantiate at runtime.
        /// </summary>
        public SystemType InputSystemType
        {
            get { return inputSystemType; }
            private set { inputSystemType = value; }
        }

        [SerializeField]
        [Tooltip("Enable the Boundary on Startup")]
        private bool enableBoundarySystem = false;

        /// <summary>
        /// Enable and configure the boundary system.
        /// </summary>
        public bool IsBoundarySystemEnabled
        {
            get { return boundarySystemType != null && boundarySystemType.Type != null && enableBoundarySystem; }
            private set { enableBoundarySystem = value; }
        }

        [SerializeField]
        [Tooltip("Boundary System Class to instantiate at runtime.")]
        [Implements(typeof(IMixedRealityBoundarySystem), TypeGrouping.ByNamespaceFlat)]
        private SystemType boundarySystemType;

        /// <summary>
        /// Boundary System Script File to instantiate at runtime.
        /// </summary>
        public SystemType BoundarySystemSystemType
        {
            get { return boundarySystemType; }
            private set { boundarySystemType = value; }
        }

        [SerializeField]
        [Tooltip("Profile for wiring up boundary visualization assets.")]
        private MixedRealityBoundaryVisualizationProfile boundaryVisualizationProfile;

        /// <summary>
        /// Active profile for controller mapping configuration
        /// </summary>
        public MixedRealityBoundaryVisualizationProfile BoundaryVisualizationProfile
        {
            get { return boundaryVisualizationProfile; }
            private set { boundaryVisualizationProfile = value; }
        }

        [SerializeField]
        [Tooltip("Enable the Teleport System on Startup")]
        private bool enableTeleportSystem = false;

        /// <summary>
        /// Enable and configure the boundary system.
        /// </summary>
        public bool IsTeleportSystemEnabled
        {
            get { return teleportSystemType != null && teleportSystemType.Type != null && enableTeleportSystem; }
            private set { enableTeleportSystem = value; }
        }

        [SerializeField]
        [Tooltip("Boundary System Class to instantiate at runtime.")]
        [Implements(typeof(IMixedRealityTeleportSystem), TypeGrouping.ByNamespaceFlat)]
        private SystemType teleportSystemType;

        /// <summary>
        /// Teleport System Script File to instantiate at runtime.
        /// </summary>
        public SystemType TeleportSystemSystemType
        {
            get { return teleportSystemType; }
            private set { teleportSystemType = value; }
        }

        [SerializeField]
        [Tooltip("Enable the Spatial Awareness system on Startup")]
        private bool enableSpatialAwarenessSystem = false;

        /// <summary>
        /// Enable and configure the spatial awareness system.
        /// </summary>
        public bool IsSpatialAwarenessSystemEnabled
        {
            get { return spatialAwarenessSystemType != null && spatialAwarenessSystemType.Type != null && enableSpatialAwarenessSystem; }
            private set { enableSpatialAwarenessSystem = value; }
        }

        [SerializeField]
        [Tooltip("Spatial Awareness System Class to instantiate at runtime.")]
        [Implements(typeof(IMixedRealitySpatialAwarenessSystem), TypeGrouping.ByNamespaceFlat)]
        private SystemType spatialAwarenessSystemType;

        /// <summary>
        /// Boundary System Script File to instantiate at runtime.
        /// </summary>
        public SystemType SpatialAwarenessSystemSystemType
        {
            get { return spatialAwarenessSystemType; }
            private set { spatialAwarenessSystemType = value; }
        }

        [SerializeField]
        [Tooltip("Profile for configuring the Spatial Awareness system.")]
        private MixedRealitySpatialAwarenessProfile spatialAwarenessProfile;

        /// <summary>
        /// Active profile for spatial awareness configuration
        /// </summary>
        public MixedRealitySpatialAwarenessProfile SpatialAwarenessProfile
        {
            get { return spatialAwarenessProfile; }
            private set { spatialAwarenessProfile = value; }
        }

        [SerializeField]
        [Tooltip("Profile for wiring up diagnostic assets.")]
        private MixedRealityDiagnosticsProfile diagnosticsSystemProfile;

        /// <summary>
        /// Active profile for diagnostic configuration
        /// </summary>
        public MixedRealityDiagnosticsProfile DiagnosticsSystemProfile
        {
            get { return diagnosticsSystemProfile; }
            private set { diagnosticsSystemProfile = value; }
        }

        [SerializeField]
        [Tooltip("Enable diagnostic system")]
        private bool enableDiagnosticsSystem = false;

        public bool IsDiagnosticsSystemEnabled
        {
            get { return enableDiagnosticsSystem && DiagnosticsSystemSystemType?.Type != null; }
            private set { enableDiagnosticsSystem = value; }
        }

        [SerializeField]
        [Tooltip("Diagnostics System Class to instantiate at runtime.")]
        [Implements(typeof(IMixedRealityDiagnosticsManager), TypeGrouping.ByNamespaceFlat)]
        private SystemType diagnosticsSystemType;

        /// <summary>
        /// Diagnostics Manager Script File to instantiate at runtime
        /// </summary>
        public SystemType DiagnosticsSystemSystemType
        {
            get { return diagnosticsSystemType; }
            private set { diagnosticsSystemType = value; }
        }

        [SerializeField]
        [Tooltip("All the additional non-required systems, features, and managers registered with the Mixed Reality Manager.")]
        private MixedRealityRegisteredComponentsProfile registeredComponentsProfile;

        /// <summary>
        /// All the additional non-required systems, features, and managers registered with the Mixed Reality Manager.
        /// </summary>
        public MixedRealityRegisteredComponentsProfile RegisteredComponentsProfile
        {
            get { return registeredComponentsProfile; }
            private set { registeredComponentsProfile = value; }
        }

        #endregion Mixed Reality Manager configurable properties

        #region ISerializationCallbackReceiver Implementation

        /// <inheritdoc />
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var count = ActiveManagers.Count;
            initialManagerTypes = new SystemType[count];

            foreach (var manager in ActiveManagers)
            {
                --count;
                initialManagerTypes[count] = new SystemType(manager.Value.GetType());
            }
        }

        /// <inheritdoc />
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (ActiveManagers.Count == 0)
            {
                for (int i = 0; i < initialManagerTypes?.Length; i++)
                {
                    ActiveManagers.Add(initialManagerTypes[i], Activator.CreateInstance(initialManagerTypes[i]) as IMixedRealityManager);
                }
            }
        }
    }
    #endregion  ISerializationCallbackReceiver Implementation
}
