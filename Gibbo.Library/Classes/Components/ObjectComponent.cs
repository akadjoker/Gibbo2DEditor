﻿#region Copyrights
/*
Gibbo2D - Copyright - 2013 Gibbo2D Team
Founders - Joao Alves <joao.cpp.sca@gmail.com> and Luis Fernandes <luisapidcloud@hotmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Runtime.Serialization;

#if WINRT
using System.Runtime.Serialization;
#endif

namespace Gibbo.Library
{
    /// <summary>
    /// This class represents one object component.
    /// </summary>
#if WINDOWS
    [Serializable]
#elif WINRT
    [DataContract]
#endif
    public class ObjectComponent
#if WINDOWS
        : ICloneable
#endif
    {
        #region fields

        [DataMember]
        private string name = string.Empty;

        [DataMember]
        private bool editorExpanded = false;

        [DataMember]
        private bool disabled = false;


#if WINDOWS
        [NonSerialized]
#endif
        private Transform transform;

        #endregion

        #region properties

        /// <summary>
        /// Determines if the component is disabled
        /// </summary>
#if WINDOWS
        [Browsable(false)]
#endif
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        /// <summary>
        /// The name of the component
        /// </summary>
#if WINDOWS
        [Browsable(false)]
#endif
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
#if WINDOWS
        [Browsable(false)]
#endif
        public bool EditorExpanded
        {
            get { return editorExpanded; }
            set { editorExpanded = value; }
        }

        /// <summary>
        /// The reference transform of the component's object 
        /// </summary>
#if WINDOWS
        [Browsable(false)]
#endif
        public Transform Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Virtual Removed.
        /// This method is called when the script is removed from the game object.
        /// </summary>
        public virtual void Removed()
        {

        }

        /// <summary>
        /// Virtual Initialize
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Virtual Update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Virtual Draw
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Virtual OnMouseDown Event
        /// Event thrown when there is a mouse click collision
        /// </summary>
        public virtual void OnMouseDown(MouseEventButton buttonPressed)
        {

        }

        /// <summary>
        /// Virtual OnMouseClick Event
        /// Event thrown when there is a mouse click collision
        /// </summary>
        public virtual void OnMouseClick(MouseEventButton buttonPressed)
        {

        }

        /// <summary>
        /// Event thrown when the mouse up event on the game object is triggered
        /// </summary>
        public virtual void OnMouseUp()
        {

        }

        /// <summary>
        /// Event thrown when the mouse is not over the object
        /// </summary>
        public virtual void OnMouseOut()
        {

        }

        /// <summary>
        /// Virtual OnMouseMove Event
        /// Event thrown when there is a mouse move collision
        /// </summary>
        public virtual void OnMouseMove()
        {

        }

        /// <summary>
        /// Virtual OnMouseEnter Event
        /// Event thrown when the mouse enters in collision with the game object
        /// </summary>
        public virtual void OnMouseEnter()
        {

        }

        /// <summary>
        /// Virtual OnCollisionEnter Event
        /// Event thrown when there is an object collision.
        /// </summary>
        /// <param name="other">The collided object</param>
        public virtual void OnCollisionEnter(GameObject other)
        {

        }

        /// <summary>
        /// Virtual OnCollisionEnter Event
        /// Event thrown in a frame where there is no collision with other objects
        /// </summary>
        public virtual void OnCollisionFree()
        {

        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.name;
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
