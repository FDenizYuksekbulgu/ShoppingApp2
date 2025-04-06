using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingApp2.WebApi.Filters
{
    public class TimeRestrictedActionFilter : Attribute, IActionFilter
    {
        private readonly TimeSpan _startTime;
        private readonly TimeSpan _endTime;

        public TimeRestrictedActionFilter(string startTime, string endTime)
        {
            _startTime = TimeSpan.Parse(startTime);
            _endTime = TimeSpan.Parse(endTime);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var currentTime = DateTime.Now.TimeOfDay;

            if (currentTime < _startTime || currentTime > _endTime)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403, // Forbidden
                    Content = $"This API is only accessible between {_startTime} and {_endTime}."
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }
    }
}
