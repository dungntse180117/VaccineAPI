using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Implement
{

    public class FeedbackService : IFeedbackService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(VaccinationTrackingContext context, ILogger<FeedbackService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<FeedbackResponse>> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            _logger.LogInformation($"CreateFeedbackAsync được gọi với request: {System.Text.Json.JsonSerializer.Serialize(request)}");
            try
            {

                var account = await _context.Accounts.FindAsync(request.AccountId);
                if (account == null)
                {
                    return new BadRequestObjectResult(new FeedbackResponse { Success = false, Message = $"Không tìm thấy Account với ID = {request.AccountId}" });
                }


                var newFeedback = new Feedback
                {
                    AccountId = request.AccountId,
                    Comment = request.Comment,
                    Rating = request.Rating,
                    FeedbackDate = DateTime.UtcNow,
                    VisitId = request.VisitId,
                    Status = "Pending"
                };

                _context.Feedbacks.Add(newFeedback);
                await _context.SaveChangesAsync();

                var response = new FeedbackResponse
                {
                    FeedbackId = newFeedback.FeedbackId,
                    AccountId = newFeedback.AccountId,
                    AccountName = account?.Name,
                    Comment = newFeedback.Comment,
                    Rating = (int)newFeedback.Rating,
                    FeedbackDate = newFeedback.FeedbackDate,
                    VisitId = (int)newFeedback.VisitId,
                    Status = newFeedback.Status,
                    Success = true,
                    Message = "Feedback created successfully."
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo feedback");
                return new BadRequestObjectResult(new FeedbackResponse
                {
                    Success = false,
                    Message = "Không thể tạo feedback: " + ex.Message
                });
            }
        }

        public async Task<ActionResult<FeedbackResponse>> GetFeedbackByIdAsync(int feedbackId)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(feedbackId);

                if (feedback == null)
                {
                    return new NotFoundResult();
                }

                var response = new FeedbackResponse
                {
                    FeedbackId = feedback.FeedbackId,
                    AccountId = feedback.AccountId,
                    Comment = feedback.Comment,
                    Rating = (int)feedback.Rating,
                    FeedbackDate = feedback.FeedbackDate,
                    VisitId = (int)feedback.VisitId,
                    Status = feedback.Status,
                    Success = true,
                    Message = "Feedback retrieved successfully."
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy feedback theo ID: {feedbackId}", feedbackId);
                return new BadRequestObjectResult(new FeedbackResponse
                {
                    Success = false,
                    Message = "Không thể lấy feedback: " + ex.Message
                });
            }
        }

        public async Task<ActionResult<List<FeedbackResponse>>> ListFeedbacksAsync()
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Include(f => f.Account)
                    .Include(f => f.Visit)
                    .ToListAsync();

                var feedbackResponses = feedbacks.Select(feedback => new FeedbackResponse
                {
                    FeedbackId = feedback.FeedbackId,
                    AccountId = feedback.AccountId,
                    AccountName = feedback.Account?.Name,
                    Comment = feedback.Comment,
                    Rating = (int)feedback.Rating,
                    FeedbackDate = feedback.FeedbackDate,
                    VisitId = (int)feedback.VisitId,
                    VisitDate = feedback.Visit?.VisitDate,
                    Status = feedback.Status,
                    Success = true,
                    Message = "Feedbacks retrieved successfully."
                }).ToList();
                return new OkObjectResult(feedbackResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi liệt kê feedbacks");
                return new BadRequestObjectResult(new FeedbackResponse
                {
                    Success = false,
                    Message = "Không thể liệt kê feedbacks: " + ex.Message
                });
            }
        }
        public async Task<ActionResult<FeedbackResponse>> UpdateFeedbackAsync(UpdateFeedbackRequest request)
        {
            try
            {
                var existingFeedback = await _context.Feedbacks.FindAsync(request.FeedbackId);
                if (existingFeedback == null)
                {
                    return new NotFoundResult();
                }

                if (request.Comment != null) existingFeedback.Comment = request.Comment;
                if (request.Rating.HasValue) existingFeedback.Rating = request.Rating.Value;
                if (request.Status != null) existingFeedback.Status = request.Status;

                await _context.SaveChangesAsync();

                var response = new FeedbackResponse
                {
                    FeedbackId = existingFeedback.FeedbackId,
                    AccountId = existingFeedback.AccountId,
                    Comment = existingFeedback.Comment,
                    Rating = (int)existingFeedback.Rating,
                    FeedbackDate = existingFeedback.FeedbackDate,
                    VisitId = (int)existingFeedback.VisitId,
                    Status = existingFeedback.Status,
                    Success = true,
                    Message = "Feedback updated successfully."
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật feedback ID: {feedbackId}", request.FeedbackId);
                return new BadRequestObjectResult(new FeedbackResponse
                {
                    Success = false,
                    Message = "Không thể cập nhật feedback: " + ex.Message
                });
            }
        }

        public async Task<ActionResult<IActionResult>> DeleteFeedbackAsync(int feedbackId)
        {
            try
            {
                var feedbackToRemove = await _context.Feedbacks.FindAsync(feedbackId);
                if (feedbackToRemove == null)
                {
                    return new NotFoundResult();
                }

                _context.Feedbacks.Remove(feedbackToRemove);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa feedback ID: {feedbackId}", feedbackId);
                return new BadRequestObjectResult(new FeedbackResponse
                {
                    Success = false,
                    Message = "Không thể xóa feedback: " + ex.Message
                });
            }
        }

        public async Task<bool> CheckFeedbackExistsAsync(int visitId)
        {
            try
            {
                var visitExists = await _context.Visits.AnyAsync(v => v.VisitId == visitId);
                if (!visitExists)
                {
                    return false; 
                }

                return await _context.Feedbacks.AnyAsync(f => f.VisitId == visitId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra Feedback tồn tại và Visit tồn tại cho VisitId: {visitId}", visitId);
                return false;
            }
        }
    }
}