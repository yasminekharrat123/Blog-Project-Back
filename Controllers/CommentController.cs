﻿using Blog.Dto.CommentDto;
using Blog.Models;
using Blog.Services.Comments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace Blog.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost()]
        public IActionResult CreateComment([FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comment = _commentService.CreateComment(commentDto.UserId, commentDto.BlogId, null, commentDto.Content);
            return Ok(comment);
            
        }

        [HttpPost("reply")]
        public IActionResult ReplyToComment([FromBody] CreateReplyDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = _commentService.CreateComment(commentDto.UserId, null, commentDto.ParentCommentId, commentDto.Content);
            return Ok(comment);

        }


        [HttpGet("{commentId}")]
        public IActionResult GetComment(int commentId)
        {
            var comment = _commentService.FindComment(commentId);
            return Ok(comment);
        }




        [HttpGet("{commentId}/replies")]
        public IActionResult GetRepliesByComment(int commentId, int recursionDepth=2, int page = 1, int limit = 3)
        {
 
                var replies = _commentService.GetRepliesByComment(commentId, recursionDepth, page, limit);
                return Ok(replies);

        }



        [HttpPut("{commentId}")]
        public IActionResult UpdateComment(int commentId, [FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedComment = _commentService.UpdateComment(commentId, commentDto.Content);
                return Ok(updatedComment);

        }

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                _commentService.DeleteComment(commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("count/replies/{commentId}")]
        public IActionResult GetRepliesCountByComment(int commentId)
        {
                var count = _commentService.GetRepliesCountByComment(commentId);
                return Ok(count);
        }
    }
}
