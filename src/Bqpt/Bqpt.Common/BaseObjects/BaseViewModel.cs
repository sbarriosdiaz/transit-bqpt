////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

namespace Bqpt.Common
{
    public abstract class BaseViewModel<TKey>
    {
        public TKey Id { get; set; }
        public string DomainKey { get; set; }
        public bool IsActive { get; set; } = true;
        public FormActions FormAction { get; set; } = FormActions.Add;
    }
}