namespace MiniORM
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
   

    public class DbSet<TEntity> : ICollection<TEntity> where TEntity : class, new()
	{
		internal DbSet(IEnumerable<TEntity> entities)
        {
			this.Entities = entities.ToList();
			this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }
		internal ChangeTracker<TEntity> ChangeTracker { get; set; }
		internal IList<TEntity> Entities { get; set; }
		public void Add(TEntity item)
        {
            if (item == null)
            {
				throw new ArgumentNullException(nameof(item), "Item cannot be null!");
            }
            else
            {
				this.Entities.Add(item);
				this.ChangeTracker.Add(item);
            }
        }
		public bool Remove(TEntity item)
        {
            if (item==null)
            {
				throw new ArgumentNullException(nameof(item), "Item cannot be null!");
            }
			bool successfullyRemoved = this.Entities.Remove(item);
            if (successfullyRemoved)
            {
				this.ChangeTracker.Remove(item);
            }
			return successfullyRemoved;
        }
		public void Clear()
        {
            while (this.Entities.Any())
            {
				TEntity entityToRemove = this.Entities.First();
				this.Remove(entityToRemove);
            }
        }
		public bool Contains(TEntity item)
        {
			return this.Entities.Contains(item);
        }
		public void CopyTo(TEntity[] array, int arrayIndex) => this.Entities.CopyTo(array, arrayIndex);
		public int Count => this.Entities.Count;
		public bool IsReadOnly => this.Entities.IsReadOnly;

		public IEnumerator<TEntity> GetEnumerator()
        {
			return this.Entities.GetEnumerator();
        }
		IEnumerator IEnumerable.GetEnumerator()
        {
			return this.GetEnumerator();
        }
		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities.ToArray())
			{
				this.Remove(entity);
			}
		}
	}
}