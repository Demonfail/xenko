﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Paradox.Graphics;
using SiliconStudio.Paradox.Shaders.Compiler;

namespace SiliconStudio.Paradox.Effects
{
    /// <summary>
    /// Performs render pipeline transformations attached to a specific <see cref="RenderPass"/>.
    /// </summary>
    public abstract class Renderer : ComponentBase
    {
        private readonly IGraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        protected Renderer(IServiceRegistry services)
        {
            if (services == null) throw new ArgumentNullException("services");

            Enabled = true;
            Services = services;
            EffectSystem = services.GetSafeServiceAs<EffectSystem>();
            graphicsDeviceService = services.GetSafeServiceAs<IGraphicsDeviceService>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Renderer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        public IServiceRegistry Services { get; private set; }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return (graphicsDeviceService != null) ? graphicsDeviceService.GraphicsDevice : null;
            }
        }

        /// <summary>
        /// Gets the effect system.
        /// </summary>
        /// <value>The effect system.</value>
        public EffectSystem EffectSystem { get; private set; }
        
        /// <summary>
        /// Gets or sets the name of the debug.
        /// </summary>
        /// <value>The name of the debug.</value>
        public string DebugName { get; set; }


        protected virtual void BeginRendering(RenderContext context)
        {
            if (DebugName != null)
            {
                context.GraphicsDevice.BeginProfile(Color.Green, DebugName);
            }
        }

        protected virtual void EndRendering(RenderContext context)
        {
            if (DebugName != null)
            {
                context.GraphicsDevice.EndProfile();
            }
        }

        protected virtual void OnRendering(RenderContext context)
        {
        }

        public void Draw(RenderContext context)
        {
            if (Enabled)
            {
                BeginRendering(context);
                OnRendering(context);
                EndRendering(context);
            }
        }

        protected CompilerParameters GetDefaultCompilerParameters()
        {
            var compilerParameters = new CompilerParameters();
            compilerParameters.Set(CompilerParameters.GraphicsProfileKey, GraphicsDevice.Features.Profile);
            return compilerParameters;
        }
    }
}