//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Korsun_PP23.DataFolder
{
    using System;
    using System.Collections.Generic;
    
    public partial class Warehouse
    {
        public int IdWarehouse { get; set; }
        public int IdRegion { get; set; }
        public int IdCity { get; set; }
        public int IdStreet { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string WarehouseName { get; set; }
        public string WarPhoneNumber { get; set; }
    
        public virtual City City { get; set; }
        public virtual Region Region { get; set; }
        public virtual Street Street { get; set; }
    }
}