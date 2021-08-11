using System;

namespace BusinessLogicLayer.Model
{
    public class ChatModal
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int MessageBy { get; set; }
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}