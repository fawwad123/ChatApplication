﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model
{
    public class GroupConversationModel
    {
        public int GroupId { get; set; }
        public string Message { get; set; }
        public int MessageBy { get; set; }
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
