using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Entities
{
    [Table("Posts")]
    public class Post : IEntity, IEquatable<Post>
    {
        public Post()
        {
            PostComments = new List<PostComment>();
        }
        public int Id { get; set; }
        public string Url { get; set; }

        public ICollection<PostComment> PostComments { get; set; }

        public bool Equals(Post other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return this.Id == other.Id && string.Equals(this.Url, other.Url) && Equals(this.PostComments, other.PostComments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Post)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Id;
                hashCode = (hashCode * 397) ^ (this.Url != null ? this.Url.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.PostComments != null ? this.PostComments.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Post left, Post right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Post left, Post right)
        {
            return !Equals(left, right);
        }
    }
}
