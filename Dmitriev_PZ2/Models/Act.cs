//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmitriev_PZ2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Act
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Act()
        {
            this.ObjectInspection = new HashSet<ObjectInspection>();
        }
    
        public int Act_ID { get; set; }
        public System.DateTime CreationActDate { get; set; }
        public string Text { get; set; }
        public string AdditionalInfo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ObjectInspection> ObjectInspection { get; set; }
    }
}
