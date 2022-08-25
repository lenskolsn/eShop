namespace eShop.Database.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? UpdateUserId { get; set; }
        public int? CreateUserId { get; set; }
    }
}
