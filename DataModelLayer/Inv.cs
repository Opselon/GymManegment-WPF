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
    
    public partial class Inv
    {
        public int InvID { get; set; }
        public Nullable<int> InvCount { get; set; }
        public string InvDate { get; set; }
        public string InvEnterDate { get; set; }
        public string InvOutDate { get; set; }
        public string InvDes { get; set; }
        public Nullable<int> UserID { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
