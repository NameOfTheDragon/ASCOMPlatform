﻿//-----------------------------------------------------------------------
// <summary>Defines the MemberFactory class.</summary>
//-----------------------------------------------------------------------
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// A factory class to access any registered driver members
    /// </summary>
    public class MemberFactory : IDisposable
    {
        #region MemberFactory

        private readonly TraceLogger _tl;
        private readonly bool _isComObject;
        private readonly List<Type> _objInterfaceList;
        private readonly Type _objType;
        private readonly String _strProgId;
        private object _objLateBound;


        /// <summary>
        /// Constructor, creates an instance of the of the ASCOM driver
        /// 
        /// </summary> 
        /// <param name="progId">The program ID of the driver</param>
        internal MemberFactory(string progId)
        {
            _strProgId = progId;
            _objInterfaceList = new List<Type>();

            // Get Type Information 
            _objType = Type.GetTypeFromProgID(progId);

            //check to see if it found the type information
            if (_objType == null)
            {
                //no type information found throw error
                throw new Exception("Check Driver: cannot create object type of progID: " + _strProgId);
            }
            _tl = new TraceLogger("", "MemberFactory");
            _tl.Enabled = true;

            //setup the property
            _isComObject = _objType.IsCOMObject;

            // Create an instance of the object
            _objLateBound = Activator.CreateInstance(_objType);

            // Get list of interfaces
            Type[] objInterfaces = _objType.GetInterfaces();

            foreach (Type objInterface in objInterfaces)
            {
                _objInterfaceList.Add(objInterface);
                _tl.LogMessage("Interface", objInterface.AssemblyQualifiedName);
            }

            MemberInfo[] members = _objType.GetMembers();
            foreach (MemberInfo mi in members)
            {
                _tl.LogMessage("Member", Enum.GetName(typeof (MemberTypes), mi.MemberType) + " " + mi.Name);
                if (mi.MemberType == MemberTypes.Method)
                {
                    foreach (ParameterInfo pi in ((MethodInfo) mi).GetParameters())
                    {
                        _tl.LogMessage("Parameter",
                                       "  " + pi.Name + " " + pi.ParameterType.Name + " " +
                                       pi.ParameterType.AssemblyQualifiedName);
                    }
                }
            }

            //no instance found throw error
            if (_objLateBound == null)
            {
                throw new Exception("Check Driver: cannot create driver isntance of progID: " + _strProgId);
            }
        }

        /// <summary>
        /// Returns the instance of the driver
        /// </summary> 
        /// <returns>object</returns>
        internal object GetLateBoundObject
        {
            get { return _objLateBound; }
        }

        /// <summary>
        /// Returns true is the driver is COM based
        /// </summary> 
        /// <returns>object</returns>
        internal bool IsComObject
        {
            get { return _isComObject; }
        }

        /// <summary>
        /// Returns the driver type
        /// </summary> 
        /// <returns>type</returns>
        internal Type GetObjType
        {
            get { return _objType; }
        }

        internal List<Type> GetInterfaces
        {
            get { return _objInterfaceList; }
        }


        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        /// <returns>nothing</returns>
        public void Dispose()
        {
            if (_objLateBound != null)
            {
                try
                {
                    Marshal.ReleaseComObject(_objLateBound);
                }
                catch
                {
                }
                _objLateBound = null;
            }
        }

        /// <summary>
        /// Calls a method on an object dynamically. 
        /// 
        /// parameterTypes must match the parameters and in the same order.
        /// </summary> 
        /// <param name="memberCode">1-GetProperty, 2-SetProperty, 3-Method</param>
        /// <param name="memberName">The member name to call as a string</param>
        /// <param name="parameterTypes">Array of paramerter types in order</param> 
        /// <param name="parms">Array of parameters in order</param>
        /// <exception cref="PropertyNotImplementedException"></exception>
        /// <exception cref="MethodNotImplementedException"></exception>
        /// <returns>object</returns>
        internal object CallMember(int memberCode, string memberName, Type[] parameterTypes, params object[] parms)
        {
            switch (memberCode)
            {
                case 1:
                    _tl.LogMessage("PropertyGet", memberName);

                    PropertyInfo propertyGetInfo = _objType.GetProperty(memberName);
                    if (propertyGetInfo != null)
                    {
                        _tl.LogMessage("PropertyGet", "propertyGetInfo is not null");
                        try
                        {
                            //run the .net object
                            return propertyGetInfo.GetValue(_objLateBound, null);
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage("PropertyGetEx1", e.ToString());
                            if (e.InnerException.GetType() == typeof (PropertyNotImplementedException))
                            {
                                throw new PropertyNotImplementedException(
                                    memberName + " is not implemented in this driver", true, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (InvalidValueException))
                            {
                                throw new InvalidValueException(e.InnerException.Message, "", "", e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (DriverException))
                            {
                                throw new DriverException(e.InnerException.Message, e.InnerException);
                            }
                            throw e.InnerException;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertyGetEx2", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage("PropertyGet", "propertyGetInfo is null");
                    //check the type to see if it's a COM object
                    if (_isComObject)
                    {
                        _tl.LogMessage("PropertyGet", "propertyGetInfo is COM Object");

                        try
                        {
                            //run the COM object property
                            return
                                (_objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.GetProperty,
                                                       null, _objLateBound, new object[] {}));
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessage("PropertyGetEx3", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);
                            else throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertyGetEx4", e.ToString());

                            throw e.InnerException;
                        }
                    }
                    //evertyhing failed so throw an exception
                    _tl.LogMessage("PropertyGet", "propertyGetInfo is .NET object");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, false);
                case 2:
                    _tl.LogMessage("PropertySet", memberName);
                    PropertyInfo propertySetInfo = _objType.GetProperty(memberName);
                    if (propertySetInfo != null)
                    {
                        _tl.LogMessage("PropertySet", "propertyGetInfo is not null");
                        try
                        {
                            propertySetInfo.SetValue(_objLateBound, parms[0], null);
                            return null;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage("PropertySetEx1", e.ToString());
                            if (e.InnerException.GetType() == typeof (PropertyNotImplementedException))
                            {
                                throw new PropertyNotImplementedException(
                                    memberName + " is not implemented in this driver", true, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (DriverException))
                            {
                                throw new DriverException(e.InnerException.Message, e.InnerException);
                            }
                            if (e.InnerException.GetType() == typeof (InvalidValueException))
                            {
                                throw e.InnerException;
                            }
                            throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertySetEx1", e.ToString());
                            throw;
                        }
                    }
                    _tl.LogMessage("PropertySet", "propertyGetInfo is null");
                    //check the type to see if it's a COM object
                    if (_isComObject)
                    {
                        _tl.LogMessage("PropertySet", "propertyGetInfo is COM Object");
                        try
                        {
                            //run the COM object property
                            _objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.SetProperty, null,
                                                  _objLateBound, parms);
                            return null;
                        }
                        catch (COMException e)
                        {
                            _tl.LogMessage("PropertySetEx3", e.ToString());
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);
                            else throw;
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage("PropertySetEx4", e.ToString());
                            throw e.InnerException;
                        }
                    }
                    _tl.LogMessage("PropertySet", "propertyGetInfo is .NET object");
                    throw new PropertyNotImplementedException(_strProgId + " " + memberName, true);
                case 3:
                    _tl.LogMessage(memberName, "Start");
                    foreach (Type t in parameterTypes)
                    {
                        _tl.LogMessage(memberName, "  Parameter: " + t.FullName);
                    }


                    MethodInfo methodInfo = _objType.GetMethod(memberName);
                    //, parameterTypes); //Peter: Had to take parameterTypes out to get CanMoveAxis to work with .NET drivers
                    if (methodInfo != null)
                    {
                        _tl.LogMessage(memberName, "  Got MethodInfo");

                        ParameterInfo[] pars = methodInfo.GetParameters();
                        foreach (ParameterInfo p in pars)
                        {
                            _tl.LogMessage(memberName, "  Parameter: " + p.ParameterType);
                            _tl.LogMessage(memberName,
                                          "    AssemblyQualifiedName: " + p.ParameterType.AssemblyQualifiedName);
                            _tl.LogMessage(memberName,
                                          "    AssemblyQualifiedName: " + parameterTypes[0].AssemblyQualifiedName);
                            _tl.LogMessage(memberName, "    FullName: " + p.ParameterType.FullName);
                            _tl.LogMessage(memberName, "    FullName: " + parameterTypes[0].FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + p.ParameterType.Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyFullName: " + parameterTypes[0].Assembly.FullName);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + p.ParameterType.Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyCodeBase: " + parameterTypes[0].Assembly.CodeBase);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + p.ParameterType.Assembly.Location);
                            _tl.LogMessage(memberName, "    AssemblyLocation: " + parameterTypes[0].Assembly.Location);
                            _tl.LogMessage(memberName,
                                          "    AssemblyGlobalAssemblyCache: " +
                                          p.ParameterType.Assembly.GlobalAssemblyCache);
                            _tl.LogMessage(memberName,
                                          "    AssemblyGlobalAssemblyCache: " +
                                          parameterTypes[0].Assembly.GlobalAssemblyCache);
                        }


                        try
                        {
                            object result = methodInfo.Invoke(_objLateBound, parms);
                            _tl.LogMessage(memberName, "  Successfully called method");
                            return result;
                        }
                        catch (TargetInvocationException e)
                        {
                            _tl.LogMessage(memberName, "  ***** TargetInvocationException: " + e);
                            if (e.InnerException is DriverException)
                            {
                                throw e.InnerException;
                            }
                            else
                                throw new MethodNotImplementedException(_strProgId + " " + memberName,
                                                                        e.InnerException);
                        }
                        catch (Exception e)
                        {
                            _tl.LogMessage(memberName, "  ***** Exception: " + e);
                            throw;
                        }
                    }
                    _tl.LogMessage(memberName, "  Didn't Get MethodInfo");
                    //check the type to see if it's a COM object
                    if (_isComObject)
                    {
                        _tl.LogMessage(memberName, "  It is a COM object");
                        try
                        {
                            //run the COM object method
                            return _objType.InvokeMember(memberName, BindingFlags.Default | BindingFlags.InvokeMethod,
                                                         null, _objLateBound, parms);
                        }
                        catch (COMException e)
                        {
                            if (e.ErrorCode == int.Parse("80020006", NumberStyles.HexNumber))
                                throw new MethodNotImplementedException(_strProgId + " " + memberName);
                            else throw;
                        }
                        catch (Exception e)
                        {
                            throw e.InnerException;
                        }
                    }
                    _tl.LogMessage(memberName, "  It is NOT a COM object");
                    methodInfo = null;
                    throw new MethodNotImplementedException(_strProgId + " " + memberName);
                default:
                    return null;
            }
        }

        #endregion
    }
}