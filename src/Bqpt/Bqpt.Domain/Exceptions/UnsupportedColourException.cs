////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System;

namespace Bqpt.Domain
{
    public class UnsupportedColorException : Exception
    {
        public UnsupportedColorException(string code)
            : base($"Color \"{code}\" is unsupported.")
        {
        }
    }
}