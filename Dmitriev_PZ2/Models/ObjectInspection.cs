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
    
    public partial class ObjectInspection
    {
        public int ObjectInspection_ID { get; set; }
        public int Act_ID { get; set; }
        public int TriggeringAlarm_ID { get; set; }
        public int ResponsibePerson_ID { get; set; }
        public int Employee_ID { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Act Act { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ResponsiblePerson ResponsiblePerson { get; set; }
        public virtual TriggeringAlarm TriggeringAlarm { get; set; }
    }
}
