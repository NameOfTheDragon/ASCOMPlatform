//-----------------------------------------------------------------------
// <summary>Defines the Rotator class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    #region Rotator wrapper
    /// <summary>
    /// Provides universal access to Rotator drivers
    /// </summary>
    public class Rotator : ASCOM.Interface.IRotator, IDisposable
    {

        #region IRotator constructors

        private MemberFactory memberFactory;

        /// <summary>
        /// Creates a rotator object with the given Prog ID
        /// </summary>
        /// <param name="rotatorID"></param>
        public Rotator(string rotatorID)
		{
            memberFactory = new MemberFactory(rotatorID);
		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Rotator
        /// </summary>
        /// <param name="rotatorID">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Rotator or null for none</returns>
        public static string Choose(string rotatorID)
        {
            try
            {
                Chooser oChooser = new Chooser();
                oChooser.DeviceType = "Rotator";			// Requires Helper 5.0.3 (May '07)
                return oChooser.Choose(rotatorID);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region IRotator Members

        /// <summary>
        /// Returns True if the Rotator supports the Rotator.Reverse() method.
        /// </summary>
        public bool CanReverse
        {
            get { return (bool)memberFactory.CallMember(1, "CanReverse", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Set True to start the Connected to the Rotator; set False to terminate the Connected.
        /// The current Connected status can also be read back as this property.
        /// An exception will be raised if the Connected fails to change state for any reason.
        /// </summary>
        public bool Connected
        {
            get { return (bool)memberFactory.CallMember(1, "Connected", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Connected", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Immediately stop any Rotator motion due to a previous Move() or MoveAbsolute() method call.
        /// </summary>
        public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// True if the Rotator is currently moving to a new position. False if the Rotator is stationary.
        /// </summary>
        public bool IsMoving
        {
            get { return (bool)memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current Position value.
        /// </summary>
        /// <param name="Position">Relative position to move in degrees from current Position.</param>
        public void Move(float Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <summary>
        /// Causes the rotator to move the absolute position of Position degrees.
        /// </summary>
        /// <param name="Position">absolute position in degrees.</param>
        public void MoveAbsolute(float Position)
        {
            memberFactory.CallMember(3, "MoveAbsolute", new Type[] { typeof(float) }, new object[] { Position });
        }

        /// <summary>
        /// Current instantaneous Rotator position, in degrees.
        /// </summary>
        public float Position
        {
            get { return (float)memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sets or Returns the rotatorís Reverse state.
        /// </summary>
        public bool Reverse
        {
            get { return (bool)memberFactory.CallMember(1, "Reverse", new Type[] { }, new object[] { }); }
            set { memberFactory.CallMember(2, "Reverse", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Display a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// </summary>
        public void SetupDialog()
        {
            memberFactory.CallMember(3, "SetupDialog", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        public float StepSize
        {
            get { return (float)memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Current Rotator target position, in degrees.
        /// </summary>
        public float TargetPosition
        {
            get { return (float)memberFactory.CallMember(1, "TargetPosition", new Type[] { }, new object[] { }); }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
		/// Dispose the late-bound interface, if needed. Will release it via COM
		/// if it is a COM object, else if native .NET will just dereference it
		/// for GC.
        /// </summary>
        public void Dispose()
        {
			 memberFactory.Dispose();
		}

        #endregion
    }
    #endregion
}
