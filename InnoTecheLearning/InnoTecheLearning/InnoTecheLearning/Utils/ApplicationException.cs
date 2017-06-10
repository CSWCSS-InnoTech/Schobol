// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*=============================================================================
**
** Class: ApplicationException
**
**
** Purpose: The base class for all "less serious" exceptions that must be
**          declared or caught.
**
**
=============================================================================*/

namespace System
{

    using System.Runtime.Serialization;
    // The ApplicationException is the base class for nonfatal, 
    // application errors that occur.  These exceptions are generated 
    // (i.e., thrown) by an application, not the Runtime. Applications that need 
    // to create their own exceptions do so by extending this class. 
    // ApplicationException extends but adds no new functionality to 
    // RecoverableException.
    // 
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ApplicationException : Exception
    {
        internal const int COR_E_APPLICATION = unchecked((int)0x80131600);
        // Creates a new ApplicationException with its message string set to
        // the empty string, its HRESULT set to COR_E_APPLICATION, 
        // and its ExceptionInfo reference set to null. 
        public ApplicationException()
            : base("Application encountered an exception.")
        {
            HResult = COR_E_APPLICATION;
        }

        // Creates a new ApplicationException with its message string set to
        // message, its HRESULT set to COR_E_APPLICATION, 
        // and its ExceptionInfo reference set to null. 
        // 
        public ApplicationException(String message)
            : base(message)
        {
            HResult = COR_E_APPLICATION;
        }

        public ApplicationException(String message, Exception innerException)
            : base(message, innerException)
        {
            HResult = COR_E_APPLICATION;
        }

    }

}