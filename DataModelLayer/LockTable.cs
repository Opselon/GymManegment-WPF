//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModelLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class LockTable
    {
        public int LockID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UnlockCode { get; set; }
        public string LockStartDate { get; set; }
        public string LockEndDate { get; set; }
        public string MainBouardSerial { get; set; }
    }
}
