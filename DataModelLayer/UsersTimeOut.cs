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
    
    public partial class UsersTimeOut
    {
        public int UserID { get; set; }
        public string UserEndDate { get; set; }
        public Nullable<int> UserFindID { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
