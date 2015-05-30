﻿#region Copyrights
/*
Gibbo2D License - Version 1.0
Copyright (c) 2013 - Gibbo2D Team
Founders Joao Alves <joao.cpp.sca@gmail.com> & Luis Fernandes <luisapidcloud@hotmail.com>

Permission is granted to use this software and associated documentation files (the "Software") free of charge, 
to any person or company. The code can be used, modified and merged without restrictions, but you cannot sell 
the software itself and parts where this license applies. Still, permission is granted for anyone to sell 
applications made using this software (for example, a game). This software cannot be claimed as your own, 
except for copyright holders. This license notes should also be available on any of the changed or added files.

The software is provided "as is", without warranty of any kind, express or implied, including but not limited to 
the warranties of merchantability, fitness for a particular purpose and non-infrigement. In no event shall the 
authors or copyright holders be liable for any claim, damages or other liability.

The license applies to all versions of the software, both newer and older than the one listed, unless a newer copy 
of the license is available, in which case the most recent copy of the license supercedes all others.

*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Controls;

namespace OpenTK
{
    /// <summary>
    /// OpenGL-aware WinForms control.
    /// The WinForms designer will always call the default constructor.
    /// Inherit from this class and call one of its specialized constructors
    /// to enable antialiasing or custom <see cref="GraphicsMode"/>s.
    /// </summary>
    public partial class GLControl : System.Windows.Forms.UserControl
    {
        IGraphicsContext context;
        IGLControl implementation;
        GraphicsMode format;
        int major, minor;
        GraphicsContextFlags flags;
        bool? initial_vsync_value;
        // Indicates that OnResize was called before OnHandleCreated.
        // To avoid issues with missing OpenGL contexts, we suppress
        // the premature Resize event and raise it as soon as the handle
        // is ready.
        bool resize_event_suppressed;
        // Indicates whether the control is in design mode. Due to issues
        // wiith the DesignMode property and nested controls,we need to
        // evaluate this in the constructor.
        readonly bool design_mode;

        #region --- Constructors ---

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GLControl()
            : this(GraphicsMode.Default)
        { }

        /// <summary>
        /// Constructs a new instance with the specified GraphicsMode.
        /// </summary>
        /// <param name="mode">The OpenTK.Graphics.GraphicsMode of the control.</param>
        public GLControl(GraphicsMode mode)
            : this(mode, 1, 0, GraphicsContextFlags.Default)
        { }

        /// <summary>
        /// Constructs a new instance with the specified GraphicsMode.
        /// </summary>
        /// <param name="mode">The OpenTK.Graphics.GraphicsMode of the control.</param>
        /// <param name="major">The major version for the OpenGL GraphicsContext.</param>
        /// <param name="minor">The minor version for the OpenGL GraphicsContext.</param>
        /// <param name="flags">The GraphicsContextFlags for the OpenGL GraphicsContext.</param>
        public GLControl(GraphicsMode mode, int major, int minor, GraphicsContextFlags flags)
        {
            if (mode == null)
                throw new ArgumentNullException("mode");
            
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = false;

            this.format = mode;
            this.major = major;
            this.minor = minor;
            this.flags = flags;

            // Note: the DesignMode property may be incorrect when nesting controls.
            // We use LicenseManager.UsageMode as a workaround (this only works in
            // the constructor).
            design_mode =
                DesignMode ||
                LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            InitializeComponent();
        }

        #endregion

        #region --- Private  Methods ---

        IGLControl Implementation
        {
            get
            {
                ValidateState();

                return implementation;
            }
        }

        void ValidateState()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (!IsHandleCreated)
                CreateControl();

            if (implementation == null || context == null || context.IsDisposed)
                RecreateHandle();
        }

        #endregion

        #region --- Protected Methods ---

        /// <summary>Raises the HandleCreated event.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            if (context != null)
                context.Dispose();

            if (implementation != null)
                implementation.WindowInfo.Dispose();

            if (design_mode)
                implementation = new DummyGLControl();
            else
                implementation = new GLControlFactory().CreateGLControl(format, this);

            context = implementation.CreateContext(major, minor, flags);
            MakeCurrent();

            if (!design_mode)
                ((IGraphicsContextInternal)Context).LoadAll();

            // Deferred setting of vsync mode. See VSync property for more information.
            if (initial_vsync_value.HasValue)
            {
                Context.SwapInterval = initial_vsync_value.Value ? 1 : 0;
                initial_vsync_value = null;
            }

            base.OnHandleCreated(e);

            if (resize_event_suppressed)
            {
                OnResize(EventArgs.Empty);
                resize_event_suppressed = false;
            }
        }

        /// <summary>Raises the HandleDestroyed event.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }

            if (implementation != null)
            {
                implementation.WindowInfo.Dispose();
                implementation = null;
            }

            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            ValidateState();

            if (design_mode)
                e.Graphics.Clear(BackColor);

            base.OnPaint(e);
        }

        /// <summary>
        /// Raises the Resize event.
        /// Note: this method may be called before the OpenGL context is ready.
        /// Check that IsHandleCreated is true before using any OpenGL methods.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            // Do not raise OnResize event before the handle and context are created.
            if (!IsHandleCreated)
            {
                resize_event_suppressed = true;
                return;
            }

            if (context != null)
                context.Update(Implementation.WindowInfo);

            base.OnResize(e);
        }

        /// <summary>
        /// Raises the ParentChanged event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (context != null)
                context.Update(Implementation.WindowInfo);

            base.OnParentChanged(e);
        }

        #endregion

        #region --- Public Methods ---

        #region public void SwapBuffers()

        /// <summary>
        /// Swaps the front and back buffers, presenting the rendered scene to the screen.
        /// </summary>
        public void SwapBuffers()
        {
            ValidateState();
            Context.SwapBuffers();
        }

        #endregion

        #region public void MakeCurrent()

        /// <summary>
        /// Makes the underlying this GLControl current in the calling thread.
        /// All OpenGL commands issued are hereafter interpreted by this GLControl.
        /// </summary>
        public void MakeCurrent()
        {
            ValidateState();
            Context.MakeCurrent(Implementation.WindowInfo);
        }

        #endregion

        #region public bool IsIdle

        /// <summary>
        /// Gets a value indicating whether the current thread contains pending system messages.
        /// </summary>
        [Browsable(false)]
        public bool IsIdle
        {
            get
            {
                ValidateState();
                return Implementation.IsIdle;
            }
        }

        #endregion

        #region public IGraphicsContext Context

        /// <summary>
        /// Gets an interface to the underlying GraphicsContext used by this GLControl.
        /// </summary>
        [Browsable(false)]
        public IGraphicsContext Context
        {
            get
            {
                ValidateState();
                return context;
            }
            private set { context = value; }
        }

        #endregion

        #region public float AspectRatio

        /// <summary>
        /// Gets the aspect ratio of this GLControl.
        /// </summary>
        [Description("The aspect ratio of the client area of this GLControl.")]
        public float AspectRatio
        {
            get
            {
                ValidateState();
                return ClientSize.Width / (float)ClientSize.Height;
            }
        }

        #endregion

        #region public bool VSync

        /// <summary>
        /// Gets or sets a value indicating whether vsync is active for this GLControl.
        /// </summary>
        [Description("Indicates whether GLControl updates are synced to the monitor's refresh rate.")]
        public bool VSync
        {
            get
            {
                if (!IsHandleCreated)
                    return false;

                ValidateState();
                return Context.VSync;
            }
            set
            {
                // The winforms designer sets this to false by default which forces control creation.
                // However, events are typically connected after the VSync = false assignment, which
                // can lead to "event xyz is not fired" issues.
                // Work around this issue by deferring VSync mode setting to the HandleCreated event.
                if (!IsHandleCreated)
                {
                    initial_vsync_value = value;
                    return;
                }

                ValidateState();
                Context.VSync = value;
            }
        }

        #endregion

        #region public GraphicsMode GraphicsMode

        /// <summary>
        /// Gets the GraphicsMode of the GraphicsContext attached to this GLControl.
        /// </summary>
        /// <remarks>
        /// To change the GraphicsMode, you must destroy and recreate the GLControl.
        /// </remarks>
        public GraphicsMode GraphicsMode
        {
            get
            {
                ValidateState();
                return Context.GraphicsMode;
            }
        }

        #endregion

        #region WindowInfo

        /// <summary>
        /// Gets the <see cref="OpenTK.Platform.IWindowInfo"/> for this instance.
        /// </summary>
        public IWindowInfo WindowInfo
        {
            get { return implementation.WindowInfo; }
        }
        
        #endregion

        #region public Bitmap GrabScreenshot()

        /// <summary>Grabs a screenshot of the frontbuffer contents.</summary>
        /// <returns>A System.Drawing.Bitmap, containing the contents of the frontbuffer.</returns>
        /// <exception cref="OpenTK.Graphics.GraphicsContextException">
        /// Occurs when no OpenTK.Graphics.GraphicsContext is current in the calling thread.
        /// </exception>
        [Obsolete("This method will not work correctly with OpenGL|ES. Please use GL.ReadPixels to capture the contents of the framebuffer (refer to http://www.opentk.com/doc/graphics/save-opengl-rendering-to-disk for more information).")]
        public Bitmap GrabScreenshot()
        {
            ValidateState();

            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                             System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte,
                          data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        #endregion

        #endregion
    }
}
