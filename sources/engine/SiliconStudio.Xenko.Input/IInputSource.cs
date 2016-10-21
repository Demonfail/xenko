﻿// Copyright (c) 2016 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using SiliconStudio.Xenko.Games;

namespace SiliconStudio.Xenko.Input
{
    /// <summary>
    /// An abstraction for a platform specific mechanism that provides input in the form of one of multiple <see cref="IInputDevice"/>(s). 
    /// An input source is responsible for cleaning up it's own devices at cleanup
    /// </summary>
    public interface IInputSource : IDisposable
    {
        /// <summary>
        /// Initializes the input source
        /// </summary>
        /// <param name="inputManager">The <see cref="InputManager"/> initializing this source</param>
        void Initialize(InputManager inputManager);

        /// <summary>
        /// Used to check if this input manager should be used for the current GameContext state
        /// </summary>
        /// <param name="gameContext">The game context</param>
        /// <returns><c>true</c> if this input source can be used with the current GameContext</returns>
        bool IsEnabled(GameContext gameContext);

        /// <summary>
        /// Allows the source to take it's time to search for new devices, the source may call <see cref="OnInputDeviceAdded"/> or <see cref="OnInputDeviceRemoved"/> during this
        /// </summary>
        void Scan();

        /// <summary>
        /// Update the input source and possible input devices, the source may call <see cref="OnInputDeviceAdded"/> or <see cref="OnInputDeviceRemoved"/> during this
        /// </summary>
        void Update();

        /// <summary>
        /// Raised when an input device is added by this source
        /// </summary>
        EventHandler<IInputDevice> OnInputDeviceAdded { get; set; }

        /// <summary>
        /// Raised when an input device is removed by this source
        /// </summary>
        EventHandler<IInputDevice> OnInputDeviceRemoved { get; set; }

        /// <summary>
        /// All the input devices currently proviced by this source
        /// </summary>
        IReadOnlyList<IInputDevice> InputDevices { get; }
    }
}