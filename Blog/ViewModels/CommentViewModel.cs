﻿using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public Guid BlogId { get; set; }

        [Required]
        public int MainCommentId { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
