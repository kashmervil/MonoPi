//
//  GpioBase.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//  Derived from https://github.com/cypherkey/RaspberryPi.Net
//  by Aaron Anderson <aanderson@netopia.ca>
//
using System;
using System.Collections.Generic;
using System.Threading;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Abstract base class for the GPIO connector on the Pi (P1) (as found
	/// next to the yellow RCA video socket on the Rpi circuit board).
	/// </summary>
	public abstract class GpioBase : IGpio
	{
		#region Fields
		private Boolean _isDisposed = false;
		private BoardRevision _revision = BoardRevision.Rev2;
		private GpioPins _pin = GpioPins.GPIO_NONE;
		private PinState _state = PinState.Low;
		private PinDirection _direction = PinDirection.OUT;
		private static Dictionary<Int32, PinDirection> _exportedPins = new Dictionary<Int32, PinDirection>();
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/>
		/// class with a board Revision 1.0 GPIO pin, the pin direction, and
		/// the initial pin value.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		/// <param name="direction">
		/// The I/O pin direction.
		/// </param>
		/// <param name="value">
		/// The initial pin value.
		/// </param>
		public GpioBase(GpioPins pin, PinDirection direction, Boolean value) {
			this._pin = pin;
			this._direction = direction;
			this._revision = BoardRevision.Rev2;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/>
		/// class with a board Revision 1.0 pin and the pin direction.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		/// <param name="direction">
		/// The I/O pin direction.
		/// </param>
		public GpioBase(GpioPins pin, PinDirection direction)
			: this(pin, direction, false) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/>
		/// class with a board Revision 1.0 pin.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		public GpioBase(GpioPins pin)
			: this(pin, PinDirection.OUT, false) {
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when state changed.
		/// </summary>
		public event PinStateChangeEventHandler StateChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets the board revision. Default is <see cref="CyrusBuilt.MonoPi.IO.PinDirection.OUT"/>.
		/// </summary>
		public BoardRevision Revision {
			get { return this._revision; }
		}

		/// <summary>
		/// Gets the state of the pin. Default is <see cref="CyrusBuilt.MonoPi.IO.PinState.Low"/>.
		/// </summary>
		public PinState State {
			get {
				this._state = this.Read() ? PinState.High : PinState.Low;
				return this._state;
			}
		}

		/// <summary>
		/// Gets the physical pin being represented by this class.
		/// </summary>
		public GpioPins InnerPin {
			get { return this._pin; }
		}

		/// <summary>
		/// Gets the direction (mode) for the pin (Input or Output).
		/// </summary>
		public PinDirection Direction {
			get { return this._direction; }
		}

		/// <summary>
		/// Gets the exported pins.
		/// </summary>
		protected static Dictionary<Int32, PinDirection> ExportedPins {
			get { return _exportedPins; }
		}

		/// <summary>
		/// Gets or sets the PWM (Pulse Width Modulation) value.
		/// </summary>
		/// <value>
		/// The PWM value.
		/// </value>
		public abstract Int32 PWM { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(PinStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Changes the board revision.
		/// </summary>
		/// <param name="revision">
		/// The board revision. Default is <see cref="BoardRevision.Rev2"/>.
		/// </param>
		public void ChangeBoardRevision(BoardRevision revision) {
			this._revision = revision;
		}

		/// <summary>
		/// Gets the GPIO pin number.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		/// <returns>
		/// The GPIO pin number.
		/// </returns>
		protected static String GetGpioPinNumber(GpioPins pin) {
			return ((Int32)pin).ToString();
		}

		/// <summary>
		/// Write the specified value to the pin.
		/// </summary>
		/// <param name="value">
		/// If set to <c>true</c> value.
		/// </param>
		public virtual void Write(Boolean value) {
			this._state = value ? PinState.High : PinState.Low;
		}

		/// <summary>
		/// Pulse the pin output for the specified number of milliseconds.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to wait between states.
		/// </param>
		public virtual void Pulse(Int32 millis) {
			this.Write(true);
			Thread.Sleep(millis);
			this.Write(false);
		}

		/// <summary>
		/// Read a value from the pin.
		/// </summary>
		/// <returns>
		/// The value read from the pin.
		/// </returns> 
		public abstract Boolean Read();

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/>.The <see cref="Dispose"/>
		/// method leaves the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/> so
		/// the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.IO.GpioBase"/> was occupying.
		/// </remarks>
		public virtual void Dispose() {
			if (this._isDisposed) {
				return;
			}
			_exportedPins.Clear();
			_exportedPins = null;
			this.StateChanged = null;
			this._pin = GpioPins.GPIO_NONE;
			this._isDisposed = true;
		}
		#endregion
	}
}

