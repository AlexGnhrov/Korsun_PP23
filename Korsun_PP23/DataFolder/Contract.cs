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
    
    public partial class Contract
    {
        public int IdContract { get; set; }
        public string ContractNumber { get; set; }
        public System.DateTime ContractDate { get; set; }
        public string ContractName { get; set; }
        public int IdOrganization { get; set; }
    
        public virtual Organization Organization { get; set; }
    }
}