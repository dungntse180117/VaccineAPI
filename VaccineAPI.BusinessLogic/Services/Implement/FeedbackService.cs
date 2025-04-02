//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using VaccineAPI.BusinessLogic.Interface;
//using VaccineAPI.DataAccess.Data;
//using VaccineAPI.DataAccess.Models;
//using VaccineAPI.Shared.Request;
//using VaccineAPI.Shared.Response;

//namespace VaccineAPI.BusinessLogic.Implement
//{

//    public class FeedbackService : IFeedbackService
//    {
//        private readonly VaccinationTrackingContext _context; // Inject your DbContext
//        private readonly ILogger<FeedbackService> _logger;

//        public FeedbackService(VaccinationTrackingContext context, ILogger<FeedbackService> logger) // Inject DbContext
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<ManageFeedbackResponse> ManageFeedbackAsync(string action, ManageFeedbackRequest request)
//        {
//            ManageFeedbackResponse response = new ManageFeedbackResponse { IsSuccess = false };//FeedbackId = request.FeedbackId remove as it cannot be nullable

//            try
//            {
//                switch (action.ToLower())
//                {
//                    case "add":
//                        var feedbackToAdd = new Feedback
//                        {
//                            AppointmentId = request.AppointmentId,
//                            ServiceId = request.ServiceId,
//                            Comment = request.Comment,
//                            FeedbackDate = DateOnly.FromDateTime(DateTime.Now), // Make sure the type matches
//                        };

//                        _context.Feedbacks.Add(feedbackToAdd);
//                        await _context.SaveChangesAsync();

//                        response.FeedbackId = feedbackToAdd.FeedbackId;
//                        response.AppointmentId = feedbackToAdd.AppointmentId;
//                        response.ServiceId = feedbackToAdd.ServiceId;
//                        response.Comment = feedbackToAdd.Comment;
//                        response.FeedbackDate = (DateTime?)feedbackToAdd.FeedbackDate?.ToDateTime(TimeOnly.MinValue); // convert dateonly to datetime

//                        response.IsSuccess = true;
//                        response.Message = "Feedback added successfully.";
//                        _logger.LogInformation($"Feedback added successfully. FeedbackId: {response.FeedbackId}");
//                        break;

//                    case "update":
//                        var feedbackToUpdate = await _context.Feedbacks.FindAsync(request.FeedbackId);
//                        if (feedbackToUpdate == null)
//                        {
//                            response.Message = "Feedback not found.";
//                            _logger.LogError($"Feedback not found. FeedbackId: {request.FeedbackId}");
//                            return response;
//                        }

//                        feedbackToUpdate.AppointmentId = request.AppointmentId;
//                        feedbackToUpdate.ServiceId = request.ServiceId;
//                        feedbackToUpdate.Comment = request.Comment;

//                        _context.Entry(feedbackToUpdate).State = EntityState.Modified;
//                        await _context.SaveChangesAsync();

//                        response.FeedbackId = feedbackToUpdate.FeedbackId;
//                        response.AppointmentId = feedbackToUpdate.AppointmentId;
//                        response.ServiceId = feedbackToUpdate.ServiceId;
//                        response.Comment = feedbackToUpdate.Comment;
//                        response.FeedbackDate = (DateTime?)feedbackToUpdate.FeedbackDate?.ToDateTime(TimeOnly.MinValue);

//                        response.IsSuccess = true;
//                        response.Message = "Feedback updated successfully.";
//                        _logger.LogInformation($"Feedback updated successfully. FeedbackId: {response.FeedbackId}");
//                        break;

//                    case "delete":
//                        var feedbackToDelete = await _context.Feedbacks.FindAsync(request.FeedbackId);
//                        if (feedbackToDelete == null)
//                        {
//                            response.Message = "Feedback not found.";
//                            _logger.LogError($"Feedback not found. FeedbackId: {request.FeedbackId}");
//                            return response;
//                        }

//                        _context.Feedbacks.Remove(feedbackToDelete);
//                        await _context.SaveChangesAsync();

//                        response.IsSuccess = true;
//                        response.Message = "Feedback deleted successfully.";
//                        _logger.LogInformation($"Feedback deleted successfully. FeedbackId: {response.FeedbackId}");
//                        break;

//                    default:
//                        response.Message = "Invalid action specified.";
//                        _logger.LogError($"Invalid action specified: {action}");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error managing feedback.");
//            }

//            return response;
//        }

//        public async Task<GetFeedbackResponse> GetFeedbackAsync(int feedbackId)
//        {
//            try
//            {
//                var feedback = await _context.Feedbacks.FindAsync(feedbackId);

//                if (feedback == null)
//                {
//                    _logger.LogWarning($"Feedback not found. FeedbackId: {feedbackId}");
//                    return null;
//                }

//                var response = new GetFeedbackResponse
//                {
//                    FeedbackId = feedback.FeedbackId,
//                    AppointmentId = feedback.AppointmentId,
//                    ServiceId = feedback.ServiceId,
//                    Comment = feedback.Comment,
//                    FeedbackDate = (DateTime?)feedback.FeedbackDate?.ToDateTime(TimeOnly.MinValue)
//                };
//                _logger.LogInformation($"Feedback retrieved successfully. FeedbackId: {feedbackId}");
//                return response;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting feedback.");
//                return null;
//            }
//        }

//        public async Task<IEnumerable<GetFeedbackResponse>> GetAllFeedbacksAsync()
//        {
//            try
//            {
//                var feedbacks = await _context.Feedbacks.ToListAsync();

//                return feedbacks.Select(feedback => new GetFeedbackResponse
//                {
//                    FeedbackId = feedback.FeedbackId,
//                    AppointmentId = feedback.AppointmentId,
//                    ServiceId = feedback.ServiceId,
//                    Comment = feedback.Comment,
//                    FeedbackDate = (DateTime?)feedback.FeedbackDate?.ToDateTime(TimeOnly.MinValue)
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting all feedbacks.");
//                return new List<GetFeedbackResponse>(); //Return empty list rather than null
//            }
//        }
//    }
//}