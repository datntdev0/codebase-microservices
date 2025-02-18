using System.Collections.Generic;

namespace datntdev.Abp.Domain.Entities
{
    public interface IMultiLingualEntity<TTranslation> 
        where TTranslation : class, IEntityTranslation
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}