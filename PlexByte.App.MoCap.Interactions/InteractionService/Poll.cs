//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InteractionService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Poll
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Poll()
        {
            this.C_Poll_Option = new HashSet<C_Poll_Option>();
            this.Interaction_BinaryObject = new HashSet<Interaction_BinaryObject>();
            this.Timeslice = new HashSet<Timeslouse>();
            this.C_User_Poll_Option = new HashSet<C_User_Poll_Option>();
        }
    
        public long Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Nullable<long> ProjectId { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public Nullable<System.DateTime> DueDateTime { get; set; }
        public int VotesPerUser { get; set; }
        public bool VotesChangeable { get; set; }
        public bool AllowCustomOption { get; set; }
        public bool CompleteOnMajority { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C_Poll_Option> C_Poll_Option { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Interaction_BinaryObject> Interaction_BinaryObject { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Timeslouse> Timeslice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C_User_Poll_Option> C_User_Poll_Option { get; set; }
    }
}