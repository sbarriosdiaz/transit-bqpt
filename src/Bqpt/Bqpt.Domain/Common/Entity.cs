////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bqpt.Common;
using Newtonsoft.Json;

namespace Bqpt.Domain
{
    public abstract class Entity<TKey> : ControlFields, IEntity<TKey>, IEquatable<Entity<TKey>>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        [Required, Index(IsUnique = true)]
        [StringLength(AppConstants.HasMaxLength48)]
        public string DomainKey { get; set; }

        protected Entity() => DomainKey = Guid.NewGuid().ToString();

        public override bool Equals(object obj) => Equals(obj as Entity<TKey>);

        public virtual bool Equals(Entity<TKey> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();

                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        private static bool IsTransient(Entity<TKey> obj) => obj != null && Equals(obj.Id, default(TKey));

        private Type GetUnproxiedType() => GetType();

        public override string ToString() => JsonConvert.SerializeObject(this);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<TKey> x, Entity<TKey> y) => Equals(x, y);

        public static bool operator !=(Entity<TKey> x, Entity<TKey> y) => !(x == y);
    }
}