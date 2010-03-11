//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for OptecTCF_S_Focuser
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Interface;


namespace ASCOM.OptecTCF_S
{
    //
    // Your driver's ID is ASCOM.OptecTCF_S.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.OptecTCF_S.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("4103b899-c887-4f8b-a085-8be9e1ebc183")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuser
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        internal static string s_csDriverID = "ASCOM.OptecTCF_S.Focuser";
        private static string s_csDriverDescription = "Optec TCF-S Focuser";    //Displayed in Chooser

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            // TODO Implement your additional construction here
        }

        #region ASCOM Registration
        //
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            //Helper.Profile P = new Helper.Profile();
            Utilities.Profile P = new Utilities.Profile();
            P.DeviceType = "Focuser";					
            if (bRegister)
                P.Register(s_csDriverID, s_csDriverDescription);
            else
                P.Unregister(s_csDriverID);
            try										
            {
                Marshal.ReleaseComObject(P);
            }
            catch (Exception) { }
            P = null;
        }

        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }
        #endregion

        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        public bool Absolute
        {
            get { return true; }
        }

        public void Halt()
        {
            // This operation is not supported by Optec Drivers
            throw new ASCOM.MethodNotImplementedException("Halt");
        }

        public bool IsMoving
        {
            //This driver does not allow any other methods to be called
            //while the device is moving. So this is always false.
            get { return false; }
        }

        public bool Link
        {
            get 
            {
                if (OptecFocuser.ConnectionState != OptecFocuser.ConnectionStates.Disconnected)
                    return true;
                else return false;
            }
            set
            {

                try
                {
                    if (value)
                    {
                        //OptecTCF_S_Focuser.OptecFocuser.Connect();
                        OptecFocuser.ConnectAndEnterSerialMode();
                    }
                    else
                    {
                        OptecFocuser.Disconnect();
                    }
                }
                catch (Exception ex)
                { 
                    if (value)
                    throw new DriverException("Unable To Connect To Device.", ex);
                    else throw new DriverException("Error While Disconnecting Device.", ex);
                }
            }
        }

        public int MaxIncrement
        {
            //Max Increment is the same as Max Step in our case so use the same method...
            get { return DeviceSettings.MaxStep; }
        }

        public int MaxStep
        {
            get { return DeviceSettings.MaxStep; }
        }

        public void Move(int val)
        {
            try
            {
                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                {
                    throw new ASCOM.InvalidOperationException("Attempted to move while in TempComp Mode");
                }
                else
                {
                    OptecFocuser.MoveFocus(val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Position
        {
            get
            {
                try
                {
                    return OptecFocuser.GetPosition();
                }
                catch (Exception ex)
                {
                    throw new ASCOM.DriverException("Error Getting Position", ex);
                }
            }
        }

        public void SetupDialog()
        {
            if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.Disconnected)
            {
                SetupDialogForm F = new SetupDialogForm();
                F.ShowDialog();
            }
            else
            {
                MessageBox.Show("A connection to the focuser is currently open. \n" + 
                    "To access the driver settings please disconnect from the device first.", "Disconnect Device", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public double StepSize
        {
            get { return DeviceSettings.MaxStep; }
        }

        public bool TempComp
        {
            // TODO Replace this with your implementation
            get 
            {
                if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                    return true;
                else return false;
            }
            set 
            {
                try
                {
                    if (value)
                    {
                        if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                            return;
                        else
                            OptecFocuser.EnterTempCompMode();
                    }
                    else
                    {
                        if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.TempCompMode)
                        {
                            OptecFocuser.ExitTempCompMode();
                        }
                        else if (OptecFocuser.ConnectionState == OptecFocuser.ConnectionStates.SerialMode)
                        {
                            return;
                        }
                        else
                        {
                            throw new ASCOM.InvalidOperationException("Attemped to exit temp comp mode from unacceptable state.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ASCOM.DriverException("Failed to set temp comp mode = " + value.ToString() +
                        "\n Exception throw was: " + ex.ToString());
                }
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                if (DeviceSettings.TempProbePresent) return true;
                else return false;
            }
        }

        public double Temperature
        {
            get
            {
                try
                {
                    return OptecFocuser.GetTemperature();
                }
                catch (Exception ex)
                {
                    throw new ASCOM.DriverException("Error Getting Temperature", ex);
                }
            }
        }

        #endregion

    }

}