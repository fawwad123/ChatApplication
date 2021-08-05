using System;

namespace DataAccessLayer.Model
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int MessageBy { get; set; }
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}